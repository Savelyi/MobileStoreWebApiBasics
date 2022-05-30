using Microsoft.AspNetCore.Identity;
using MobileStore.Contracts;
using MobileStore.Models;

namespace MobileStore.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        IProductRepository productRepository;
        IOrderRepository orderRepository;
        MobileStoreDbContext context;
        public RepositoryManager(MobileStoreDbContext _context)
        {
            this.context = _context;
        }
        public IOrderRepository Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(context);
                return orderRepository;
            }
        }

        public IProductRepository Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(context);
                return productRepository;
            }
        }

        

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
