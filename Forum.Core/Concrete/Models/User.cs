using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Core.Concrete.Models
{
    public class User : IdentityUser
    {
        public ICollection<Post> Posts { get; set; }
        public ICollection<Reply> Replies { get; set; }
    }
}