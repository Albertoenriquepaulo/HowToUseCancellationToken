var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
.WithName("GetWeatherForecast");

app.MapGet("/endpoint", async (ILogger<Program> logger, CancellationToken cancellationToken) =>
{
    try
    {
        logger.LogInformation("Starting request...");

        // Run some async call
        await Task.Delay(5000, cancellationToken);

        logger.LogInformation("Waited 5 seconds");

        return "Hello World!";
    }
    catch (TaskCanceledException exception)
    {
        logger.LogError("Ooops! The operation was cancelled.");

        // Do some exception handling...

        // Here we are just throwing it again so can be visible in the logs
        throw;
    }
})
.WithName("GetEndpoint");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}