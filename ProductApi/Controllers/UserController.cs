using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductApi.Models.DTO;
using ProductApi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly ApiTranslator translator;

        public UserController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, 
            RoleManager<IdentityRole> roleManager, 
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration  )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            translator = new ApiTranslator(httpClientFactory, configuration);
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO) {

            var user = new IdentityUser { UserName = userDTO.Email, Email = userDTO.Email };

            var result = await userManager.CreateAsync(user, userDTO.Password);
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (!await roleManager.RoleExistsAsync("Customer"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Customer"));
                }
                await userManager.AddToRoleAsync(user, "Customer");

                return NoContent();
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("Response", await translator.Translate(item.Description)); //traducir al español los mensajes de error con servicio Azure Translate
            }
            return StatusCode(400, ModelState);
        
        }
    }
}
