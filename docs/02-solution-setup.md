# 02 – Create the solution and projects

## Goal

Create a .NET 10 solution containing four projects and configure the project references.

## Prerequisites

Install the .NET 10 SDK. Confirm the SDK is available:

```powershell
dotnet --version
```

The output should show a `10.0.x` SDK version.

## 1. Create the solution folder

Run these commands in PowerShell:

```powershell
mkdir BookManagement
cd BookManagement

dotnet new sln -n BookManagement
mkdir src
cd src
```

The solution uses the modern `.slnx` format when created by a .NET SDK that defaults to it. If your SDK creates a `.sln` file instead, that is also supported; use the filename generated on your machine consistently.

## 2. Create the four projects

```powershell
dotnet new classlib -n BookManagement.Domain -f net10.0
dotnet new classlib -n BookManagement.Application -f net10.0
dotnet new classlib -n BookManagement.Infrastructure -f net10.0
dotnet new webapi -n BookManagement.API -f net10.0 --use-controllers
```

The `--use-controllers` option creates a controller-based Web API template.

## 3. Add projects to the solution

Return to the solution root:

```powershell
cd ..
```

If your solution is named `BookManagement.slnx`, run:

```powershell
dotnet sln BookManagement.slnx add src/BookManagement.Domain
dotnet sln BookManagement.slnx add src/BookManagement.Application
dotnet sln BookManagement.slnx add src/BookManagement.Infrastructure
dotnet sln BookManagement.slnx add src/BookManagement.API
```

If `dotnet new sln` created `BookManagement.sln` instead, replace the filename in the commands.

## 4. Add project references

Run from the solution root:

```powershell
dotnet add src/BookManagement.Application reference src/BookManagement.Domain

dotnet add src/BookManagement.Infrastructure reference src/BookManagement.Application
dotnet add src/BookManagement.Infrastructure reference src/BookManagement.Domain

dotnet add src/BookManagement.API reference src/BookManagement.Application
dotnet add src/BookManagement.API reference src/BookManagement.Infrastructure
```

The intended dependencies are:

- Domain -> no project references.
- Application -> Domain.
- Infrastructure -> Application and Domain.
- API -> Application and Infrastructure.

## 5. Remove template files you do not need

The class-library projects may contain `Class1.cs`. Delete those placeholder files.

The Web API template may contain sample weather-related files. Remove them if they are not used by your project.

## 6. Build the solution

```powershell
dotnet restore
dotnet build
```

Fix any errors before continuing.

## Checkpoint

Your solution should contain these projects:

```text
BookManagement/
├── src/
│   ├── BookManagement.Domain/
│   ├── BookManagement.Application/
│   ├── BookManagement.Infrastructure/
│   └── BookManagement.API/
└── BookManagement.slnx
```

You have completed this lesson when all four projects are in the solution, the references match the dependency list above, and `dotnet build` succeeds.
