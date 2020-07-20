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

namespace Forum.Api.Controllers.v1
{
    [Authorize(Roles = Roles.Admin)]
    public class RolesController : ApiControllerBase
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
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var response = _mapper.Map<IEnumerable<RoleResponse>>(roles);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
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

            return Ok($"{user.UserName} is now {request.RoleName}.");
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

            return Ok($"{user.UserName} is not {request.RoleName} anymore");
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteRole([FromBody] RoleRequest request)
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