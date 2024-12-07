using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InspectorLogger
{
    [Serializable]
    public class LogEntry
    {
        public string message;
        public Color color;
        public bool bold;
        public bool italic;
        public int size;
        public bool underline;
    }

    public class InspectorLogger : MonoBehaviour
    {
        [SerializeField] private List<LogEntry> logs = new ();

        public List<LogEntry> GetLogs() => logs;
        public void AddLog(LogEntry log) => logs.Add(log);
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(InspectorLogger))]
    public class InspectorLoggerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var loggable = (InspectorLogger)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Logs", EditorStyles.boldLabel);

            var logs = loggable.GetLogs();
            if (logs.Count == 0) return;

            var previousMessage = logs[0].message;
            var repeatCount = 1;

            for (var i = 1; i <= logs.Count; i++)
            {
                var isLastLog = i == logs.Count;
                var currentMessage = isLastLog ? null : logs[i].message;

                if (currentMessage == previousMessage && !isLastLog) repeatCount++;

                else
                {
                    var log = logs[i - 1];
                    var style = new GUIStyle(EditorStyles.label)
                    {
                        normal = { textColor = log.color },
                        fontSize = log.size,
                        fontStyle = FontStyle.Normal,
                    };

                    if (log.bold) style.fontStyle |= FontStyle.Bold;
                    if (log.italic) style.fontStyle |= FontStyle.Italic;
                    if (log.underline) style.richText = true;

                    var messageToDisplay = previousMessage;
                    if (repeatCount > 1) messageToDisplay += $" (x{repeatCount})";

                    EditorGUILayout.LabelField(messageToDisplay, style);

                    previousMessage = currentMessage;
                    repeatCount = 1;
                }
            }
        }
    }

#endif
}