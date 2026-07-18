using HisaabPlus.Data.Context;
using HisaabPlus.Services.DTOs;
using HisaabPlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _db;
        public AdminService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ShopDTO>> GetAllShopsAsync()
        {
            var shops = await _db.Shops.ToListAsync();
            var shopDTOs = shops.Select(s => new ShopDTO
            {
                ShopId = s.ShopId,
                ShopName = s.ShopName,
                OwnerName = s.OwnerName,
                PhoneNumber = s.PhoneNumber,
                IsActive = s.IsActive,
                SubscriptionEndDate = s.SubscriptionEndDate,
                DaysRemaining = (s.SubscriptionEndDate - GetPakistanTime()).Days
            }).ToList();
            return shopDTOs;
        }

        public async Task<bool> LockShopAsync(int shopId)
        {
            if (shopId <= 0)
            {
                throw new Exception("Invalid shop ID!");
            }

            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.ShopId == shopId);
            if (shop == null)
            {
                throw new Exception("Shop not found!");
            }

            if (shop.IsActive == false)
            {
                throw new Exception("Shop is already locked!");
            }

            shop.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnlockShopAsync(int shopId)
        {
            if (shopId <= 0)
            {
                throw new Exception("Invalid shop ID!");
            }

            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.ShopId == shopId);
            if (shop == null)
            {
                throw new Exception("Shop not found!");
            }

            if (shop.IsActive == true)
            {
                throw new Exception("Shop is already unlocked!");
            }

            shop.IsActive = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSubscriptionAsync(int shopId, int months)
        {
            if (shopId <= 0)
            {
                throw new Exception("Invalid shop ID!");
            }

            if (months <= 0)
            {
                throw new Exception("Months must be greater than 0!");
            }

            if (months > 60)
            {
                throw new Exception("Cannot add more than 60 months at once!");
            }

            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.ShopId == shopId);
            if (shop == null)
            {
                throw new Exception("Shop not found!");
            }

            if (shop.SubscriptionEndDate < GetPakistanTime())
            {
                shop.SubscriptionStartDate = GetPakistanTime();
                shop.SubscriptionEndDate = GetPakistanTime().AddMonths(months);
            }
            else
            {
                shop.SubscriptionEndDate = shop.SubscriptionEndDate.AddMonths(months);
            }

            shop.IsActive = true;
            await _db.SaveChangesAsync();
            return true;
        }
        private DateTime GetPakistanTime()
        {
            TimeZoneInfo pakistanZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, pakistanZone);
        }
    }
}
