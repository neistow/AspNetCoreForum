using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using AutoMapper;
using Forum.Api.Requests;
using Forum.Api.Responses;
using Forum.Core.Abstract.Managers;
using Forum.Core.Concrete.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("/api/v1/posts/")]
    public class RepliesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IReplyManager _replyManager;
        private readonly IPostManager _postManager;
        private readonly IMapper _mapper;

        public RepliesController(IReplyManager replyManager, IPostManager postManager, IMapper mapper,
            UserManager<User> userManager)
        {
            _userManager = userManager;
            _replyManager = replyManager;
            _postManager = postManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{postId}/replies")]
        public async Task<IActionResult> GetAllReplies([FromRoute] int postId)
        {
            var post = await _postManager.GetPostWithReplies(postId);
            if (post == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<IEnumerable<ReplyResponse>>(post.Replies);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{postId}/replies/{replyId}")]
        public async Task<IActionResult> GetReply([FromRoute] int postId, [FromRoute] int replyId)
        {
            if (!await _postManager.PostExists(postId))
            {
                return NotFound();
            }

            var reply = await _replyManager.GetReply(replyId);
            if (reply == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<ReplyResponse>(reply);

            return Ok(response);
        }

        [HttpPut("{postId}/replies/{id}")]
        public async Task<IActionResult> EditReply([FromRoute] int postId, [FromRoute] int id,
            [FromBody] ReplyRequest request)
        {
            if (postId != request.PostId)
            {
                return BadRequest("Post id in route doesn't match request post id");
            }

            if (!await _postManager.PostExists(postId))
            {
                return NotFound();
            }

            var reply = await _replyManager.GetReply(id);
            if (reply == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(HttpContext.User);
            if (reply.AuthorId != currentUserId)
            {
                return Forbid();
            }

            _mapper.Map(request, reply);
            reply.DateEdited = DateTime.Now;
            await _replyManager.SaveChangesAsync();

            var response = _mapper.Map<ReplyResponse>(reply);
            return Ok(response);
        }

        [HttpDelete("{postId}/replies/{replyId}")]
        public async Task<IActionResult> DeleteReply([FromRoute] int postId, [FromRoute] int replyId)
        {
            var reply = await _replyManager.GetReply(replyId);
            if (reply == null)
            {
                return NotFound();
            }

            if (reply.PostId != postId)
            {
                return BadRequest("Post id in route doesn't match request post id");
            }

            var currentUserId = _userManager.GetUserId(HttpContext.User);
            if (reply.AuthorId != currentUserId)
            {
                return Forbid();
            }

            _replyManager.RemoveReply(reply);
            await _replyManager.SaveChangesAsync();

            return Ok();
        }
    }
}