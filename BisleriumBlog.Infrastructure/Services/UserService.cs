using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Identity;

namespace BisleriumBlog.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInOptions;
        public UserService() { }
        public Task<ResponseDTO> Login(UserLoginDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> Register(UserRegisterDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
