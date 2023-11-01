using JwtCrud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace JwtCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenContext _context; 

        public AuthController(JwtTokenContext context) 
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }

            var employee = _context.Employees
                .FirstOrDefault(e => e.EmpName == user.UserName && e.Password == user.Password);

            if (employee != null)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, employee.RoleName), 
                    new Claim(ClaimTypes.NameIdentifier, employee.EmpName),
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
