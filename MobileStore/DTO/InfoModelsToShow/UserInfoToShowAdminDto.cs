using MobileStore.DTO.InfoModelsToShow;
using MobileStore.Models;
using System.Collections.Generic;

namespace MobileStore.DTO
{
    public class UserInfoToShowAdminDto : UserInfoToShowDto
    {
        public UserInfoToShowAdminDto(User user)
        {
            UserOrders = GetOrdersId(user);
            
        }
       
        public IEnumerable<UserOrdersToShowExtension> UserOrders { get; set; }


        private List<UserOrdersToShowExtension> GetOrdersId(User user)
        {
            var orders = new List<UserOrdersToShowExtension>();
            foreach (var order in user.Orders)
            {
                var userOrder = new UserOrdersToShowExtension()
                {
                    ProductName = order.Product.Name,
                    Price = order.Product.PriceUSD,
                    OrderId = order.Id
                };
                orders.Add(userOrder);
            }
            return orders;
        }
        public class UserOrdersToShowExtension : UserOrdersToShowDto
        {
            public int OrderId { get; set; }
        }

    }
}
