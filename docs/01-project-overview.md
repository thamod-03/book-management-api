# 01 – Project overview

## Goal

Build a small Book Management REST API with ASP.NET Core 10. The API stores books in PostgreSQL and supports the four CRUD operations:

- **Create** a book.
- **Read** all books or one book by its ID.
- **Update** an existing book.
- **Delete** a book.

The purpose is to learn the fundamentals of ASP.NET Core Web APIs and a practical four-project Clean Architecture structure.

## What a book contains

This tutorial uses the following fields:

| Field | Type | Meaning |
|---|---|---|
| `BookId` | `int` | Database-generated identifier |
| `Name` | `string` | Book title/name |
| `Author` | `string` | Author's name |
| `Quantity` | `int` | Number of available copies |

The domain rules are simple:

- Name is required.
- Author is required.
- Quantity cannot be negative.

The database mapping additionally limits Name to 200 characters and Author to 150 characters.

## What is CRUD?

CRUD is short for Create, Read, Update, and Delete. These operations commonly map to HTTP methods:

| Operation | HTTP method | Example route |
|---|---|---|
| Create | `POST` | `/api/books` |
| Read collection | `GET` | `/api/books` |
| Read one item | `GET` | `/api/books/1` |
| Update | `PUT` | `/api/books/1` |
| Delete | `DELETE` | `/api/books/1` |

## Why use four projects?

Separating responsibilities helps keep code understandable and maintainable.

1. **Domain** contains business concepts and rules. It should not know about HTTP, EF Core, or Supabase.
2. **Application** coordinates use cases and defines abstractions that it needs, such as a book repository interface.
3. **Infrastructure** provides technical implementations, such as saving and loading books with EF Core.
4. **API** receives HTTP requests, calls application use cases, and returns HTTP responses.

This is a small Clean Architecture example, not a requirement to use this structure for every tiny application. The purpose is to practise separating business logic from technical details.

## Request flow

A typical create request travels through these steps:

```text
HTTP POST /api/books
        |
        v
BooksController
        |
        v
CreateBookUseCase
        |
        v
IBookRepository (Application abstraction)
        |
        v
BookRepository (Infrastructure implementation)
        |
        v
BookDbContext / EF Core
        |
        v
PostgreSQL (Supabase)
```

The controller does not directly write SQL or talk to Supabase. It calls a use case. The use case depends on the repository interface, not on the concrete database repository.

## What you will learn

After completing the tutorial, you should be able to:

- Create and reference .NET projects.
- Define a domain entity with business rules.
- Use records as request and response DTOs.
- Define and implement a repository interface.
- Use EF Core to map entities to PostgreSQL tables.
- Configure dependency injection.
- Create and apply database migrations.
- Build controller-based CRUD endpoints.
- Validate incoming requests.
- Handle unexpected exceptions consistently.
- Pass cancellation tokens through asynchronous operations.

## Out of scope

To keep the project focused, this tutorial does not add pagination, authentication, authorization, caching, file uploads, frontend UI, or deployment. These can be separate follow-up projects.
