using BookManagement.Application.Books.DTOs;
using BookManagement.Application.Books.UseCases.CreateBook;
using BookManagement.Application.Books.UseCases.GetAllBooks;
using BookManagement.Application.Books.UseCases.GetBookById;
using BookManagement.Application.Books.UseCases.UpdateBook;
using BookManagement.Application.Books.UseCases.DeleteBook;
using Microsoft.AspNetCore.Mvc;
using BookManagement.Application.Books.Validators;
using FluentValidation;

namespace BookManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
  private readonly CreateBookUseCase _createBookUseCase;
  private readonly GetAllBooksUseCase _getAllBooksUseCase;
  private readonly GetBookByIdUseCase _getBookByIdUseCase;
  private readonly UpdateBookUseCase _updateBookUseCase;
  private readonly DeleteBookUseCase _deleteBookUseCase;
  private readonly IValidator<CreateBookRequest> _createBookValidator;
  private readonly IValidator<UpdateBookRequest> _updateBookValidator;

  public BooksController(
      CreateBookUseCase createBookUseCase,
      GetAllBooksUseCase getAllBooksUseCase,
      GetBookByIdUseCase getBookByIdUseCase,
      UpdateBookUseCase updateBookUseCase,
      DeleteBookUseCase deleteBookUseCase,
      IValidator<CreateBookRequest> createBookValidator,
      IValidator<UpdateBookRequest> updateBookValidator)
  {
    _createBookUseCase = createBookUseCase;
    _getAllBooksUseCase = getAllBooksUseCase;
    _getBookByIdUseCase = getBookByIdUseCase;
    _updateBookUseCase = updateBookUseCase;
    _deleteBookUseCase = deleteBookUseCase;
    _createBookValidator = createBookValidator;
    _updateBookValidator = updateBookValidator;
  }

  // POST: api/books
  [HttpPost]
  public async Task<IActionResult> CreateBook(
      [FromBody] CreateBookRequest request,
      CancellationToken cancellationToken)
  {
    var validationResult = await _createBookValidator.ValidateAsync(
      request,
      cancellationToken);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors
          .GroupBy(error => error.PropertyName)
          .ToDictionary(
              group => group.Key,
              group => group.Select(error => error.ErrorMessage).ToArray()
          );

      return ValidationProblem(new ValidationProblemDetails(errors));
    }

    var result = await _createBookUseCase.ExecuteAsync(
          request,
          cancellationToken);

      return Created(
          $"/api/books/{result.BookId}",
          result);
  }

  // GET: api/books
  [HttpGet]
  public async Task<IActionResult> GetAllBooks(
      CancellationToken cancellationToken)
  {
    var books = await _getAllBooksUseCase.ExecuteAsync(
        cancellationToken);

    return Ok(books);
  }

  // GET: api/books/{id}
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetBookById(
      int id,
      CancellationToken cancellationToken)
  {
    var book = await _getBookByIdUseCase.ExecuteAsync(
        id,
        cancellationToken);

    if (book is null)
    {
      return NotFound(new
      {
        message = $"Book with ID {id} was not found."
      });
    }

    return Ok(book);
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> UpdateBook(
    int id,
    [FromBody] UpdateBookRequest request,
    CancellationToken cancellationToken)
  {
    var validationResult = await _updateBookValidator.ValidateAsync(
        request,
        cancellationToken);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors
          .GroupBy(error => error.PropertyName)
          .ToDictionary(
              group => group.Key,
              group => group.Select(error => error.ErrorMessage).ToArray()
          );

      return ValidationProblem(new ValidationProblemDetails(errors));
    }

    var result = await _updateBookUseCase.ExecuteAsync(
        id,
        request,
        cancellationToken);

    if (result is null)
    {
      return NotFound(new
      {
        message = $"Book with ID {id} was not found."
      });
    }

    return Ok(result);
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteBook(
    int id,
    CancellationToken cancellationToken)
  {
    var deleted = await _deleteBookUseCase.ExecuteAsync(
        id,
        cancellationToken);

    if (!deleted)
    {
      return NotFound(new
      {
        message = $"Book with ID {id} was not found."
      });
    }

    return NoContent();
  }
}
