using System;
using Common;
using Lykke.Backoffice.Common.Razor.Domain.AuditLog;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Backoffice.Common.Razor.Repositories.AuditLogs
{
    internal class AuditLogEntity : TableEntity, IAuditLog
    {
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
        public string Type { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string Change { get; set; }

        internal static string GenerateTypePk<T>() => typeof(T).Name;
        internal static string GenerateTypePk(string type) => type;
        internal static string GenerateAuthorIndexPk(string author) => $"Index_{author}";
        private static string GenerateRowKey(DateTime date) => IdGenerator.GenerateDateTimeIdNewFirst(date);

        internal static AuditLogEntity Create<T>(string author, string entityId, string entityName, string change)
        {
            var type = GenerateTypePk<T>();
            var now = DateTime.UtcNow;
            
            return new AuditLogEntity
            {
                PartitionKey = type,
                RowKey = GenerateRowKey(now),
                Author = author,
                CreateDate = now,
                EntityId = entityId,
                EntityName = entityName,
                Type = type,
                Change = change
            };
        }
    }
}
