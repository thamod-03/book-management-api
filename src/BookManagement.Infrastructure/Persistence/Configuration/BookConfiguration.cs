using BookManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagement.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
  public void Configure(EntityTypeBuilder<Book> builder)
  {
    builder.ToTable("Books", table =>
        table.HasCheckConstraint(
            "CK_Books_Quantity_NonNegative",
            "\"Quantity\" >= 0"));

    builder.HasKey(b => b.BookId);

    builder.Property(b => b.BookId)
        .ValueGeneratedOnAdd();

    builder.Property(b => b.Name)
        .IsRequired()
        .HasMaxLength(200);

    builder.Property(b => b.Author)
        .IsRequired()
        .HasMaxLength(150);

    builder.Property(b => b.Quantity)
        .IsRequired();
  }
}
