# unity-logger
This package allows using of the [.NET logging API](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging) in Unity.

## Installation
Add the package ([instructions](https://docs.unity3d.com/Manual/upm-ui-giturl.html)) using this Git URL:
```
https://github.com/Mini-IT/unity-logger.git
```
### Install managed DLLs from NuGet
The dependency managed DLL are not included to avoid possible duplication. You need to add them to the project manually. You can extract the needed dlls from NuGet packages (either [manually](https://stackoverflow.com/a/61187711) or using a tool like [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity))
* [Microsoft.Extensions.Logging.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions/7.0.1)

## Usage
1. Create a [logger factory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory)
   ```cs
   ILoggerFactory factory = new MiniIT.Logging.Unity.UnityLoggerFactory();
   ```
2. Create a [logger](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger)
   ```cs
   ILogger logger = factory.CreateLogger("MyCategory");
   ```
3. Write a log message with a specified [log level](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line#log-level)
   ```cs
   logger.LogTrace("Example trace");
   logger.LogWarning("Example warning");
   logger.LogError("Example error");
   ```
4. Change minimum log level in runtime
   ```cs
   UnityLoggerFactory unityFactory = (UnityLoggerFactory)factory;
   unityFactory.SetMinimumLevel(LogLevel.Warning); // Trace/Debug/Information are filtered
   unityFactory.SetMinimumLevel(LogLevel.Trace);   // enable all logs again
   unityFactory.SetMinimumLevel(LogLevel.None);    // disable all logs
   ```
### Example
```cs
using Microsoft.Extensions.Logging;
using MiniIT.Logging.Unity;
using ILogger = Microsoft.Extensions.Logging.ILogger;

public static class LogManager
{
  private static UnityLoggerFactory s_factory;
  public static UnityLoggerFactory Factory => s_factory ??= new UnityLoggerFactory();

  private static ILogger s_defaultLogger;
  public static ILogger DefaultLogger => s_defaultLogger ??= Factory.CreateLogger("");
}

public class LoggerExample : MonoBehaviour
{
  private ILogger _logger;

  void Start()
  {
    LogManager.Factory.SetMinimumLevel(LogLevel.Information);

    _logger = LogManager.Factory.CreateLogger<LoggerExample>();         // category is the full class name
    //_logger = LogManager.Factory.CreateLogger(nameof(LoggerExample)); // category is the short class name
    //_logger = LogManager.Factory.CreateLogger("LoggerExample");       // custom category

    _logger.LogTrace("Start");
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      _logger.LogTrace("Space pressed at {0}", Time.realtimeSinceStartup);
    }
  }
}

public class DefaultLoggerExample : MonoBehaviour
{
  void Start()
  {
    LogManager.DefaultLogger.LogTrace("Default logger message");
  }
}
```

`SetMinimumLevel(...)` updates filtering for both new and existing logger instances created by the same `UnityLoggerProvider`.

## ZLogger
Since this logger and [ZLogger](https://github.com/Cysharp/ZLogger#unity) both use the same API they can be interchanged by simply changing the factory.
```cs
using Microsoft.Extensions.Logging;
using ZLogger;
using Cysharp.Text;

factory = ZLogger.UnityLoggerFactory.Create(builder =>
{
  builder.SetMinimumLevel(LogLevel.Trace);
  builder.AddZLoggerUnityDebug(options =>
  {
    options.PrefixFormatter = (writer, info) => ZString.Utf8Format(writer, "[{0}] ", info.CategoryName);
  });
});
```
