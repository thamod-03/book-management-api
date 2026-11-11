using BookManagement.Application.Books.Interfaces;
using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
  private readonly BookDbContext _context;

  public BookRepository(BookDbContext context)
  {
    _context = context;
  }

  public async Task<Book?> GetByIdAsync(
      int id,
      CancellationToken cancellationToken = default)
  {
    return await _context.Books
        .FirstOrDefaultAsync(
            b => b.BookId == id,
            cancellationToken);
  }

  public async Task<IReadOnlyList<Book>> GetAllAsync(
      CancellationToken cancellationToken = default)
  {
    return await _context.Books
        .AsNoTracking()
        .ToListAsync(cancellationToken);
  }

  public async Task AddAsync(
      Book book,
      CancellationToken cancellationToken = default)
  {
    await _context.Books.AddAsync(book, cancellationToken);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task UpdateAsync(
      Book book,
      CancellationToken cancellationToken = default)
  {
    _context.Books.Update(book);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteAsync(
      Book book,
      CancellationToken cancellationToken = default)
  {
    _context.Books.Remove(book);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
