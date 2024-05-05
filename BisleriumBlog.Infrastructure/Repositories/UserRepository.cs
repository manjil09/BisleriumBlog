using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Infrastructure.Data;
using BisleriumBlog.Infrastructure.Mapper;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private IdentityUser? _user;

        public UserRepository(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<Response<string>> Register(UserRegisterDTO userForRegister, UserRole role = UserRole.User)
        {
            var existingUser = await _userManager.FindByNameAsync(userForRegister.UserName);
            if (existingUser != null)
                return new Response<string>() { IsSuccess = false, Message = "User name already exists!" };

            IdentityUser user = new()
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

        //public async Task<UserLoginDTO> UpdateUser(string id, UserRegisterDTO updatedUser)
        //{
        //    var commentForUpdate = await _appDbContext.Comments.Where(x => x.Id == commentId && !x.IsDeleted).SingleOrDefaultAsync();
        //    if (commentForUpdate != null)
        //    {
        //        await AddToCommentHistory(commentForUpdate);

        //        commentForUpdate.Body = updatedUser.Body;
        //        commentForUpdate.UpdatedAt = DateTime.Now;

        //        await _appDbContext.SaveChangesAsync();

        //        return MapperlyMapper.CommentToCommentResponseDTO(commentForUpdate);
        //    }

        //    throw new KeyNotFoundException($"Could not find Comment with the id {commentId}");
        //}

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
