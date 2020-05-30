using System.Threading.Tasks;

namespace Forum.Core.Abstract.Managers
{
    public interface IManager
    {
        public int SaveChanges();
        public Task<int> SaveChangesAsync();
    }
}