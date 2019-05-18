using EventsRepublic.InterFace;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class Tokens : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public Tokens(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(string email)
        {
            //create a claim
            var claim = new[] {
                    new Claim(JwtRegisteredClaimNames.Email,email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["HMAC256:Key"]));

            int expiryInMinutes = Convert.ToInt32(_config["Jwt:ExpiryInMinutes"]);

            var token = new JwtSecurityToken(
              issuer: _config["Jwt:Issuer"],
              audience: _config["Jwt:Audience"],
              expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiryTime"])),
              signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            );

            return
                  new JwtSecurityTokenHandler().WriteToken(token);
               
        }
    }
}
