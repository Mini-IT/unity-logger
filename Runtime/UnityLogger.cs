using Microsoft.Extensions.Logging;
using System;

namespace MiniIT.Logging.Unity
{
	public class UnityLogger : ILogger
	{
		private readonly string _categoryName;
		private string _scopeString;

		private readonly Func<LogLevel> _getMinLevel;

		public UnityLogger(string categoryName, Func<LogLevel> getMinLevel = null)
		{
			_categoryName = categoryName;
			_getMinLevel = getMinLevel;

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
			if (_getMinLevel == null)
			{
				return logLevel != LogLevel.None;
			}

			return logLevel >= _getMinLevel.Invoke()
				&& logLevel != LogLevel.None;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return;
			}

			switch (logLevel)
			{
				case LogLevel.Trace:
				case LogLevel.Debug:
				case LogLevel.Information:

					UnityEngine.Debug.Log(FormatMessage(state, exception, formatter));
					break;

				case LogLevel.Warning:

					UnityEngine.Debug.LogWarning(FormatMessage(state, exception, formatter));
					break;

				case LogLevel.Critical:
				case LogLevel.Error:
					if (exception != null)
					{
						UnityEngine.Debug.LogException(exception);
					}
					else
					{
						UnityEngine.Debug.LogError(FormatMessage(state, exception, formatter));
					}
					break;

				case LogLevel.None:
				default:
					break;
			}
		}

		private object FormatMessage<TState>(TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			string now = DateTime.UtcNow.ToString("[yyyy-MM-dd HH:mm:ss.fff UTC]");
			string str = formatter.Invoke(state, exception);

			if (string.IsNullOrEmpty(_scopeString))
			{
#if ZSTRING
				return Cysharp.Text.ZString
#else
				return string
#endif
					.Format("{0} {1}", now, str);
			}
			else
			{
#if ZSTRING
				return Cysharp.Text.ZString
#else
				return string
#endif
					.Format("{0} {1} {2}", now, _scopeString, str);
			}
		}
	}
}
