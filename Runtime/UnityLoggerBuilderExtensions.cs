using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public static class UnityLoggerBuilderExtensions
	{
		public static ILoggingBuilder AddUnityLogger(this ILoggingBuilder builder) => builder.AddUnityLogger(_ => { });
		public static ILoggingBuilder AddUnityLogger(this ILoggingBuilder builder, Action<UnityLoggerOptions> configure)
		{
			builder.Services.AddSingleton<ILoggerProvider, UnityLoggerProvider>(serviceProvider =>
			{
				var options = new UnityLoggerOptions();
				configure(options);
				options.MinimumLogLevelProvider ??= new NoneMinimumLogLevelProvider();
				options.StackTraceConfig ??= new FullStackTraceConfig();
				var processor = new UnityLogProcessor(options);
				return new UnityLoggerProvider(processor);
			});
			return builder;
		}
	}
}
