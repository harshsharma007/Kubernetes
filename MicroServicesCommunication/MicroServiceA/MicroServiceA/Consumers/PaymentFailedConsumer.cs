using MassTransit;
using MicroServiceA.Data;
using Microsoft.EntityFrameworkCore;
using static MicroServiceA.SharedEvents.OrderEvents;

namespace MicroServiceA.Consumers;

public class PaymentFailedConsumer(OrderDbContext orderDbContext) : IConsumer<PaymentFailedEvent>
{
    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        var orderId = context.Message.OrderId;

        var order = await orderDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order != null)
        {
            order.Status = "Cancelled";
            await orderDbContext.SaveChangesAsync();
        }
    }
}
