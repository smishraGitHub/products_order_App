using Microsoft.EntityFrameworkCore;
using MyConsoleApp.Data;
using MyConsoleApp.Dtos;
using MyConsoleApp.Models;
using MyConsoleApp.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapProductEndpoints();
app.MapOrderEndpoints();

app.UseSwagger();
app.UseSwaggerUI();


/*
app.MapGet("/", () => Results.Ok(new
{
    Application = "Products CRUD API",
    Endpoints = new[]
    {
        "GET /api/products",
        "GET /api/products/{id}",
        "POST /api/products",
        "PUT /api/products/{id}",
        "DELETE /api/products/{id}",
        "GET /api/orders",
        "GET /api/orders/{id}",
        "POST /api/orders",
        "PUT /api/orders/{id}",
        "DELETE /api/orders/{id}"
    }
}));

var products = app.MapGroup("/api/products");

products.MapGet("/", async (AppDbContext db) =>
    await db.Products
        .AsNoTracking()
        .OrderBy(product => product.Id)
        .Select(product => ProductResponse.FromEntity(product))
        .ToListAsync());

products.MapGet("/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products
        .AsNoTracking()
        .FirstOrDefaultAsync(product => product.Id == id);

    return product is null
        ? Results.NotFound()
        : Results.Ok(ProductResponse.FromEntity(product));
});

products.MapPost("/", async (List<CreateProductRequest> requests, AppDbContext db) =>
{
    if (requests.Count == 0)
    {
        return Results.BadRequest("At least one product is required.");
    }

    if (requests.Any(request =>
        string.IsNullOrWhiteSpace(request.Name) ||
        request.Price < 0))
    {
        return Results.BadRequest("Each product name is required and price must be zero or greater.");
    }

    var productsToCreate = requests
        .Select(request => new Product
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Price = request.Price,
            StockQuantity = request.StockQuantity
        })
        .ToList();

    db.Products.AddRange(productsToCreate);
    await db.SaveChangesAsync();

    var response = productsToCreate
        .Select(product => ProductResponse.FromEntity(product))
        .ToList();

    return Results.Created("/api/products", response);
});

products.MapPut("/{id:int}", async (int id, UpdateProductRequest request, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || request.Price < 0)
    {
        return Results.BadRequest("Product name is required and price must be zero or greater.");
    }

    var product = await db.Products.FindAsync(id);

    if (product is null)
    {
        return Results.NotFound();
    }

    product.Name = request.Name.Trim();
    product.Description = request.Description?.Trim();
    product.Price = request.Price;
    product.StockQuantity = request.StockQuantity;

    await db.SaveChangesAsync();

    return Results.Ok(ProductResponse.FromEntity(product));
});

products.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null)
    {
        return Results.NotFound();
    }

    db.Products.Remove(product);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

var orders = app.MapGroup("/api/orders");

orders.MapGet("/", async (AppDbContext db) =>
    await db.Orders
        .AsNoTracking()
        .Include(order => order.Product)
        .OrderBy(order => order.Id)
        .Select(order => OrderResponse.FromEntity(order))
        .ToListAsync());

orders.MapGet("/{id:int}", async (int id, AppDbContext db) =>
{
    var order = await db.Orders
        .AsNoTracking()
        .Include(order => order.Product)
        .FirstOrDefaultAsync(order => order.Id == id);

    return order is null
        ? Results.NotFound()
        : Results.Ok(OrderResponse.FromEntity(order));
});

orders.MapPost("/", async (CreateOrderRequest request, AppDbContext db) =>
{
    if (request.ProductId <= 0 || request.Quantity <= 0)
    {
        return Results.BadRequest("ProductId and quantity must be greater than zero.");
    }

    var productExists = await db.Products.AnyAsync(product => product.Id == request.ProductId);

    if (!productExists)
    {
        return Results.BadRequest("Product does not exist.");
    }

    var order = new Orders
    {
        ProductId = request.ProductId,
        Quantity = request.Quantity
    };

    db.Orders.Add(order);
    await db.SaveChangesAsync();

    await db.Entry(order)
        .Reference(createdOrder => createdOrder.Product)
        .LoadAsync();

    return Results.Created($"/api/orders/{order.Id}", OrderResponse.FromEntity(order));
});

orders.MapPut("/{id:int}", async (int id, UpdateOrderRequest request, AppDbContext db) =>
{
    if (request.ProductId <= 0 || request.Quantity <= 0)
    {
        return Results.BadRequest("ProductId and quantity must be greater than zero.");
    }

    var order = await db.Orders.FindAsync(id);

    if (order is null)
    {
        return Results.NotFound();
    }

    var productExists = await db.Products.AnyAsync(product => product.Id == request.ProductId);

    if (!productExists)
    {
        return Results.BadRequest("Product does not exist.");
    }

    order.ProductId = request.ProductId;
    order.Quantity = request.Quantity;

    await db.SaveChangesAsync();

    await db.Entry(order)
        .Reference(updatedOrder => updatedOrder.Product)
        .LoadAsync();

    return Results.Ok(OrderResponse.FromEntity(order));
});

orders.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
{
    var order = await db.Orders.FindAsync(id);

    if (order is null)
    {
        return Results.NotFound();
    }

    db.Orders.Remove(order);
    await db.SaveChangesAsync();

    return Results.NoContent();
});
*/
app.Run();
