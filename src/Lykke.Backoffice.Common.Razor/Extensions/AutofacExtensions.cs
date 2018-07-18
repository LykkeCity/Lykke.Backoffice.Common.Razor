using System;
using Autofac;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;
using Lykke.Backoffice.Common.Razor.Domain.AuditLog;
using Lykke.Backoffice.Common.Razor.Domain.AuditLogType;
using Lykke.Backoffice.Common.Razor.Repositories.AuditLogs;
using Lykke.Backoffice.Common.Razor.Repositories.AuditLogTypes;
using Lykke.Backoffice.Common.Razor.Services;
using Lykke.Common.Log;
using Lykke.SettingsReader;

namespace Lykke.Backoffice.Common.Razor.Extensions
{
    public static class AutofacExtensions
    {
        [Obsolete("Use RegisterAuditLogsService with ILogFactory as a parameter")]
        public static void RegisterAuditLogsService(this ContainerBuilder builder, IReloadingManager<string> connStringManager, 
            string logsTableName, string logTypeTableName, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (connStringManager == null) throw new ArgumentNullException(nameof(connStringManager));
            
            if (string.IsNullOrWhiteSpace(logsTableName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(logsTableName));

            builder.RegisterInstance(
                    new AuditLogsRepository(
                        AzureTableStorage<AuditLogEntity>.Create(connStringManager,
                            logsTableName, log),
                        AzureTableStorage<AzureIndex>.Create(connStringManager, logsTableName, log))
                ).As<IAuditLogsRepository>()
                .SingleInstance();
            
            builder.RegisterInstance(
                    new AuditLogTypesRepository(
                        AzureTableStorage<AuditLogTypeEntity>.CreateWithCache(connStringManager, logTypeTableName, log))
                ).As<IAuditLogTypesRepository>()
                .SingleInstance();
            
            builder.RegisterType<AuditLogsService>()
                .As<IAuditLogsService>()
                .SingleInstance();
        }
        
        public static void RegisterAuditLogsService(this ContainerBuilder builder, IReloadingManager<string> connStringManager, 
            string logsTableName, string logTypeTableName, ILogFactory logFactory)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (logFactory == null) throw new ArgumentNullException(nameof(logFactory));
            if (connStringManager == null) throw new ArgumentNullException(nameof(connStringManager));
            
            if (string.IsNullOrWhiteSpace(logsTableName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(logsTableName));

            builder.RegisterInstance(
                    new AuditLogsRepository(
                        AzureTableStorage<AuditLogEntity>.Create(connStringManager,
                            logsTableName, logFactory),
                        AzureTableStorage<AzureIndex>.Create(connStringManager, logsTableName, logFactory))
                ).As<IAuditLogsRepository>()
                .SingleInstance();
            
            builder.RegisterInstance(
                    new AuditLogTypesRepository(
                        AzureTableStorage<AuditLogTypeEntity>.Create(connStringManager, logTypeTableName, logFactory))
                ).As<IAuditLogTypesRepository>()
                .SingleInstance();
            
            builder.RegisterType<AuditLogsService>()
                .As<IAuditLogsService>()
                .SingleInstance();
        }
    }
}
