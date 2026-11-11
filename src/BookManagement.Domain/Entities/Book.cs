namespace  BookManagement.Domain.Entities;

public class Book
{
  public int BookId { get; private set; }
  public string Name { get; private set; }
  public string Author { get; private set; }
  public int Quantity { get; private set; }

  public Book(string Name, string Author, int Quantity)
  {
    if (string.IsNullOrWhiteSpace(Name))
    {
      throw new ArgumentException("Book name cannot be null or empty.", nameof(Name));
    }

    if (string.IsNullOrWhiteSpace(Author))
    {
      throw new ArgumentException("Author name cannot be null or empty.", nameof(Author));
    }

    if (Quantity < 0)
    {
      throw new ArgumentException("Quantity cannot be negative.", nameof(Quantity));
    }

    this.Name = Name;
    this.Author = Author;
    this.Quantity = Quantity;
  }


  public void update(string name, string author, int quantity)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      throw new ArgumentException("Book name cannot be null or empty.", nameof(name));
    }

    if (string.IsNullOrWhiteSpace(author))
    {
      throw new ArgumentException("Author name cannot be null or empty.", nameof(author));
    }

    if (quantity < 0)
    {
      throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
    }

    this.Name = name;
    this.Author = author;
    this.Quantity = quantity;
  }
}
