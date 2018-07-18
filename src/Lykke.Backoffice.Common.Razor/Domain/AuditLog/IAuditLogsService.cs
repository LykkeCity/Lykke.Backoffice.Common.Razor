using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Backoffice.Common.Razor.Domain.AuditLog
{
    /// <summary>
    /// Service to manage audit logs
    /// </summary>
    public interface IAuditLogsService
    {
        /// <summary>
        /// Adds new audit log record
        /// </summary>
        /// <param name="author">name of the changer</param>
        /// <param name="oldValue">old entity</param>
        /// <param name="newValue">changed entity</param>
        /// <param name="getEntityIdFunc">function to return entity Id</param>
        /// <param name="getEntityNameFunc">funtion to return entity name</param>
        /// <typeparam name="T">type of the object to log</typeparam>
        /// <remarks>
        /// <para><paramref name="getEntityIdFunc"/> should return entity Id, for example for Asset it can be
        /// a function to return asset.Id: asset => asset.Id</para>
        /// <para><paramref name="getEntityNameFunc"/> can return entity Name, for example for Asset it can be
        /// a function to return asset.DisplayId: asset => asset.Id</para>
        /// </remarks>
        /// <example>
        /// <code>
        /// AddLogsAsync{Asset}("admin", currentAsset, newAsset, x => x.Id, x => x.DisplayId);
        /// </code>
        /// </example>
        /// <returns></returns>
        Task AddLogAsync<T>(string author, T oldValue, T newValue, Func<T, string> getEntityIdFunc, Func<T, string> getEntityNameFunc = null) where T : class;
        
        /// <summary>
        /// Gets logs
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<IAuditLog>> GetLogsAsync(AuditLogsQuery query);
        
        /// <summary>
        /// Gets all available type names (logged objects)
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<string>> GetTypesAsync();
    }
}
