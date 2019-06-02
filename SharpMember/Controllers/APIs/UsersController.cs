
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharpMember.Core.Data.Models;
using SharpMember.Models.ApiModels;

namespace SharpMember.Controllers.APIs
{
    [Route("api/[controller]")]
    public class UsersController: ControllerBase
    {
        public static string test_security_key = "my-super-strong-securitykey_fjkaur-1442-4pe4-nqkda-2434lj-;234/-134@#!!-&**lllsei";
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        private string GetToken(string secretKey, Claim[] claims)
        {

            // --> SymmetricSecurityKey
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // --> JwtSecurityToken 
            var token = new JwtSecurityToken(
                //issuer: "http://sharpmember.com",
                //audience: "http://sharpmember.com",
                expires: DateTime.UtcNow.AddHours(12),

                signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature),
                claims: claims
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("token")]
        public ActionResult Token()
        {
            var claims = new[]
            {
                new Claim("WHATEVER_CLAIM", "Our custom claim")
            };
            return Ok(GetToken(test_security_key, claims));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim("WHATEVER_CLAIM", "Our custom claim")
                };

                return Ok(GetToken(test_security_key, claims));
            }
            return Unauthorized();
        }
    }
}