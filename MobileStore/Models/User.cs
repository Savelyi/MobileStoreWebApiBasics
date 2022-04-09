using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileStore.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }
        
        public virtual ICollection<Order> Orders { get; set; }



    }
}
