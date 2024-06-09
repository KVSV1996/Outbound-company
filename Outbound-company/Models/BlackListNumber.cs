using System.ComponentModel.DataAnnotations;

namespace Outbound_company.Models
{
    public class BlackListNumber
    {
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        public string Reason { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
