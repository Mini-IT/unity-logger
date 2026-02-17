using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public class UnityLoggerFactory : ILoggerFactory
	{
		public static UnityLoggerFactory Default => s_instance ??= new UnityLoggerFactory();

		private static UnityLoggerFactory s_instance;

		private ILoggerProvider _provider;

		public UnityLoggerFactory()
		{
			s_instance ??= this;

			UnityEngine.Application.quitting += Dispose;
		}

		public void SetMinimumLevel(LogLevel level)
		{
			var provider = GetProvider();

			if (provider is not UnityLoggerProvider unityProvider)
			{
				return;
			}

			unityProvider.MinimumLevel = level;
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
			if (s_instance == this)
			{
				s_instance = null;
			}

			UnityEngine.Application.quitting -= Dispose;
			_provider?.Dispose();
		}

		private ILoggerProvider GetProvider()
		{
			_provider ??= new UnityLoggerProvider();
			return _provider;
		}
	}
}
