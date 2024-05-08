using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.UserDTO;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userForRegister)
        {
            var result = await _userRepository.Register(userForRegister);

            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegisterDTO userForRegister)
        {
            var result = await _userRepository.Register(userForRegister, UserRole.Admin);

            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userForLogin)
        {
            if (await _userRepository.ValidateUser(userForLogin))
            {
                var result = await _userRepository.CreateToken();

                return Ok(result);
            }
            return BadRequest("Invalid username or password!");
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserUpdateDTO updatedUser)
        {
            var result = await _userRepository.UpdateUser(id, updatedUser);

            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                var response = await _userRepository.ChangePassword(model.UserId, model.CurrentPassword, model.NewPassword);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string> { IsSuccess = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            try
            {
                var response = await _userRepository.ForgotPassword(model.Email, model.NewPassword);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string> { IsSuccess = false, Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
