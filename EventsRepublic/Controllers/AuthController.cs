using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EventsRepublic.Data;
using EventsRepublic.InterFace;
using EventsRepublic.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventsRepublic.Controllers
{    
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;
        public AuthController(IJwtTokenService jwtTokenService, Microsoft.AspNetCore.Identity.UserManager<AppUser>userManager,IConfiguration configuration)
        {
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
        }
        // POST: api/Auth
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginViewmodel value)
        {
            var user = await _userManager.FindByEmailAsync(value.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, value.Password))
            {
                _jwtTokenService.CreateToken(user);
            }
            return Unauthorized();
        }
        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]RegisterViewModel value)
        {
            var user = new AppUser
            {
                Email = value.Email,
                UserName = value.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user,value.Password);
            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role,value.UserType));
            }
            return Ok(new { Email = user.Email });
        }

        // PUT: api/Auth/5
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateGuestCustomerinfo([FromBody]EditGuestUser value)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var appuser = new AppUser()
            {
                UserName = value.Name,
                Email = value.Email,
                PhoneNumber = value.PhoneNo
            };
            if (user != null)
            {
                await _userManager.UpdateAsync(appuser);
                return Ok();
            }
            return NotFound();
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
