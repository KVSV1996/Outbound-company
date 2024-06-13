using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Outbound_company.Models;
using Outbound_company.Services.Interfaces;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace Outbound_company.Controllers
{
    [Authorize]
    [Authorize(Policy = "AdminOnly")]
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
                Log.Information($"User {user.UserName} created successfully.");
                return RedirectToAction(nameof(Index));
            }
            Log.Warning("Failed to create user. Model state is invalid.");
            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                Log.Warning($"User with ID {id} not found.");
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
                Log.Warning($"User with ID {id} not found or ID mismatch.");
                return NotFound();
            }

            try
            {
                if (!string.IsNullOrEmpty(username) && username != updateUser.UserName)
                {
                    updateUser.UserName = username;
                    Log.Information($"Username for user ID {id} updated to {username}.");
                }

                if (role != updateUser.Role)
                {
                    updateUser.Role = role;
                    Log.Information($"Role for user ID {id} updated to {role}.");
                }

                if (!string.IsNullOrEmpty(password))
                {
                    using (var sha256 = SHA256.Create())
                    {
                        var bytes = Encoding.UTF8.GetBytes(password);
                        var hash = sha256.ComputeHash(bytes);
                        updateUser.PasswordHash = Convert.ToBase64String(hash);
                    }
                    Log.Information($"Password for user ID {id} updated.");
                }

                await _userService.UpdateAsync(updateUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(updateUser.Id))
                {
                    Log.Warning($"User with ID {updateUser.Id} not found during update.");
                    return NotFound();
                }
                else
                {
                    Log.Error($"Concurrency exception occurred while updating user ID {updateUser.Id}.");
                    throw;
                }
            }
            Log.Information($"User ID {updateUser.Id} updated successfully.");
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                Log.Warning($"User with ID {id} not found.");
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteAsync(id);
            Log.Information($"User ID {id} deleted successfully.");
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserExists(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user != null;
        }
    }
}
