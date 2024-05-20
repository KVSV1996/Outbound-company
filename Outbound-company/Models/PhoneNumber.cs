namespace Outbound_company.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public string Number { get; set; }

        // Внешний ключ на OutboundCompany
        public int OutboundCompanyId { get; set; }
        public OutboundCompany OutboundCompany { get; set; }
    }
}
