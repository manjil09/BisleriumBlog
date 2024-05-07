namespace BisleriumBlog.Application.DTOs.BlogDTO
{
    public class BlogResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int TotalUpvotes { get; set; }
        public int TotalDownvotes { get; set; }
        public int TotalComments{ get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
