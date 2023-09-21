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
}
