# net6_webapi_Swagger_Test
嘗試於 Swagger 加入授權登入

參考

帳號授權：
https://medium.com/@niteshsinghal85/securing-swagger-in-production-92d0a045a5
https://medium.com/@niteshsinghal85/securing-swagger-ui-in-production-in-asp-net-core-part-2-dc2ae0f03c73

jwt授權：
https://www.infoworld.com/article/3650668/implement-authorization-for-swagger-in-aspnet-core-6.html

windows授權：
https://blog.poychang.net/asp-net-core-windows-authentication/

ASP.NET Web API 2 中的驗證篩選：
https://docs.microsoft.com/zh-tw/aspnet/web-api/overview/security/authentication-filters

Swagger UI + Basic Authentication 訪問受保護的 Web API
https://dotblogs.com.tw/yc421206/2020/03/22/nswag_integrate_basic_authentication_in_asp_net_core_web_api

新增 Auth 串 Ad 驗證，客製化 UI

參考：

swaggerUI 自訂登入
https://blog.csdn.net/u010476739/article/details/104638766

問題
https://github.com/abpframework/abp/issues/1872

module-zero-core-template
https://github.com/aspnetboilerplate/module-zero-core-template/blob/master/aspnet-core/src/AbpCompanyName.AbpProjectName.Web.Host/wwwroot/swagger/ui/index.html#L72

調整 AuthController 中   new PrincipalContext(ContextType.Domain, "Domain url") 即可使用。
