using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Core.Abstract.Managers;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Concrete.Managers
{
    public class PostManager : IPostManager
    {
        private readonly IPostRepository _postRepository;

        public PostManager(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public Task<List<Post>> GetAllPosts()
        {
            return _postRepository.GetAllPostsWithTags();
        }

        public Task<Post> GetPost(int id)
        {
            return _postRepository.GetPostWithTags(id);
        }

        public void Create(Post post)
        {
            _postRepository.Add(post);
        }

        public void DeletePost(Post post)
        {
            _postRepository.Remove(post);
        }

        public int SaveChanges()
        {
            return _postRepository.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _postRepository.SaveChangesAsync();
        }
    }
}