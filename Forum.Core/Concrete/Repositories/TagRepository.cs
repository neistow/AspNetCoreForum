using System;
using Forum.Core.Abstract.Repositories;
using Forum.Core.Concrete.Models;
using Forum.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Forum.Core.Concrete.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private ApplicationDbContext ApplicationDbContext => Context as ApplicationDbContext;

        public TagRepository(DbContext context) : base(context)
        {
        }

    }
}