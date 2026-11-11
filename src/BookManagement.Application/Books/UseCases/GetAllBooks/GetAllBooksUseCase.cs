using BookManagement.Application.Books.DTOs;
using BookManagement.Application.Books.Interfaces;

namespace BookManagement.Application.Books.UseCases.GetAllBooks;

public class GetAllBooksUseCase
{
  private readonly IBookRepository _bookRepository;

  public GetAllBooksUseCase(IBookRepository bookRepository)
  {
    _bookRepository = bookRepository;
  }

  public async Task<IReadOnlyList<BookDto>> ExecuteAsync(
      CancellationToken cancellationToken = default)
  {
    var books = await _bookRepository.GetAllAsync(cancellationToken);

    return books.Select(book => new BookDto(
        book.BookId,
        book.Name,
        book.Author,
        book.Quantity
    )).ToList();
  }
}
