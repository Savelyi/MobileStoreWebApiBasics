namespace MobileStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        
        public int ProductId { get; set; }
        
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
