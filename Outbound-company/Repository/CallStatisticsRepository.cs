using Microsoft.EntityFrameworkCore;
using Outbound_company.Context;
using Outbound_company.Models;
using Outbound_company.Repository.Interface;

namespace Outbound_company.Repository
{
    public class CallStatisticsRepository : ICallStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        public CallStatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CallStatistics>> GetAllAsync()
        {
            return await _context.CallStatistics
                .Include(cs => cs.Company)
                .Include(cs => cs.PhoneNumber)
                .Include(cs => cs.CallStatus)
                .ToListAsync();
        }

        public async Task<CallStatistics> GetByIdAsync(int id)
        {
            return await _context.CallStatistics
                .Include(cs => cs.Company)
                .Include(cs => cs.PhoneNumber)
                .Include(cs => cs.CallStatus)
                .FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public async Task<IEnumerable<CallStatistics>> GetStatysticByCompanyIdAsync(int companyId)
        {
            return await _context.CallStatistics
                .Where(cs => cs.CompanyId == companyId)
                .ToListAsync();
        }
        public async Task<CallStatistics> GetLatestByCompanyIdAsync(int companyId)
        {
            return await _context.CallStatistics
                .Where(cs => cs.CompanyId == companyId)
                .OrderByDescending(cs => cs.CallTime)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(CallStatistics callStatistics)
        {
            await _context.CallStatistics.AddAsync(callStatistics);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CallStatistics callStatistics)
        {
            _context.CallStatistics.Update(callStatistics);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStatysticByCompanyIdAsync(int companyId)
        {
            // Находим все записи CallStatistics для данной компании
            var entities = await _context.CallStatistics
                .Where(cs => cs.CompanyId == companyId)
                .ToListAsync();

            if (entities != null && entities.Any())
            {
                _context.CallStatistics.RemoveRange(entities); // Удаляем все найденные записи
                await _context.SaveChangesAsync();
            }
        }
    }

}
