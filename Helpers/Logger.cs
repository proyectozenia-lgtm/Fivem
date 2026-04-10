using System;
using System.IO;
using System.Text;

namespace FiveMOptimizerPro.Helpers
{
    /// <summary>
    /// Sistema de logging para toda la aplicación
    /// Registra todos los cambios, errores y acciones
    /// </summary>
    public static class Logger
    {
        private static readonly string _logDirectory = Path.Combine(Constants.AppDataPath, "Logs");
        private static readonly string _logFilePath = Path.Combine(_logDirectory, $"FiveMOptimizerPro_{DateTime.Now:yyyy-MM-dd}.log");
        private static readonly object _lockObject = new();

        // Eventos para notificaciones en tiempo real
        public static event Action<LogEntry>? OnLogEntry;

        /// <summary>
        /// Niveles de severidad del log
        /// </summary>
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            Critical
        }

        /// <summary>
        /// Estructura de entrada del log
        /// </summary>
        public class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public LogLevel Level { get; set; }
            public string Category { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public Exception? Exception { get; set; }
        }

        /// <summary>
        /// Inicializa el sistema de logging
        /// </summary>
        public static void Initialize()
        {
            try
            {
                if (!Directory.Exists(_logDirectory))
                {
                    Directory.CreateDirectory(_logDirectory);
                }

                Log(LogLevel.Info, "Logger", "=== FiveM Optimizer Pro Iniciado ===");
                Log(LogLevel.Info, "Logger", $"Versión: {Constants.AppVersion}");
                Log(LogLevel.Info, "Logger", $"Sistema Operativo: {GetOSVersion()}");
                Log(LogLevel.Info, "Logger", $"Procesador: {GetProcessorInfo()}");
                Log(LogLevel.Info, "Logger", $"RAM: {GetRAMInfo()}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error inicializando logger: {ex.Message}");
            }
        }

        /// <summary>
        /// Registra un mensaje de log
        /// </summary>
        public static void Log(LogLevel level, string category, string message, Exception? exception = null)
        {
            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Level = level,
                Category = category,
                Message = message,
                Exception = exception
            };

            OnLogEntry?.Invoke(entry);

            lock (_lockObject)
            {
                try
                {
                    var logLine = BuildLogLine(entry);
                    File.AppendAllText(_logFilePath, logLine + Environment.NewLine, Encoding.UTF8);
                }
                catch
                {
                    // Silenciar errores de log para no quebrar la aplicación
                }
            }
        }

        /// <summary>
        /// Log de nivel Debug
        /// </summary>
        public static void Debug(string category, string message) => Log(LogLevel.Debug, category, message);

        /// <summary>
        /// Log de nivel Info
        /// </summary>
        public static void Info(string category, string message) => Log(LogLevel.Info, category, message);

        /// <summary>
        /// Log de nivel Warning
        /// </summary>
        public static void Warning(string category, string message, Exception? ex = null) => Log(LogLevel.Warning, category, message, ex);

        /// <summary>
        /// Log de nivel Error
        /// </summary>
        public static void Error(string category, string message, Exception? ex = null) => Log(LogLevel.Error, category, message, ex);

        /// <summary>
        /// Log de nivel Critical
        /// </summary>
        public static void Critical(string category, string message, Exception? ex = null) => Log(LogLevel.Critical, category, message, ex);

        /// <summary>
        /// Construye una línea formateada del log
        /// </summary>
        private static string BuildLogLine(LogEntry entry)
        {
            var sb = new StringBuilder();
            sb.Append($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] ");
            sb.Append($"[{entry.Level.ToString().ToUpper()}] ");
            sb.Append($"[{entry.Category}] ");
            sb.Append(entry.Message);

            if (entry.Exception != null)
            {
                sb.AppendLine();
                sb.Append($"EXCEPCIÓN: {entry.Exception.GetType().Name}");
                sb.AppendLine();
                sb.Append($"Mensaje: {entry.Exception.Message}");
                sb.AppendLine();
                sb.Append($"StackTrace: {entry.Exception.StackTrace}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Obtiene información del SO
        /// </summary>
        private static string GetOSVersion()
        {
            return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        }

        /// <summary>
        /// Obtiene información del procesador
        /// </summary>
        private static string GetProcessorInfo()
        {
            try
            {
                var processorCount = Environment.ProcessorCount;
                return $"{processorCount} cores";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Obtiene información de RAM
        /// </summary>
        private static string GetRAMInfo()
        {
            try
            {
                var ram = GC.GetTotalMemory(false) / (1024 * 1024);
                return $"{ram} MB";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Obtiene el contenido del archivo de log
        /// </summary>
        public static string GetLogContent()
        {
            try
            {
                if (File.Exists(_logFilePath))
                {
                    return File.ReadAllText(_logFilePath);
                }
            }
            catch
            {
                // Silenciar
            }

            return "No logs available";
        }

        /// <summary>
        /// Obtiene la ruta del archivo de log
        /// </summary>
        public static string GetLogFilePath() => _logFilePath;
    }
}