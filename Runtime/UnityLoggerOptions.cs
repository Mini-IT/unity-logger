namespace MiniIT.Logging.Unity
{
	public sealed class UnityLoggerOptions
	{
		public IMinimumLogLevelProvider MinimumLogLevelProvider { get; set; }
		public IStackTraceMapper StackTraceMapper { get; set; }
	}
}
