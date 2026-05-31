using MyConsoleApp.Models;

namespace MyConsoleApp.Dtos;

public record OrderResponse(
    int Id,
    int ProductId,
    string ProductName,
    int Quantity,
    DateTime OrderDateUtc)
{
    public static OrderResponse FromEntity(Orders order) =>
        new(
            order.Id,
            order.ProductId,
            order.Product.Name,
            order.Quantity,
            order.OrderDateUtc);
}
