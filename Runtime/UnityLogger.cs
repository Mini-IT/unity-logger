using Microsoft.Extensions.Logging;
using System;

namespace MiniIT.Logging.Unity
{
	public sealed class UnityLogger : ILogger
	{
		private readonly UnityLogProcessor _logProcessor;
		private readonly string _categoryName;
        
		private string _scopeString;

		public UnityLogger(string categoryName, UnityLogProcessor logProcessor)
		{
			_logProcessor = logProcessor;
			_categoryName = categoryName;

			_scopeString = $"[{_categoryName}]";
		}

		public IDisposable BeginScope<TState>(TState state) where TState : notnull
		{
			string stateString = state?.ToString() ?? string.Empty;
			string scope = string.IsNullOrEmpty(stateString) ? string.Empty : $" ({stateString})";
            
			_scopeString = $"[{_categoryName}]{scope}";
            
			return new NullDisposable();
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return _logProcessor.IsEnabled(logLevel);
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (_logProcessor == null || formatter == null)
			{
				return;
			}

			_logProcessor.Log(logLevel,eventId, state, exception, formatter, _scopeString);
		}
	}
}
