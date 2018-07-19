using System;

namespace Lykke.Backoffice.Common.Razor.Domain.AuditLog
{
    public interface IAuditLog
    {
        string Author { get; set; }
        DateTime CreateDate { get; set; }
        string Type { get; set; }
        string EntityId { get; set; }
        string EntityName { get; set; }
        string Change { get; set; }
    }
}
