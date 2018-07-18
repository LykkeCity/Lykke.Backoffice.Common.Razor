using Microsoft.AspNetCore.Mvc;

namespace Lykke.Backoffice.Common.Razor.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string GetAuditLogsListUrl(this IUrlHelper urlHelper)
        {
            return urlHelper.Action("Index", "AuditLogs", new {area = "BoCommonAuditLogs"});
        }
    }
}
