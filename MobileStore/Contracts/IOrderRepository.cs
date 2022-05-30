using MobileStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStore.Contracts
{
    public interface IOrderRepository
    {
        void CreateOrder(Order product);
        void DeleteOrder(Order product);
        IQueryable<Order> GetOrders(bool trackChanges);
        Order GetOrder(int Id, bool trackChanges);
        IQueryable<Order> GetUserOrders(string Id, bool trackChanges);
    }
}
