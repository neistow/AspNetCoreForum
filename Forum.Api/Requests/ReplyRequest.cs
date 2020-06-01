namespace Forum.Api.Requests
{
    public class ReplyRequest
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; }
    }
}