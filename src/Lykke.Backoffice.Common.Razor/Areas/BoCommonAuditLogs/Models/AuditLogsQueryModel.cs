using System;
using System.Collections.Generic;
using Lykke.Backoffice.Common.Razor.Domain.AuditLog;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lykke.Backoffice.Common.Razor.Areas.BoCommonAuditLogs.Models
{
    public class AuditLogsQueryModel
    {
        public SelectListItem[] Types { get; set; }
        public string Type { get; set; }
        public string Author { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public IEnumerable<IAuditLog> AuditLogs { get; set; }
    }
}
