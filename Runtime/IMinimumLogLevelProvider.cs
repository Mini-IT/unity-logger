using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public interface IMinimumLogLevelProvider
	{
		LogLevel GetMinimumLogLevel();
	}

	public class ConstantMinimumLogLevelProvider : IMinimumLogLevelProvider
	{
		private readonly LogLevel _logLevel;

		public ConstantMinimumLogLevelProvider(LogLevel logLevel)
		{
			_logLevel = logLevel;
		}

		public LogLevel GetMinimumLogLevel() => _logLevel;
	}

	public sealed class NoneMinimumLogLevelProvider : ConstantMinimumLogLevelProvider
	{
		public NoneMinimumLogLevelProvider() : base(LogLevel.None) { }
	}
}
