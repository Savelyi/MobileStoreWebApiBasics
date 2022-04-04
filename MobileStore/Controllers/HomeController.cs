using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileStore.DTO;
using MobileStore.DTO.InfoModelsToShow;
using MobileStore.DTO.ModelsToShow;
using MobileStore.Models;
using MobileStore.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MobileStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        MobileStoreDbContext db;
        public HomeController(MobileStoreDbContext context)
        {
            db = context;
        }

        [AllowAnonymous]
        [HttpGet("/Products/{orderby?}")]
        public IActionResult ShowProducts(ProductsShowSortOrder orderby)
        {


            var products =
               from prs in db.Products
               select new ProductsToShowDto
               {
                   Name = prs.Name,
                   PriceUSD = prs.PriceUSD
               };

            return Json(orderby switch
            {
                ProductsShowSortOrder.ByNameAsc => products.OrderBy(p => p.Name),
                ProductsShowSortOrder.ByNameDesc => products.OrderByDescending(p => p.Name),
                ProductsShowSortOrder.ByPriceAsc => products.OrderBy(p => p.PriceUSD),
                ProductsShowSortOrder.ByPriceDesc => products.OrderByDescending(p => p.PriceUSD),
                _ => products.OrderBy(p => p.Name),
            });
        }


        [HttpGet("/MyAccount/About")]
        public async Task<IActionResult> GetUserInfo()
        {

            User user = await db.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var userInfo = new UserInfoToShowDto()
            {
                Name = User.Identity.Name,
                Password = user.Password,
                UserRole = user.Role.Name

            };
            return Json(userInfo);
        }


        [HttpGet("/MyAccount/Orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            User user = await db.Users.Include(u => u.Orders)
                .ThenInclude(o => o.Product)
               .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            List<UserOrdersToShowDto> orders = new List<UserOrdersToShowDto>();
            foreach (var order in user.Orders)
            {
                var userOrder = new UserOrdersToShowDto()
                {
                    ProductName = order.Product.Name,
                    Price = order.Product.PriceUSD
                };
                orders.Add(userOrder);
            }
            return Json(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/Accounts/{userId:int}")]
        public async Task<IActionResult> GetAnyUserInfo(int userId)
        {
            var user = await db.Users.Include(u => u.Orders)
                .ThenInclude(o => o.Product)
                .Include(u=>u.Role)
               .FirstOrDefaultAsync(u => u.Id == userId);

            UserInfoToShowAdminDto userInfo = new UserInfoToShowAdminDto(user)
            {
                Name=user.UserName,
                Password=user.Password,
                UserRole=user.Role.Name,
            };
            

            return user == null ? NotFound("User With This Id Does not exist") : Json(userInfo);
        }


    }
}
