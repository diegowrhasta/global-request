using GlobalRequest.Controllers;
using GlobalRequest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services.AddSingleton<IService, Service>();

var app = builder.Build();

var (httpContextAccessor, webHostEnvironment) = (
    app.Services.GetRequiredService<IHttpContextAccessor>(),
    app.Services.GetRequiredService<IWebHostEnvironment>());

HttpContextHelper.Configure(httpContextAccessor, webHostEnvironment);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapControllers();

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.MapPost("/vanilla", (HttpRequest request) => new
{
    FilesPresent = request.Form.Files.Any(),
});

app.MapPost("/static", () => new
{
    FilesPresent = HttpContextHelper.Current.Request.Form.Files.Any(),
});

app.MapPost("/service", (IService service) => new
{
    FilesPresent = service.FilesArePresent(),
});

app.MapPost("/coupled-controller", () =>
{
    var controller = new DummyController();

    controller.GetMvc();

    return Results.Ok("Did the processing :)");
});

app.MapGet("/location", () =>
{
    var local = Path.Combine(
        "Storage", "Recipes", "Customers", 1.ToString(), 3.ToString());

    return new
    {
        Path = HttpContextHelper.MapPath(local),
    };
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}