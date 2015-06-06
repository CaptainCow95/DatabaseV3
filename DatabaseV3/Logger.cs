using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace DatabaseV3
{
    /// <summary>
    /// Logs messages to a file and the console.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// The messages to be logged during the next flush.
        /// </summary>
        private static readonly ConcurrentQueue<Tuple<string, LogLevel, DateTime>> Messages = new ConcurrentQueue<Tuple<string, LogLevel, DateTime>>();

        /// <summary>
        /// The minimum log level to log.
        /// </summary>
        private static LogLevel _logLevel;

        /// <summary>
        /// The location to log to.
        /// </summary>
        private static string _logLocation;

        /// <summary>
        /// The thread that is occasionally flushing messages.
        /// </summary>
        private static Thread _logThread;

        /// <summary>
        /// A value indicating whether the logger is running.
        /// </summary>
        private static bool _running = true;

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        /// <param name="logLocation">The location of the log file.</param>
        /// <param name="logLevel">The minimum log level to log.</param>
        public static void Init(string logLocation, LogLevel logLevel)
        {
            _logLocation = logLocation;
            _logLevel = logLevel;

            _logThread = new Thread(RunLogger);
            _logThread.Start();
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="logLevel">The log level of the message.</param>
        public static void Log(string message, LogLevel logLevel)
        {
            Messages.Enqueue(new Tuple<string, LogLevel, DateTime>(message, logLevel, DateTime.UtcNow));
        }

        /// <summary>
        /// Shuts down the logger.
        /// </summary>
        public static void Shutdown()
        {
            _running = false;
            _logThread.Join();
        }

        /// <summary>
        /// Flushes all of the messages to disk.
        /// </summary>
        private static void FlushMessages()
        {
            StringBuilder text = new StringBuilder();
            while (!Messages.IsEmpty)
            {
                Tuple<string, LogLevel, DateTime> item;
                if (Messages.TryDequeue(out item) && item.Item2 <= _logLevel)
                {
                    text.AppendFormat("[{0} {1} {2}] {3}\n", item.Item3.ToShortDateString(), item.Item3.ToLongTimeString(), Enum.GetName(typeof(LogLevel), item.Item2), item.Item1);
                }
            }

            File.AppendAllText(Path.Combine(_logLocation, "debug.log"), text.ToString());
            Console.Write(text.ToString());
        }

        /// <summary>
        /// Runs the logger thread.
        /// </summary>
        private static void RunLogger()
        {
            while (_running)
            {
                FlushMessages();
                Thread.Sleep(100);
            }

            FlushMessages();
        }
    }
}