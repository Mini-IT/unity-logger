using Microsoft.Extensions.Logging;

namespace MiniIT.Logging
{
	public static class LoggerExtension
	{
		public static void Log(this ILogger logger, string message, params object[] args)
		{
			logger.Log(LogLevel.Trace, message, args);
		}
	}
}
