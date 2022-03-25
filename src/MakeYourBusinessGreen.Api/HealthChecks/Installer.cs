using MakeYourBusinessGreen.Application.Responses.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

namespace MakeYourBusinessGreen.Api.HealthChecks;
public static class Installer
{
    public static IApplicationBuilder InstallHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/api/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new HealthCheckResponse(report.Status.ToString(), report.Entries.Select(x => new HealthCheck(x.Value.Status.ToString(),
                x.Key, x.Value.Description)), report.TotalDuration);

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        });

        return app;
    }


}

