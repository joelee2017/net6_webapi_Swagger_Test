using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Test Api", Version = "v1" });
    var xmlPath = Path.Combine(builder.Environment.ContentRootPath, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, true);
    }
    options.AddSecurityDefinition(CookieAuthenticationDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Name = CookieAuthenticationDefaults.AuthenticationScheme,
        Scheme = CookieAuthenticationDefaults.AuthenticationScheme
    });
});

services.AddControllers();
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
services.AddAuthorization();
services.AddEndpointsApiExplorer();


var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseStaticFiles();
app.UseHttpsRedirection();
//app.UseSwaggerAuthorized();
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger v1");
    string path = Path.Combine(builder.Environment.WebRootPath, "swagger/ui/index.html");
    if (File.Exists(path)) options.IndexStream = () => new MemoryStream(File.ReadAllBytes(path));
});

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast").RequireAuthorization();

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}