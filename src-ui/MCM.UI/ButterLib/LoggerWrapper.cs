using BUTR.DependencyInjection.Logger;

using Microsoft.Extensions.Logging;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace MCM.UI.ButterLib;

internal class LogValuesFormatter
{
    private const string NullValue = "(null)";
    private static readonly object[] EmptyArray = [];
    private readonly string _format;
    private readonly List<string> _valueNames = [];

    public LogValuesFormatter(string format)
    {
        OriginalFormat = format;
        StringBuilder stringBuilder = new StringBuilder();
        int startIndex = 0;
        int length = format.Length;
        while (startIndex < length)
        {
            int braceIndex1 = FindBraceIndex(format, '{', startIndex, length);
            int braceIndex2 = FindBraceIndex(format, '}', braceIndex1, length);
            int indexOf = FindIndexOf(format, ',', braceIndex1, braceIndex2);
            if (indexOf == braceIndex2)
                indexOf = FindIndexOf(format, ':', braceIndex1, braceIndex2);
            if (braceIndex2 == length)
            {
                stringBuilder.Append(format, startIndex, length - startIndex);
                startIndex = length;
            }
            else
            {
                stringBuilder.Append(format, startIndex, braceIndex1 - startIndex + 1);
                stringBuilder.Append(_valueNames.Count.ToString(CultureInfo.InvariantCulture));
                _valueNames.Add(format.Substring(braceIndex1 + 1, indexOf - braceIndex1 - 1));
                stringBuilder.Append(format, indexOf, braceIndex2 - indexOf + 1);
                startIndex = braceIndex2 + 1;
            }
        }

        _format = stringBuilder.ToString();
    }

    public string OriginalFormat { get; private set; }

    public List<string> ValueNames => _valueNames;

    private static int FindBraceIndex(string format, char brace, int startIndex, int endIndex)
    {
        int braceIndex = endIndex;
        int index = startIndex;
        int num = 0;
        for (; index < endIndex; ++index)
        {
            if (num > 0 && format[index] != brace)
            {
                if (num % 2 == 0)
                {
                    num = 0;
                    braceIndex = endIndex;
                }
                else
                    break;
            }
            else if (format[index] == brace)
            {
                if (brace == '}')
                {
                    if (num == 0)
                        braceIndex = index;
                }
                else
                    braceIndex = index;

                ++num;
            }
        }

        return braceIndex;
    }

    private static int FindIndexOf(string format, char ch, int startIndex, int endIndex)
    {
        int num = format.IndexOf(ch, startIndex, endIndex - startIndex);
        return num != -1 ? num : endIndex;
    }

    public string Format(object[]? values)
    {
        if (values != null)
        {
            for (int index = 0; index < values.Length; ++index)
            {
                switch (values[index])
                {
                    case null:
                        values[index] = "(null)";
                        break;
                    case IEnumerable source:
                        values[index] = string.Join(", ", source.Cast<object>().Select(o => o ?? "(null)"));
                        break;
                }
            }
        }

        return string.Format(CultureInfo.InvariantCulture, _format, values ?? EmptyArray);
    }

    public KeyValuePair<string, object> GetValue(object[] values, int index)
    {
        if (index < 0 || index > _valueNames.Count)
            throw new IndexOutOfRangeException(nameof(index));
        return _valueNames.Count > index
            ? new KeyValuePair<string, object>(_valueNames[index], values[index])
            : new KeyValuePair<string, object>("{OriginalFormat}", OriginalFormat);
    }

    public IEnumerable<KeyValuePair<string, object>> GetValues(object[] values)
    {
        KeyValuePair<string, object>[] values1 = new KeyValuePair<string, object>[values.Length + 1];
        for (int index = 0; index != _valueNames.Count; ++index)
            values1[index] = new KeyValuePair<string, object>(_valueNames[index], values[index]);
        values1[values1.Length - 1] = new KeyValuePair<string, object>("{OriginalFormat}", OriginalFormat);
        return values1;
    }
}

internal class FormattedLogValues : IReadOnlyList<KeyValuePair<string, object>>
{
    internal const int MaxCachedFormatters = 1024;
    private const string NullFormat = "[null]";
    private static int _count;
    private static ConcurrentDictionary<string, LogValuesFormatter> _formatters = new();
    private readonly LogValuesFormatter? _formatter;
    private readonly object[]? _values;
    private readonly string _originalMessage;

    internal LogValuesFormatter? Formatter => _formatter;

    public FormattedLogValues(string? format, params object[]? values)
    {
        if ((values != null ? (values.Length != 0 ? 1 : 0) : 1) != 0 && format != null)
        {
            if (_count >= 1024)
            {
                if (!_formatters.TryGetValue(format, out _formatter))
                    _formatter = new LogValuesFormatter(format);
            }
            else
                _formatter = _formatters.GetOrAdd(format, f =>
                {
                    Interlocked.Increment(ref _count);
                    return new LogValuesFormatter(f);
                });
        }

        _originalMessage = format ?? "[null]";
        _values = values;
    }

    public KeyValuePair<string, object> this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(nameof(index));
            return index == Count - 1 ? new KeyValuePair<string, object>("{OriginalFormat}", _originalMessage) : _formatter.GetValue(_values, index);
        }
    }

    public int Count => _formatter == null ? 1 : _formatter.ValueNames.Count + 1;

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        for (int i = 0; i < Count; ++i)
            yield return this[i];
    }

    public override string ToString() => _formatter == null ? _originalMessage : _formatter.Format(_values);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}


internal class LoggerWrapper : IBUTRLogger
{
    private readonly ILogger _logger;

    public LoggerWrapper(ILogger logger)
    {
        _logger = logger;
    }

    public void LogMessage(BUTR.DependencyInjection.Logger.LogLevel logLevel, string message, params object[] args)
    {
        _logger.Log((Microsoft.Extensions.Logging.LogLevel) logLevel, default, new FormattedLogValues(message, args), null, (state, _) => state.ToString());
    }
}

internal class LoggerWrapper<T> : IBUTRLogger<T>
{
    private readonly ILogger<T> _logger;

    public LoggerWrapper(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogMessage(BUTR.DependencyInjection.Logger.LogLevel logLevel, string message, params object[] args)
    {
        _logger.Log((Microsoft.Extensions.Logging.LogLevel) logLevel, default, new FormattedLogValues(message, args), null, (state, _) => state.ToString());
    }
}