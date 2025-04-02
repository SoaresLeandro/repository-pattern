using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}