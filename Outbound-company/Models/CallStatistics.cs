namespace Outbound_company.Models
{
    public class CallStatistics
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public OutboundCompany Company { get; set; }
        public int PhoneNumberId { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public int CallStatusId { get; set; }
        public CallStatus CallStatus { get; set; }
        public DateTime CallTime { get; set; }
    }
}
