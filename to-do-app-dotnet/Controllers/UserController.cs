using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using to_do_app_dotnet.DTOs.User;
using to_do_app_dotnet.Models;

namespace to_do_app_dotnet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        [ExcludeFromCodeCoverageAttribute]
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data passed.");
            }
            var appUser = await _userManager.FindByNameAsync(userDTO.Name);
            if(appUser != null)
            {
                return BadRequest("Duplicate username.");
            }
            var userModel = new User
            {
                UserName = userDTO.Name,
            };

            var result = await _userManager.CreateAsync(userModel, userDTO.Password);
            
            if(result.Succeeded)
                return Ok(result);
            else
                return BadRequest(result.ToString());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data passed.");
            }
            var appUser = await _userManager.FindByNameAsync(userDTO.Name);
            if (appUser == null)
            {
                return BadRequest("No such user found");
            }
            var result = await _signInManager.PasswordSignInAsync(appUser, userDTO.Password, false, false);
            if (!result.Succeeded)
            {
                return BadRequest("Incorrect login data passed.");
            }

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, appUser.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return Ok(stringToken);
        }
    }
}
