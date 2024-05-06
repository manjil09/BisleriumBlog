﻿using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.UserDTO;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<Response<string>> Register(UserRegisterDTO userForRegister, UserRole role = UserRole.User);
        Task<bool> ValidateUser(UserLoginDTO userForLogin);
        Task<Response<string>> CreateToken();
    }
}
