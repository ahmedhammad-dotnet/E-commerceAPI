using AutoMapper;
using E_commerceAPI.DTOs;
using E_commerceAPI.Models;
using E_commerceAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_commerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        [HttpPost("Register")]

        public async Task<IActionResult> Register(ApplicationUserDTOs userDTOs)
        {
            if (roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new(SD.adminRole));
                await roleManager.CreateAsync(new(SD.CustomerRole));
            }


            //ApplicationUser application = new()
            //{
            //    UserName = userDTOs.name,
            //    Address = userDTOs.Address,
            //    Email = userDTOs.email
            //};
            var user = mapper.Map<ApplicationUser>(userDTOs);

            var result = await userManager.CreateAsync(user, userDTOs.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                await userManager.AddToRoleAsync(user, SD.CustomerRole);

                return Ok(userDTOs);
            }
            return BadRequest(result.Errors);


        }
        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDTOs loginDTOs)
        {
            var user = await userManager.FindByNameAsync(loginDTOs.UserName);
            if (user != null)
            {
                var result = await userManager.CheckPasswordAsync(user, loginDTOs.Password);
                if (result)
                {
                    await signInManager.SignInAsync(user, false);
                    return Ok(user);
                }
                else ModelState.AddModelError("", "there are Errors");
            }
            return NotFound();
        }
        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}
