using System;
using System.Diagnostics;
using NLog;

namespace AIOAuto.Common
{
    public static class AppLogger
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static string GetClassName()
        {
            var method = new StackFrame(2).GetMethod();
            return method.DeclaringType?.Name;
        }

        private static void Log(string message, LogLevel level)
        {
            var className = GetClassName();
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] [{className}] {message}";
            Logger.Log(level, logMessage);
        }

        public static void Debug(string message)
        {
            Log("-----------------------------------------------------------------------------", LogLevel.Debug);
            Log(message, LogLevel.Debug);
        }

        public static void Info(string message)
        {
            Log("-----------------------------------------------------------------------------", LogLevel.Info);
            Log(message, LogLevel.Info);
        }

        public static void Warn(string message)
        {
            Log("-----------------------------------------------------------------------------", LogLevel.Warn);
            Log(message, LogLevel.Warn);
        }

        public static void Error(string message)
        {
            Log(message, LogLevel.Error);
        }

        public static void ErrorDetail(Exception ex, string message = "")
        {
            Log("-----------------------------------------------------------------------------", LogLevel.Error);
            if (message != "")
                Log($"Error: {message}", LogLevel.Error);
            if (ex == null) return;
            Log($"Type: {ex.GetType().FullName}", LogLevel.Error);
            Log($"Message: {ex.Message}", LogLevel.Error);
            Log($"StackTrace: {ex.StackTrace}", LogLevel.Error);
        }

        public static void Fatal(string message)
        {
            Log("-----------------------------------------------------------------------------", LogLevel.Fatal);
            Log(message, LogLevel.Fatal);
        }
    }
}