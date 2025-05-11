var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/send", async (IHttpClientFactory factory) =>
{
    var client = factory.CreateClient();
    var response = await client.PostAsJsonAsync("http://microserviceb:7074/api/JustB/process", "Hello from A");
    var ack = await response.Content.ReadAsStringAsync();
    return Results.Ok($"[A] Got response: {ack}");
});

app.Run();
