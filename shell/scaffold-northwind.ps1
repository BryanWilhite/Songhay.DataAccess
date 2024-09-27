Set-Location $PSScriptRoot

$dbPath = Resolve-Path -Path "../db/northwind.db"
dotnet ef dbcontext scaffold `
    "Data Source=$dbPath" `
    Microsoft.EntityFrameworkCore.Sqlite `
    --project "../Songhay.Northwind.DataAccess/Songhay.Northwind.DataAccess.csproj" `
    -o "../Songhay.Northwind.DataAccess/Models" `
    --force
