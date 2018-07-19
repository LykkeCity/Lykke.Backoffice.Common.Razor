using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Backoffice.Common.Razor.Domain.AuditLogType;

namespace Lykke.Backoffice.Common.Razor.Repositories.AuditLogTypes
{
    internal class AuditLogTypesRepository : IAuditLogTypesRepository
    {
        private readonly INoSQLTableStorage<AuditLogTypeEntity> _tableStorage;

        public AuditLogTypesRepository(INoSQLTableStorage<AuditLogTypeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public Task AddAsync(string name)
        {
            var entity = AuditLogTypeEntity.Create(name);
            return _tableStorage.InsertOrMergeAsync(entity);
        }

        public async Task<IEnumerable<IAuditLogType>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync(AuditLogTypeEntity.GeneratePartitionKey());
        }
    }
}
