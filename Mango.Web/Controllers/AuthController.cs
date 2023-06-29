using Mango.Web.Models;
using Mango.Web.Resources;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        { 
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequest = new LoginRequestDto();
            return View(loginRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            ResponseDto response = await _authService.LoginAsync(loginRequest);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

                await SignInUser(loginResponse);
                _tokenProvider.SetToken(loginResponse.Token);
                TempData[ConstantResource.Tempdata_Success] = response.Message;
                return RedirectToAction(nameof(Index), nameof(HomeController).Replace(ConstantResource.ReplaceString, ""));
            }
            else
            {
                TempData[ConstantResource.Tempdata_Error] = response.Message;
                return View(loginRequest);
            }            
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{ Text = ConstantResource.AdminRole, Value = ConstantResource.AdminRole },
                new SelectListItem{ Text = ConstantResource.CustomerRole, Value = ConstantResource.CustomerRole}
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequest)
        {
            ResponseDto response = await _authService.RegisterAsync(registrationRequest);
            ResponseDto assignRole;
            if(response != null && response.IsSuccess) 
            {
                if (string.IsNullOrEmpty(registrationRequest.Role))
                {
                    registrationRequest.Role = ConstantResource.CustomerRole;
                }
                assignRole = await _authService.AssignRoleAsync(registrationRequest);
                if(assignRole != null && assignRole.IsSuccess)
                {
                    TempData[ConstantResource.Tempdata_Success] = response.Message;
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData[ConstantResource.Tempdata_Error] = response.Message;
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{ Text = ConstantResource.AdminRole, Value = ConstantResource.AdminRole },
                new SelectListItem{ Text = ConstantResource.CustomerRole, Value = ConstantResource.CustomerRole}
            };

            ViewBag.RoleList = roleList;
            return View(registrationRequest);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction(nameof(Index), nameof(HomeController).Replace(ConstantResource.ReplaceString, ""));
        }

        private async Task SignInUser(LoginResponseDto loginResponse)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponse.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == ConstantResource.RoleType).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
