using Microsoft.EntityFrameworkCore;
using Outbound_company.Context;
using Outbound_company.Models;
using Outbound_company.Repository.Interface;

namespace Outbound_company.Repository
{
    public class NumberRepository : INumberRepository
    {
        private IApplicationDbContext context;
        public NumberRepository(IApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }

        public async Task<IEnumerable<NumberPool>> GetAllNumberPoolsAsync()
        {
            if (context.OutboundCompanies == null)
            {
                throw new NullReferenceException();
            }

            return await context.NumberPools.Include(p => p.PhoneNumbers).ToListAsync();
        }

        public async Task<NumberPool> GetByIdAsync(int id)
        {
            if (context.NumberPools == null)
            {
                throw new NullReferenceException();
            }

            return await context.NumberPools
                .Include(p => p.PhoneNumbers)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PhoneNumber>> GetPhoneNumbersStartingFromAsync(int numberPoolId, int startingPhoneId)
        {
            var startingPhoneNumber = await context.PhoneNumbers
                .Where(pn => pn.NumberPoolId == numberPoolId && pn.Id >= startingPhoneId)
                .FirstOrDefaultAsync();

            if (startingPhoneNumber == null)
            {
                return new List<PhoneNumber>();
            }

            return await context.PhoneNumbers
                .Where(pn => pn.NumberPoolId == numberPoolId && pn.Id >= startingPhoneNumber.Id)
                .ToListAsync();
        }

        public async Task InsertNumberPoolsAsync(NumberPool numberPool)
        {
            if (numberPool == null)
            {
                throw new NullReferenceException();
            }

            await context.NumberPools.AddAsync(numberPool);
        }

        public async Task DeleteNumberPoolsAsync(int id)
        {
            NumberPool numberPool = await context.NumberPools.FindAsync(id);

            if (numberPool == null)
            {
                throw new NullReferenceException();
            }
            context.NumberPools.Remove(numberPool);
        }

        public async Task UpdateNumberPoolsAsync(NumberPool numberPool)
        {
            if (numberPool == null)
            {
                throw new NullReferenceException();
            }
            context.NumberPools.Update(numberPool);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
