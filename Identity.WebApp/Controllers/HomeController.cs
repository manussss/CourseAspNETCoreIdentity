using System.Diagnostics;
using System.Security.Claims;
using Identity.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;

    public HomeController(
        ILogger<HomeController> logger,
        UserManager<User> userManager,
        IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(CreateUserModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user is null)
            {
                user = new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
            }

            return View("Success");
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

                await HttpContext.SignInAsync("Identity.Application", principal);

                return RedirectToAction("About");
            }

            ModelState.AddModelError("", "Usuário ou senha inválidos");
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpGet]
    [Authorize]
    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
