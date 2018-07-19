using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Backoffice.Common.Razor.Domain.AuditLogType
{
    internal interface IAuditLogTypesRepository
    {
        Task AddAsync(string name);
        Task<IEnumerable<IAuditLogType>> GetAllAsync();
    }
}
