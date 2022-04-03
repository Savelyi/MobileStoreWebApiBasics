using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileStore.Models
{
    public class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public virtual ICollection<Order> Orders { get; set; }



    }
}
