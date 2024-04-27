namespace BisleriumBlog.Application.DTOs
{
    public class CommentDTO
    {
        public string Body { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
    }
}
