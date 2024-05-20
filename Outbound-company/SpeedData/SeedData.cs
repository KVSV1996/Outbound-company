//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Outbound_company.Context;
//using Outbound_company.Models;

//namespace Outbound_company.SpeedData
//{
//    public class SeedData
//    {
//        public static void Initialize(ApplicationDbContext context)
//        {
//            // Check if there are any companies, if not, initialize the data
//            if (!context.OutboundCompanies.Any())
//            {
//                // Add initial companies with phone numbers
//                var companies = new[]
//                {
//                new OutboundCompany
//                {
//                    Name = "Company A",
//                    Channel = "PJSIP/6001",
//                    Extension = "1000",
//                    Context = "test4",
//                    CallerId = "1234",
//                    PhoneNumbers = new List<PhoneNumber>
//                    {
//                        new PhoneNumber { Number = "+1234567890" },
//                        new PhoneNumber { Number = "+1234567891" },
//                    }
//                },
//                new OutboundCompany
//                {
//                    Name = "Company B",
//                    Channel = "PJSIP/6002",
//                    Extension = "1001",
//                    Context = "test4",
//                    CallerId = "5678",
//                    PhoneNumbers = new List<PhoneNumber>
//                    {
//                        new PhoneNumber { Number = "+9876543210" },
//                        new PhoneNumber { Number = "+9876543211" },
//                    }
//                }
//            };

//                context.OutboundCompanies.AddRange(companies);
//                context.SaveChanges();
//            }
//        }
//    }
//}
