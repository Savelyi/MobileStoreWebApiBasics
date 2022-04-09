using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileStore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        MobileStoreDbContext db;
        public OrderController(MobileStoreDbContext context)
        {
            db = context;
        }
        //[HttpPost("/Details/{productId:int}")]
        //public async Task<IActionResult> Order([FromRoute]int productId)
        //{
        //    var product = db.Products.FirstOrDefault(p => p.Id == productId);
        //    if (product == null)
        //    {
        //        return NotFound("No Product with this Id");
        //    }
        //    var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
           
        //    db.Orders.Add(new Order()
        //    {
        //        ProductId = product.Id,
        //        UserId = user.Id
        //    });
        //    await db.SaveChangesAsync();

        //    return Ok("Succeded");
        //}

    }
}
