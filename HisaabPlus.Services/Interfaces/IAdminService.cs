using HisaabPlus.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<ShopDTO>> GetAllShopsAsync();
        Task<bool> LockShopAsync(int shopId);
        Task<bool> UnlockShopAsync(int shopId);
        Task<bool> AddSubscriptionAsync(int shopId, int months);
    }
}
