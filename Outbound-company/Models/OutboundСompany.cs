namespace Outbound_company.Models
{
    public class OutboundСompany
    {
        public int Id { get; set; }
        public string Channel { get; set; }
        public string Extension { get; set; }
        public string Context { get; set; }
        public int Priority { get; set; }
        public string CallerId { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
}
}
