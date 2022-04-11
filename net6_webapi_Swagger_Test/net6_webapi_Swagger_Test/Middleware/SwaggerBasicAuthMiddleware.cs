public class SwaggerBasicAuthMiddleware
{
    private readonly RequestDelegate next;
    public SwaggerBasicAuthMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            //string authHeader = context.Request.Headers["Authorization"];
            string authHeader = context.Request.Headers.Authorization;
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Get the credentials from request header
                var header = AuthenticationHeaderValue.Parse(authHeader);
                var inBytes = Convert.FromBase64String(header.Parameter);
                var credentials = Encoding.UTF8.GetString(inBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                string testPassword = "abcd" + DateTime.Now.ToString("MMddHH");
                // validate credentials
                if (username.Equals("abcd")
                  && password.Equals(testPassword))
                {
                    await next.Invoke(context).ConfigureAwait(false);
                    return;
                }
            }
            //context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.Headers.WWWAuthenticate = "Basic";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else
        {
            await next.Invoke(context).ConfigureAwait(false);
        }
    }
}


public static class MiddlewareExtension
{
    public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SwaggerBasicAuthMiddleware>();
    }
}


