using BookManagement.Application.Books.DTOs;
using BookManagement.Domain.Entities;
using BookManagement.Application.Books.Interfaces;

namespace BookManagement.Application.Books.UseCases.CreateBook;

public class CreateBookUseCase
{
  private readonly IBookRepository _bookRepository;

  public CreateBookUseCase(IBookRepository bookRepository)
  {
    _bookRepository = bookRepository;
  }

  public async Task<BookDto> ExecuteAsync(CreateBookRequest request, CancellationToken cancellationToken = default)
  {
    var book = new Book(request.Name, request.Author, request.Quantity);

    await _bookRepository.AddAsync(book, cancellationToken);

    var bookDto = new BookDto(book.BookId, book.Name, book.Author, book.Quantity);

    return bookDto;
  }
}
