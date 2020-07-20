using System;
using System.Threading.Tasks;
using Forum.Core.Abstract;
using Forum.Core.Abstract.Managers;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Concrete.Managers
{
    public class ReplyManager : IReplyManager
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IReplyRepository _replyRepository;
        
        public ReplyManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _replyRepository = _unitOfWork.ReplyRepository;
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
            _unitOfWork.Complete();
        }

        public void RemoveReply(Reply reply)
        {
            _replyRepository.Remove(reply);
            _unitOfWork.Complete();
        }

        public void UpdateReply(Reply reply)
        {
            reply.DateEdited = DateTime.Now;
            
            _replyRepository.Update(reply);
            _unitOfWork.Complete();
        }
    }
}