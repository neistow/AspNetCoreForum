using System.Linq;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;
using Forum.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Concrete.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext ApplicationDbContext => Context as ApplicationDbContext;

        public UserRepository(DbContext context) : base(context)
        {
        }

        public bool HasUniqueEmailAndUsername(User user)
        {
            return ApplicationDbContext.Users.Any(u => u.Email == user.Email && u.Username == user.Username);
        }
    }
}