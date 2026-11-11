# 10 – Run and test the API

## Goal

Verify the complete request flow and test all five CRUD endpoints.

## 1. Build the solution

From the repository root:

```powershell
dotnet restore
dotnet build
```

Resolve build errors before running the application.

## 2. Run the API

```powershell
dotnet run --project src/BookManagement.API
```

The actual local URLs are shown in the terminal or configured in:

```text
src/BookManagement.API/Properties/launchSettings.json
```

The example development profile may use:

- `http://localhost:5122`
- `https://localhost:7223`

Your ports may differ. Use the URL printed by your application.

## 3. Open the OpenAPI document

When running in Development, the project maps the OpenAPI document using:

```csharp
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
```

The generated document is commonly available at:

```text
/openapi/v1.json
```

This is an OpenAPI JSON document. To interactively send requests, use a compatible API client such as Visual Studio's tooling, Postman, Bruno, or another REST client. If you add Swagger UI separately, document that setup too.

## 4. Test create

Send:

```http
POST /api/books
Content-Type: application/json
```

Body:

```json
{
  "name": "Clean Code",
  "author": "Robert C. Martin",
  "quantity": 5
}
```

Expected result: `201 Created`, with the created book in the response and a location for the new resource if `CreatedAtAction` is used.

## 5. Test get all

Send:

```http
GET /api/books
```

Expected result: `200 OK` and a JSON array. This project intentionally does not use pagination.

## 6. Test get by ID

Send:

```http
GET /api/books/1
```

Replace `1` with the ID returned by your create request.

Expected result:

- `200 OK` if the book exists.
- `404 Not Found` if it does not exist.

## 7. Test update

Send:

```http
PUT /api/books/1
Content-Type: application/json
```

Body:

```json
{
  "name": "Clean Code - Revised",
  "author": "Robert C. Martin",
  "quantity": 8
}
```

Expected result:

- `200 OK` with the updated book if it exists.
- `404 Not Found` if it does not exist.

## 8. Test delete

Send:

```http
DELETE /api/books/1
```

Expected result:

- `204 No Content` if deleted.
- `404 Not Found` if it does not exist.

## 9. Test validation

Try sending an empty name or a negative quantity to POST or PUT. Expected result: `400 Bad Request` with validation details.

## 10. Test persistence

Create a book, stop the API, start it again, and retrieve the book. It should still exist because it is stored in PostgreSQL rather than only in application memory.

## Suggested manual test checklist

- [ ] Create a book with valid data.
- [ ] Retrieve all books.
- [ ] Retrieve the created book by ID.
- [ ] Update the book.
- [ ] Confirm the update using GET.
- [ ] Delete the book.
- [ ] Confirm GET by ID returns 404.
- [ ] Try invalid request data.
- [ ] Try a non-existing ID.
- [ ] Restart the API and confirm database persistence.

## Checkpoint

The project is functionally complete when all these tests produce the expected responses.
