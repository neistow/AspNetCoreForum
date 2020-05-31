using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Abstract.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<List<Post>> GetAllPostsWithTags();
        Task<Post> GetPostWithTags(int id);
        Task<Post> GetPostWithReplies(int id);
    }
}