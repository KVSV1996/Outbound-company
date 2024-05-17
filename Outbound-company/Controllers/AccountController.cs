using Microsoft.AspNetCore.Mvc;
using Outbound_company.Models;

namespace Outbound_company.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Простая проверка логина и пароля
                if (model.Username == "admin" && model.Password == "admin")
                {
                    // Аутентификация успешна
                    //Session["Username"] = model.Username;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Аутентификация неудачна
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // Если модель не прошла валидацию, отобразите форму входа с ошибками
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            //Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
