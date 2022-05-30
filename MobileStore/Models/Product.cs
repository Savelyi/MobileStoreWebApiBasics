using System.Collections.Generic;

namespace MobileStore.Models
{
    public class Product
    {
        public Product()
        {
            Orders=new HashSet<Order>();
        }
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int PriceUSD { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
