using DataModels.Auth;
using DataModels.Auth.View;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManger = userManager;
            _configuration = configuration;
        }

        public async Task<AuthViewModel> GetTokenAsync(ApplicationUser user)
        {
            var roles = await _userManger.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("userEmail", user.Email),
                new Claim("name", user.FullName)
            };

            foreach (var x in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, x));
            }

            JwtSecurityTokenHandler tokenHandeller = new();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],

                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandeller.CreateToken(descriptor);
            var tokenString = tokenHandeller.WriteToken(token);

            AuthViewModel returnModel = new()
            {
                FullName = user.FullName,
                Email = user.Email,
                CustomerId = user.CustomerId,
                Roles = roles.ToList(),
                Token = tokenString,
                TokenExpireTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
            };

            return returnModel;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
