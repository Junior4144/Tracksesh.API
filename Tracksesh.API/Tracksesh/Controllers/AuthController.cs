using System.Security.Claims;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tracksesh.Models.DTO;
using Tracksesh.Repositories.Interface;

namespace Tracksesh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // find user by email
            var identityUser = await userManager.FindByEmailAsync(request.Email);
            if (identityUser is not null)
            {
                // check password
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);

                    //create a token and response
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    var response = new LoginResponseDto
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                    };

                    Response.Cookies.Append("access_token", jwtToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.UtcNow.AddMinutes(15)
                    });


                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Invalid email or password");
            return ValidationProblem(ModelState);
        }

        // POST::
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // create IdentityUser object

            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };
            //Create user
            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
                // add role to user (reader)
                identityResult = await userManager.AddToRoleAsync(user, "reader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }

        [Authorize]
        [HttpGet]
        [Route("me")]
        public IActionResult UserDetails()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var response = new LoginResponseDto
            {
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                Roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList()
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            //Override previous cookie
            Response.Cookies.Append("access_token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            return Ok();
        }

    }
}
