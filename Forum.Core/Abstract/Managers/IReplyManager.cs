﻿using System.Threading.Tasks;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Abstract.Managers
{
    public interface IReplyManager
    {
        ValueTask<Reply> GetReply(int id);
        Task<bool> ReplyExists(int id);
        void AddReply(Reply reply);
        void RemoveReply(Reply reply);
        void UpdateReply(Reply reply);
    }
}