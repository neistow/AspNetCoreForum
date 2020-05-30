using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Forum.Core.Concrete.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEdited { get; set; }

        public ICollection<PostTag> PostTags { get; set; }

        public Post()
        {
            PostTags = new Collection<PostTag>();
        }
    }
}