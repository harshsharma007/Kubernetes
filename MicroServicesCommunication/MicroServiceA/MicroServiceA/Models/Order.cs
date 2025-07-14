namespace MicroServiceA.Models;

public class Order
{
    public Guid OrderId { get; set; }
    public string Product { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
