using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Infrastructure.Repositories
{
    public class UserAuthRepository : IUserAuthRepository
    {
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
