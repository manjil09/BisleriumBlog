using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Application.DTOs.UserDTO
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string UserEmail { get; set; }
    }
}
