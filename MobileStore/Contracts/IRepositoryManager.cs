
using Microsoft.AspNetCore.Identity;
using MobileStore.Models;

namespace MobileStore.Contracts
{
    public interface IRepositoryManager
    {
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        //UserManager<User> Users { get; }
        void Save();
    }
}
