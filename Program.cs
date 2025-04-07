using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Data;
using RepositoryPattern.Entities;
using RepositoryPattern.Repositories;
using RepositoryPattern.Repositories.Abstractions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddTransient<IProductRepository, ProductRepository>();

        var app = builder.Build();

        app.MapGet("/v1/products", async (CancellationToken token, IProductRepository productRepository) 
            => Results.Ok(await productRepository.GetAllAsync(0, 20, token)));

        app.MapGet("/v1/products/{id}", async (CancellationToken token, int id, IProductRepository productRepository)
            => Results.Ok(await productRepository.GetByIdAsync(id, token)));

        app.MapPut("/v1/products/{id}", async (CancellationToken token, int id, IProductRepository productRepository) =>
        {
            Product? product = await productRepository.GetByIdAsync(id);

            return product is null ?
                Results.NotFound() :
                Results.Ok(await productRepository.UpdateAsync(product, token));
        });

        app.MapPost("/v1/products/", async (Product product, IProductRepository productRepository, CancellationToken token) =>
        {
            await productRepository.CreateAsync(product, token);

            return Results.Ok(product);
        });

        app.MapDelete("/v1/products/{id}", async (int id, IProductRepository productRepository, CancellationToken token) =>
        {
            Product? product = await productRepository.GetByIdAsync(id);

            if(product is null)
                return Results.NotFound();

            await productRepository.DeleteAsync(product);

            return Results.Ok(product);
        });

        app.Run();
    }
}