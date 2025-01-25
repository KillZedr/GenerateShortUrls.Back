using GenerateShortUrsl.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrls.BLL.Contracts
{
    public interface IJwtTokenGenerator : IService
    {
        string GenerateToken(User user);
        Guid? ValidateToken(string token);
    }
}
