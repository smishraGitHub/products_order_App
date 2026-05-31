namespace MyConsoleApp.Models;

public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public List<Orders> Orders { get; set; } = [];

}
