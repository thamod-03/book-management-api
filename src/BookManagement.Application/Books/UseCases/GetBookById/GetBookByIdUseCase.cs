using BookManagement.Application.Books.DTOs;
using BookManagement.Application.Books.Interfaces;

namespace BookManagement.Application.Books.UseCases.GetBookById;

public class GetBookByIdUseCase
{
  private readonly IBookRepository _bookRepository;

  public GetBookByIdUseCase(IBookRepository bookRepository)
  {
    _bookRepository = bookRepository;
  }

  public async Task<BookDto?> ExecuteAsync(
      int id,
      CancellationToken cancellationToken = default)
  {
    var book = await _bookRepository.GetByIdAsync(
        id,
        cancellationToken);

    if (book is null)
    {
      return null;
    }

    return new BookDto(
        book.BookId,
        book.Name,
        book.Author,
        book.Quantity
    );
  }
}
