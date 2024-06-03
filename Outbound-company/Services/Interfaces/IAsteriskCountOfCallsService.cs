namespace Outbound_company.Services.Interfaces
{
    public interface IAsteriskCountOfCallsService
    {
        Task<int> GetActiveCallsAsync();
    }
}
