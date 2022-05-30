using MobileStore.Contracts;
using MobileStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace MobileStore.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(MobileStoreDbContext context) : base(context)
        {
        }

        public void CreateProduct(Product product)
        {
            Create(product);
        }

        public void DeleteProduct(Product product)
        {
            Delete(product);
        }

        public Product GetProduct(int Id, bool trackChanges)
        {
            return FindByCondition(or => or.Id == Id, trackChanges).SingleOrDefault();
        }

        public IQueryable<Product> GetProducts(bool trackChanges)
        {
            return FindAll(trackChanges);
        }
    }
}
