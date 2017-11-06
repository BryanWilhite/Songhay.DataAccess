# `Songhay.DataAccess net452`

Currently `net452` support has exclusive support for auto-generating POCO .NET classes from metadata for Oracle tables and views. We can see this support in action by looking at the integration tests in the `OracleTableMetadataTest` [class](./Songhay.DataAccess-net452.Tests/OracleTableMetadataTest.cs).

## file properties for `OracleEntityGenerator.tt`

The Visual Studio File Properties for the `OracleEntityGenerator.tt` [template](./TextTemplating/OracleEntityGenerator.tt) _must_ have **Custom Tool** set to `TextTemplatingFilePreprocessor`:

![**Custom Tool** set to `TextTemplatingFilePreprocessor`](../docs/images/custom-tool-TextTemplatingFilePreprocessor.png)

Without this setting, you may see compilation errors like this:

```plaintext
An exception was thrown while trying to compile the transformation code. The following Exception was thrown:
System.IO.FileNotFoundException: Could not find file 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\**Songhay.DataAccess**'.
File name: 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\**Songhay.DataAccess**'
   at System.IO.__Error.WinIOError(Int32 errorCode, String maybeFullPath)
   at System.IO.FileStream.Init(String path, FileMode mode, FileAccess access, Int32 rights, Boolean useRights, FileShare share, Int32 bufferSize, FileOptions options, SECURITY_ATTRIBUTES secAttrs, String msgPath, Boolean bFromProxy, Boolean useLongPath, Boolean checkHost)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share)
   at Roslyn.Utilities.FileUtilities.OpenFileStream(String path)
```

The reference to `Songhay.DataAccess` is in the error because `OracleEntityGenerator.tt` has this line:

```plaintext
<#@ assembly name="Songhay.DataAccess" #>
```