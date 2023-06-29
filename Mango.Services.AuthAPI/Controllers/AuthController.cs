using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Resources;
using Mango.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegistrationRequestDto registrationRequest)
        {
            var message = await _authService.Register(registrationRequest);
            if(!string.IsNullOrEmpty(message))
            {
                _response.IsSuccess = false;
                _response.Message = message;
                return BadRequest(_response);
            }
            _response.Message = NotificationMessageResource.RegisterSuccessFul;
            return Ok(_response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var loginResponse = await _authService.Login(loginRequest);
            if(loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = NotificationMessageResource.LoginError;
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            _response.Message = NotificationMessageResource.LoginSuccessful;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequest)
        {
            var response = await _authService.AssignRole(registrationRequest.Email, registrationRequest.Role.ToUpper());
            if (!response)
            {
                _response.IsSuccess = false;
                _response.Message = NotificationMessageResource.ErrorMessage;
                return BadRequest(_response);
            }
            _response.Message = NotificationMessageResource.RoleAssigned;
            return Ok(_response);
        }
    }
}
