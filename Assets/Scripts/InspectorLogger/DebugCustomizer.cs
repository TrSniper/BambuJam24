using UnityEngine;

namespace InspectorLogger
{
    public struct LogStyle
    {
        public Color color;
        public bool bold;
        public bool italic;
        public int size;
        public bool underline;

        public LogStyle(Color color, bool bold = false, bool italic = false, int size = 12, bool underline = false)
        {
            this.color = color;
            this.bold = bold;
            this.italic = italic;
            this.size = size;
            this.underline = underline;
        }
    }

    public static class DebugCustomizer
    {
        public static void Log(this MonoBehaviour obj, string message, LogStyle style, bool willLogToConsole = false, bool willPrintTime = false)
        {
            if (willLogToConsole) message += " |> " + obj.name; //Add object name to message for identifying object in console
            if (willPrintTime) message += " - " + Time.time.ToString("0.00"); //Add time to message for comparing method execution order

            var logEntry = new LogEntry
            {
                message = message,
                color = style.color,
                bold = style.bold,
                italic = style.italic,
                size = style.size,
                underline = style.underline
            };

            LogToInspector(obj, logEntry);

            var styledMessage = StyleMessage(message, style);
            if (willLogToConsole) Debug.Log(styledMessage, obj);
        }

        public static void LogWarning(this MonoBehaviour obj, string message, LogStyle style, bool willLogToConsole = false)
        {
            if (willLogToConsole) message += " |> " + obj.name; //Add object name to message for identifying object in console
            message += " - " + Time.time.ToString("0.00"); //Add time to message for comparing method execution order

            var logEntry = new LogEntry
            {
                message = message,
                color = style.color,
                bold = style.bold,
                italic = style.italic,
                size = style.size,
                underline = style.underline
            };

            LogToInspector(obj, logEntry);

            var styledMessage = StyleMessage(message, style);
            if (willLogToConsole) Debug.LogWarning(styledMessage, obj);
        }

        public static void LogError(this MonoBehaviour obj, string message, LogStyle style, bool willLogToConsole = false)
        {
            if (willLogToConsole) message += " |> " + obj.name; //Add object name to message for identifying object in console
            message += " - " + Time.time.ToString("0.00"); //Add time to message for comparing method execution order

            var logEntry = new LogEntry
            {
                message = message,
                color = style.color,
                bold = style.bold,
                italic = style.italic,
                size = style.size,
                underline = style.underline
            };

            LogToInspector(obj, logEntry);

            var styledMessage = StyleMessage(message, style);
            if (willLogToConsole) Debug.LogError(styledMessage, obj);
        }

        private static void LogToInspector(MonoBehaviour obj, LogEntry logEntry)
        {
#if UNITY_EDITOR
            if (obj == null) return;
            var inspectorLogger = obj.GetComponent<global::InspectorLogger.InspectorLogger>();
            if (inspectorLogger != null) inspectorLogger.AddLog(logEntry);
#endif
        }

        private static string StyleMessage(string message, LogStyle style)
        {
            var colorCode = ColorUtility.ToHtmlStringRGBA(style.color);
            var styledMessage = $"<color=#{colorCode}>{message}</color>";

            if (style.bold) styledMessage = $"<b>{styledMessage}</b>";
            if (style.italic) styledMessage = $"<i>{styledMessage}</i>";
            if (style.size > 0) styledMessage = $"<size={style.size}>{styledMessage}</size>";
            if (style.underline) styledMessage = $"<u>{styledMessage}</u>";

            return styledMessage;
        }
    }
}
