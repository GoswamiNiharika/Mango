using Mango.Web.Models;
using Mango.Web.Resources;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService) 
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = registrationRequest,
                ApiUrl = StaticDetails.AuthAPIBase + ApiUrlResource.Auth_AssignRole
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = loginRequest,
                ApiUrl = StaticDetails.AuthAPIBase + ApiUrlResource.Auth_Login
            },withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = registrationRequest,
                ApiUrl = StaticDetails.AuthAPIBase + ApiUrlResource.Auth_Register
            }, withBearer: false);
        }
    }
}
