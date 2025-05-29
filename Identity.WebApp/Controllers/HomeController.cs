using System.Diagnostics;
using Identity.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
    private readonly SignInManager<User> _signInManager;

    public HomeController(
        UserManager<User> userManager,
        IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _signInManager = signInManager;
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
            // var user = await _userManager.FindByNameAsync(model.UserName);

            // if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password))
            // {
            //     var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            //     await HttpContext.SignInAsync("Identity.Application", principal);

            //     return RedirectToAction("About");
            // }

            var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (signInResult.Succeeded)
                return RedirectToAction("About");

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
