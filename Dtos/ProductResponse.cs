using MyConsoleApp.Models;

namespace MyConsoleApp.Dtos;

public record ProductResponse(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity,
    DateTime CreatedAtUtc)
{
    public static ProductResponse FromEntity(Product product) =>
        new(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.StockQuantity,
            product.CreatedAtUtc);
}
