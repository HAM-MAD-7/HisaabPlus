using HisaabPlus.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HisaabPlus.Api.Middleware
{
    public class SubscriptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        public SubscriptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            if(path.ToLower().Contains("/api/auth") || path.ToLower().Contains("/swagger"))
            {
                await _requestDelegate(context);
                return;
            }
            var shopIdClaims = context.User.Claims.FirstOrDefault(c => c.Type == "ShopId");
            if(shopIdClaims == null)
            {
                await _requestDelegate(context);
                return;
            }
            var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if(role == "Admin")
            {
                await _requestDelegate(context);
                return;
            }
            var shopId = int.Parse(shopIdClaims.Value);
            var db = context.RequestServices.GetRequiredService<AppDbContext>();
            var isValidBool = await db.Shops.AnyAsync(s => s.ShopId == shopId && s.IsActive == true && s.SubscriptionEndDate > DateTime.UtcNow);
            if(isValidBool == false)
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\":\"Subscription expired. Please COntact Support.\"}");
                return;
            }
            await _requestDelegate(context);
            return;
        }
    }
}
