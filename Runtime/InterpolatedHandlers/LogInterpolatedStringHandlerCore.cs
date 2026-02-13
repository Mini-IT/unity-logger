using Microsoft.Extensions.Logging;
using System;
#if ZSTRING
using Cysharp.Text;
#else
using System.Text;
#endif

namespace MiniIT.Logging
{
    internal ref struct LogInterpolatedStringHandlerCore
    {
 #if ZSTRING
        private Utf16ValueStringBuilder _builder;
 #else
        private readonly StringBuilder _builder;
 #endif
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
 #if ZSTRING
            _builder = enabled ? ZString.CreateStringBuilder(true) : default;
 #else
            _builder = enabled ? new StringBuilder(literalLength) : null;
 #endif
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

 #if ZSTRING
            string s = _builder.ToString();
            _builder.Dispose();
 #else
            string s = _builder.ToString();
            _builder.Clear();
 #endif

            return s;
        }
    }
}
