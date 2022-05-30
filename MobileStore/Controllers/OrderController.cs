using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileStore.Models;
using System.Linq;
using System.Threading.Tasks;
using MobileStore.DTO.InfoModelsToShow;
using MobileStore.Repository;
using MobileStore.Contracts;

namespace MobileStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        UserManager<User> userManager;
        IRepositoryManager _repositoryManager;
        public OrderController(UserManager<User> manager, IRepositoryManager repositoryManager)
        {
            userManager = manager;
            _repositoryManager = repositoryManager;
        }

        [HttpPost("Details/{productId?}")]
        public async Task<IActionResult> Order([FromQuery] int productId)
        {

            var product = _repositoryManager.Products.GetProduct(productId, false);
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
            _repositoryManager.Orders.CreateOrder(order);
            _repositoryManager.Save();
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
