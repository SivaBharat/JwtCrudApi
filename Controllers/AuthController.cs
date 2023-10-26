using JwtCrud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }
            if (user.UserName == "Siva" && user.Password == "Siva@123")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                });

                var tokeOptions = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.Now.AddMinutes(10),
                    SigningCredentials = signinCredentials,
                };

                var tokenString = new JwtSecurityTokenHandler().CreateToken(tokeOptions);

                return Ok(new AuthenticatedResponse { Token = new JwtSecurityTokenHandler().WriteToken(tokenString) });

            }

            return Unauthorized();
        }
    }
}
