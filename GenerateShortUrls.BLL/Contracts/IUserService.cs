using GenerateShortUrls.BLL.DTOs;
using GenerateShortUrsl.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrls.BLL.Contracts
{
    public interface IUserService : IService
    {
        Task<User> CreateUserAsync(CreateUserDto user);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto);
        Task<string> UpdatePasswordAsync(Guid userId, UpdatePasswordDto updatePasswordDto);
        Task<string> DeleteUserAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
    }
}
