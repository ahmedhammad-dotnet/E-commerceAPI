using E_commerceAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public HomeController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [HttpGet("Index")]
        public IActionResult Index() 
        {
            var products = productRepository.GetAll();
            return Ok(products);
        }
        [HttpGet("Details")]
        public IActionResult Details(int id) 
        {
         var product = productRepository.GetOne(expression: e=>e.Id==id);

            if (product != null) 
            {
                return Ok(product);
            }
            return NotFound();
        }
    }
}
