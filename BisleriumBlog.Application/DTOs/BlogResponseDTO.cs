namespace BisleriumBlog.Application.DTOs
{
    public class BlogResponseDTO
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
