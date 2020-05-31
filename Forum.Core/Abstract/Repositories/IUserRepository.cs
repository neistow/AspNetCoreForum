using Forum.Core.Concrete.Models;

namespace Forum.Core.Abstract.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool HasUniqueEmailAndUsername(User user);
    }
}