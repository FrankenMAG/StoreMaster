using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreMaster.Core.Entities;
using StoreMaster.Web.ViewModels;

namespace StoreMaster.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // ── LOGIN ────────────────────────────────────────────
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        // Si ya está autenticado, redirigir al home
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError("", "Cuenta bloqueada por demasiados intentos. Intenta en 5 minutos.");
            return View(model);
        }

        ModelState.AddModelError("", "Email o contraseña incorrectos.");
        return View(model);
    }

    // ── LOGOUT ───────────────────────────────────────────
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    // ── ACCESO DENEGADO ──────────────────────────────────
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}