using System.ComponentModel.DataAnnotations;

namespace Outbound_company.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public int Role { get; set; }
    }
}
