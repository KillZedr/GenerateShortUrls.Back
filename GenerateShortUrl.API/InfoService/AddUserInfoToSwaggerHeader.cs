using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GenerateShortUrl.API.InfoService
{
    public class AddUserInfoToSwaggerHeader : IOperationFilter
    {
        private readonly UserInfoService _userInfoService;

        public AddUserInfoToSwaggerHeader(UserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var (email, name) = _userInfoService.GetCurrentUserInfo();

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "User Info",
                    In = ParameterLocation.Header,
                    Description = $"Email: {email}, Name: {name}",
                    Required = false
                });
            }
        }
    }
}
