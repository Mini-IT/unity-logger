using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public static class UnityLoggerFactory
	{
		public static ILoggerFactory Default => s_factory ??= LoggerFactory.Create(builder =>
		{
			builder.SetMinimumLevel(LogLevel.Trace);
			builder.AddUnityLogger();
		});

		private static ILoggerFactory s_factory;
	}
}
