using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MyConsoleApp.Data;
using MyConsoleApp.Dtos;
using MyConsoleApp.Models;

namespace MyConsoleApp.Endpoints;

public static class OrdersEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
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
            await db.Entry(order).Reference(createdOrder => createdOrder.Product).LoadAsync();

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
            await db.Entry(order).Reference(updatedOrder => updatedOrder.Product).LoadAsync();

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

        return app;
    }
}
