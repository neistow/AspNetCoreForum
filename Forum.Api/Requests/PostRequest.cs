using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Forum.Api.Requests
{
    public class PostRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public ICollection<int> PostTags { get; set; } = new Collection<int>();
    }
}