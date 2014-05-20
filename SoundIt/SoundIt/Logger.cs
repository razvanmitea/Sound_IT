using System;
using System.IO;
using Java.IO;

namespace SoundIt
{
    public class Logger
    {
        private const string logFilePath = "/sdcard/media/audio";
        public static BufferedWriter outWriter;

        public Logger()
        {
        }

        public static void LogThis(string message, string provider, string classToLog)
        {
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(logFilePath, "soundItLog.txt");
            using (var file = System.IO.File.Open(filePath, FileMode.Append, FileAccess.Write))
            using (var strm = new StreamWriter(file))
            {
                strm.WriteLine("Class: " +classToLog + "; Method: " + provider + "; Exception: " +message);
            }

        }
    }
}

