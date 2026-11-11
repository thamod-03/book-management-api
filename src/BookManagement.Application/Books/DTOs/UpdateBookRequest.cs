namespace BookManagement.Application.Books.DTOs;

public sealed record UpdateBookRequest(
    string Name,
    string Author,
    int Quantity
);
