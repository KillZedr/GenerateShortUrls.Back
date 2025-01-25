using GenerateShortUrls.BLL.Contracts;
using GenerateShortUrls.BLL.DTOs;
using GenerateShortUrsl.Data.GenerateShortUrls.DAL.Contracts;
using GenerateShortUrsl.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrls.BLL.RealizationServiceInterface
{
    public class UserAuthorization : IUserAuthorization
    {
        private readonly IUrlRepository<User> _userRepository;
        private readonly IUserService _userService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public UserAuthorization(IUrlRepository<User> userRepository, IUserService userService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<string> LoginAsync(LoginUserDto loginDto)
        {
            var user = await _userRepository.AsQueryable()
           .FirstOrDefaultAsync(u => u.Email == loginDto.UserEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return "Incorrect login or password.";
            }
            string existingToken = user.Token;
            if (!string.IsNullOrEmpty(existingToken))
            {
                var userId = _jwtTokenGenerator.ValidateToken(existingToken);

                
                if (userId != null && userId == user.Identifier)
                {
                    return $"You are already logged in. Your token: {existingToken}";
                }
            }

            
            var newToken = _jwtTokenGenerator.GenerateToken(user);


            return $"Login successful. Your new token: {newToken}";
        }

        public async Task<string> RegisterAsync(CreateUserDto createUserDto)
        {
            var existingUser = await _userRepository.AsQueryable()
        .FirstOrDefaultAsync(u => u.Email == createUserDto.Email);

            if (existingUser != null)
            {
                if (existingUser.Token == null)
                {
                    var token = _jwtTokenGenerator.GenerateToken(existingUser);
                    existingUser.Token = token;
                    await _userRepository.SaveChangesAsync();
                }
                return $"You are already logged in. Your token: {existingUser.Token}";
            }

            var user = await _userService.CreateUserAsync(createUserDto);
            if (user == null)
            {
                return "Error while creating user.";
            }

            var newToken = _jwtTokenGenerator.GenerateToken(user);
            user.Token = newToken;
            await _userRepository.SaveChangesAsync();

            return $"Registration successful. Your token: {newToken}";
        }
    }
}
