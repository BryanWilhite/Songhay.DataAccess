# Songhay.DataAccess

These reusable definitions for `System.Data.Common` ([ADO.NET](https://en.wikipedia.org/wiki/ADO.NET)) remind us that the excellent [Entity Framework](https://github.com/aspnet/EntityFramework6) (EF) and [EF Core](https://github.com/aspnet/EntityFrameworkCore) began as an application of the types in this namespace. Microsoft preserves the 2008 article, “[Writing Generic Data Access Code in ASP.NET 2.0 and ADO.NET 2.0](https://learn.microsoft.com/en-us/previous-versions/dotnet/articles/ms971499(v=msdn.10)),” which provides some historical and contextual background to the work done here. The following from this 2008 article asserts the main motivation for this “common” database approach:

>Writing generic data access code is especially important in data-driven Web applications because data comes from many different sources, including Microsoft SQL Server, Oracle, XML documents, flat files, and Web services, just to name a few.

What was not mentioned above is [SQLite](https://www.sqlite.org/) which is the primary focus of this Studio. It follows that the `Microsoft.Data.Sqlite` NuGet [package](https://www.nuget.org/packages/Microsoft.Data.Sqlite) is installed and supported here. This package was chosen over the [original SQLite package](https://www.nuget.org/packages/System.Data.SQLite), `System.Data.SQLite`, for reasons described in “[Comparison to System.Data.SQLite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/compare).”

## the `Common*` utilities

This Solution features the `Common*` utilities of reusable routines around:

- `System.Data.IDbConnection` [[docs](https://docs.microsoft.com/en-us/dotnet/api/system.data.IDbConnection?view=netcore-2.0)]
- `System.Data.Common.DbParameter` [[docs](https://docs.microsoft.com/en-us/dotnet/api/system.data.common.dbparameter?view=netcore-2.0)]
- `System.Data.IDataReader` [[docs](https://docs.microsoft.com/en-us/dotnet/api/system.data.IDataReader?view=netcore-2.0)]

The interfaces `IDbConnection` and `IDataReader` are implemented in .NET as `System.Data.Common.DbConnection` [[docs](https://docs.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection?view=netcore-2.0)] and `System.Data.Common.DbDataReader` [[docs](https://docs.microsoft.com/en-us/dotnet/api/system.data.common.dbdatareader?view=netcore-2.0)], respectively.

With respect to the list above, these `Common*` utilities are:

- the `CommonDbmsUtility` [class](https://github.com/BryanWilhite/Songhay.DataAccess/blob/master/Songhay.DataAccess/CommonDbmsUtility.cs) and the `CommonScalarUtility` [class](https://github.com/BryanWilhite/Songhay.DataAccess/blob/master/Songhay.DataAccess/CommonScalarUtility.cs)
- the `CommonParameterUtility` [class](https://github.com/BryanWilhite/Songhay.DataAccess/blob/master/Songhay.DataAccess/CommonParameterUtility.cs)
- the `CommonReaderUtility` [class](https://github.com/BryanWilhite/Songhay.DataAccess/blob/master/Songhay.DataAccess/CommonReaderUtility.cs)

## the Extensions

The [extension method classes](https://github.com/BryanWilhite/Songhay.DataAccess/tree/master/Songhay.DataAccess/Extensions) of this Solution, feature routines around `IDbDataReader` for `XObject` and `JObject`.

## related links

- ADO.NET [[docs](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/)]
- Entity Framework Overview [[docs](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/overview)]

@[BryanWilhite](https://twitter.com/bryanwilhite)
