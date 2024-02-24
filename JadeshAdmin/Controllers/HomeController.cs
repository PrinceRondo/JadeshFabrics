using JadeshAdmin.Models;
using JadeshGeneral.Data;
using JadeshGeneral.Dtos.RequestDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JadeshAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl)
        {
            var model = new LoginDto
            {
                ReturnUrl = returnUrl
            };
            ViewBag.Message = null;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginDto viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(viewModel.Email);
                    if (user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user, viewModel.Password)))
                    {
                        ViewBag.Message = "Email not confirmed yet";
                        ViewBag.Code = 300;
                        return View(viewModel);
                    }
                    var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, true);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(viewModel.ReturnUrl) && Url.IsLocalUrl(viewModel.ReturnUrl))
                        {
                            return Redirect(viewModel.ReturnUrl);
                        }
                        else
                        {
                            if (await _userManager.IsInRoleAsync(user, "Admin"))
                                return RedirectToAction("AdminDashBoard", "Home");
                            else
                                return RedirectToAction("AdminDashBoard", "Home");
                        }
                    }
                    ViewBag.Message = "Login failed";
                    ViewBag.Code = 301;
                    return View();
                }
                ViewBag.Message = "Invalid entry";
                ViewBag.Code = 302;
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}