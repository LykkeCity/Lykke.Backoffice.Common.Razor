
# BoCommonAuditLogs area  
  
This area is used to show Audit logs. To make it work:  
1) Register AuditLogsService in the DI container:  
```csharp  
 builder.RegisterAuditLogsService(  
    _settings.ConnectionString(x => x.Db.AuditLogsConnString, "AuditLogs", "AuditLogTypes", _log);  
```  
where:   
**AuditLogs** - name of the table to store audit logs  
**AuditLogTypes** - table to store audit log type (object type names)  
  
2) use ```AddLogAsync``` method of IAuditLogsService to add a record to audit log
  
3) Put a link somewhere to the audit logs list page using extension method:  
  
```html  
<a href="@Url.GetAuditLogsListUrl()">Audit logs</a>  
```  
  
4) Add a feature named ```MenuBoCommonAuditLogs``` in the backoffice user/roles management section to controll access to the   
audit logs page (```AuditLogs``` controller has a ```FilterFeaturesAccess``` attribute with ```MenuBoCommonAuditLogs``` value)
