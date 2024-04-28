using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthRepository userAuthRepository;
        public UserAuthController(IUserAuthRepository userAuthRepository)
        {
            this.userAuthRepository = userAuthRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userForRegister)
        {
            //bool isAdmin = HttpContext.User.IsInRole(UserRole.Admin.ToString());
            //var result = await userAuthRepository.Register(userForRegister, isAdmin ? UserRole.Admin : UserRole.User);

            var result = await userAuthRepository.Register(userForRegister);

            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userForLogin)
        {
            if (await userAuthRepository.ValidateUser(userForLogin))
            {
                string token = await userAuthRepository.CreateToken();
                return Ok(token);
            }
            return BadRequest("Invalid username or password!");
        }
    }
}
