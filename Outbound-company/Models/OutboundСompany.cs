namespace Outbound_company.Models
{
    public class OutboundCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Channel { get; set; }
        public string Extension { get; set; }
        public string Context { get; set; }
        public string CallerId { get; set; }
        public int NumberPoolId { get; set; }
        public NumberPool NumberPool { get; set; }
    }
}
