using Outbound_company.Context;
using Outbound_company.Models;
using System.Security.Cryptography;
using System.Text;

namespace Outbound_company.SpeedData
{
    public class SpeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            if (!context.CallStatuses.Any())
            {
                context.CallStatuses.AddRange(
                    new CallStatus { Status = "Successful call" },
                    new CallStatus { Status = "Number in Blacklist" },
                    new CallStatus { Status = "Problem when sending HTTP" }
                );
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { UserName = "Admin", PasswordHash = HashPassword("Admin"), Role = 1 },
                    new User { UserName = "User", PasswordHash = HashPassword("User"), Role = 0 }
                );
            }

            context.SaveChanges();
        }

        private static string HashPassword(string password)
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
