using BookManagement.Application.Books.DTOs;
using BookManagement.Application.Books.Interfaces;

namespace BookManagement.Application.Books.UseCases.UpdateBook;

public class UpdateBookUseCase
{
  private readonly IBookRepository _bookRepository;

  public UpdateBookUseCase(IBookRepository bookRepository)
  {
    _bookRepository = bookRepository;
  }

  public async Task<BookDto?> ExecuteAsync(
      int id,
      UpdateBookRequest request,
      CancellationToken cancellationToken = default)
  {
    var book = await _bookRepository.GetByIdAsync(
        id,
        cancellationToken);

    if (book is null)
    {
      return null;
    }

    book.update(
        request.Name,
        request.Author,
        request.Quantity
    );

    await _bookRepository.UpdateAsync(
        book,
        cancellationToken);

    return new BookDto(
        book.BookId,
        book.Name,
        book.Author,
        book.Quantity
    );
  }
}
