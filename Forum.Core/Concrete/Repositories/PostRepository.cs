using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;
using Forum.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Concrete.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private ApplicationDbContext ApplicationDbContext => Context as ApplicationDbContext;

        public PostRepository(DbContext context) : base(context)
        {
        }

        public Task<List<Post>> GetAllPostsWithTags()
        {
            return ApplicationDbContext.Posts.Include(p => p.PostTags).ToListAsync();
        }

        public Task<Post> GetPostWithTags(int id)
        {
            return ApplicationDbContext.Posts.Include(p => p.PostTags).SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}