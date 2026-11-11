using BookManagement.Domain.Entities;

namespace BookManagement.Application.Books.Interfaces;

public interface IBookRepository
{
  Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

  Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);

  Task AddAsync(Book book, CancellationToken cancellationToken = default);

  Task UpdateAsync(Book book, CancellationToken cancellationToken = default);

  Task DeleteAsync(Book book, CancellationToken cancellationToken = default);
}
