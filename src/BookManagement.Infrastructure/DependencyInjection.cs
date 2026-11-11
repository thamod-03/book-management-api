using BookManagement.Application.Books.Interfaces;
using BookManagement.Infrastructure.Persistence;
using BookManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookManagement.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    var connectionString =
        configuration.GetConnectionString("Supabase")
        ?? throw new InvalidOperationException(
            "Supabase connection string is missing.");

    services.AddDbContext<BookDbContext>(options =>
        options.UseNpgsql(connectionString));

    services.AddScoped<IBookRepository, BookRepository>();

    return services;
  }
}
