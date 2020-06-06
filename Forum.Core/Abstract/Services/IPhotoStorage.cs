using System.Threading.Tasks;

namespace Forum.Core.Abstract.Services
{
    public interface IPhotoStorage
    {
        Task<string> StorePhoto(string uploadsFolderPath, IFile file);
    }
}