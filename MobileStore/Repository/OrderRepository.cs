using MobileStore.Contracts;
using MobileStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStore.Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(MobileStoreDbContext context) : base(context)
        {
        }

        public void CreateOrder(Order order)
        {
            Create(order);
        }

        public void DeleteOrder(Order order)
        {
            Delete(order);
        }

        public IQueryable<Order> GetOrders(bool trackChanges)
        {
            return FindAll(trackChanges);
        }

        public Order GetOrder(int Id, bool trackChanges)
        {
            return FindByCondition(or=>or.Id==Id, trackChanges).SingleOrDefault();
        }

        public IQueryable<Order> GetUserOrders(string Id, bool trackChanges)
        {
            return FindByCondition(or => or.UserId == Id, trackChanges);
        }
    }
}
