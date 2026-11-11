# Book Management API – ASP.NET Core 10

A beginner-friendly Book Management REST API built with ASP.NET Core 10, Entity Framework Core, PostgreSQL, Supabase, and Clean Architecture.

This repository is both a working example and a step-by-step learning guide. The `docs/` directory explains how to build the application from an empty solution.

## Features

- Create a book
- Retrieve all books
- Retrieve a book by ID
- Update a book
- Delete a book
- Request validation with FluentValidation
- Global exception handling with ASP.NET Core `IExceptionHandler`
- PostgreSQL persistence through Entity Framework Core
- Cancellation-token propagation through asynchronous operations

## Technology stack

- .NET 10 / ASP.NET Core 10
- C#
- Entity Framework Core 10
- PostgreSQL
- Supabase (hosted PostgreSQL)
- Npgsql EF Core provider
- FluentValidation

## Solution structure

```text
BookManagement/
├── src/
│   ├── BookManagement.Domain/
│   ├── BookManagement.Application/
│   ├── BookManagement.Infrastructure/
│   └── BookManagement.API/
├── docs/
├── .gitignore
├── README.md
└── BookManagement.slnx
```

### Project responsibilities

| Project | Responsibility |
|---|---|
| `BookManagement.Domain` | Domain entities and business rules |
| `BookManagement.Application` | DTOs, repository abstractions, validation, and use cases |
| `BookManagement.Infrastructure` | EF Core DbContext, database mapping, and repository implementation |
| `BookManagement.API` | HTTP controllers, dependency injection composition, and middleware |

Dependency direction:

```text
API ───────────────> Application
 │                       │
 └──> Infrastructure ────┴──> Domain
              │
              └──────────────> Application
```

The Domain project does not depend on the other projects. Application depends on Domain. Infrastructure implements abstractions defined by Application and works with Domain entities. API is the composition root that wires the pieces together.

## Prerequisites

- .NET 10 SDK
- Git
- A code editor such as Visual Studio 2026 or Visual Studio Code
- A PostgreSQL database; this tutorial uses Supabase
- Basic familiarity with C# and HTTP

## Getting started

1. Clone the repository.
2. Open a terminal in the repository root.
3. Configure the Supabase connection string using .NET User Secrets. Do not commit credentials.
4. Restore packages and build the solution.
5. Apply the EF Core migration.
6. Run the API and test the endpoints.

The exact commands and explanations are in [`docs/`](docs/).

## Step-by-step learning path

Read these documents in order:

1. [Project overview](docs/01-project-overview.md)
2. [Create the solution](docs/02-solution-setup.md)
3. [Build the Domain layer](docs/03-domain-layer.md)
4. [Build the Application layer](docs/04-application-layer.md)
5. [Build the Infrastructure layer](docs/05-infrastructure-layer.md)
6. [Configure Supabase and migrations](docs/06-database-setup.md)
7. [Build the CRUD API](docs/07-crud-api-endpoints.md)
8. [Add request validation](docs/08-validation.md)
9. [Add global exception handling](docs/09-global-exception-handling.md)
10. [Run and test the API](docs/10-testing-the-api.md)
11. [Publish the project to GitHub](docs/11-publish-to-github.md)

## API endpoints

The API exposes these basic CRUD routes:

| Method | Route | Description | Typical success response |
|---|---|---|---|
| `POST` | `/api/books` | Create a book | `201 Created` |
| `GET` | `/api/books` | Get all books | `200 OK` |
| `GET` | `/api/books/{id}` | Get a book by ID | `200 OK` |
| `PUT` | `/api/books/{id}` | Update a book | `200 OK` |
| `DELETE` | `/api/books/{id}` | Delete a book | `204 No Content` |

A missing book should return `404 Not Found`. Invalid request data should return `400 Bad Request`.

## Configuration and secrets

The API reads the database connection string from:

```text
ConnectionStrings:Supabase
```

For local development, use .NET User Secrets. Never commit a real database password, Supabase service-role key, or production connection string.

## Scope

This is intentionally a simple learning project. It does not include pagination, authentication, authorization, caching, CQRS libraries, message brokers, or deployment automation.

## License

Choose a license before publishing if you want others to reuse or modify the project. For example, the MIT License is a common permissive option. Add a `LICENSE` file only after deciding which license you want to use.
