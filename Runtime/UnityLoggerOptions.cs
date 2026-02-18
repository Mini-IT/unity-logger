namespace MiniIT.Logging.Unity
{
	public sealed class UnityLoggerOptions
	{
		public IMinLogLevelProvider MinLogLevelProvider { get; set; }
		public IUnityLogStackTraceConfig StackTraceConfig { get; set; }
	}
}
