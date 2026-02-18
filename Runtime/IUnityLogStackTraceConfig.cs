using UnityEngine;

namespace MiniIT.Logging.Unity
{
	public interface IUnityLogStackTraceConfig
	{
		StackTraceLogType GetStackTraceEnabled(UnityEngine.LogType logType);
	}

	public sealed class FullStackTraceConfig : IUnityLogStackTraceConfig
	{
		public StackTraceLogType GetStackTraceEnabled(LogType logType)
		{
			return StackTraceLogType.Full;
		}
	}

	public sealed class ScriptOnlyStackTraceConfig : IUnityLogStackTraceConfig
	{
		public StackTraceLogType GetStackTraceEnabled(LogType logType)
		{
			return StackTraceLogType.ScriptOnly;
		}
	}

	public sealed class NoneStackTraceConfig : IUnityLogStackTraceConfig
	{
		public StackTraceLogType GetStackTraceEnabled(LogType logType)
		{
			return StackTraceLogType.None;
		}
	}
}
