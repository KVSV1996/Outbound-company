using Microsoft.EntityFrameworkCore;
using Outbound_company.Context;
using Outbound_company.Models;

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

        public IEnumerable<NumberPool> GetAllNumberPools()
        {

            if (context.OutboundCompanies == null)
            {
                throw new NullReferenceException();
            }

            return context.NumberPools.Include(p => p.PhoneNumbers).ToList();
        }

        public NumberPool GetById(int id)
        {
            if (context.NumberPools == null)
            {
                throw new NullReferenceException();
            }

            return context.NumberPools
                .Include(p => p.PhoneNumbers)
                             .FirstOrDefault(p => p.Id == id);
        }

        public void InsertNumberPools(NumberPool numberPool)
        {
            if (numberPool == null)
            {
                throw new NullReferenceException();
            }

            context.NumberPools.Add(numberPool);
        }

        public void DeleteNumberPools(int id)
        {
            NumberPool numberPool = context.NumberPools.Find(id);

            if (numberPool == null)
            {
                throw new NullReferenceException();
            }
            context.NumberPools.Remove(numberPool);
        }

        public void UpdateNumberPools(NumberPool numberPool)
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
