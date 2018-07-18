using Lykke.Backoffice.Common.Razor.Domain.AuditLogType;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Backoffice.Common.Razor.Repositories.AuditLogTypes
{
    internal class AuditLogTypeEntity : TableEntity, IAuditLogType
    {
        public string Name => RowKey;

        internal static string GeneratePartitionKey() => "AuditLogType";

        internal static AuditLogTypeEntity Create(string name)
        {
            return new AuditLogTypeEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = name
            };
        }
    }
}
