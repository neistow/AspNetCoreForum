using System.Linq;
using AutoMapper;
using Forum.Api.Mapper.AfterMaps;
using Forum.Api.Requests;
using Forum.Api.Responses;
using Forum.Core.Concrete.Models;

namespace Forum.Api.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // Post
            CreateMap<PostRequest, Post>()
                .ForMember(p => p.Id
                    , opt => opt.Ignore())
                .ForMember(p => p.PostTags
                    , opt => opt.Ignore())
                .AfterMap<UpdateTags>();

            CreateMap<Post, PostResponse>()
                .ForMember(p => p.PostTags, opt => opt.MapFrom(p => p.PostTags.Select(t => t.TagId)));

            // Reply
            CreateMap<ReplyRequest, Reply>()
                .ForMember(r => r.Id
                    , opt => opt.Ignore());
            CreateMap<Reply, ReplyResponse>();


            // Tag
            CreateMap<TagRequest, Tag>()
                .ForMember(t => t.Id
                    , opt => opt.Ignore());
            CreateMap<Tag, TagResponse>();

            // User
            CreateMap<RegisterRequest, User>();

            // Token
            CreateMap<string, TokenResponse>()
                .ForMember(t => t.Token
                    , opt => opt.MapFrom(t => t));
        }
    }
}