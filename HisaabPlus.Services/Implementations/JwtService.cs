using HisaabPlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HisaabPlus.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(int shopId, int userId, string role, string shopName)
        {
            var key = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiry = _configuration["JwtSettings:ExpiryHours"];
            var claims = new List<Claim>
            {
                new Claim("ShopId", shopId.ToString()),
                new Claim("UserId", userId.ToString()),
                new Claim("Role", role),
                new Claim("ShopName", shopName)
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(expiry)),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}
