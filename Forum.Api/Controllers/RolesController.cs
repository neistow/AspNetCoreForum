using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Api.Requests;
using Forum.Api.Responses;
using Forum.Core.Concrete.Constants;
using Forum.Core.Concrete.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Api.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RolesController(RoleManager<IdentityRole> roleManager, IMapper mapper, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var response = _mapper.Map<IEnumerable<RoleResponse>>(roles);
            
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] RoleRequest request)
        {
            var role = _mapper.Map<IdentityRole>(request);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Role was successfully created.");
        }

        [HttpPost("assign")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            if (!await _roleManager.RoleExistsAsync(request.RoleName))
            {
                return BadRequest("Role does not exist.");
            }

            await _userManager.AddToRoleAsync(user, request.RoleName);

            return Ok();
        }

        [HttpPost("remove")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveRole([FromBody] RemoveRoleRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            if (!await _roleManager.RoleExistsAsync(request.RoleName))
            {
                return BadRequest("Role does not exist.");
            }
            
            await _userManager.RemoveFromRoleAsync(user, request.RoleName);

            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete([FromBody] RoleRequest request)
        {
            var roleInDb = await _roleManager.FindByNameAsync(request.Name);
            if (roleInDb == null)
            {
                return BadRequest("Role does not exist.");
            }

            await _roleManager.DeleteAsync(roleInDb);

            return Ok();
        }
    }
}