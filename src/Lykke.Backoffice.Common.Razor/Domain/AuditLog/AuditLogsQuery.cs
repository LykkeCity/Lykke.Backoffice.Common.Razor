using System;

namespace Lykke.Backoffice.Common.Razor.Domain.AuditLog
{
    public class AuditLogsQuery
    {
        public string Type { get; set; }
        public string Author { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
