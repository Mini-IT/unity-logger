using Microsoft.Extensions.Logging;
using System;

namespace MiniIT.Logging.Unity
{
	public sealed class UnityLogProcessor
	{
		public LogLevel MinLevel { get; set; } = LogLevel.None;

		public bool IsEnabled(LogLevel logLevel)
		{
			return logLevel >= MinLevel;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
			Func<TState, Exception, string> formatter, string scopeString)
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

					UnityEngine.Debug.Log(FormatMessage(state, exception, formatter, scopeString));
					break;

				case LogLevel.Warning:

					UnityEngine.Debug.LogWarning(FormatMessage(state, exception, formatter, scopeString));
					break;

				case LogLevel.Critical:
				case LogLevel.Error:
					if (exception != null)
					{
						UnityEngine.Debug.LogException(exception);
					}
					else
					{
						UnityEngine.Debug.LogError(FormatMessage(state, exception, formatter, scopeString));
					}

					break;

				case LogLevel.None:
				default:
					break;
			}
		}

		private object FormatMessage<TState>(TState state, Exception exception,
			Func<TState, Exception, string> formatter, string scopeString)
		{
			string now = DateTime.UtcNow.ToString("[yyyy-MM-dd HH:mm:ss.fff UTC]");
			string str = formatter.Invoke(state, exception);

			if (string.IsNullOrEmpty(scopeString))
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
					.Format("{0} {1} {2}", now, scopeString, str);
			}
		}
	}
}