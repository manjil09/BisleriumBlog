using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthRepository _userAuthRepository;
        public UserAuthController(IUserAuthRepository userAuthRepository)
        {
            this._userAuthRepository = userAuthRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userForRegister)
        {
            var result = await _userAuthRepository.Register(userForRegister);

            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegisterDTO userForRegister)
        {
            var result = await _userAuthRepository.Register(userForRegister, UserRole.Admin);

            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userForLogin)
        {
            if (await _userAuthRepository.ValidateUser(userForLogin))
            {
                var result = await _userAuthRepository.CreateToken();

                return Ok(result);
            }
            return BadRequest("Invalid username or password!");
        }
    }
}
