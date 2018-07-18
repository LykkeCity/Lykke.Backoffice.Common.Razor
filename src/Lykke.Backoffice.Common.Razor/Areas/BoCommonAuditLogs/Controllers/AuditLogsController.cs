using System;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Backoffice.Common.Razor.Areas.BoCommonAuditLogs.Models;
using Lykke.Backoffice.Common.Razor.Domain.AuditLog;
using Lykke.Service.BackofficeMembership.Client.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lykke.Backoffice.Common.Razor.Areas.BoCommonAuditLogs.Controllers
{
    [Area("BoCommonAuditLogs")]
    [FilterFeaturesAccess("MenuBoCommonAuditLogs")]
    public class AuditLogsController : Controller
    {
        private readonly IAuditLogsService _auditLogsService;

        public AuditLogsController(IAuditLogsService auditLogsService)
        {
            _auditLogsService = auditLogsService;
        }
        
        public async Task<IActionResult> Index()
        {
            var types = (await _auditLogsService.GetTypesAsync())
                .Select(item => new SelectListItem{Text = item, Value = item}).ToArray();
            
            var model = new AuditLogsQueryModel
            {
                Types = types,
                AuditLogs = types.Any()
                    ? await _auditLogsService.GetLogsAsync(new AuditLogsQuery{Type = types[0].Value})
                    : Array.Empty<IAuditLog>()
            };

            return View(model);
        }

        public async Task<IActionResult> Query(AuditLogsQuery query)
        {
            var logs = await _auditLogsService.GetLogsAsync(query);
            return PartialView("_AuditLogsList", logs);
        }
    }
}
