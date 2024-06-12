using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Outbound_company.Models;
using Outbound_company.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Outbound_company.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user, string username, string password, int role)
        {
            var updateUser = await _userService.GetByIdAsync(id);
            if (id != user.Id || updateUser == null)
            {
                return NotFound();
            }

            try
            {
                // Обновляем логин, если он не пуст и отличается от текущего
                if (!string.IsNullOrEmpty(username) && username != updateUser.UserName)
                {
                    updateUser.UserName = username;
                }

                // Обновляем роль, если она отличается от текущей
                if (role != updateUser.Role)
                {
                    updateUser.Role = role;
                }

                // Обновляем пароль, если он не пустой
                if (!string.IsNullOrEmpty(password))
                {
                    using (var sha256 = SHA256.Create())
                    {
                        var bytes = Encoding.UTF8.GetBytes(password);
                        var hash = sha256.ComputeHash(bytes);
                        updateUser.PasswordHash = Convert.ToBase64String(hash);
                    }
                }

                await _userService.UpdateAsync(updateUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(updateUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserExists(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user != null;
        }
    }
}
