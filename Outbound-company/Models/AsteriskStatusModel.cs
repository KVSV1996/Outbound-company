using Outbound_company.Models.Enums;

namespace Outbound_company.Models
{
    public class AsteriskStatusModel
    {
        public AsteriskStatus Status { get; set; } = AsteriskStatus.Unknown;
        public DateTime LastChecked { get; set; } = DateTime.MinValue;
    }
}
