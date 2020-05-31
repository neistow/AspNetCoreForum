using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Api.Helpers;
using Forum.Api.Requests;
using Forum.Core.Abstract.Managers;
using Forum.Core.Concrete.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Forum.Api.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(IUserManager userManager, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            var user = await _userManager.Authenticate(request.UserName, request.Password);
            if (user == null)
            {
                return BadRequest("Username or password is incorrect");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                Token = tokenString
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = _mapper.Map<User>(request);

            var result = _userManager.Create(user, request.Password);
            if (!result)
            {
                return BadRequest("Email or Username is already in use!");
            }

            await _userManager.SaveChangesAsync();
            return Ok();
        }
    }
}