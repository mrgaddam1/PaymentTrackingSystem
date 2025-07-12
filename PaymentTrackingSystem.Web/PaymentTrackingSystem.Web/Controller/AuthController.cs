using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace PaymentTrackingSystem.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IConfiguration configuration;
        private IAuthManager authManager { get; set; }
        public AuthController(IAuthManager _authManager,
                                 ILogger<AuthController> _logger,
                                 IConfiguration _configuration)
        {
            authManager = _authManager;
            logger = _logger;
            configuration = _configuration;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = new UserViewModel();

            try
            {
                user = await authManager.AuthenticateUser(model.Username, model.Password);

                if (user == null) return Unauthorized();

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(configuration["JWTToken"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Email) }),
                    Expires = DateTime.UtcNow.AddHours(3),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var Token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(Token).ToString();

                //await localStorage.SetItemAsync("authToken", token);

                // Update authentication state
                //((CustomAuthStateProvider)authStateProvider).NotifyUserAuthentication(user.Token.ToString());

                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", user.Token);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "SuperAdmin"),
                    new Claim(ClaimTypes.NameIdentifier, user.Email ?? "")
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal);

                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occured while processing your request.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = ex.Message,
                    Details = ex.StackTrace
                });
            }

        }

        

    }
}

