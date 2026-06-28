using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int shopId, int userId, string role, string shopName);
    }
}
