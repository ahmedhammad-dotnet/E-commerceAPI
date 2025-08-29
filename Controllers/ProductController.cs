using AutoMapper;
using E_commerceAPI.DTOs;
using E_commerceAPI.Models;
using E_commerceAPI.Repository;
using E_commerceAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        [HttpGet]

        public IActionResult Index(int page = 1, string? search = null)
        {
            if (page <= 0)
                page = 1;
            var products = productRepository.GetAll([e => e.Category]);
            if (search != null)
            {
                products = products.Where(e => e.Name.Contains(search));
            }

            products = products.Skip((page - 1) * 5).Take(5);

            if (products.Any())
            {
                List<ProductDTO> productDTOs = new List<ProductDTO>();
                foreach (var item in products)
                {
                    productDTOs.Add(mapper.Map<ProductDTO>(item));
                }
                return Ok(productDTOs);
            }
            return NoContent();
        }

        [HttpPost("Create")]
        public IActionResult Create(ProductImagesDTO productImagesDTO) 
        {

            if (ModelState.IsValid)
            {
                if (productImagesDTO != null && productImagesDTO.ImgUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productImagesDTO.ImgUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);
                 

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        productImagesDTO.ImgUrl.CopyTo(stream);
                    }


                }

                var product = new Product()
                {
                    Name = productImagesDTO.Name,
                    Price = productImagesDTO.Price,
                    Quantity = productImagesDTO.Quantity,
                    Rate = productImagesDTO.Rate,
                    Description = productImagesDTO.Description,
                    ImgUrl = productImagesDTO.ImgUrl.FileName,
                    CategoryId = productImagesDTO.CategoryId,
                };
                productRepository.Add(product);
                productRepository.Save();
               
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append("seccess", "add product", cookieOptions);

                return Ok(product);
            }
          return BadRequest(productImagesDTO);
        }

        [HttpPut("Edit")]
        public IActionResult Edit(ProductImagesDTO productImagesDTO)
        {
            var oldproduct = productRepository.GetOne(expression: e => e.Id == productImagesDTO.Id);

            if (ModelState.IsValid)
            {
                if (productImagesDTO.ImgUrl != null && productImagesDTO.ImgUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productImagesDTO.ImgUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);
                    var oldfilePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", oldproduct.ImgUrl);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        productImagesDTO.ImgUrl.CopyTo(stream);
                    }

                    if (System.IO.File.Exists(oldfilePath))
                    {
                        System.IO.File.Delete(oldfilePath);
                    }

                    oldproduct.ImgUrl = fileName;
                }
                else
                {
                    oldproduct.ImgUrl = oldproduct.ImgUrl;
                }
                productRepository.Edit(oldproduct);
                productRepository.Save();

                return Ok(oldproduct);
            }
            return BadRequest(productImagesDTO);
          
            
            
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(int Id)
        {
            var product = productRepository.GetOne(expression: e => e.Id == Id);

            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", product.ImgUrl);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            productRepository.Delete(product);
            productRepository.Save();

            return Ok(product);
        }
    }
}
