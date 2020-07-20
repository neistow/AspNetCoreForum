using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Core.Abstract;
using Forum.Core.Abstract.Managers;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Concrete.Managers
{
    public class PostManager : IPostManager
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IPostRepository _postRepository;

        public PostManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _postRepository = _unitOfWork.PostRepository;
        }

        public Task<List<Post>> GetAllPosts()
        {
            return _postRepository.GetAllPostsWithTags();
        }

        public Task<Post> GetPost(int id)
        {
            return _postRepository.GetPostWithTags(id);
        }

        public Task<Post> GetPostWithReplies(int id)
        {
            return _postRepository.GetPostWithReplies(id);
        }

        public async Task<bool> PostExists(int id)
        {
            return await _postRepository.GetAsync(id) != null;
        }

        public void AddPost(Post post)
        {
            _postRepository.Add(post);
            _unitOfWork.Complete();
        }

        public void RemovePost(Post post)
        {
            _postRepository.Remove(post);
            _unitOfWork.Complete();
        }

        public void UpdatePost(Post post)
        {
            post.DateEdited = DateTime.Now;
            
            _postRepository.Update(post);
            _unitOfWork.Complete();
        }
    }
}