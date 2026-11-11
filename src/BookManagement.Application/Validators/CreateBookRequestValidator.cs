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
