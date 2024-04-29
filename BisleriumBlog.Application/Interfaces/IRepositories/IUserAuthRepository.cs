using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IUserAuthRepository
    {
        Task<Response<string>> Register(UserRegisterDTO userForRegister, UserRole role = UserRole.User);
        Task<bool> ValidateUser(UserLoginDTO userForLogin);
        Task<Response<string>> CreateToken();
    }
}
