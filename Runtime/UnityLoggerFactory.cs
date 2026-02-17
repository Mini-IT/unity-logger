using System;
using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public class UnityLoggerFactory : ILoggerFactory
	{
		public static UnityLoggerFactory Default
		{
			get
			{
				s_logProcessor ??= new UnityLogProcessor();
				return s_instance ??= new UnityLoggerFactory(s_logProcessor);
			}
		}

		private static UnityLoggerFactory s_instance;
		private static UnityLogProcessor s_logProcessor;
        
		private ILoggerProvider _provider;

		public UnityLoggerFactory(UnityLogProcessor logProcessor)
		{
			s_logProcessor = logProcessor ?? s_logProcessor ?? new UnityLogProcessor();

			// If the default factory was created before DI configured the processor,
			// reset provider so it gets rebuilt with the latest processor.
			if (s_instance != null && s_instance._provider != null)
			{
				s_instance._provider.Dispose();
				s_instance._provider = null;
			}

			s_instance ??= this;

			UnityEngine.Application.quitting += Dispose;
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
			s_logProcessor ??= new UnityLogProcessor();
			_provider ??= new UnityLoggerProvider(s_logProcessor);
			return _provider;
		}
	}
}
