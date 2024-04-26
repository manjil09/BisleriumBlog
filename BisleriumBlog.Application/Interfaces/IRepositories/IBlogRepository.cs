﻿using BisleriumBlog.Application.DTOs;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IBlogRepository
    {
        Task<BlogDTO> AddBlog(BlogDTO blog);
        Task<List<BlogDTO>> GetAllBlogs();
        Task<BlogDTO> GetBlogById(int id);
    }
}
