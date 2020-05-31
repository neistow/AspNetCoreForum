using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Abstract.Managers
{
    public interface ITagManager : IManager
    {
        Task<List<Tag>> GetAllTags();
        ValueTask<Tag> GetTag(int id);
        void CreateTag(Tag tag);
        void DeleteTag(Tag tag);
    }
}