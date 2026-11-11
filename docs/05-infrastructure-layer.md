# 05 – Build the Infrastructure layer

## Goal

Implement the repository abstraction using Entity Framework Core. Infrastructure is responsible for database-specific details.

## 1. Install packages

From the solution root, install the PostgreSQL EF Core provider in the Infrastructure project:

```powershell
dotnet add src/BookManagement.Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL
```

Install the EF Core design package in the API startup project:

```powershell
dotnet add src/BookManagement.API package Microsoft.EntityFrameworkCore.Design
```

Use compatible stable package versions for .NET 10. Check the package versions selected by NuGet and keep EF Core tooling/provider versions compatible.

## 2. Create the DbContext

Create `Persistence/BookDbContext.cs` in Infrastructure:

```csharp
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
```

A `DbContext` represents a session with the database. `DbSet<Book>` represents the collection of books that EF Core maps to a database table.

## 3. Configure the Book table

Create `Persistence/Configurations/BookConfiguration.cs`:

```csharp
using BookManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagement.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books", table =>
            table.HasCheckConstraint(
                "CK_Books_Quantity_NonNegative",
                "\"Quantity\" >= 0"));

        builder.HasKey(b => b.BookId);

        builder.Property(b => b.BookId)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.Quantity)
            .IsRequired();
    }
}
```

This configuration defines the table name, primary key, generated ID, required columns, maximum text lengths, and a database check constraint.

The quoted column names in the check constraint are PostgreSQL-compatible for the column names used here. If you change table or column naming conventions, update the constraint accordingly.

## 4. Implement the repository

Create `Repositories/BookRepository.cs` and implement `IBookRepository`.

The implementation should use `BookDbContext`:

- `GetByIdAsync`: query by `BookId` and return the matching book or `null`.
- `GetAllAsync`: use `AsNoTracking()` and return all books.
- `AddAsync`: call `AddAsync` and `SaveChangesAsync`.
- `UpdateAsync`: mark the entity as updated if needed and call `SaveChangesAsync`.
- `DeleteAsync`: remove the entity and call `SaveChangesAsync`.

Pass the supplied `CancellationToken` to every EF Core asynchronous operation that accepts one. For example, use `FirstOrDefaultAsync(cancellationToken)`, `ToListAsync(cancellationToken)`, and `SaveChangesAsync(cancellationToken)`.

`AsNoTracking()` is suitable for read-only collection queries because EF Core does not need to track the returned entities for updates.

## 5. Register Infrastructure services

Create `DependencyInjection.cs` in the Infrastructure project:

```csharp
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
```

This extension method keeps Infrastructure registration in one place. `AddScoped` creates one repository instance per request scope, which is a common lifetime for EF Core repositories.

## Checkpoint

Build the solution:

```powershell
dotnet build
```

The Infrastructure project should implement the Application repository interface and register both the DbContext and repository.
