﻿using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.UserDTO;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<Response<string>> Register(UserRegisterDTO userForRegister, UserRole role = UserRole.User);
        Task<Response<List<UserProfileDTO>>> GetAllUsers();
        Task<Response<UserProfileDTO>> GetProfile(string id);
        Task<bool> ValidateUser(UserLoginDTO userForLogin);
        Task<Response<string>> CreateToken();
        Task<Response<string>> UpdateUser(string id, UserUpdateDTO updatedUser);
        Task<Response<string>> ForgotPassword(string email, string newPassword);
        Task<Response<string>> ChangePassword(string userId, string currentPassword, string newPassword);
    }
}
