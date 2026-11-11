# 11 – Publish the project to GitHub

## Goal

Publish the working API and its learning documents without exposing secrets.

## 1. Review the repository

Before committing, confirm that the repository contains:

- `src/` with the four projects.
- `docs/` with the numbered learning documents.
- `README.md`.
- The solution file (`.slnx` or `.sln`).
- A suitable `.gitignore`.

## 2. Check for secrets

Inspect configuration files and staged changes. Never commit:

- Supabase database passwords.
- Full production connection strings.
- Supabase service-role keys.
- API keys or access tokens.
- Personal credentials.

Use .NET User Secrets locally. For deployment, use the hosting platform's secret/environment-variable configuration.

If a secret was committed, deleting it in a later commit is not enough: rotate/revoke the exposed credential immediately and clean repository history if appropriate.

## 3. Add a .gitignore

A .NET `.gitignore` should exclude build outputs, IDE state, and local secrets. You can start with the `.gitignore` included at the repository root in this learning package.

## 4. Initialize Git

Run these commands from the repository root if Git has not already been initialized:

```powershell
git init
git status
```

Review the files shown by `git status`.

## 5. Stage and commit

```powershell
git add .
git status
git commit -m "Build Book Management CRUD API with ASP.NET Core 10"
```

Review the staged changes before committing. Do not proceed if credentials or unwanted local files appear.

## 6. Create a GitHub repository

1. Sign in to GitHub.
2. Create a new repository, for example `BookManagement`.
3. Choose public or private visibility.
4. If you already have a local README, it is simplest to create the remote repository without generating another README, license, or `.gitignore`.
5. Copy the repository's remote URL.

## 7. Connect and push

Replace the URL below with your repository URL:

```powershell
git branch -M main
git remote add origin https://github.com/YOUR-USERNAME/BookManagement.git
git push -u origin main
```

If `origin` already exists, inspect it with `git remote -v` rather than adding it a second time.

## 8. Verify the GitHub repository

Open the repository in your browser and confirm:

- README displays correctly.
- The `docs/` folder contains every lesson.
- Source projects are present.
- No secrets or local build output were uploaded.
- Links in the README work.

## 9. Invite friends to learn

Tell learners to:

1. Read the README.
2. Follow documents `01` through `11` in order.
3. Build each layer themselves rather than copying everything at once.
4. Use their own Supabase project and User Secrets.
5. Test each endpoint before moving to the next lesson.

## Optional: add a license

If you want others to reuse, modify, and redistribute the code, choose a license and add its official text in a `LICENSE` file. Do not claim a license in the README until you have added the corresponding license file.
