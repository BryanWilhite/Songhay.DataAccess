# the `Songhay.Northwind.DataAccess` project

The `Songhay.Northwind.DataAccess` project is based off  of Northwind-SQLite3 \[🔗 [GitHub](https://github.com/jpwhite3/northwind-SQLite3) \] by JP White (recommended by `awesome-sqlite` \[🔗 [GitHub](https://github.com/planetopendata/awesome-sqlite) \]). Specifically, what is needed here is the `northwind.db` database [file](https://raw.githubusercontent.com/jpwhite3/northwind-SQLite3/main/dist/northwind.db).

## `dotnet ef dbcontext scaffold` setup

This `northwind.db` database file can be used in a <acronym title="Entity Framework">EF</acronym> database-first, scaffold operation with the `dotnet ef dbcontext scaffold` command (see “[Scaffolding (Reverse Engineering)](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli)”).  The following PowerShell [script](../shell/scaffold-northwind.ps1) was used to generate C♯ models based on `northwind.db` tables:

```powershell
Set-Location $PSScriptRoot

$dbPath = Resolve-Path -Path "../db/northwind.db"
dotnet ef dbcontext scaffold `
    "Data Source=$dbPath" `
    Microsoft.EntityFrameworkCore.Sqlite `
    --project "../Songhay.Northwind.DataAccess/Songhay.Northwind.DataAccess.csproj" `
    -o "../Songhay.Northwind.DataAccess/Models" `
    --force
```

Notice how the `Songhay.Northwind.DataAccess/Models` directory is being targeted here with the `-o` parameter so the bulk of the output, the model files, will be positioned according to conventions. According to the `dotnet ef dbcontext scaffold` command in the script above, a sub-class of `DbContext` will also be generated in the `/Models` folder. This can be used to help quickly hand-edit the final/actual `DbContext` sub-class defined in the `Songhay.Northwind.DataAccess/NorthwindDbContext.cs` [file](./NorthwindDbContext.cs) and then deleted.

[Bryan Wilhite is on LinkedIn](https://www.linkedin.com/in/wilhite)🇺🇸💼
