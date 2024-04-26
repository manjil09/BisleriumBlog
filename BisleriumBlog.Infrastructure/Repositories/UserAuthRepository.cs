using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace BisleriumBlog.Infrastructure.Repositories
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public UserAuthRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<ResponseDTO> Register(UserRegisterDTO userForRegister)
        {
            var existingUser = await userManager.FindByNameAsync(userForRegister.UserName);
            if (existingUser != null)
                return new ResponseDTO() { IsSuccess = false, Message = "User already exists!" };

            IdentityUser user = new()
            {
                UserName = userForRegister.UserName,
                Email = userForRegister.UserEmail,
            };

            var result = await userManager.CreateAsync(user, userForRegister.Password);
            if (!result.Succeeded)
                return new ResponseDTO() { IsSuccess = false, Message = "Failed to register user! " + result.Errors.FirstOrDefault()?.Description ?? "Please enter the details again." };

            return new ResponseDTO() { IsSuccess = true, Message = "User registration successful!" };
        }
        public Task<ResponseDTO> Login(UserLoginDTO userForLogin)
        {
            throw new NotImplementedException();
        }
    }
}
