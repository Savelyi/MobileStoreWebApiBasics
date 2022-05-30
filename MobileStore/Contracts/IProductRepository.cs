using MobileStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStore.Contracts
{
    public interface IProductRepository
    {
        void CreateProduct(Product product);
        void DeleteProduct(Product product);
        IQueryable<Product> GetProducts(bool trackChanges);
        Product GetProduct(int Id, bool trackChanges);
    }
}
