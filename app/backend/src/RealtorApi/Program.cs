var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new { application = "RealtorApi", status = "ready" }));
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
