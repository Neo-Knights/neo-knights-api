using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using neo_knights_api;
using System.Text;

WebHost.CreateDefaultBuilder().
ConfigureServices(s => s.AddSingleton<KnightService>()).
Configure(app =>
{
    app.UseRouting();
    app.UseEndpoints(e =>
    {
        var knightService = e.ServiceProvider.GetRequiredService<KnightService>();
        e.MapGet("/api/{hash}",
            async c =>
            {
                c.Response.ContentType = "application/json; charset=utf-8";
                await c.Response.WriteAsync(await knightService.GetKnight((string)c.Request.RouteValues["hash"]), Encoding.UTF8);
            });
    });
}).Build().Run();