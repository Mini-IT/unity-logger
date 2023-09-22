# unity-logger
This package allows using [.NET logging API](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging) in Unity.

## Installation
Add the package ([instructions](https://docs.unity3d.com/Manual/upm-ui-giturl.html)) using this Git URL:
```
https://github.com/Mini-IT/AdvertisingIdentifierFetcher.git
```
### Install managed DLLs from NuGet
The dependency managed DLL are not included to avoid possible duplication. You need to add them to the project manually. You can extract the needed dlls from NuGet packages (either [manually](https://stackoverflow.com/a/61187711) or using a tool like [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity))
* [Microsoft.Extensions.Logging.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions/7.0.1)

## Usage
```cs
using Microsoft.Extensions.Logging;
using MiniIT.Logging.Unity;

public static class LogManager
{
  private static ILoggerFactory s_factory;
  public static ILoggerFactory Factory
  {
    get
    {
      s_factory ??= new MiniIT.Logging.Unity.UnityLoggerFactory();
      return s_factory;
    }
  }

  private static ILogger s_defaultLogger;
  public static ILogger DefaultLogger => s_defaultLogger ??= Factory.CreateLogger("");
}

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
