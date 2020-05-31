using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Concrete.Repositories
{
    public class ReplyRepository : Repository<Reply>, IReplyRepository
    {
        public ReplyRepository(DbContext context) : base(context)
        {
        }
    }
}