﻿using System.ComponentModel.DataAnnotations;

namespace Outbound_company.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }

        // Внешний ключ на OutboundCompany
        public int NumberPoolId { get; set; }
        public NumberPool NumberPool { get; set; }
    }
}
