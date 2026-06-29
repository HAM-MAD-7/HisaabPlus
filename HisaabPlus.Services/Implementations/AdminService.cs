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
                DaysRemaining = (s.SubscriptionEndDate - DateTime.UtcNow).Days
            }).ToList();
            return shopDTOs;
        }
        public async Task<bool> LockShopAsync(int shopId)
        {
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.ShopId == shopId);
            if(shop == null)
            {
                return false;
            }
            shop.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UnlockShopAsync(int shopId)
        {
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.ShopId == shopId);
            if (shop == null)
            {
                return false;
            }
            shop.IsActive = true;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddSubscriptionAsync(int shopId, int months)
        {
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.ShopId == shopId);
            if (shop == null)
            {
                return false;
            }
            if(shop.SubscriptionEndDate < DateTime.UtcNow)
            {
                shop.SubscriptionStartDate = DateTime.UtcNow;
                shop.SubscriptionEndDate = DateTime.UtcNow.AddMonths(months);
            }
            else
            {
                shop.SubscriptionEndDate = shop.SubscriptionEndDate.AddMonths(months);
            }
            shop.IsActive = true;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
