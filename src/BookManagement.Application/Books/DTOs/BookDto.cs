namespace BookManagement.Application.Books.DTOs;

public sealed record BookDto(
    int BookId,
    string Name,
    string Author,
    int Quantity
);
