using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Core.Abstract;
using Forum.Core.Abstract.Managers;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Concrete.Managers
{
    public class TagManager : ITagManager
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly ITagRepository _tagRepository;

        public TagManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tagRepository = _unitOfWork.TagRepository;
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
            _unitOfWork.Complete();
        }

        public void DeleteTag(Tag tag)
        {
            _tagRepository.Remove(tag);
            _unitOfWork.Complete();
        }

        public bool TagExists(int tagId)
        {
            return _tagRepository.Get(tagId) != null;
        }

        public void UpdateTag(Tag tag)
        {
            _tagRepository.Update(tag);
            _unitOfWork.Complete();
        }
    }
}