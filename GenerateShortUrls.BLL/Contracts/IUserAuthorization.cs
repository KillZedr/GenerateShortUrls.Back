using GenerateShortUrls.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrls.BLL.Contracts
{
    public interface IUserAuthorization : IService
    {
        Task<string> RegisterAsync(CreateUserDto createUserDto);
        Task<string> LoginAsync(LoginUserDto loginDto);

    }
}
