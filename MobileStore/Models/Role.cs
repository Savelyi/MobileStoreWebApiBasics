using System.Collections.Generic;

namespace MobileStore.Models
{
    public class Role
    {
        public Role()
        {
            Users=new HashSet<User>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users{ get; set; }
    }
}
