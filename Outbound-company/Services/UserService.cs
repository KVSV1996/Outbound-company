﻿using Outbound_company.Models;
using Outbound_company.Repository.Interface;
using Outbound_company.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Outbound_company.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetByIdAsync(int id) => await _userRepository.GetByIdAsync(id);
        public async Task<IEnumerable<User>> GetAllAsync() => await _userRepository.GetAllAsync();
        public async Task UpdateAsync(User user) => await _userRepository.UpdateAsync(user);
        public async Task DeleteAsync(int id) => await _userRepository.DeleteAsync(id);
        public async Task<User> GetByUserNameAsync(string userName) => await _userRepository.GetByUserNameAsync(userName);

        public async Task AddAsync(User user)
        {
            user.PasswordHash = HashPassword(user.PasswordHash); // Hash the password before saving
            await _userRepository.AddAsync(user);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
