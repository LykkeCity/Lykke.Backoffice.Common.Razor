using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Backoffice.Common.Razor.Domain.AuditLog
 {
     internal interface IAuditLogsRepository
     {
         Task AddLogAsync<T>(string author, string entityId, string entityName, string change) where T : class;
         Task<IEnumerable<IAuditLog>> GetLogsAsync(AuditLogsQuery query);
     }
 }
