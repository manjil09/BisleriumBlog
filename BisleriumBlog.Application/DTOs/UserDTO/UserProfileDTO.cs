using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Application.DTOs.UserDTO
{
    public class UserProfileDTO
    {
        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string Role { get; set; }
    }
}
