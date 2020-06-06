using System.Threading.Tasks;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Abstract.Services
{
    public interface IPhotoService<T> where T : class
    {
        Task<Photo> UploadPhoto(T entity, string uploadsFolderPath, IFile file);
    }
}