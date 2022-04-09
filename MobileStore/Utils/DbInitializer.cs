using Microsoft.AspNetCore.Identity;
using MobileStore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStore.Utils
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, MobileStoreDbContext db)
        {
            string adminUserName = "admin";
            string password = "12345";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminUserName) == null)
            {
                User admin = new User { UserName = adminUserName };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product()
                    {
                        Name = "Iphone",
                        PriceUSD = 100
                    },
                    new Product()
                    {
                        Name = "Samsung",
                        PriceUSD = 150
                    },
                    new Product()
                    {
                        Name = "Pixel",
                        PriceUSD = 200
                    });
            }
            await db.SaveChangesAsync();
        }
    }
}
