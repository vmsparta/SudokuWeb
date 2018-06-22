using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SudokuWeb.Data;
using SudokuWeb.ViewModels;
using SudokuWeb.Models;
using SudokuWeb.Interfaces;
using Microsoft.Extensions.Logging;

namespace SudokuWeb.Controllers
{
    /// <summary>
    /// Контроллер аутентификации
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        /// <summary>
        /// Страница авторизации
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="model">Модель авторизации</param>
        /// <returns>Результат авторизации</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (_accountService.CheckUser(model.Name, model.Password) != null)
                {
                    await Authenticate(model.Name);
                    _logger.LogInformation(string.Format("Пользователь {0} вошёл в систему.", model.Name));
                    return RedirectToAction("Index", "Game");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }


        /// <summary>
        /// Страница регистрации
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model">Модель регистрации</param>
        /// <returns>Результат регистрации</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (_accountService.GetUser(model.Name) == null)
                {
                    _accountService.RegisterUser(model.Name, model.Password);
                    _logger.LogInformation(string.Format("Пользователь {0} зарегистрирован.", model.Name));
                    await Authenticate(model.Name);
                    _logger.LogInformation(string.Format("Пользователь {0} вошёл в систему.", model.Name));
                    return RedirectToAction("Index", "Game");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns></returns>
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        /// <summary>
        /// Закрытие сессии
        /// </summary>
        /// <returns>Перенаправление на страницу авторизации</returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation(string.Format("Пользователь {0} вышел из системы.", User.Identity.Name));
            return RedirectToAction("Login", "Account");
        }
    }
}