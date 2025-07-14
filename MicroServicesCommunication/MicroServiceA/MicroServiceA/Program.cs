using MassTransit;
using MicroServiceA.Consumers;
using MicroServiceA.Data;
using Microsoft.EntityFrameworkCore;
using static MicroServiceA.SharedEvents.OrderEvents;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDatabase")));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();
    x.AddConsumer<PaymentFailedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("host.docker.internal", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("order-created-queue", e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
    });
});

builder.Services.Configure<MassTransitHostOptions>(options =>
{
    options.WaitUntilStarted = true;
    options.StartTimeout = TimeSpan.FromSeconds(30);
    options.StopTimeout = TimeSpan.FromMinutes(1);
});

builder.WebHost.ConfigureKestrel(opts =>
{
    opts.ListenAnyIP(80);
});

var app = builder.Build();

/*
 * This code is needed if you want to send synchronous HTTP requests to another microservice.
app.MapGet("/send", async (IHttpClientFactory factory) =>
{
    var client = factory.CreateClient();
    var response = await client.PostAsJsonAsync("http://microserviceb:7074/api/JustB/process", "Hello from A");
    var ack = await response.Content.ReadAsStringAsync();
    return Results.Ok($"[A] Got response: {ack}");
});
*/

app.MapGet("/", () => "MicroServiceA is running!");

app.MapPost("/create-order", async (IPublishEndpoint publishEndpoint) =>
{
    var orderId = Guid.NewGuid();
    await publishEndpoint.Publish(new OrderCreatedEvent(orderId, "Sample Product"));
    return Results.Ok($"Order {orderId} created.");
});

app.Run();
