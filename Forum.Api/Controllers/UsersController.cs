using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Api.Requests;
using Forum.Api.Responses;
using Forum.Core.Concrete.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var response = _mapper.Map<IEnumerable<UserListResponse>>(users);

            return Ok(response);
        }

        [HttpGet("{username}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.NormalizedUserName == username.Normalize());

            if (user == null)
            {
                return NotFound("User does not exist");
            }

            var response = _mapper.Map<UserResponse>(user);
            response.Roles = await _userManager.GetRolesAsync(user);

            return Ok(response);
        }
    }
}