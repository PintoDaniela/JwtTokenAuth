using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtTokenTest2.Data;
using JwtTokenTest2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace JwtTokenTest2.Helper
{
    public static class JwtTokenManager
    {                
        public static string GenerateJwtToken(User user, IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                // Guardar el ID del usuario en el claim para poder recuperarlo en otras operaciones
                new Claim("UserId", user.Id.ToString())
            };

            int TokenExpiration = Convert.ToInt32(configuration["JwtSettings:TokenExpirationMinutes"]);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(TokenExpiration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
      
        /*
        public static string ValidRefreshToken(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return "";
            }

            // Decodificar y validar el refresh token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"]
            };

            try
            {
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out securityToken);                               

                
                return refreshToken;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        */

        /*
        public static User GetUserFromRefreshToken(string refreshJwt, IConfiguration configuration, AppDbContext dbContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"]
            };

            try
            {
                SecurityToken securityToken;
                var claimsPrincipal = tokenHandler.ValidateToken(refreshJwt, validationParameters, out securityToken);

                // Recuperar el ID del usuario del claim
                var userIdClaim = claimsPrincipal.FindFirst("UserId");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    // Buscar al usuario en tu base de datos o almacén de usuarios
                    var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);

                    return user;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }
        */

    }
}
