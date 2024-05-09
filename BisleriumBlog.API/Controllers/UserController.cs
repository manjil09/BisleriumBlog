using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.UserDTO;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            try
            {
                var result = await _userRepository.Register(userForRegister);

                if (!result.IsSuccess)
                    return BadRequest(result.Message);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }


        [HttpGet("getAllAdmin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            try
            {
                var result = await _userRepository.GetAllAdmin();
                return Ok(result);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [HttpGet("getProfile/{id}")]
        public async Task<IActionResult> GetProfile(string id)
        {
            try
            {
                var result = await _userRepository.GetProfile(id);
                return Ok(result);
            }catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegisterDTO userForRegister)
        {
            try
            {
                var result = await _userRepository.Register(userForRegister, UserRole.Admin);

                if (!result.IsSuccess)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userForLogin)
        {
            try
            {
                if (await _userRepository.ValidateUser(userForLogin))
                {
                    var result = await _userRepository.CreateToken();

                    return Ok(result);
                }
                return BadRequest("Invalid username or password!");
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [Authorize]
        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserUpdateDTO updatedUser)
        {
            try
            {
                var result = await _userRepository.UpdateUser(id, updatedUser);

                if (!result.IsSuccess)
                    return BadRequest(result.Message);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [Authorize]
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
