using System.Threading.Tasks;
using Forum.Core.Abstract;
using Forum.Core.Abstract.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public IPostRepository PostRepository { get; }
        public IReplyRepository ReplyRepository { get; }
        public ITagRepository TagRepository { get; }

        public UnitOfWork(DbContext dbContext, IPostRepository postRepository,
            IReplyRepository replyRepository, ITagRepository tagRepository)
        {
            _dbContext = dbContext;
            PostRepository = postRepository;
            ReplyRepository = replyRepository;
            TagRepository = tagRepository;
        }


        public Task CompleteAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }
    }
}