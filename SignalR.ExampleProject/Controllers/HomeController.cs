using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SignalR.ExampleProject.Models;
using SignalR.ExampleProject.Models.ViewModels;
using SignalR.ExampleProject.Service;
using System.Diagnostics;

namespace SignalR.ExampleProject.Controllers
{
    public class HomeController(
        ILogger<HomeController> logger,
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager,
        AppDbContext context,
        FileService fileService) : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid) return View(model);  //Gelen model kontrol ediliyor

            var userToCreate = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(userToCreate, model.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(SignIn));
            }

        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid) return View(model);  //Gelen model kontrol ediliyor

            var hasUser = await userManager.FindByEmailAsync(model.Email);

            if(hasUser is null)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is Wrong");
            }

            var result = await signInManager.PasswordSignInAsync(hasUser, model.Password, true, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is Wrong");
            }

             return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> ProductList()
        {
            var user = await userManager.FindByEmailAsync("furkanayyildiz55@hotmail.com");

            if (context.Products.Any(x => x.UserId == user!.Id))
            {
                var products = context.Products.Where(x => x.UserId == user!.Id).ToList();

                return View(products);
            }

            var productList = new List<Product>()
            {
                new Product() { Name = "Pen 1", Description = "Description 1", Price = 100, UserId = user!.Id },
                new Product() { Name = "Pen 2", Description = "Description 2", Price = 200, UserId = user!.Id },
                new Product() { Name = "Pen 3", Description = "Description 3", Price = 300, UserId = user!.Id },
                new Product() { Name = "Pen 4", Description = "Description 4", Price = 400, UserId = user!.Id },
                new Product() { Name = "Pen 5", Description = "Description 5", Price = 500, UserId = user!.Id }
            };

            context.Products.AddRange(productList);
            await context.SaveChangesAsync();

            return View(productList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateExcel()
        {
            var response = new
            {
                Status = await fileService.AddMessageToQueue()
            };

            return Json(response);  
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
