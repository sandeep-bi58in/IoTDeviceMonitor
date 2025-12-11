using System;
using System.IO;

namespace IoTDeviceMonitor.Services
{
    public static class Logger
    {
        private static readonly string LogPath = "Data/logs.txt";

        static Logger()
        {
            Directory.CreateDirectory("Data");
            if (!File.Exists(LogPath))
                File.Create(LogPath).Close();
        }

        public static void Log(string message)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            Console.WriteLine($"[LOG] {message}");
            File.AppendAllText(LogPath, entry + Environment.NewLine);
        }
    }
}