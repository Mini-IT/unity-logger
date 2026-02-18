# unity-logger
This package allows you to use the [.NET logging API](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging) in Unity.

## Installation
Add the package ([instructions](https://docs.unity3d.com/Manual/upm-ui-giturl.html)) using this Git URL:
```
https://github.com/Mini-IT/unity-logger.git
```
### Install managed DLL from NuGet
The required managed DLL is not included to avoid possible duplication. You need to add it to the project manually. You can extract this DLL from a NuGet package (either [manually](https://stackoverflow.com/a/61187711) or using a tool like [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity))
* [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging)

## Usage
1. Create a [logger factory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory)
   ```cs
   ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddUnityLogger());
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
### Options
You can configure the logger using options, provided by `builder.AddUnityLogger()`:
- `MinimumLogLevelProvider` allows controlling the minimum log level in runtime.
- `StackTraceMapper` allows controlling whether a log output should contain a stack trace.

You can manually implement the needed interfaces or use the predefined classes:
- `IMinimumLogLevelProvider`:
  - `NoneMinimumLogLevelProvider` (used by default) allows all [log levels](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line#log-level)
  - `ConstantMinimumLogLevelProvider` sets an immutable minimum log level value
- `IStackTraceMapper`:
  - `FullStackTraceMapper` applies [StackTraceLogType.Full](https://docs.unity3d.com/ScriptReference/StackTraceLogType.html) to all [Unity LogTypes](https://docs.unity3d.com/ScriptReference/LogType.html)
  - `ScriptOnlyStackTraceMapper` (used by default) applies [StackTraceLogType.ScriptOnly](https://docs.unity3d.com/ScriptReference/StackTraceLogType.html) to all Unity LogTypes
  - `NoneStackTraceMapper` applies [StackTraceLogType.None](https://docs.unity3d.com/ScriptReference/StackTraceLogType.html) to all Unity LogTypes
```csharp
var factory = LoggerFactory.Create(builder => builder.AddUnityLogger(options =>
{
  options.MinimumLogLevelProvider = new ConstantMinimumLogLevelProvider(LogLevel.Error);
  options.StackTraceMapper = new NoneStackTraceMapper();
}));
```

### Example
```cs
using Microsoft.Extensions.Logging;
using MiniIT.Logging.Unity;
using ILogger = Microsoft.Extensions.Logging.ILogger;

public static class LogManager
{
  private static ILoggerFactory s_factory;
  public static ILoggerFactory Factory => s_factory ??= LoggerFactory.Create(builder => builder.AddUnityLogger());

  private static ILogger s_defaultLogger;
  public static ILogger DefaultLogger => s_defaultLogger ??= Factory.CreateLogger("");
}
```
```cs
public class LoggerExample : MonoBehaviour
{
  private ILogger _logger;

  void Start()
  {
    _logger = LogManager.Factory.CreateLogger<LoggerExample>();         // category is the full class name
    //_logger = LogManager.Factory.CreateLogger(nameof(LoggerExample)); // category is the short class name
    //_logger = LogManager.Factory.CreateLogger("LoggerExample");       // custom category

    _logger.LogTrace("Start");
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      _logger.LogTrace("Key pressed at {0}", Time.realtimeSinceStartup);
    }
  }
}
```
```cs
public class DefaultLoggerExample : MonoBehaviour
{
  void Start()
  {
    LogManager.DefaultLogger.LogTrace("Default logger message");
  }
}
```
### Custom MinimumLogLevelProvider example
```cs
public class CustomMinimumLogLevelProvider : IMinimumLogLevelProvider
{
  private LogLevel _level = LogLevel.None;

  public LogLevel GetMinimumLogLevel() => _level;
  public void SetMinimumLogLevel(LogLevel value) => _level = value;
}

public class LogExample
{
  private readonly CustomMinimumLogLevelProvider _minLogLevelProvider = new CustomMinimumLogLevelProvider();
  private readonly ILogger _logger;

  public LogExample()
  {
    var loggerFactory = LoggerFactory.Create(builder => builder.AddUnityLogger(options =>
    {
      options.MinimumLogLevelProvider = _minLogLevelProvider;
    }));

    _logger = loggerFactory.CreateLogger(nameof(LogExample));

    PringLog();
  }

  public void PringLog()
  {
    _logger.LogTrace("This message will be printed");

    _minLogLevelProvider.SetMinimumLogLevel(LogLevel.Error);

    _logger.LogTrace("This message will NOT be printed");
    _logger.LogError("This message will be printed");
  }
}
```

## ZLogger interchangeability
Since this logger and [ZLogger](https://github.com/Cysharp/ZLogger#unity) both use the same API, you can interchange them simply by changing the factory.
```cs
using Microsoft.Extensions.Logging;
using ZLogger;
using Cysharp.Text;

factory = LoggerFactory.Create(builder =>
{
  builder.SetMinimumLevel(LogLevel.Trace);
  builder.AddZLoggerUnityDebug(options =>
  {
    options.PrefixFormatter = (writer, info) => ZString.Utf8Format(writer, "[{0}] ", info.CategoryName);
  });
});
```
