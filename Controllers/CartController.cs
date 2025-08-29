using AutoMapper;
using E_commerceAPI.DTOs;
using E_commerceAPI.Models;
using E_commerceAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_commerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepositery;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public CartController(ICartRepository cartRepositery, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.cartRepositery = cartRepositery;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        [HttpPost("AddToCart")]
        public IActionResult AddToCart(CartDTO cartDTO)
        {
            cartDTO.ApplicationUserId = userManager.GetUserId(User);
            if (cartDTO.ApplicationUserId != null)
            {
                var cart = mapper.Map<Cart>(cartDTO);
                var product = cartRepositery.GetOne(expression: e => e.ApplicationUserId == cartDTO.ApplicationUserId && e.ProductId == cart.ProductId);
                if (product != null) 
                
                    product.Count += cartDTO.Count;
                
                else 
                
                    cartRepositery.Add(cart);
                
                
                cartRepositery.Save();
                return Ok(cart);
            }
            return NotFound(); 
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {

            var user = userManager.GetUserId(User);

            if (user != null) 
            {
                var carts = cartRepositery.GetAll(expression: e => e.ApplicationUserId == user);

                if(carts != null) 
                {
                return Ok(carts);
                }

                return NotFound();
            }

            return NotFound();

           
        }
        [HttpPut("Increment")]

        public IActionResult Increment(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepositery.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.Count++;
                cartRepositery.Save();

                return Ok(product);
            }

            return NotFound();
        }
        [HttpPut("Decrement")]

        public IActionResult Decrement(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepositery.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.Count--;
                if (product.Count > 0)

                    cartRepositery.Save();

                else
                    product.Count = 1;

                return Ok(product);
            }

            return NotFound();
        }
        [HttpPut("Delete")]
        public IActionResult Delete(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepositery.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                cartRepositery.Delete(product);
                cartRepositery.Save();

                return Ok(product);
            }

            return NotFound();
        }
        [HttpPost("Pay")]
        public IActionResult Pay()
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var Product = cartRepositery.GetAll([e=>e.Product],expression: e=>e.ApplicationUserId == ApplicationUserId);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in Product)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                        UnitAmount = (long)item.Product.Price * 100,
                    },
                    Quantity = item.Count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Ok(session.Url);
        }
    }
}
