namespace MyConsoleApp.Models;

public class Orders
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }

    public DateTime OrderDateUtc { get; set; } = DateTime.UtcNow;

}