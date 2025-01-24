using GenerateShortUrls.BLL.Contracts;
using GenerateShortUrls.BLL.DTOs;
using GenerateShortUrsl.Data.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenerateShortUrl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromQuery] CreateUserDto createUserDto)
        {
            if (createUserDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                var user = await _userService.CreateUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

            // GET: api/User/5
            [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/User/email/{email}
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromForm] UpdateUserDto updateUserDto)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, updateUserDto);
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
        }

        // PUT: api/user/{userId}/update-password
        [HttpPut("{userId}/update-password")]
        public async Task<IActionResult> UpdatePasswordAsync(Guid userId, [FromBody] UpdatePasswordDto updatePasswordDto)
        {
            if (updatePasswordDto == null)
            {
                return BadRequest("Password update data is required.");
            }

            if (string.IsNullOrEmpty(updatePasswordDto.OldPassword) || string.IsNullOrEmpty(updatePasswordDto.NewPassword))
            {
                return BadRequest("Both old and new passwords are required.");
            }

            try
            {
                var result = await _userService.UpdatePasswordAsync(userId, updatePasswordDto);
                return Ok(result); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Другие ошибки
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result == "User not found")
            {
                return NotFound(result);
            }

            return Ok(result); // User successfully deleted
        }
    }
}
