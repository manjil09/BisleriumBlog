namespace BisleriumBlog.Application.DTOs
{
    public class CommentResponseDTO
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
