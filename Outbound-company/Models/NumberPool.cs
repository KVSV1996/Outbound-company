using System.ComponentModel.DataAnnotations;

namespace Outbound_company.Models
{
    public class NumberPool
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
