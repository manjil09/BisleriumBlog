using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Application.DTOs.UserDTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
