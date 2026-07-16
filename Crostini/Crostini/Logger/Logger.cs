//     ______                __  _       _ 
//    / ____/________  _____/ /_(_)___  (_)
//   / /   / ___/ __ \/ ___/ __/ / __ \/ / 
//  / /___/ /  / /_/ (__  ) /_/ / / / / /  
//  \____/_/   \____/____/\__/_/_/ /_/_/   
//        
//Copyright (C) NeCrost36
using Crostini
using System;
using System.IO;
using System.Text;

namespace Crostini
{
    public class Logger
    {
        private static Logger _instance;
        private static readonly object _lock = new object();
        private string _logFilePath;
        /// <summary>
        /// Cross-OS path: WIndows and Linux
        /// </summary>
        private const string LogPath4Win = "%USERPROFILE%/.crostini";
        private const string LogPath4Linux = "$HOME/.crostini";
        /// <summary>
        /// Private constructor(Instance mode)
        /// </summary>
        private Logger()
        {
            InitializeLogPath();
        }

        /// <summary>
        ///  Get singleton instance
        /// </summary>
        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Logger();
                        }
                    }
                }
                return _instance;
            }
        }
        // Initialize log
        private void InitializeLogPath()
        {
            string basePath;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT ||
                Environment.OSVersion.Platform == PlatformID.Win32Windows)
            {
                // Windows
                basePath = Environment.ExpandEnvironmentVariables(LogPath4Win);
            }
            else
            {
                // Linux/Mac
                basePath = Environment.GetEnvironmentVariable("HOME") + "/.crostini";
            }

            // Ensure directory exists
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            _logFilePath = Path.Combine(basePath, $"log_{DateTime.Now:yyyy-MM-dd}.log");
        }
        // Normal(Info)
        public void Log(string message)
        {
            WriteLog("INFO", message);
        }

        // Info
        public void Info(string message)
        {
            WriteLog("INFO", message);
        }

        // Warning
        public void Warning(string message)
        {
            WriteLog("WARN", message);
        }

        // Error
        public void Error(string message)
        {
            WriteLog("ERROR", message);
        }

        // Log error messages (with exceptions)
        public void Error(string message, Exception ex)
        {
            WriteLog("ERROR", $"{message} - Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
        }

        // Debug log
        public void Debug(string message)
        {
#if DEBUG
            // Nah,I'm enough

            WriteLog("DEBUG", message);
#endif
        }

        private void WriteLog(string level, string message)
        {
            try
            {
                lock (_lock)
                {
                    CheckLogSize();
                    //ISO8601 format
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}";

                    // Writing to log
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine, Encoding.UTF8);
                    Console.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                // If writing to the file fails, at least print it to the console
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }

        // Get log files path
        public string GetLogFilePath()
        {
            return _logFilePath;
        }

        /// <summary>
        ///  Clean log.Max 30 days
        /// </summary>
        /// <param name="daysToKeep"></param>
        public void CleanLogs(int daysToKeep = 30)
        {
            try
            {
                string directory = Path.GetDirectoryName(_logFilePath);
                var files = Directory.GetFiles(directory, "log_*.log");
                DateTime cutoffDate = DateTime.Now.AddDays(-daysToKeep);

                foreach (var file in files)
                {
                    if (File.GetCreationTime(file) < cutoffDate)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Crostini] Failed to clean logs: {ex.Message}");
            }
        }
        private void CheckLogSize()
        {
            var fileInfo = new FileInfo(_logFilePath);
            if (fileInfo.Exists && fileInfo.Length > 10 * 1024 * 1024) // 10MB
            {
                File.Move(_logFilePath, _logFilePath.Replace(".log", ".old.log"));
            }
        }
    }
}

//This is its own log
//Usually not treated as a callable function
//But if you need, Fine!(In case sometimes exception can't analysis log file
//using CrostiniLog;
//    static void Main()
//{
    // Get logger instance
//    var logger = Logger.Instance;
//    logger.Info("InFO");
//    logger.Warning("WaRnInG!!!");
//    logger.Error("eRrOrRrRrRr!!!!!!!");
//    try
//    {
//        throw new Exception("This is an exception");
//    }
//    catch (Exception ex)
//    {
//        logger.Error("Caught an exception:", ex);
//    }
//    Console.WriteLine($"Logging at:{logger.GetLogFilePath()}");

//If you want clean
//    logger.CleanLogs(30);
//}
