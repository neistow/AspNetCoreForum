using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Api.Requests;
using Forum.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Forum.Core.Abstract.Managers;
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
        private readonly IReplyManager _replyManager;
        private readonly IMapper _mapper;

        public PostsController(UserManager<User> userManager, IPostManager postManager, IMapper mapper,
            IReplyManager replyManager)
        {
            _userManager = userManager;
            _postManager = postManager;
            _replyManager = replyManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postManager.GetAllPosts();
            var response = _mapper.Map<IEnumerable<PostResponse>>(posts);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost([FromRoute] int id)
        {
            var post = await _postManager.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<PostResponse>(post);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] PostRequest postRequest)
        {
            var post = _mapper.Map<Post>(postRequest);

            _postManager.AddPost(post);
            await _postManager.SaveChangesAsync();

            var response = _mapper.Map<PostResponse>(post);
            return CreatedAtAction(nameof(GetPost), new {id = post.Id}, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] PostRequest postRequest)
        {
            var postInDb = await _postManager.GetPost(id);
            if (postInDb == null)
            {
                return NotFound();
            }

            _mapper.Map(postRequest, postInDb);
            postInDb.DateEdited = DateTime.Now;
            await _postManager.SaveChangesAsync();

            var response = _mapper.Map<PostResponse>(postInDb);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            var post = await _postManager.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }

            _postManager.RemovePost(post);
            await _postManager.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{postId}/replies")]
        public async Task<IActionResult> AddReplyToPost([FromRoute] int postId, [FromBody] ReplyRequest request)
        {
            if (postId != request.PostId)
            {
                return BadRequest();
            }

            var post = await _postManager.GetPostWithReplies(postId);
            if (post == null)
            {
                return NotFound();
            }

            var authorId = _userManager.GetUserId(HttpContext.User);
            var reply = _mapper.Map<Reply>(request);
            reply.AuthorId = authorId;

            _replyManager.AddReply(reply);
            await _replyManager.SaveChangesAsync();

            var response = _mapper.Map<ReplyResponse>(reply);

            return CreatedAtAction(nameof(GetPost), new {id = postId}, response);
        }
    }
}