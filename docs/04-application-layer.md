# 04 – Build the Application layer

## Goal

Create the objects and use cases that describe what the application can do, without coupling them to a specific database.

## 1. Create the folders

Inside `BookManagement.Application`, create:

```text
Books/
├── DTOs/
├── Interfaces/
├── Validators/
└── UseCases/
    ├── CreateBook/
    ├── GetAllBooks/
    ├── GetBookById/
    ├── UpdateBook/
    └── DeleteBook/
```

## 2. Create request and response DTOs

`Books/DTOs/CreateBookRequest.cs`:

```csharp
namespace BookManagement.Application.Books.DTOs;

public sealed record CreateBookRequest(
    string Name,
    string Author,
    int Quantity
);
```

`Books/DTOs/UpdateBookRequest.cs`:

```csharp
namespace BookManagement.Application.Books.DTOs;

public sealed record UpdateBookRequest(
    string Name,
    string Author,
    int Quantity
);
```

`Books/DTOs/BookDto.cs`:

```csharp
namespace BookManagement.Application.Books.DTOs;

public sealed record BookDto(
    int BookId,
    string Name,
    string Author,
    int Quantity
);
```

A DTO (Data Transfer Object) defines the data crossing an application boundary. The create request does not include `BookId`, because the database generates it. The response DTO includes the ID.

## 3. Define the repository abstraction

Create `Books/Interfaces/IBookRepository.cs`:

```csharp
using BookManagement.Domain.Entities;

namespace BookManagement.Application.Books.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Book>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Book book,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Book book,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Book book,
        CancellationToken cancellationToken = default);
}
```

This interface describes the persistence operations the Application layer needs. It does not say how those operations are implemented.

`CancellationToken` allows a caller to signal that an asynchronous operation is no longer needed. Pass the token down to EF Core rather than ignoring it.

## 4. Create the five use cases

Create one class for each operation:

| Use case | Responsibility |
|---|---|
| `CreateBookUseCase` | Create a valid Book, save it through `IBookRepository`, and return a `BookDto` |
| `GetAllBooksUseCase` | Retrieve all books and map them to DTOs |
| `GetBookByIdUseCase` | Retrieve one book; return `null` when it does not exist |
| `UpdateBookUseCase` | Find a book, update it through the domain method, save it, and return a DTO; return `null` if missing |
| `DeleteBookUseCase` | Find a book and delete it; return `false` if missing and `true` when deleted |

Each use case should receive `IBookRepository` through its constructor. This is constructor injection.

The use cases should not depend on `BookRepository` or `BookDbContext`. Those are Infrastructure implementation details.

## 5. Map entities to DTOs

The use cases map a `Book` entity to `BookDto`. For example:

```csharp
var result = new BookDto(
    book.BookId,
    book.Name,
    book.Author,
    book.Quantity);
```

For a collection, map each entity and return a read-only list or another suitable collection of DTOs.

## 6. Understand the dependency inversion

The Application layer owns `IBookRepository`. The Infrastructure layer will implement it.

This means Application defines what it needs, while Infrastructure supplies the technical implementation. The Application project does not need to know whether data is stored in PostgreSQL, SQL Server, or another database.

## Checkpoint

Build the solution:

```powershell
dotnet build
```

Confirm that the Application project references Domain and that no Application class directly references EF Core or Npgsql.
