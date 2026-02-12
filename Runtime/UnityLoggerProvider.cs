using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public class UnityLoggerProvider : ILoggerProvider
	{
		private readonly LogLevel _minLogLevel;

		public UnityLoggerProvider()
			: this(LogLevel.Trace)
		{
		}

		public UnityLoggerProvider(LogLevel minLogLevel)
		{
			_minLogLevel = minLogLevel;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return new UnityLogger(categoryName, _minLogLevel);
		}

		public void Dispose()
		{
		}
	}
}
