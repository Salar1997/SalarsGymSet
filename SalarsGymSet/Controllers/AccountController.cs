using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SalarsGymSet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SalarsGymSet.Controllers
{
    public class AccountController : Controller
    {
        Helper _helper = new Helper();
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string submit)
        {
            Account account = new Account()
            {
                UserName = username,
                Password = password,
            };
            bool accountExists = _helper.IfAccountExists(account);
            if (accountExists && submit == "Login")
            {
                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, account.UserName)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                return RedirectToAction("Index", "Home");
            }
            else if (!accountExists && submit == "Login")
            {
                TempData["textmsg"] = "<script>alert('You have to Register, this user doesnt exists.');</script>";
                return RedirectToAction("Login", "Account");
            }
            else if (!accountExists && submit == "Register")
            {
                _helper.CreateAccount(account);

                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, account.UserName)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                return RedirectToAction("Index", "Home");
            }
            else if (accountExists && submit == "Register")
            {
                TempData["textmsg"] = "<script>alert('this email is already registered.');</script>";
                return RedirectToAction("Login", "Account");
            }

            return Redirect("/Account/Login");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
