using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Backoffice.Common.Razor.Domain.AuditLog;

namespace Lykke.Backoffice.Common.Razor.Repositories.AuditLogs
{
    internal class AuditLogsRepository : IAuditLogsRepository
    {
        private readonly INoSQLTableStorage<AuditLogEntity> _tableStorage;
        private readonly INoSQLTableStorage<AzureIndex> _authorIndex;

        public AuditLogsRepository(INoSQLTableStorage<AuditLogEntity> tableStorage,
            INoSQLTableStorage<AzureIndex> authorIndex)
        {
            _tableStorage = tableStorage;
            _authorIndex = authorIndex;
        }
        
        public Task AddLogAsync<T>(string author, string entityId, string entityName, string change) where T : class
        {
            var entity = AuditLogEntity.Create<T>(author, entityId, entityName, change);
            var authorIndex = AzureIndex.Create(AuditLogEntity.GenerateAuthorIndexPk(author), Guid.NewGuid().ToString(), entity.PartitionKey, entity.RowKey);

            return Task.WhenAll(
                _tableStorage.InsertOrMergeAsync(entity),
                _authorIndex.InsertOrMergeAsync(authorIndex)
            );
        }

        public async Task<IEnumerable<IAuditLog>> GetLogsAsync(AuditLogsQuery query)
        {
            IEnumerable<IAuditLog> logs;

            if (!string.IsNullOrEmpty(query.Author))
            {
                var authorIndexes = await _authorIndex.GetDataAsync(AuditLogEntity.GenerateAuthorIndexPk(query.Author));
                logs = (await _tableStorage.GetDataAsync(authorIndexes))
                    .Where(entity => entity.PartitionKey == AuditLogEntity.GenerateTypePk(query.Type));
            }
            else
            {
                logs = await _tableStorage.GetDataAsync(AuditLogEntity.GenerateTypePk(query.Type));
            }

            return logs.Where(item => (!query.StartDate.HasValue || item.CreateDate >= query.StartDate) && (!query.EndDate.HasValue || item.CreateDate <= query.EndDate));
        }
    }
}
