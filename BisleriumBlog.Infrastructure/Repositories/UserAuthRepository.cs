using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BisleriumBlog.Infrastructure.Repositories
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private IdentityUser? user;
        
        public UserAuthRepository(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<ResponseDTO> Register(UserRegisterDTO userForRegister, UserRole role = UserRole.User)
        {
            var existingUser = await userManager.FindByNameAsync(userForRegister.UserName);
            if (existingUser != null)
                return new ResponseDTO() { IsSuccess = false, Message = "User name already exists!" };

            IdentityUser user = new()
            {
                UserName = userForRegister.UserName,
                Email = userForRegister.UserEmail,
            };

            var result = await userManager.CreateAsync(user, userForRegister.Password);
            if (!result.Succeeded)
                return new ResponseDTO() { IsSuccess = false, Message = "Failed to register user! " + result.Errors.FirstOrDefault()?.Description ?? "Please enter the details again." };

            try
            {
                var addToRoleResult = await userManager.AddToRoleAsync(user, role.ToString());
            }
            catch (InvalidOperationException)
            {
                await userManager.DeleteAsync(user);
                return new ResponseDTO() { IsSuccess = false, Message = "Failed to assign a role to the user!" };
            }

            return new ResponseDTO() { IsSuccess = true, Message = "User registration successful!" };
        }

        public async Task<bool> ValidateUser(UserLoginDTO userForLogin)
        {
            user = await userManager.FindByNameAsync(userForLogin.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, userForLogin.Password))
                return true;
            return false;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = configuration.GetSection("JwtConfig");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["SecretKey"]!));

            return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = configuration.GetSection("JwtConfig");
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
