using Microsoft.EntityFrameworkCore;
using Outbound_company.Context;
using Outbound_company.Models;
using Outbound_company.Repository.Interface;

namespace Outbound_company.Repository
{
    public class BlackListNumberRepository : IBlackListNumberRepository
    {
        private readonly ApplicationDbContext _context;

        public BlackListNumberRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlackListNumber>> GetAllByPagAsync(int pageNumber, int pageSize)
        {
            return await _context.BlackListNumbers
                                 .OrderBy(b => b.Id)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<BlackListNumber>> GetAllAsync()
        {
            return await _context.BlackListNumbers
                                 .OrderBy(b => b.Id)
                                 .ToListAsync();
        }

        public async Task<BlackListNumber> GetByIdAsync(int id)
        {
            return await _context.BlackListNumbers.FindAsync(id);
        }

        public async Task AddAsync(BlackListNumber blackListNumber)
        {
            _context.BlackListNumbers.Add(blackListNumber);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BlackListNumber blackListNumber)
        {
            _context.BlackListNumbers.Update(blackListNumber);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.BlackListNumbers.FindAsync(id);
            if (entity != null)
            {
                _context.BlackListNumbers.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.BlackListNumbers.CountAsync();
        }
    }
}

