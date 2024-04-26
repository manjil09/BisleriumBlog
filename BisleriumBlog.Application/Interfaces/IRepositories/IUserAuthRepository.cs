using BisleriumBlog.Application.DTOs;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IUserAuthRepository
    {
        Task<ResponseDTO> Register(UserRegisterDTO userForRegister);
        Task<ResponseDTO> Login(UserLoginDTO userForLogin);
    }
}
