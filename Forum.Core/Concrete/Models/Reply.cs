using System;

namespace Forum.Core.Concrete.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }

        public User Author { get; set; }
        public string AuthorId { get; set; }

        public Post Post { get; set; }
        public int PostId { get; set; }
    }
}