using System.Threading.Tasks;
using Forum.Core.Abstract.Repositories;

namespace Forum.Core.Abstract
{
    public interface IUnitOfWork
    {
        IPostRepository PostRepository { get; }
        IReplyRepository ReplyRepository { get; }
        ITagRepository TagRepository { get; }
        
        Task CompleteAsync();
        int Complete();
    }
}