namespace MyConsoleApp.Dtos;

public record CreateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity);

public record UpdateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity);
