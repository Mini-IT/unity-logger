using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public sealed class UnityLoggerProvider : ILoggerProvider
	{
		private readonly UnityLogProcessor _logProcessor;

		public UnityLoggerProvider(UnityLogProcessor logProcessor)
		{
			_logProcessor = logProcessor;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return new UnityLogger(categoryName, _logProcessor);
		}

		public void Dispose() { }
	}
}
