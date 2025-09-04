using Microsoft.OpenApi.Models;
using AdService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ILocationIndex, LocationTrieIndex>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/platforms/upload", async (HttpRequest request, ILocationIndex index) =>
{
    if (!request.HasFormContentType) return Results.BadRequest("Invalid from data");
    var from = await request.ReadFormAsync();
    var file = from.Files.FirstOrDefault();
    if (file == null || file.Length == 0) return Results.BadRequest("File is empty");

    using var reader = new StreamReader(file.OpenReadStream());
    var lines = new List<string>();
    while (!reader.EndOfStream)
    {
        lines.Add(await reader.ReadLineAsync() ?? "");
    }
    index.Load(lines);
    return Results.Ok("Platforms loaded");
});

app.MapGet("/api/platforms/search", (string location, ILocationIndex index) =>
{
    var result = index.Search(location);
    return Results.Ok(result);
});

app.Run();
