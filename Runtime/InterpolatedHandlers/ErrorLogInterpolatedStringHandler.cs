using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace MiniIT.Logging
{
    [InterpolatedStringHandler]
    public ref struct ErrorLogInterpolatedStringHandler
    {
        private LogInterpolatedStringHandlerCore _core;

        public bool Enabled => _core.Enabled;

        public ErrorLogInterpolatedStringHandler(
            int literalLength,
            int formattedCount,
            ILogger logger,
            out bool enabled)
        {
            _core = new LogInterpolatedStringHandlerCore(
                literalLength,
                formattedCount,
                logger,
                LogLevel.Error,
                out enabled);
        }

        public void AppendLiteral(string value) => _core.AppendLiteral(value);
        public void AppendFormatted<T>(T value) => _core.AppendFormatted(value);
        public void AppendFormatted<T>(T value, string format) => _core.AppendFormatted(value, format);
        public void AppendFormatted<T>(T value, int alignment) => _core.AppendFormatted(value, alignment);
        public void AppendFormatted<T>(T value, int alignment, string format) => _core.AppendFormatted(value, alignment, format);
        public string ToStringAndClear() => _core.ToStringAndClear();
    }
}
