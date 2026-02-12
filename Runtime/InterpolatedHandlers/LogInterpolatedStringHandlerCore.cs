using Microsoft.Extensions.Logging;
using System.Text;
using System;

namespace MiniIT.Logging
{
    internal ref struct LogInterpolatedStringHandlerCore
    {
        private readonly StringBuilder _builder;
        private readonly bool _enabled;

        public bool Enabled => _enabled;

        public LogInterpolatedStringHandlerCore(
            int literalLength,
            int formattedCount,
            ILogger logger,
            LogLevel level,
            out bool enabled)
        {
            enabled = logger != null && logger.IsEnabled(level);
            _enabled = enabled;
            _builder = enabled ? new StringBuilder(literalLength) : null;
        }

        public void AppendLiteral(string value)
        {
            if (!_enabled)
            {
                return;
            }

            _builder.Append(value);
        }

        public void AppendFormatted<T>(T value)
        {
            if (!_enabled)
            {
                return;
            }

            _builder.Append(value);
        }

        public void AppendFormatted<T>(T value, string format)
        {
            if (!_enabled)
            {
                return;
            }

            if (value is IFormattable f)
            {
                _builder.Append(f.ToString(format, null));
            }
            else
            {
                _builder.Append(value);
            }
        }

        public void AppendFormatted<T>(T value, int alignment)
        {
            if (!_enabled)
            {
                return;
            }

            string s = value?.ToString() ?? string.Empty;

            if (alignment < 0)
            {
                s = s.PadRight(-alignment);
            }
            else if (alignment > 0)
            {
                s = s.PadLeft(alignment);
            }

            _builder.Append(s);
        }

        public void AppendFormatted<T>(T value, int alignment, string format)
        {
            if (!_enabled)
            {
                return;
            }

            string s = value is IFormattable f
                ? f.ToString(format, null)
                : value?.ToString() ?? string.Empty;

            if (alignment < 0)
            {
                s = s.PadRight(-alignment);
            }
            else if (alignment > 0)
            {
                s = s.PadLeft(alignment);
            }

            _builder.Append(s);
        }

        public string ToStringAndClear()
        {
            if (!_enabled)
            {
                return string.Empty;
            }

            string s = _builder.ToString();
            _builder.Clear();

            return s;
        }
    }
}
