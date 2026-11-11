using BookManagement.Application.Books.Interfaces;

namespace BookManagement.Application.Books.UseCases.DeleteBook;

public class DeleteBookUseCase
{
  private readonly IBookRepository _bookRepository;

  public DeleteBookUseCase(IBookRepository bookRepository)
  {
    _bookRepository = bookRepository;
  }

  public async Task<bool> ExecuteAsync(
      int id,
      CancellationToken cancellationToken = default)
  {
    var book = await _bookRepository.GetByIdAsync(
        id,
        cancellationToken);

    if (book is null)
    {
      return false;
    }

    await _bookRepository.DeleteAsync(
        book,
        cancellationToken);

    return true;
  }
}
