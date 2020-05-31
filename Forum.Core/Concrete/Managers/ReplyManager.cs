using System.Threading.Tasks;
using Forum.Core.Abstract.Managers;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Concrete.Managers
{
    public class ReplyManager : IReplyManager
    {
        private readonly IReplyRepository _repository;
        private readonly IPostManager _postManager;

        public ReplyManager(IReplyRepository repository, IPostManager postManager)
        {
            _repository = repository;
            _postManager = postManager;
        }

        public ValueTask<Reply> GetReply(int id)
        {
            return _repository.GetAsync(id);
        }

        public void AddReply(Reply reply)
        {
            _repository.Add(reply);
        }

        public void RemoveReply(Reply reply)
        {
            _repository.Remove(reply);
        }

        public int SaveChanges()
        {
            return _repository.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _repository.SaveChangesAsync();
        }
    }
}