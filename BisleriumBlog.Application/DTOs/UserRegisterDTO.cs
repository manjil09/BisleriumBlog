using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Application.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
