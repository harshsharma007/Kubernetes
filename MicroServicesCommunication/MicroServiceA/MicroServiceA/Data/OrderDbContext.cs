using MicroServiceA.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceA.Data;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; } = null!;
}
