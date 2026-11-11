# 08 – Add request validation with FluentValidation

## Goal

Validate incoming create and update requests before calling the use cases.

## 1. Install FluentValidation packages

Run from the solution root:

```powershell
dotnet add src/BookManagement.Application package FluentValidation
dotnet add src/BookManagement.API package FluentValidation.DependencyInjectionExtensions
```

If your project already has these packages installed in different projects, keep the package placement consistent with the code that uses them.

## 2. Create the create-request validator

Create `Books/Validators/CreateBookRequestValidator.cs` in Application:

```csharp
using BookManagement.Application.Books.DTOs;
using FluentValidation;

namespace BookManagement.Application.Books.Validators;

public class CreateBookRequestValidator
    : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Book name is required.")
            .MaximumLength(200)
            .WithMessage("Book name cannot exceed 200 characters.");

        RuleFor(x => x.Author)
            .NotEmpty()
            .WithMessage("Author name is required.")
            .MaximumLength(150)
            .WithMessage("Author name cannot exceed 150 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity cannot be negative.");
    }
}
```

## 3. Create the update-request validator

Create `UpdateBookRequestValidator.cs` using the same rules, but target `UpdateBookRequest` instead of `CreateBookRequest`.

Both validators ensure the request follows the API's expected input rules.

## 4. Register validators

In `Program.cs`, register validators from the assembly containing the create validator:

```csharp
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();
```

Make sure the API project has the correct `using` statement for the validator namespace.

## 5. Validate in controller actions

Inject:

```csharp
IValidator<CreateBookRequest>
IValidator<UpdateBookRequest>
```

For a create action, validate the request asynchronously:

```csharp
var validationResult =
    await _createBookValidator.ValidateAsync(
        request,
        cancellationToken);

if (!validationResult.IsValid)
{
    var errors = validationResult.Errors
        .GroupBy(error => error.PropertyName)
        .ToDictionary(
            group => group.Key,
            group => group.Select(error => error.ErrorMessage).ToArray());

    return ValidationProblem(errors);
}
```

Use the update validator in the update action in the same way.

The controller should return the validation problem response without calling the use case when the request is invalid.

## 6. Why keep domain validation too?

FluentValidation provides structured request validation and helpful client-facing errors. The Book entity still enforces its own rules because domain rules must remain protected even if an entity is created from somewhere other than this HTTP API.

## Example invalid request

```json
{
  "name": "",
  "author": "",
  "quantity": -2
}
```

The API should return `400 Bad Request` with a validation problem response that identifies the invalid properties.

## Checkpoint

Test empty names, empty authors, values exceeding the maximum lengths, and negative quantities. Confirm that invalid requests do not create or update database records.
