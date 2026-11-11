namespace BookManagement.Application.Books.DTOs;

public sealed record CreateBookRequest(
    string Name,
    string Author,
    int Quantity
);
