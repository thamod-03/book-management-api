using BookManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Persistence;

public class BookDbContext : DbContext
{
  public BookDbContext(
      DbContextOptions<BookDbContext> options)
      : base(options)
  {
  }

  public DbSet<Book> Books => Set<Book>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.ApplyConfigurationsFromAssembly(
        typeof(BookDbContext).Assembly);
  }
}
