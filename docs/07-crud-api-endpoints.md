# 07 – Build the CRUD API endpoints

## Goal

Expose the Application use cases through an ASP.NET Core controller.

## 1. Create BooksController

Create `Controllers/BooksController.cs` in the API project.

Use:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    // Inject the five book use cases through the constructor.
    // Add one action for each CRUD endpoint.
}
```

The `[ApiController]` attribute enables API-specific conventions. `[Route("api/[controller]")]` maps this controller to `/api/books`.

Inject these use cases through the constructor:

- `CreateBookUseCase`
- `GetAllBooksUseCase`
- `GetBookByIdUseCase`
- `UpdateBookUseCase`
- `DeleteBookUseCase`

Store injected dependencies in `private readonly` fields. A `private` field is accessible only inside the class. `readonly` prevents the field from being reassigned after construction.

## 2. Implement the endpoints

Use the following endpoint contract:

### Create a book

```http
POST /api/books
Content-Type: application/json
```

Example request:

```json
{
  "name": "Clean Code",
  "author": "Robert C. Martin",
  "quantity": 5
}
```

Call `CreateBookUseCase` and return `201 Created`. Prefer `CreatedAtAction` so the response includes a location pointing to the newly created book.

### Get all books

```http
GET /api/books
```

Call `GetAllBooksUseCase` and return `200 OK` with a collection of `BookDto` objects. This tutorial intentionally returns the full collection without pagination.

### Get a book by ID

```http
GET /api/books/1
```

Call `GetBookByIdUseCase`. Return `200 OK` when found and `404 Not Found` when the use case returns `null`.

### Update a book

```http
PUT /api/books/1
Content-Type: application/json
```

Example request:

```json
{
  "name": "Clean Code – Updated Edition",
  "author": "Robert C. Martin",
  "quantity": 8
}
```

Call `UpdateBookUseCase`. Return `200 OK` with the updated DTO, or `404 Not Found` if the ID does not exist.

### Delete a book

```http
DELETE /api/books/1
```

Call `DeleteBookUseCase`. Return `204 No Content` when deleted, or `404 Not Found` if the ID does not exist.

## 3. Register use cases

In `Program.cs`, register the use cases with dependency injection:

```csharp
builder.Services.AddScoped<CreateBookUseCase>();
builder.Services.AddScoped<GetAllBooksUseCase>();
builder.Services.AddScoped<GetBookByIdUseCase>();
builder.Services.AddScoped<UpdateBookUseCase>();
builder.Services.AddScoped<DeleteBookUseCase>();
```

Also register Infrastructure:

```csharp
builder.Services.AddInfrastructure(builder.Configuration);
```

The exact `using` statements depend on the namespaces used in your project.

## 4. Pass cancellation tokens

Add a `CancellationToken cancellationToken` parameter to asynchronous controller actions and pass it to the use case. The use case should pass it to repository methods, which pass it to EF Core.

ASP.NET Core binds the action token to the request-aborted token. If a client disconnects, the operation can observe the cancellation request.

## 5. Understand the HTTP status codes

| Status | Meaning | Used here |
|---|---|---|
| `200 OK` | Request succeeded | GET and successful PUT |
| `201 Created` | Resource created | POST |
| `204 No Content` | Request succeeded without a response body | DELETE |
| `400 Bad Request` | Request data is invalid | Validation failure |
| `404 Not Found` | Resource does not exist | GET/PUT/DELETE by ID |
| `500 Internal Server Error` | Unexpected server error | Unhandled exception |

## Checkpoint

Run the API and confirm that all five routes are available. Test them using the OpenAPI page or a REST client. The next lesson adds explicit FluentValidation rules for request DTOs.
