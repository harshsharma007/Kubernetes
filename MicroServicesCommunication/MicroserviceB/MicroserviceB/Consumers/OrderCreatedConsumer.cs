using MassTransit;
using static MicroserviceB.Shared.OrderEvents;

namespace MicroserviceB.Consumers;

public class OrderCreatedConsumer(IPublishEndpoint publishEndpoint) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;
        Console.WriteLine($"[Inventory] Received OrderCreatedEvent for {message.OrderId} - {message.Product}");

        var inStock = !message.Product.Contains("Fail", StringComparison.OrdinalIgnoreCase);

        if (inStock)
        {
            Console.WriteLine($"[Inventory] Stock Reversed");
            await publishEndpoint.Publish(new InventoryReservedEvent(message.OrderId));
        }
        else
        {
            Console.WriteLine($"[Inventory] Stock unavailable");
            await publishEndpoint.Publish(new InventoryFailedEvent(message.OrderId, "Stock unavailable"));
        }
    }
}
