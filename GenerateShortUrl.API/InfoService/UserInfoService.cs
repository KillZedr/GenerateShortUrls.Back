using System.IdentityModel.Tokens.Jwt;

namespace GenerateShortUrl.API.InfoService
{
    public class UserInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public (string Email, string Name) GetCurrentUserInfo()
        {
            var userClaims = _httpContextAccessor.HttpContext.User?.Claims;
            var email = userClaims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            var name = userClaims?.FirstOrDefault(c => c.Type == "name")?.Value;  

            return (email, name);
        }
    }
}
