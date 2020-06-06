using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Forum.Core.Abstract.Services
{
    public interface IFile
    {
        long Length { get; }
        string Name { get; }
        Stream OpenReadStream();
        void CopyTo(Stream target);
        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
    }
}