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

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("JWT secret key is not configured!");
            }

            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new Exception("JWT issuer is not configured!");
            }

            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new Exception("JWT audience is not configured!");
            }

            if (string.IsNullOrWhiteSpace(expiry))
            {
                throw new Exception("JWT expiry is not configured!");
            }

            if (key.Length < 32)
            {
                throw new Exception("JWT secret key must be at least 32 characters!");
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                throw new Exception("Role is required!");
            }

            if (string.IsNullOrWhiteSpace(shopName))
            {
                throw new Exception("Shop name is required!");
            }

            if (!int.TryParse(expiry, out var expiryHours) || expiryHours <= 0)
            {
                throw new Exception("JWT expiry must be a positive number!");
            }

            var claims = new List<Claim>
            {
               new Claim("ShopId", shopId.ToString()),
               new Claim("UserId", userId.ToString()),
               new Claim(ClaimTypes.Role, role),
               new Claim("ShopName", shopName)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: GetPakistanTime().AddHours(expiryHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private DateTime GetPakistanTime()
        {
            TimeZoneInfo pakistanZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, pakistanZone);
        }
    }
}
