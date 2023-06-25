using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utlities;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService, ITokenProvider tokenProvider)
        {
            _baseService = baseService;
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto )
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utlities.SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + $"/api/auth/assignRole",

            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utlities.SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + $"/api/auth/login",

            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Utlities.SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + $"/api/auth/register",

            });
        }

        private async Task SignInUser(LoginResponseDto loginResponseDto)
        {
            
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler()
        }
        private string CreateIdentity()
        {
            HttpContext.
        }
    }
}
