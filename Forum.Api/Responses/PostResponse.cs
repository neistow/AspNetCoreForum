using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Forum.Api.Responses
{
    public class PostResponse
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEdited { get; set; }
        public ICollection<int> PostTags { get; set; }

        public PostResponse()
        {
            PostTags = new Collection<int>();
        }
    }
}