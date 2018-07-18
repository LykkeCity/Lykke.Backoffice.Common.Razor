using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JsonDiffPatchDotNet;
using Lykke.Backoffice.Common.Razor.Domain.AuditLog;
using Lykke.Backoffice.Common.Razor.Domain.AuditLogType;
using Lykke.Backoffice.Common.Razor.Repositories.AuditLogs;
using Newtonsoft.Json.Linq;

namespace Lykke.Backoffice.Common.Razor.Services
{
    /// <inheritdoc />
    [UsedImplicitly]
    internal class AuditLogsService : IAuditLogsService
    {
        private readonly IAuditLogsRepository _auditLogsRepository;
        private readonly IAuditLogTypesRepository _auditLogTypesRepository;
        private readonly JsonDiffPatch _diffPatch;

        public AuditLogsService(
            IAuditLogsRepository auditLogsRepository,
            IAuditLogTypesRepository auditLogTypesRepository
            )
        {
            _auditLogsRepository = auditLogsRepository;
            _auditLogTypesRepository = auditLogTypesRepository;
            _diffPatch = new JsonDiffPatch();
        }
        
        public Task AddLogAsync<T>(string author, T oldValue, T newValue, Func<T, string> getEntityIdFunc, Func<T, string> getEntityNameFunc = null) where T : class
        {
            var empty = new JValue("{}");
            var left = oldValue == null ? empty : JToken.FromObject(oldValue);
            var right = newValue == null ? empty : JToken.FromObject(newValue);

            var change = _diffPatch.Diff(left, right);
            
            string entityId = getEntityIdFunc(oldValue ?? newValue);
            string entityName = getEntityNameFunc?.Invoke(oldValue ?? newValue);

            if (change != null)
            {
                return Task.WhenAll(
                    _auditLogsRepository.AddLogAsync<T>(author, entityId, entityName, change.ToString()),
                    _auditLogTypesRepository.AddAsync(AuditLogEntity.GenerateTypePk<T>())
                );
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<IAuditLog>> GetLogsAsync([NotNull] AuditLogsQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.Type))
                throw new ArgumentNullException(nameof(query.Type));
            
            if (query.StartDate.HasValue && query.EndDate.HasValue && query.StartDate > query.EndDate)
                throw new ArgumentException($"{nameof(query.EndDate)} must be greater than {nameof(query.StartDate)}");
            
            return _auditLogsRepository.GetLogsAsync(query);
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return (await _auditLogTypesRepository.GetAllAsync())
                .Select(item => item.Name)
                .OrderBy(item => item)
                .ToArray();
        }
    }
}
