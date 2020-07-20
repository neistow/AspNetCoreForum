using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using AutoMapper;
using Forum.Api.Requests;
using Forum.Api.Responses;
using Forum.Core.Abstract.Managers;
using Forum.Core.Concrete.Constants;
using Forum.Core.Concrete.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("/api/v1/posts/")]
    public class RepliesController : ControllerBase
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
        [HttpGet("{postId:min(1)}/replies")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllReplies([FromRoute] int postId)
        {
            var post = await _postManager.GetPostWithReplies(postId);
            if (post == null)
            {
                return NotFound("Post does not exist.");
            }

            var response = _mapper.Map<IEnumerable<ReplyResponse>>(post.Replies);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{postId:min(1)}/replies/{replyId:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReply([FromRoute] int postId, [FromRoute] int replyId)
        {
            if (!await _postManager.PostExists(postId))
            {
                return NotFound("Post does not exist.");
            }

            var reply = await _replyManager.GetReply(replyId);
            if (reply == null)
            {
                return NotFound("Reply does not exist.");
            }

            var response = _mapper.Map<ReplyResponse>(reply);

            return Ok(response);
        }

        [HttpPost("{postId:min(1)}/replies")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AddReplyToPost([FromRoute] int postId, [FromBody] ReplyRequest request)
        {
            if (postId != request.PostId)
            {
                return BadRequest("Post id in route doesn't match post id in request.");
            }

            if (!await _postManager.PostExists(postId))
            {
                return NotFound("Post does not exist.");
            }

            var authorId = _userManager.GetUserId(HttpContext.User);

            var reply = _mapper.Map<Reply>(request);
            reply.AuthorId = authorId;

            _replyManager.AddReply(reply);

            var response = _mapper.Map<ReplyResponse>(reply);

            return CreatedAtAction(nameof(GetReply), new {postId = postId, replyId = reply.Id}, response);
        }

        [HttpPut("{postId:min(1)}/replies/{id:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EditReply([FromRoute] int postId, [FromRoute] int id,
            [FromBody] ReplyRequest request)
        {
            if (postId != request.PostId)
            {
                return BadRequest("Post id in route doesn't match request post id.");
            }

            if (!await _postManager.PostExists(postId))
            {
                return NotFound("Post does not exist.");
            }

            var replyInDb = await _replyManager.GetReply(id);
            if (replyInDb == null)
            {
                return NotFound("Reply does not exist.");
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (replyInDb.AuthorId != user.Id && !await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                return BadRequest("You are not author of reply.");
            }

            _mapper.Map(request, replyInDb);
            _replyManager.UpdateReply(replyInDb);

            var response = _mapper.Map<ReplyResponse>(replyInDb);
            return Ok(response);
        }

        [HttpDelete("{postId:min(1)}/replies/{replyId:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReply([FromRoute] int postId, [FromRoute] int replyId)
        {
            var reply = await _replyManager.GetReply(replyId);
            if (reply == null)
            {
                return NotFound("Reply not found.");
            }

            if (reply.PostId != postId)
            {
                return BadRequest("Post id in route doesn't match request post id.");
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (reply.AuthorId != user.Id && !await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                return BadRequest("You are not author of reply.");
            }

            _replyManager.RemoveReply(reply);

            return Ok();
        }
    }
}