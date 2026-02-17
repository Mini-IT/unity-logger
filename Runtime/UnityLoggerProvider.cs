using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public class UnityLoggerProvider : ILoggerProvider
	{
		private LogLevel _minimumLevel = LogLevel.Trace;

		public LogLevel MinimumLevel
		{
			get => _minimumLevel;
			set => _minimumLevel = value;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return new UnityLogger(categoryName, GetLogLevel);
		}

		public void Dispose() { }

		private LogLevel GetLogLevel()
		{
			return _minimumLevel;
		}
	}
}
