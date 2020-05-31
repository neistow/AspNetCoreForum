using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Forum.Core.Abstract.Managers;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;

namespace Forum.Core.Concrete.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _repository;

        public UserManager(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _repository.SingleOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return null;
            }

            return !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? null : user;
        }

        public async Task<List<User>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        public async ValueTask<User> GetById(int id)
        {
            return await _repository.GetAsync(id);
        }

        public User Get(int id)
        {
            return _repository.Get(id);
        }

        public bool CreateUser(User user, string password)
        {
            if (_repository.HasUniqueEmailAndUsername(user))
            {
                return false;
            }

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _repository.Add(user);

            return true;
        }

        public void Delete(User user)
        {
            _repository.Remove(user);
        }

        public int SaveChanges()
        {
            return _repository.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _repository.SaveChangesAsync();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                if (computedHash.Where((t, i) => t != storedHash[i]).Any())
                {
                    return false;
                }
            }

            return true;
        }
    }
}