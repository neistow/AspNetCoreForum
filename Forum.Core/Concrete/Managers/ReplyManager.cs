using System.Threading.Tasks;
using Forum.Core.Abstract.Managers;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Concrete.Managers
{
    public class ReplyManager : IReplyManager
    {
        private readonly IReplyRepository _replyRepository;

        public ReplyManager(IReplyRepository replyRepository)
        {
            _replyRepository = replyRepository;
        }

        public ValueTask<Reply> GetReply(int id)
        {
            return _replyRepository.GetAsync(id);
        }

        public async Task<bool> ReplyExists(int id)
        {
            return await _replyRepository.GetAsync(id) != null;
        }

        public void AddReply(Reply reply)
        {
            _replyRepository.Add(reply);
        }

        public void RemoveReply(Reply reply)
        {
            _replyRepository.Remove(reply);
        }

        public int SaveChanges()
        {
            return _replyRepository.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _replyRepository.SaveChangesAsync();
        }
    }
}