using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileStore.Contracts;
using MobileStore.DTO;
using MobileStore.DTO.InfoModelsToShow;
using MobileStore.DTO.ModelsToShow;
using MobileStore.Models;
using MobileStore.Repository;
using MobileStore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MobileStore.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        readonly UserManager<User> userManager;
        readonly IRepositoryManager _repositoryManager;
        public HomeController(UserManager<User> manager, IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
            userManager = manager;
        }

        [AllowAnonymous]
        [HttpGet("Products/{orderby?}")]
        public IActionResult ShowProducts([FromQuery] ProductsShowSortOrder orderby)
        {

            var products=from prs in (_repositoryManager.Products.GetProducts(false))
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


        [HttpGet("MyAccount/About")]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var userInfo = new UserInfoToShowDto()
            {
                Name = user.UserName,
                UserRoles = await userManager.GetRolesAsync(user)

            };
            return Json(userInfo);
        }


        [HttpGet("MyAccount/Orders")]
        public async Task<IActionResult> GetUserOrders()
        {

            var orders = _repositoryManager.Orders.GetUserOrders((await userManager.FindByNameAsync(User.Identity.Name)).Id, true);
            List<UserOrdersToShowDto> ordersToShow = new List<UserOrdersToShowDto>();
            foreach (var order in orders)
            {
                var userOrder = new UserOrdersToShowDto()
                {
                    ProductName = order.Product.Name,
                    Price = order.Product.PriceUSD
                };
                ordersToShow.Add(userOrder);
            }
            return Json(ordersToShow);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("Accounts/{userId}")]
        public async Task<IActionResult> GetAnyUserInfo([FromRoute] string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            UserInfoToShowAdminDto userInfo = new UserInfoToShowAdminDto(user)
            {
                Name = user.UserName,
                UserRoles = await userManager.GetRolesAsync(user)
            };
            return user == null ? NotFound("User With This Id Does not exist") : Json(userInfo);
        }


    }
}
