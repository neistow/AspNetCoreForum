using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Api.Requests;
using Forum.Api.Responses;
using Forum.Core.Concrete.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Forum.Api.Controllers.v1
{
    public class AccountsController : ApiControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<User> userManager, SignInManager<User> signInManager,
            IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("signin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
            if (!result.Succeeded)
            {
                return BadRequest("Incorrect username or password.");
            }

            var user = _userManager.Users.SingleOrDefault(r => r.UserName == request.UserName);
            var token = await GenerateJwtToken(user);

            var response = new TokenResponse {Token = token};
            return Ok(response);
        }

        [HttpPost("signup")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
        {
            var user = _mapper.Map<User>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return Ok("Successfully registered.");
        }

        [Authorize]
        [HttpPost("change_password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return Ok("Password was successfully changed.");
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:JwtIssuer"],
                _configuration["Jwt:JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            var result = new JwtSecurityTokenHandler().WriteToken(token);
            return result;
        }
    }
}