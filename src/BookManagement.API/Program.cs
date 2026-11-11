using BookManagement.API.Exceptions;
using BookManagement.Application.Books.UseCases.CreateBook;
using BookManagement.Application.Books.UseCases.GetAllBooks;
using BookManagement.Application.Books.UseCases.GetBookById;
using BookManagement.Application.Books.UseCases.UpdateBook;
using BookManagement.Application.Books.UseCases.DeleteBook;
using BookManagement.Application.Books.Validators;
using BookManagement.Infrastructure;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register use cases
builder.Services.AddScoped<CreateBookUseCase>();
builder.Services.AddScoped<GetAllBooksUseCase>();
builder.Services.AddScoped<GetBookByIdUseCase>();
builder.Services.AddScoped<UpdateBookUseCase>();
builder.Services.AddScoped<DeleteBookUseCase>();

// Register Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<
    CreateBookRequestValidator>();

// Register ProblemDetails service
builder.Services.AddProblemDetails();

// Register Global Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Activate Global Exception Handler
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
