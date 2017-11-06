# Songhay.DataAccess Tests [`net452`]

Since `System.Data.Common` is pretty much useless without [providers](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/working-with-data-providers), this test project concentrates on two, popular, file-based database providers: `System.Data.SqlServerCe.4.0` [[download](https://www.microsoft.com/en-us/download/details.aspx?id=17876)] and `System.Data.SQLite` [[NuGet](https://www.nuget.org/packages/System.Data.SQLite)].

## SQLite engine

The SQLite database engine is available as a download [manually](http://sqlite.org/download.html) or [through Chocolatey](https://chocolatey.org/packages/SQLite) (on Windows). The [unit test](https://github.com/BryanWilhite/Songhay.DataAccess/blob/master/Songhay.DataAccess-net452.Tests/SQLiteTest.cs#L70), `ShouldFindSQLiteDll()`, is a cryptic reminder of this need for the engine.

## testing of `OracleTableMetadata`

The testing around `OracleTableMetadata` is detailed in [additional notes](../Songhay.DataAccess-net452/README.md).

## related links

* “[Installing and Deploying on a Desktop (SQL Server Compact)](https://technet.microsoft.com/en-us/library/bb190958(v=sql.110).aspx)”