using E_Commerce.Repository;
using E_commerceAPI.Data;
using E_commerceAPI.Models;
using E_commerceAPI.Repository.IRepository;

namespace E_commerceAPI.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbcontext dbContext;

        public ProductRepository(ApplicationDbcontext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
