using UnityEngine;

namespace MiniIT.Logging.Unity
{
	public interface IStackTraceMapper
	{
		StackTraceLogType GetStackTraceEnabled(LogType logType);
	}

	public sealed class FullStackTraceMapper : IStackTraceMapper
	{
		public StackTraceLogType GetStackTraceEnabled(LogType logType)
		{
			return StackTraceLogType.Full;
		}
	}

	public sealed class ScriptOnlyStackTraceMapper : IStackTraceMapper
	{
		public StackTraceLogType GetStackTraceEnabled(LogType logType)
		{
			return StackTraceLogType.ScriptOnly;
		}
	}

	public sealed class NoneStackTraceMapper : IStackTraceMapper
	{
		public StackTraceLogType GetStackTraceEnabled(LogType logType)
		{
			return StackTraceLogType.None;
		}
	}
}
