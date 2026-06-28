using HisaabPlus.Data.Context;
using HisaabPlus.Services.DTOs;
using HisaabPlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;
using HisaabPlus.Data.Entities;

namespace HisaabPlus.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly AppDbContext _db;
        public AuthService(IJwtService jwtService, AppDbContext db)
        {
            _jwtService = jwtService;
            _db = db;
        }
        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var phoneExists = await _db.Shops.AnyAsync(p => p.PhoneNumber == registerDTO.PhoneNumber);
            if(phoneExists)
            {
                throw new Exception("Phone Number Already Registered!");
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
            Shop shop = new Shop
            {
                ShopName = registerDTO.ShopName,
                OwnerName = registerDTO.OwnerName,
                PhoneNumber = registerDTO.PhoneNumber,
                Address = registerDTO.Address,
                Email = registerDTO.Email,
                PasswordHash = passwordHash,
                IsActive = true,
                SubscriptionStartDate = DateTime.UtcNow,
                SubscriptionEndDate = DateTime.UtcNow.AddDays(14),
                CreatedAt = DateTime.UtcNow
            };
            _db.Shops.Add(shop);
            await _db.SaveChangesAsync();
            User user = new User
            {
                ShopId = shop.ShopId,
                FullName = registerDTO.OwnerName,
                PhoneNumber = registerDTO.PhoneNumber,
                PasswordHash = passwordHash,
                Role = "Owner",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            var generatedToken = _jwtService.GenerateToken(shop.ShopId,user.UserId,user.Role,shop.ShopName);
            return new AuthResponseDTO
            {
                Token = generatedToken,
                ShopId = shop.ShopId,
                ShopName = shop.ShopName,
                OwnerName = shop.OwnerName,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };
        }
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var shopExists = await _db.Shops.FirstOrDefaultAsync(s => s.PhoneNumber == loginDTO.PhoneNumber);
            if(shopExists == null)
            {
                throw new Exception("Shop Not Found!");
            }
            var veriftPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, shopExists.PasswordHash);
            if(veriftPassword == false)
            {
                throw new Exception("Invalid Password!");
            }
            if(shopExists.IsActive == false)
            {
                throw new Exception("Your account is locked. Please contact to support!");
            }
            var userExists = await _db.Users.FirstOrDefaultAsync(u => u.ShopId == shopExists.ShopId && u.Role == "Owner");
            var generatedToken = _jwtService.GenerateToken(shopExists.ShopId, userExists.UserId, userExists.Role, shopExists.ShopName);
            return new AuthResponseDTO
            {
                Token = generatedToken,
                ShopId = shopExists.ShopId,
                ShopName = shopExists.ShopName,
                OwnerName = shopExists.OwnerName,
                Role = userExists.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };
        }
    }
}
