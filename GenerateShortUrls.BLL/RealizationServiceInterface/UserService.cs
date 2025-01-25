using GenerateShortUrls.BLL.Contracts;
using GenerateShortUrls.BLL.DTOs;
using GenerateShortUrsl.Data.GenerateShortUrls.DAL.Contracts;
using GenerateShortUrsl.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenerateShortUrls.BLL.RealizationServiceInterface
{
    public class UserService : IUserService
    {
        private readonly IUrlRepository<User> _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public UserService(IUrlRepository<User> userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<User> CreateUserAsync(CreateUserDto createUserDto)
        {
            if (string.IsNullOrEmpty(createUserDto.UserName))
            {
                throw new ArgumentException("UserName cannot be null or empty");
            }

            if (string.IsNullOrEmpty(createUserDto.Email) || !IsValidEmail(createUserDto.Email))
            {
                throw new ArgumentException("Invalid email address");
            }

            var existingUser = await _userRepository.AsQueryable()
                                                     .FirstOrDefaultAsync(u => u.Email == createUserDto.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("A user with this email already exists");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            var user = new User
            {
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
            };
            user.Token = _jwtTokenGenerator.GenerateToken(user);

            _userRepository.Create(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }

        public async Task<string> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return "User not found";
            }
            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
            return "User successfully deleted"; 
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.AsQueryable().ToListAsync<User>();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
            {
                throw new ArgumentException("Invalid email address");
            }

            var user = await _userRepository.AsQueryable()
                                             .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }

        public async Task<string> UpdatePasswordAsync(Guid userId, UpdatePasswordDto updatePasswordDto)
        {
            var user = await _userRepository.AsQueryable().FirstOrDefaultAsync(uId => uId.Identifier == userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            
            if (!BCrypt.Net.BCrypt.Verify(updatePasswordDto.OldPassword, user.PasswordHash))
            {
                throw new ArgumentException("Old password is incorrect");
            }

            if (updatePasswordDto.NewPassword != updatePasswordDto.ConfirmPassword)
            {
                throw new ArgumentException("Confirm password is incorrect");
            }

            
            var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(updatePasswordDto.NewPassword);

            // Обновление пароля
            user.PasswordHash = hashedNewPassword;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return "Password successfully updated";
        }

        public async Task<User> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
            {
                throw new ArgumentNullException(nameof(updateUserDto), "Update data cannot be null");
            }
         
            var user = await _userRepository.AsQueryable().FirstOrDefaultAsync(uId => uId.Identifier == userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
          
            if (!string.IsNullOrEmpty(updateUserDto.UserName))
            {
                user.UserName = updateUserDto.UserName;
            }

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }


        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
    }
}
