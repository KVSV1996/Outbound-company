using System.ComponentModel.DataAnnotations;

namespace Outbound_company.Models
{
    public class OutboundCompany
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Channel { get; set; }
        [Required]
        public string TrunkType { get; set; }
        [Required]
        public string Extension { get; set; }
        [Required]
        public string Context { get; set; }
        [Required]
        public string CallerId { get; set; }
        [Required]
        public int NumberPoolId { get; set; }
    }
}
