using Microsoft.AspNetCore.Http.Features;
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

        products.MapGet("/",async (AppDbContext db) =>
            await db.Products
                .AsNoTracking()
                .OrderBy(product => product.Id)
                .Select(product => ProductResponse.FromEntity(product))
                .ToListAsync());

        products.MapGet("/{id:int}", async (int id, AppDbContext db) =>
        {
            var product = await db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Id == id);

            return product is null
                ? Results.NotFound()
                : Results.Ok(ProductResponse.FromEntity(product)); 
        });

        products.MapPost("/", async (List<CreateProductRequest> requests,
        AppDbContext db) =>
        {
            var products = requests.Select(requests => new Product
            {
                Name = requests.Name,
                Description = requests.Description,
                Price = requests.Price
            }).ToList();
            db.Products.AddRange(products);
            await db.SaveChangesAsync();

            return Results.Created("/api/products",products);
        });

        products.MapPut("/{id:int}", async (int id, UpdateProductRequest request, AppDbContext db) =>
        {
            var product = await db.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
            {
                return Results.NotFound();
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;

            await db.SaveChangesAsync();
            return Results.Ok(ProductResponse.FromEntity(product));
        });
        
        products.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
        {
            var product = await db.Products.FirstOrDefaultAsync(x=>x.Id == id);
            if(product is null)
            {
                return Results.NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
        
        return app;
    }
}