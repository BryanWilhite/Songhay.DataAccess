# Songhay.DataAccess

These reusable definitions for `System.Data.Common` ([ADO.NET](https://en.wikipedia.org/wiki/ADO.NET)) remind us that the excellent [Entity Framework](https://github.com/aspnet/EntityFramework6) (EF) and [EF Core](https://github.com/aspnet/EntityFrameworkCore) began as an application of the types in this namespace. This package is based on [a project file](https://github.com/BryanWilhite/Songhay.DataAccess/blob/master/Songhay.DataAccess/Songhay.DataAccess.csproj) that supports [multi-targeting](http://gigi.nullneuron.net/gigilabs/multi-targeting-net-standard-class-libraries/), declaring support for `net5.0` and `netstandard2.0`.

This repository represents the work horse that I have driven for over 15 years (depending on my other work horse, `SonghayCore` [[GitHub](https://github.com/BryanWilhite/SonghayCore), [NuGet](https://www.nuget.org/packages/SonghayCore/)]). It is quite a pleasure to finally share this work here on relatively newfangled GitHub.

## version 5.0 breaking changes

The dominant theme in version 5.0 is about dropping direct support for .NET Framework. Microsoft strongly suggests that we support the .NET Framework legacy through targeting .NET Standard 2.0 because .NET Framework 4.71 supports .NET Standard 2.0. Moreover, the course of Entity Framework Core [[GitHub](https://github.com/dotnet/efcore)] dominate breaking changes:

- validation is not baked into Entity Framework Core [[StackOverflow](https://stackoverflow.com/a/43427057/22944)]
- the `DbContext.Configuration` pattern has been replaced by overriding `DbContext.OnConfiguring` with `DbContextOptionsBuilder` [[docs](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontextoptionsbuilder?view=efcore-5.0)]
- support for `ConnectionStringSettings` dropped
- `EntityConnectionStringBuilder` and `ObjectStateManager` is not migrated to .NET 5.0
- `RegistryKeyExtensions` dropped
- Oracle-related `TextTemplating` dropped
- SQL Server Compact testing dropped

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
