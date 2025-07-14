namespace MicroserviceB.Shared;

public static class OrderEvents
{
    public record OrderCreatedEvent(Guid OrderId, string Product);
    public record InventoryReservedEvent(Guid OrderId);
    public record InventoryFailedEvent(Guid OrderId, string Reason);
    public record PaymentProcessedEvent(Guid OrderId);
    public record PaymentFailedEvent(Guid OrderId, string Reason);
}
