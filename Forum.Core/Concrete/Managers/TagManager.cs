using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Core.Abstract.Managers;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Concrete.Managers
{
    public class TagManager : ITagManager
    {
        private readonly ITagRepository _tagRepository;

        public TagManager(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public Task<List<Tag>> GetAllTags()
        {
            return _tagRepository.GetAllAsync();
        }

        public ValueTask<Tag> GetTag(int id)
        {
            return _tagRepository.GetAsync(id);
        }

        public void AddTag(Tag tag)
        {
            _tagRepository.Add(tag);
        }

        public void DeleteTag(Tag tag)
        {
            _tagRepository.Remove(tag);
        }

        public bool TagExists(int tagId)
        {
            return _tagRepository.Get(tagId) != null;
        }

        public int SaveChanges()
        {
            return _tagRepository.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _tagRepository.SaveChangesAsync();
        }
    }
}