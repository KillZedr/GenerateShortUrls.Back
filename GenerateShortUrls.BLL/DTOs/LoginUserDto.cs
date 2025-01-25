using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrls.BLL.DTOs
{
    public class LoginUserDto
    {
        public string UserEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
