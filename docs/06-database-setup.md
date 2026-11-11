# 06 – Configure Supabase PostgreSQL and migrations

## Goal

Connect the API to a Supabase PostgreSQL database and create the `Books` table through an EF Core migration.

## 1. Create a Supabase project

1. Sign in to Supabase.
2. Create a project.
3. Set and securely store the database password.
4. Wait for the database to finish provisioning.
5. Open the project's database connection settings and choose a connection method suitable for your environment.

Supabase offers different connection options, including direct and pooler connections. Copy the connection details from your own project's dashboard. Do not guess the host, username, port, or password.

## 2. Store the connection string with User Secrets

From the solution root, initialize User Secrets for the API project:

```powershell
dotnet user-secrets init --project src/BookManagement.API
```

Set the connection string:

```powershell
dotnet user-secrets set "ConnectionStrings:Supabase" "Host=YOUR_HOST;Port=YOUR_PORT;Database=postgres;Username=YOUR_USERNAME;Password=YOUR_PASSWORD;SSL Mode=Require;" --project src/BookManagement.API
```

Replace the placeholders with the exact values from Supabase. The port and username depend on the connection option you selected.

Do not paste real credentials into source files, screenshots, public issues, or GitHub commits. Do not share your database password in a public repository.

## 3. Install the EF Core command-line tool

Install a .NET 10-compatible `dotnet-ef` tool version:

```powershell
dotnet tool install --global dotnet-ef
```

If it is already installed, update it:

```powershell
dotnet tool update --global dotnet-ef
```

Check it:

```powershell
dotnet ef --version
```

Keep the EF Core tools, design package, and provider compatible.

## 4. Create the first migration

Run from the solution root:

```powershell
dotnet ef migrations add InitialCreate --project src/BookManagement.Infrastructure --startup-project src/BookManagement.API --output-dir Persistence/Migrations
```

Why are two projects specified?

- `--project` identifies where EF Core should place the migration.
- `--startup-project` identifies the executable project that supplies configuration and application services at design time.

The migration should be generated under:

```text
src/BookManagement.Infrastructure/Persistence/Migrations/
```

## 5. Apply the migration

```powershell
dotnet ef database update --project src/BookManagement.Infrastructure --startup-project src/BookManagement.API
```

This applies the migration to the database configured by your API's connection string.

## 6. Verify the table

Open the Supabase Table Editor and confirm that the `Books` table exists. You should see the columns corresponding to the Book entity and its EF Core configuration.

## Troubleshooting

### Connection string is missing

Check that User Secrets were initialized for the API project and that the key is exactly:

```text
ConnectionStrings:Supabase
```

### Authentication or connection failure

Recheck the host, port, database, username, password, and SSL settings against the connection details shown in Supabase. If using a pooler, use the pooler-specific credentials and port.

### EF Core tool or provider version mismatch

Check:

```powershell
dotnet ef --version
dotnet list src/BookManagement.API package
dotnet list src/BookManagement.Infrastructure package
```

Use compatible EF Core 10 versions for the design package, tooling, and provider.

## Checkpoint

You have completed this lesson when the migration exists, `database update` succeeds, and the `Books` table is visible in Supabase.
