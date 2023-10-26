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
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)                
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


        /*//Test para renovar el token
        public static string ValidateRefreshTokenFromCookies(HttpContext httpContext, IConfiguration configuration)
        {
            var refreshToken = httpContext.Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                try
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
                    var tokenHandler = new JwtSecurityTokenHandler();

                    SecurityToken securityToken;
                    var claimsPrincipal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = securityKey,
                    }, out securityToken);

                    if (securityToken is JwtSecurityToken jwtSecurityToken && claimsPrincipal.Identity.IsAuthenticated)
                    {
                        return refreshToken;
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }
        */
        //Test para renovar el token
        public static bool ValidateToken(string token, IConfiguration configuration)
        {
            

            if (!string.IsNullOrEmpty(token))
            {
                try { 
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    //verificar validez del token, excepto fecha de expiración
                    //Mientras no tenga registro de logs lo hago por separado para ver en qué falla. Después se pondría esta validación en true y se borraría la verificación posterior de la fecha
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = true, 
                        ValidateAudience = true,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = securityKey,
                        ValidateLifetime = false
                    }, out SecurityToken validatedToken);

                    // Verificar la fecha de expiración
                    if (validatedToken is JwtSecurityToken jwtToken && jwtToken.ValidTo > DateTime.Now)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

    }
}
