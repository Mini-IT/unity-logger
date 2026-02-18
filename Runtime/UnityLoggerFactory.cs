using System;
using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public class UnityLoggerFactory : ILoggerFactory
	{
		public static UnityLoggerFactory Default => s_instance ??= new UnityLoggerFactory();

		private static UnityLoggerFactory s_instance;

		private ILoggerProvider _provider;
		private LogLevel _minLogLevel = LogLevel.Trace;

		public UnityLoggerFactory()
		{
			s_instance ??= this;

			UnityEngine.Application.quitting += Dispose;
		}

		public UnityLoggerFactory(LogLevel minLogLevel)
			: this()
		{
			_minLogLevel = minLogLevel;
		}

		public static UnityLoggerFactory Create(LogLevel minLogLevel)
		{
			return new UnityLoggerFactory(minLogLevel);
		}

		public void AddProvider(ILoggerProvider provider)
		{
			if (_provider == provider)
			{
				return;
			}

			if (_provider is IDisposable disposable)
			{
				disposable.Dispose();
			}

			_provider = provider;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return GetProvider().CreateLogger(categoryName);
		}

		public void Dispose()
		{
			if (s_instance == this)
			{
				s_instance = null;
			}

			UnityEngine.Application.quitting -= Dispose;
			_provider?.Dispose();
		}

		private ILoggerProvider GetProvider()
		{
			_provider ??= new UnityLoggerProvider(_minLogLevel);
			return _provider;
		}
	}
}
