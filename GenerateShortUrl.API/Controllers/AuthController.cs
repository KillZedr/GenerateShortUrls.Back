using GenerateShortUrls.BLL.Contracts;
using GenerateShortUrls.BLL.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenerateShortUrl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthorization _userAuthorization;

        public AuthController(IUserAuthorization userAuthorization)
        {
            _userAuthorization = userAuthorization;
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.UserEmail) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var result = await _userAuthorization.LoginAsync(loginDto);
            if (result.StartsWith("Incorrect login or password"))
            {
                return Unauthorized(result); 
            }

            return Ok(result); 
        }

       
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null || string.IsNullOrEmpty(createUserDto.Email) || string.IsNullOrEmpty(createUserDto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var result = await _userAuthorization.RegisterAsync(createUserDto);
            if (result.StartsWith("You are already registered"))
            {
                return Ok(result); 
            }

            return Ok(result); 
        }
    }
}
