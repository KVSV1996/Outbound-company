using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Outbound_company.Context;
using Outbound_company.Models;

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
                    new CallStatus { Status = "Problem when sending HTTP" }
                );
            }
            context.SaveChanges();
        }

    }
}
