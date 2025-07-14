using MassTransit;
using MicroServiceA.Data;
using MicroServiceA.Models;
using static MicroServiceA.SharedEvents.OrderEvents;

namespace MicroServiceA.Consumers;

public class OrderCreatedEventConsumer(OrderDbContext dbContext) : IConsumer<OrderCreatedEvent>
{
    private readonly OrderDbContext _dbContext = dbContext;

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        var order = new Order
        {
            OrderId = message.OrderId,
            Product = message.Product,
            Status = "Created"
        };

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
    }
}
