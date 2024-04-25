using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthRepository userAuthRepository;
        public UserAuthController(IUserAuthRepository userAuthRepository)
        {
            this.userAuthRepository = userAuthRepository;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDTO userForRegister)
        {
            var result = await userAuthRepository.Register(userForRegister);
            if(!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}
