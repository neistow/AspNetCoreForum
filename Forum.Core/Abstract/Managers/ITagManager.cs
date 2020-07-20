using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Abstract.Managers
{
    public interface ITagManager
    {
        Task<List<Tag>> GetAllTags();
        ValueTask<Tag> GetTag(int id);
        void AddTag(Tag tag);
        void DeleteTag(Tag tag);
        bool TagExists(int tagId);
        void UpdateTag(Tag tag);
    }
}