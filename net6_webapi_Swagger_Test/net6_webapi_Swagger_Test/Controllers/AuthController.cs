using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    [HttpPost]
    public async Task<object> Login([FromForm] string account, [FromForm] string password)
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            return Ok(new { success = false, Data = "不允許重複登入!" });
        }
        else
        {
            if (ValidateCredentials(account, password))
            {
                var claims = new List<Claim>
                 {
                    new Claim("account", account),
                    //new Claim("password", password),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                //await HttpContext.SignInAsync(principal);
                await HttpContext.SignInAsync(
                // 如果沒有在 SignInAsync 指定 AuthenticationScheme，會使用預設值
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    // 這裡可以自訂驗證選項...
                    // 是否可自動更新 Cookie(時效?)
                    //AllowRefresh = <bool>,
                    // IsPersistent 設置 Persistent cookies，否則瀏覽器 session 關閉就失效
                    //IsPersistent = true, 
                    // Persistent cookie 可進一步設置失效時間：
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5),
                    //IssuedUtc = <DateTimeOffset>,
                    //RedirectUri = <string>
                });
                return new { success = false, Data = "登入成功!" };
            }
            else
            {
                return new { success = false, Data = "登入失敗!" };
            }
        }

    }
    [HttpGet]
    [Authorize]
    public async Task<object> LogOut()
    {
        await HttpContext.SignOutAsync();

        return Ok(new { success = true, Data = "登出成功!" });
    }


    private static bool ValidateCredentials(string userName, string password)
    {
        try
        {
            using (var adContext = new PrincipalContext(ContextType.Domain, "Domain url"))
            {
                var valid = adContext.ValidateCredentials(userName, password);

                return valid;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}