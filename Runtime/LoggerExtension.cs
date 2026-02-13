using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using System;

namespace MiniIT.Logging
{
	public static class LoggerExtension
	{
		public static void Log(this ILogger logger, string message, params object[] args)
		{
			logger.Log(LogLevel.Trace, message, args);
		}

		public static void LogDebug(this ILogger logger, [InterpolatedStringHandlerArgument("logger")] ref DebugLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogDebug(handler.ToStringAndClear());
		}

		public static void LogTrace(this ILogger logger, [InterpolatedStringHandlerArgument("logger")] ref TraceLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogTrace(handler.ToStringAndClear());
		}

		public static void LogInformation(this ILogger logger, [InterpolatedStringHandlerArgument("logger")] ref InformationLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogInformation(handler.ToStringAndClear());
		}

		public static void LogWarning(this ILogger logger, [InterpolatedStringHandlerArgument("logger")] ref WarningLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogWarning(handler.ToStringAndClear());
		}

		public static void LogError(this ILogger logger, [InterpolatedStringHandlerArgument("logger")] ref ErrorLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogError(handler.ToStringAndClear());
		}

		public static void LogCritical(this ILogger logger, [InterpolatedStringHandlerArgument("logger")] ref CriticalLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogCritical(handler.ToStringAndClear());
		}

		public static void LogDebug(this ILogger logger, Exception exception, [InterpolatedStringHandlerArgument("logger")] ref DebugLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogDebug(exception, handler.ToStringAndClear());
		}

		public static void LogTrace(this ILogger logger, Exception exception, [InterpolatedStringHandlerArgument("logger")] ref TraceLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogTrace(exception, handler.ToStringAndClear());
		}

		public static void LogInformation(this ILogger logger, Exception exception, [InterpolatedStringHandlerArgument("logger")] ref InformationLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogInformation(exception, handler.ToStringAndClear());
		}

		public static void LogWarning(this ILogger logger, Exception exception, [InterpolatedStringHandlerArgument("logger")] ref WarningLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogWarning(exception, handler.ToStringAndClear());
		}

		public static void LogError(this ILogger logger, Exception exception, [InterpolatedStringHandlerArgument("logger")] ref ErrorLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogError(exception, handler.ToStringAndClear());
		}

		public static void LogCritical(this ILogger logger, Exception exception, [InterpolatedStringHandlerArgument("logger")] ref CriticalLogInterpolatedStringHandler handler)
		{
			if (!handler.Enabled)
			{
				return;
			}

			logger.LogCritical(exception, handler.ToStringAndClear());
		}
	}
}
