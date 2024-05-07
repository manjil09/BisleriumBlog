using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Application.DTOs.BlogDTO
{
    public class BlogUpdateDTO
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public IFormFile Image { get; set; }
    }
}
