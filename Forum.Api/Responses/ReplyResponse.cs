using System;

namespace Forum.Api.Responses
{
    public class ReplyResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEdited { get; set; }
        public string AuthorId { get; set; }
        public int PostId { get; set; }
    }
}