using E_Commerce.Repository;
using E_commerceAPI.Data;
using E_commerceAPI.Models;
using E_commerceAPI.Repository.IRepository;

namespace E_commerceAPI.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbcontext dbcontext;
        public CategoryRepository(ApplicationDbcontext dbcontext) :base(dbcontext) 
        {
            this.dbcontext = dbcontext;
        }
    }
}
