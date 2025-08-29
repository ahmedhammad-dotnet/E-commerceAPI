using E_commerceAPI.Models;
using E_commerceAPI.Repository.IRepository;
using E_commerceAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = SD.adminRole)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepositery;
        public CategoryController(ICategoryRepository categoryRepositery)
        {
            this.categoryRepositery = categoryRepositery;
        }
        [HttpGet]
        [Route("Index")]
        public IActionResult Index(int page = 1, string? search = null)
        {
            if (page <= 0)
                page = 1;
            IQueryable<Category> cts = categoryRepositery.GetAll();
            if (search != null && search.Length > 0)
            {
                search = search.TrimStart();
                search = search.TrimEnd();
                cts = cts.Where(e => e.Name.Contains(search));
            }
            cts = cts.Skip((page - 1) * 5).Take(5);
            if (cts.Any())
                return Ok(cts.ToList());

            return NotFound();
        }
        [HttpPost("Create")]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepositery.Add(category);
                categoryRepositery.Save();
                return Created($"{Request.Scheme}://{Request.Host}/api/Category/Details?categoryId={category.Id}", category);
            }
            return BadRequest(category);
        }
        [HttpGet("Details")]
        public IActionResult Details(int categoryId)
        {

            var cts = categoryRepositery.GetOne([], e => e.Id == categoryId);
            if (cts != null)
            {
                return Ok(cts);
            }
            return NotFound();
        }
        [HttpPut("Edit")]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepositery.Edit(category);
                categoryRepositery.Save();
                return Created($"{Request.Scheme}://{Request.Host}/api/Category/Details?categoryId={category.Id}", category);
            }
            return BadRequest(category);
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(int categoryId)
        {
            var category = categoryRepositery.GetOne([], e => e.Id == categoryId);
            if (category != null)
            {
                categoryRepositery.Delete(category);
                categoryRepositery.Save();
                return Ok();
            }
            return NotFound();
        }
    }
}
