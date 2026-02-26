using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskWebApi.Common;
using TaskWebApi.Models;

namespace TaskWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly Jwtsettings _jwtsettings;

        public LoginController(Jwtsettings jwtsettings)
        {
            _jwtsettings = jwtsettings;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login model)
        {
            if (model.Email != "kunalvndra@test.com" || model.Password != "123456")
                return Unauthorized("Invalid credentials");

            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, model.Email),
               new Claim(ClaimTypes.Email, model.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: _jwtsettings.Issuer,
                                             audience: _jwtsettings.Audience,
                                             claims: claims,
                                             expires: DateTime.UtcNow.AddMinutes(_jwtsettings.ExpireMinutes),
                                             signingCredentials: creds);

            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
