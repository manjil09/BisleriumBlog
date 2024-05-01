namespace BisleriumBlog.Application.DTOs
{
    public class CommentCreateDTO
    {
        public string Body { get; set; }
        public int BlogId { get; set; }
        public string UserId { get; set; }
    }
}
