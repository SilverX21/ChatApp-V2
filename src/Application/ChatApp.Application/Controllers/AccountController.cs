using ChatApp.Domain.Models.Auth.Login;
using ChatApp.Infrastructure.Http.Auth;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace ChatApp.Application.Controllers;

public class AccountController : Controller
{
    private readonly IAuthHttpService _authHttpService;

    public AccountController()
    {
        _authHttpService = RestService.For<IAuthHttpService>("https://localhost:7076");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginInputModel model)
    {
        var response = await _authHttpService.Login(model);

        if (!response.IsSuccessStatusCode)
        {
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Register()
    {
        return View();
    }
}