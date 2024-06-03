namespace Outbound_company.Services
{
    public interface IAsteriskCountOfCallsService
    {
        Task<int> GetActiveCallsAsync();
    }
}
