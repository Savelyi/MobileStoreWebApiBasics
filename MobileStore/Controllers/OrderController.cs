using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileStore.Models;
using System.Linq;
using System.Threading.Tasks;
using MobileStore.DTO.InfoModelsToShow;

namespace MobileStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        MobileStoreDbContext db;
        UserManager<User> userManager;
        public OrderController(MobileStoreDbContext context, UserManager<User> manager)
        {
            userManager = manager;
            db = context;
        }
        [HttpPost("/Details/{productId?}")]
        public async Task<IActionResult> Order([FromQuery] int productId)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound("No Product with this Id");
            }
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var order = new Order()
            {
                ProductId = product.Id,
                UserId = user.Id
            };
            db.Orders.Add(order);
            await db.SaveChangesAsync();
            var response = new
            {
                Price = product.PriceUSD,
                ProductName = product.Name,
                OrderId = order.Id
            };
            return Ok(response);
        }

    }
}
