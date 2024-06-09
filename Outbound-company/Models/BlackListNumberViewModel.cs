namespace Outbound_company.Models
{
    public class BlackListNumberViewModel
    {
        public IEnumerable<BlackListNumber> BlackListNumbers { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}
