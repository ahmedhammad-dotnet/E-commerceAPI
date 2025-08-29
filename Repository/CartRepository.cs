using E_Commerce.Repository;
using E_commerceAPI.Data;
using E_commerceAPI.Models;
using E_commerceAPI.Repository.IRepository;

namespace E_commerceAPI.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbcontext dbcontext;
        public CartRepository(ApplicationDbcontext dbcontext) : base(dbcontext)
        {
            this.dbcontext = dbcontext;
        }
    }
}
