using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Forum.Api.Requests;
using Forum.Api.Responses;
using Forum.Core.Abstract.Managers;
using Forum.Core.Concrete.Constants;
using Forum.Core.Concrete.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.v1
{
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagManager _tagManager;
        private readonly IMapper _mapper;

        public TagsController(ITagManager tagManager, IMapper mapper)
        {
            _tagManager = tagManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _tagManager.GetAllTags();
            var response = _mapper.Map<IEnumerable<TagResponse>>(tags);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id:min(1)}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTag([FromRoute] int id)
        {
            var tag = await _tagManager.GetTag(id);
            if (tag == null)
            {
                return NotFound("Tag does not exist.");
            }

            var response = _mapper.Map<TagResponse>(tag);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> AddTag([FromBody] TagRequest tagRequest)
        {
            var tag = _mapper.Map<Tag>(tagRequest);

            _tagManager.AddTag(tag);
            await _tagManager.SaveChangesAsync();

            var response = _mapper.Map<TagResponse>(tag);
            return CreatedAtAction(nameof(GetTag), new {id = tag.Id}, response);
        }

        [HttpPut("{id:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EditTag([FromRoute] int id, [FromBody] TagRequest tagRequest)
        {
            if (id != tagRequest.Id)
            {
                return BadRequest("Tag id in route doesn't match tag id in request");
            }

            var tagInDb = await _tagManager.GetTag(id);
            if (tagInDb == null)
            {
                return NotFound("Tag does not exist.");
            }

            _mapper.Map(tagRequest, tagInDb);
            await _tagManager.SaveChangesAsync();

            var response = _mapper.Map<TagResponse>(tagInDb);

            return Ok(response);
        }

        [HttpDelete("{id:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTag([FromRoute] int id)
        {
            var tag = await _tagManager.GetTag(id);
            if (tag == null)
            {
                return NotFound("Tag does not exist.");
            }

            _tagManager.DeleteTag(tag);
            await _tagManager.SaveChangesAsync();

            return Ok();
        }
    }
}