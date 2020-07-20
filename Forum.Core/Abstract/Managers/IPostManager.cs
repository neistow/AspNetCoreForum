using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Abstract.Managers
{
    public interface IPostManager
    {
        Task<List<Post>> GetAllPosts();
        Task<Post> GetPost(int id);
        Task<Post> GetPostWithReplies(int id);
        Task<bool> PostExists(int id);
        void AddPost(Post post);
        void RemovePost(Post post);
        void UpdatePost(Post post);
    }
}