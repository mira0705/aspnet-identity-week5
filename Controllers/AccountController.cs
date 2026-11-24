using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;
using StudentApp.ViewModels;

namespace StudentApp.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogWarning("Тіркелу қатесі: {Code} - {Description}", error.Code, error.Description);
                if (error.Code is "DuplicateUserName" or "DuplicateEmail")
                {
                    continue;
                }
                ModelState.AddModelError(string.Empty, error.Description);
            }

            if (result.Errors.Any(e => e.Code is "DuplicateUserName" or "DuplicateEmail"))
            {
                ModelState.AddModelError(nameof(model.Email), "Бұл email бұрыннан тіркелген");
            }

            return View(model);
        }

        // Рөлді тек сервер тағайындайды
        await _userManager.AddToRoleAsync(user, "Student");
        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction("Index", "Students");
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Students");
        }

        ModelState.AddModelError(string.Empty, "Email немесе құпиясөз қате");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}
