using System;
using Microsoft.Extensions.Logging;
using UnityEngine;

namespace MiniIT.Logging.Unity
{
	public sealed class UnityLogProcessor
	{
		public bool IsEnabled(LogLevel logLevel)
		{
			return logLevel >= _options.MinLogLevelProvider.GetMinimumLogLevel();
		}

		private readonly UnityLoggerOptions _options;

		public UnityLogProcessor(UnityLoggerOptions options)
		{
			_options = options;
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
				case LogLevel.Warning:
					UnityLog(LogType.Warning, FormatMessage(state, exception, formatter, scopeString));
					break;

				case LogLevel.Critical:
				case LogLevel.Error:
					if (exception != null)
					{
						UnityLog(LogType.Exception, exception);
					}
					else
					{
						UnityLog(LogType.Error, FormatMessage(state, null, formatter, scopeString));
					}
					break;

				case LogLevel.Trace:
				case LogLevel.Debug:
				case LogLevel.Information:
				case LogLevel.None:
				default:
					UnityLog(LogType.Log, FormatMessage(state, exception, formatter, scopeString));
					break;
			}
		}

		private void UnityLog(LogType logType, object message)
		{
			var prevStackLogType = Application.GetStackTraceLogType(logType);
			Application.SetStackTraceLogType(logType, _options.StackTraceConfig.GetStackTraceEnabled(logType));
			Debug.unityLogger.Log(logType, message);
			Application.SetStackTraceLogType(logType, prevStackLogType);
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
