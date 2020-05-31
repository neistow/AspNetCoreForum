using System.Linq;
using AutoMapper;
using Forum.Api.Requests;
using Forum.Core.Concrete.Models;

namespace Forum.Api.Mapper.AfterMaps
{
    public class UpdateTagsForPost : IMappingAction<PostRequest, Post>
    {
        public void Process(PostRequest postRequest, Post post, ResolutionContext context)
        {
            var removedTags = post.PostTags.Where(tag => !postRequest.PostTags.Contains(tag.TagId)).ToList();
            var addedTags = postRequest.PostTags
                .Where(id => post.PostTags
                    .All(t => t.TagId != id))
                .Select(id => new PostTag {TagId = id, PostId = post.Id}).ToList();
            
            removedTags.ForEach(tag => post.PostTags.Remove(tag));
            addedTags.ForEach(tag => post.PostTags.Add(tag));
        }
    }
}