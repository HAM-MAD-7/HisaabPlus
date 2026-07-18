using HisaabPlus.Data.Context;
using HisaabPlus.Services.DTOs;
using HisaabPlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;
using HisaabPlus.Data.Entities;
using Microsoft.Extensions.Configuration;

namespace HisaabPlus.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        public AuthService(IJwtService jwtService, AppDbContext db, IConfiguration configuration)
        {
            _jwtService = jwtService;
            _db = db;
            _configuration = configuration;
        }
        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            if (string.IsNullOrWhiteSpace(registerDTO.ShopName))
            {
                throw new Exception("Shop name is required!");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.OwnerName))
            {
                throw new Exception("Owner name is required!");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.PhoneNumber))
            {
                throw new Exception("Phone number is required!");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.Address))
            {
                throw new Exception("Address is required!");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.Email))
            {
                throw new Exception("Email is required!");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.Password))
            {
                throw new Exception("Password is required!");
            }

            var phoneExistsInShops = await _db.Shops.AnyAsync(s => s.PhoneNumber == registerDTO.PhoneNumber);
            var phoneExistsInCustomers = await _db.Customers.AnyAsync(c => c.PhoneNumber == registerDTO.PhoneNumber);
            var phoneExistsInSuppliers = await _db.Suppliers.AnyAsync(s => s.PhoneNumber == registerDTO.PhoneNumber);

            if (phoneExistsInShops || phoneExistsInCustomers || phoneExistsInSuppliers)
            {
                throw new Exception("This phone number is already registered in the system!");
            }

            if (!registerDTO.PhoneNumber.All(char.IsDigit) || registerDTO.PhoneNumber.Length != 11)
            {
                throw new Exception("Phone number must be 11 digits!");
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(registerDTO.Email);
                if (addr.Address != registerDTO.Email)
                {
                    throw new Exception("Invalid email format!");
                }
            }
            catch
            {
                throw new Exception("Invalid email format!");
            }

            if (registerDTO.Password.Length < 6)
            {
                throw new Exception("Password must be at least 6 characters!");
            }

            var phoneExists = await _db.Shops.AnyAsync(p => p.PhoneNumber == registerDTO.PhoneNumber);
            if (phoneExists)
            {
                throw new Exception("Phone number already registered!");
            }

            var emailExists = await _db.Shops.AnyAsync(s => s.Email == registerDTO.Email);
            if (emailExists)
            {
                throw new Exception("Email already registered!");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
            Shop shop = new Shop
            {
                ShopName = registerDTO.ShopName.Trim(),
                OwnerName = registerDTO.OwnerName.Trim(),
                PhoneNumber = registerDTO.PhoneNumber.Trim(),
                Address = registerDTO.Address.Trim(),
                Email = registerDTO.Email.Trim().ToLower(),
                PasswordHash = passwordHash,
                IsActive = true,
                SubscriptionStartDate = GetPakistanTime(),
                SubscriptionEndDate = GetPakistanTime().AddDays(14),
                CreatedAt = GetPakistanTime()
            };
            _db.Shops.Add(shop);
            await _db.SaveChangesAsync();
            User user = new User
            {
                ShopId = shop.ShopId,
                FullName = registerDTO.OwnerName.Trim(),
                PhoneNumber = registerDTO.PhoneNumber.Trim(),
                PasswordHash = passwordHash,
                Role = "Owner",
                IsActive = true,
                CreatedAt = GetPakistanTime(),
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            var generatedToken = _jwtService.GenerateToken(shop.ShopId, user.UserId, user.Role, shop.ShopName);
            return new AuthResponseDTO
            {
                Token = generatedToken,
                ShopId = shop.ShopId,
                ShopName = shop.ShopName,
                OwnerName = shop.OwnerName,
                Role = user.Role,
                ExpiresAt = GetPakistanTime().AddHours(8)
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            if (string.IsNullOrWhiteSpace(loginDTO.PhoneNumber))
            {
                throw new Exception("Phone number is required!");
            }

            if (string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                throw new Exception("Password is required!");
            }

            var shopExists = await _db.Shops.FirstOrDefaultAsync(s => s.PhoneNumber == loginDTO.PhoneNumber);
            if (shopExists == null)
            {
                throw new Exception("Invalid phone or password!");
            }

            var verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, shopExists.PasswordHash);
            if (verifyPassword == false)
            {
                throw new Exception("Invalid phone or password!");
            }

            if (shopExists.IsActive == false)
            {
                throw new Exception("Your account is locked. Please contact support!");
            }

            if (shopExists.SubscriptionEndDate < GetPakistanTime())
            {
                throw new Exception("Your subscription has expired. Please renew!");
            }

            var userExists = await _db.Users.FirstOrDefaultAsync(u => u.ShopId == shopExists.ShopId && u.Role == "Owner");
            if (userExists == null)
            {
                throw new Exception("User not found!");
            }

            var generatedToken = _jwtService.GenerateToken(shopExists.ShopId, userExists.UserId, userExists.Role, shopExists.ShopName);
            return new AuthResponseDTO
            {
                Token = generatedToken,
                ShopId = shopExists.ShopId,
                ShopName = shopExists.ShopName,
                OwnerName = shopExists.OwnerName,
                Role = userExists.Role,
                ExpiresAt = GetPakistanTime().AddHours(8)
            };
        }

        public async Task<AuthResponseDTO> AdminLoginAsync(AdminLoginDTO adminLoginDTO)
        {
            if (string.IsNullOrWhiteSpace(adminLoginDTO.UserName))
            {
                throw new Exception("Username is required!");
            }

            if (string.IsNullOrWhiteSpace(adminLoginDTO.Password))
            {
                throw new Exception("Password is required!");
            }

            var username = _configuration["AdminSettings:Username"];
            var password = _configuration["AdminSettings:Password"];

            if (adminLoginDTO.UserName != username || adminLoginDTO.Password != password)
            {
                throw new Exception("Invalid admin credentials!");
            }

            var generatedToken = _jwtService.GenerateToken(0, 0, "Admin", "AdminPanel");
            return new AuthResponseDTO
            {
                Token = generatedToken,
                ShopId = 0,
                ShopName = "AdminPanel",
                OwnerName = username,
                Role = "Admin",
                ExpiresAt = GetPakistanTime().AddHours(8)
            };
        }
        private DateTime GetPakistanTime()
        {
            TimeZoneInfo pakistanZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, pakistanZone);
        }
    }
}
