using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utlities;
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
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ResponseDto responseDto = await _authService.LoginAsync(loginRequestDto);
            if(responseDto is not null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResonseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                await SignInUser(loginResponseDto); 
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", responseDto?.Message ?? "Login failed");
                return View(loginRequestDto); 
            }
            
        }
        public IActionResult  Register()
        {
            var roleList = new List<SelectListItem>() {
                new  SelectListItem(SD.RoleAdmin, SD.RoleAdmin),
                new  SelectListItem(SD.RoleCustomer, SD.RoleCustomer)
            };
            ViewBag.RoleList = roleList; 
            RegistrationRequestDto registrationRequestDto = new RegistrationRequestDto();
            return View(registrationRequestDto); 
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponseDto result = await  _authService.RegisterAsync(registrationRequestDto);
            ResponseDto assignRole; 
            if(result is not null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDto.RoleName))
                {
                    registrationRequestDto.RoleName = SD.RoleCustomer; 
                }

                assignRole = await _authService.AssignRoleAsync(registrationRequestDto);
                if(assignRole is not null && assignRole.IsSuccess) 
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login)); 
                }
            }
            var roleList = new List<SelectListItem>() {
                new  SelectListItem(SD.RoleAdmin, SD.RoleAdmin),
                new  SelectListItem(SD.RoleCustomer, SD.RoleCustomer)
            };
            ViewBag.RoleList = roleList;
            return View(registrationRequestDto);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View(); 
        }

        private async Task SignInUser(LoginResponseDto loginResponseDto)
        {

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(loginResponseDto.Token);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimsIdentity.AddClaims(new List<Claim>
            {

                new Claim(JwtRegisteredClaimNames.Email, jwtToken.Claims.FirstOrDefault( c => c.Type == JwtRegisteredClaimNames.Email).Value),
                new Claim(JwtRegisteredClaimNames.Sub, jwtToken.Claims.FirstOrDefault( c => c.Type == JwtRegisteredClaimNames.Sub).Value),
                new Claim(JwtRegisteredClaimNames.Name, jwtToken.Claims.FirstOrDefault( c => c.Type == JwtRegisteredClaimNames.Name).Value),


                //Required by Microsoft Identity 
                new Claim(ClaimTypes.Name, jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value),





            });

            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

    }
}
