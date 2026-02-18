using Microsoft.Extensions.Logging;

namespace MiniIT.Logging.Unity
{
	public interface IMinLogLevelProvider
	{
		LogLevel GetMinimumLogLevel();
	}

	public sealed class NoneMinLogLevelProvider : IMinLogLevelProvider
	{
		public LogLevel GetMinimumLogLevel() => LogLevel.None;
	}
}
