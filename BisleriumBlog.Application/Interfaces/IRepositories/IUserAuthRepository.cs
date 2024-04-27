using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IUserAuthRepository
    {
        Task<ResponseDTO> Register(UserRegisterDTO userForRegister, UserRole role = UserRole.User);
        Task<bool> ValidateUser(UserLoginDTO userForLogin);
        Task<string> CreateToken();
    }
}
