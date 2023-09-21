using System;
using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public class UnityLoggerFactory : ILoggerFactory
	{
		private ILoggerProvider _provider;

		public UnityLoggerFactory()
		{
			UnityEngine.Application.quitting += Dispose;
		}

		public void AddProvider(ILoggerProvider provider)
		{
			_provider = provider;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return GetProvider().CreateLogger(categoryName);
		}

		public void Dispose()
		{
			UnityEngine.Application.quitting -= Dispose;
			_provider?.Dispose();
		}

		private ILoggerProvider GetProvider()
		{
			_provider ??= new UnityLoggerProvider();
			return _provider;
		}
	}

	public class UnityLoggerProvider : ILoggerProvider
	{
		public ILogger CreateLogger(string categoryName)
		{
			return new UnityLogger(categoryName);
		}

		public void Dispose()
		{
		}
	}

	public class UnityLogger : ILogger
	{
		private readonly string _categoryName;

		public UnityLogger(string categoryName)
		{
			_categoryName = categoryName;
		}

		public IDisposable BeginScope<TState>(TState state) where TState : notnull
		{
			return new NullDisposable();
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return logLevel != LogLevel.None;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			switch (logLevel)
			{
				case LogLevel.Trace:
				case LogLevel.Debug:
				case LogLevel.Information:
					
					UnityEngine.Debug.Log(FormatMessage(state, exception, formatter));
					break;

				case LogLevel.Warning:
				case LogLevel.Critical:
					UnityEngine.Debug.LogWarning(FormatMessage(state, exception, formatter));
					break;

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
					break;

				default:
					break;
			}
		}

		private object FormatMessage<TState>(TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			string now = DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm:ss");
			string str = formatter.Invoke(state, exception);

			if (string.IsNullOrEmpty(_categoryName))
			{
#if ZSTRING
				return Cysharp.Text.ZString
#else
				return string
#endif
					.Format("<i>{0} UTC</i> {1}", now, str);
			}
			else
			{
#if ZSTRING
				return Cysharp.Text.ZString
#else
				return string
#endif
					.Format("<i>{0} UTC</i> [{1}] {2}", now, _categoryName, str);
			}
		}
	}

	internal class NullDisposable : IDisposable
	{
		public void Dispose()
		{
		}
	}
}
