using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MyConsoleApp.Data;
using MyConsoleApp.Dtos;
using MyConsoleApp.Models;

namespace MyConsoleApp.Endpoints;

public static class ProductsEndpoint
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
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
                .FirstOrDefaultAsync(x => x.Id == id);

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

            if (requests.Any(request => string.IsNullOrWhiteSpace(request.Name) || request.Price < 0))
            {
                return Results.BadRequest("Each product name is required and price must be zero or greater.");
            }

            var productsToCreate = requests.Select(request => new Product
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                Price = request.Price,
                StockQuantity = request.StockQuantity
            }).ToList();

            db.Products.AddRange(productsToCreate);
            await db.SaveChangesAsync();

            var response = productsToCreate.Select(ProductResponse.FromEntity).ToList();
            return Results.Created("/api/products", response);
        });

        products.MapPut("/{id:int}", async (int id, UpdateProductRequest request, AppDbContext db) =>
        {
            var product = await db.Products.FindAsync(id);

            if (product is null)
            {
                return Results.NotFound();
            }

            if (string.IsNullOrWhiteSpace(request.Name) || request.Price < 0)
            {
                return Results.BadRequest("Product name is required and price must be zero or greater.");
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

        return app;
    }
}
