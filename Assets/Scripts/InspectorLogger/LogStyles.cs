using UnityEngine;

namespace InspectorLogger
{
    public static class LogStyles
    {
        public static readonly LogStyle Positive = new LogStyle(
            color: new Color(0.2f, 1.0f, 0.2f),
            bold: true,
            size: 14
        );

        public static readonly LogStyle Negative = new LogStyle(
            color: new Color(1.0f, 0.2f, 0.2f),
            bold: true,
            italic: true,
            size: 14
        );

        public static readonly LogStyle Info = new LogStyle(
            color: new Color(0.6f, 0.6f, 1.0f),
            italic: true,
            size: 12
        );

        public static readonly LogStyle AnimationPositive = new LogStyle(
            color: new Color(0.0f, 0.8f, 0.8f),
            bold: true,
            size: 12
        );

        public static readonly LogStyle AnimationNegative = new LogStyle(
            color: new Color(0.7f, 0.0f, 0.7f),
            bold: true,
            italic: true,
            size: 12
        );

        public static readonly LogStyle AnimationInfo = new LogStyle(
            color: new Color(0.1f, 0.4f, 0.6f),
            italic: true,
            size: 12
        );

        //

        public static readonly LogStyle SchedulerPositive = new LogStyle(
            color: new Color(0.2f, 0.6f, 1.0f),
            bold: true,
            size: 14
        );

        public static readonly LogStyle SchedulerNegative = new LogStyle(
            color: new Color(1.0f, 0.4f, 0.2f),
            bold: true,
            italic: true,
            size: 14
        );

        public static readonly LogStyle SchedulerInfo = new LogStyle(
            color: new Color(0f, 1.0f, 0.5f),
            italic: true,
            size: 12
        );

        public static readonly LogStyle TaskPositive = new LogStyle(
            color: new Color(0.9f, 0.8f, 0.0f),
            bold: true,
            size: 12
        );

        public static readonly LogStyle TaskNegative = new LogStyle(
            color: new Color(0.9f, 0.2f, 0.4f),
            bold: true,
            italic: true,
            size: 12
        );

        public static readonly LogStyle TaskInfo = new LogStyle(
            color: new Color(0.2f, 0.8f, 1.0f),
            italic: true,
            size: 12
        );
    }
}
