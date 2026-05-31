namespace MyConsoleApp.Dtos;

public record CreateOrderRequest(
    int ProductId,
    int Quantity);

public record UpdateOrderRequest(
    int ProductId,
    int Quantity);
