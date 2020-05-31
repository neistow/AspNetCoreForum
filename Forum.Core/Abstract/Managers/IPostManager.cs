using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Abstract.Managers
{
    public interface IPostManager : IManager
    {
        Task<List<Post>> GetAllPosts();
        Task<Post> GetPost(int id);
        void AddPost(Post post);
        void RemovePost(Post post);
    }
}