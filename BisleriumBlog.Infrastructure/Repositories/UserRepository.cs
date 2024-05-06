﻿using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.UserDTO;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BisleriumBlog.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApplicationUser? _user;

        public UserRepository(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<Response<string>> Register(UserRegisterDTO userForRegister, UserRole role = UserRole.User)
        {
            var existingUser = await _userManager.FindByNameAsync(userForRegister.UserName);
            if (existingUser != null)
                return new Response<string>() { IsSuccess = false, Message = "User name already exists!" };

            ApplicationUser user = new()
            {
                UserName = userForRegister.UserName,
                Email = userForRegister.UserEmail,
            };

            var result = await _userManager.CreateAsync(user, userForRegister.Password);
            if (!result.Succeeded)
                return new Response<string>() { IsSuccess = false, Message = "Failed to register user! " + result.Errors.FirstOrDefault()?.Description ?? "Please enter the details again." };

            try
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, role.ToString());
            }
            catch (InvalidOperationException)
            {
                await _userManager.DeleteAsync(user);
                return new Response<string>() { IsSuccess = false, Message = "Failed to assign a role to the user!" };
            }

            return new Response<string>() { IsSuccess = true, Message = "User registration successful!" };
        }

        public async Task<bool> ValidateUser(UserLoginDTO userForLogin)
        {
            _user = await _userManager.FindByNameAsync(userForLogin.UserName);
            if (_user != null && await _userManager.CheckPasswordAsync(_user, userForLogin.Password))
                return true;
            return false;
        }

        public async Task<Response<string>> UpdateUser(string id, UserUpdateDTO updatedUser)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return new Response<string>() { IsSuccess = false, Message = "User not found!" };

            var existingUserName = await _userManager.FindByNameAsync(updatedUser.UserName);
            if (existingUserName != null && existingUserName.Id != user.Id)
                return new Response<string>() { IsSuccess = false, Message = "Username already exists!" };

            var existingUserEmail= await _userManager.FindByEmailAsync(updatedUser.UserEmail);
            if (existingUserEmail != null && existingUserEmail.Id != user.Id)
                return new Response<string>() { IsSuccess = false, Message = "Email address already exists!" };

            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.UserEmail;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return new Response<string>() { IsSuccess = false, Message = "Failed to update user details!" };

            return new Response<string>() { IsSuccess = true, Message = "User details updated successfully!" };
        }

        public async Task<Response<string>> ChangePassword(string userId, string currentPassword, string newPassword)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
                return new Response<string>() { IsSuccess = false, Message = "User not found!" };

            if (newPassword == currentPassword)
                return new Response<string>() { IsSuccess = false, Message = "New password cannot be the same as previous password!" };

            var result = await _userManager.CheckPasswordAsync(existingUser, currentPassword);
            if (!result)
                return new Response<string>() { IsSuccess = false, Message = "Incorrect current password!" };

            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var changePasswordResult = await _userManager.ResetPasswordAsync(existingUser, token, newPassword);
            if (!changePasswordResult.Succeeded)
                return new Response<string>() { IsSuccess = false, Message = "Failed to change password!" };

            return new Response<string>() { IsSuccess = true, Message = "Password changed successfully!" };
        }

        public async Task<Response<string>> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new Response<string>()
            {
                IsSuccess = true,
                Message = "User Login successful!",
                Result = new JwtSecurityTokenHandler().WriteToken(tokenOptions)
            };
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("JwtConfig");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["SecretKey"]!));

            return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim("username", _user.UserName),
                new Claim("id", _user.Id)
            };
            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(Convert.ToDouble(jwtSettings["ExpiresIn"])),
            signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
    }
}
