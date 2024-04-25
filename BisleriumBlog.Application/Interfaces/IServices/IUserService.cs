using BisleriumBlog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<ResponseDTO> Register(UserRegisterDTO user);
        Task<ResponseDTO> Login(UserLoginDTO user);
    }
}
