namespace MakeYourBusinessGreen.Application.Responses.HealthChecks;
public record HealthCheckResponse(string Status, IEnumerable<HealthCheck> HealthChecks, TimeSpan Duration);
