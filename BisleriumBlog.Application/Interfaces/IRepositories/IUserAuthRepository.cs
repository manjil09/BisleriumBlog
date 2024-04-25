﻿using BisleriumBlog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IUserAuthRepository
    {
        Task<ResponseDTO> Register(UserRegisterDTO userForRegister);
        Task<ResponseDTO> Login(UserLoginDTO userForLogin);
    }
}
