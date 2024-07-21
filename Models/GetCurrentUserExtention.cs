using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace firstMVC.Models
{
    public static class GetCurrentUserExtention
    {
        public static async Task<Customer> GetCurrentUserAsync(this Controller controller, UserManager<Customer> userManager)
        {
            var currentUserId = controller.User.Claims.First(x=>x.Type == ClaimTypes.NameIdentifier).Value;
            return await userManager.Users.FirstAsync(x=>x.Id.ToString() == currentUserId);
        }
    }
}
