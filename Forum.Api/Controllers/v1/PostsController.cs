using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Api.Requests;
using Forum.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Forum.Core.Abstract.Managers;
using Forum.Core.Concrete.Constants;
using Forum.Core.Concrete.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Forum.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IPostManager _postManager;
        private readonly ITagManager _tagManager;
        private readonly IMapper _mapper;

        public PostsController(UserManager<User> userManager, IPostManager postManager, IMapper mapper,
            ITagManager tagManager)
        {
            _userManager = userManager;
            _postManager = postManager;
            _tagManager = tagManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postManager.GetAllPosts();
            var response = _mapper.Map<IEnumerable<PostResponse>>(posts);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPost([FromRoute] int id)
        {
            var post = await _postManager.GetPost(id);
            if (post == null)
            {
                return NotFound("Post does not exist.");
            }

            var response = _mapper.Map<PostResponse>(post);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddPost([FromBody] PostRequest postRequest)
        {
            if (!postRequest.PostTags.All(t => _tagManager.TagExists(t)))
            {
                return BadRequest("Post contains nonexistent tag(s).");
            }

            var post = _mapper.Map<Post>(postRequest);
            post.AuthorId = _userManager.GetUserId(HttpContext.User);

            _postManager.AddPost(post);
            await _postManager.SaveChangesAsync();

            var response = _mapper.Map<PostResponse>(post);
            return CreatedAtAction(nameof(GetPost), new {id = post.Id}, response);
        }

        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EditPost([FromRoute] int id, [FromBody] PostRequest postRequest)
        {
            var postInDb = await _postManager.GetPost(id);
            if (postInDb == null)
            {
                return NotFound("Post does not exist.");
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (postInDb.AuthorId != user.Id && !await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                return BadRequest("You are not author of the post.");
            }

            if (!postRequest.PostTags.All(t => _tagManager.TagExists(t)))
            {
                return BadRequest("Post contains nonexistent tag(s).");
            }

            _mapper.Map(postRequest, postInDb);
            postInDb.DateEdited = DateTime.Now;
            await _postManager.SaveChangesAsync();

            var response = _mapper.Map<PostResponse>(postInDb);

            return Ok(response);
        }

        [HttpDelete("{id:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            var postInDb = await _postManager.GetPost(id);
            if (postInDb == null)
            {
                return NotFound("Post does not exist.");
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (postInDb.AuthorId != user.Id && !await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                return BadRequest("You are not author of the post.");
            }

            _postManager.RemovePost(postInDb);
            await _postManager.SaveChangesAsync();

            return Ok();
        }
    }
}