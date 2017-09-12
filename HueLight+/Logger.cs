using System;
using System.Collections.Generic;
using System.IO;

namespace HueLightPlus
{
    class Logger
    {
        private static List<string> logger = new List<string>();

        public static void Add(String logLine)
        {
            logger.Add(logLine);
        }

        public static void AddLine(String logLine)
        {
            Add("");
            Add("****" + logLine + "****");
            Add("");
        }

        public static void ToFile()
        {
            File.WriteAllLines("log.txt", logger);
            Reset();
        }

        public static void Reset()
        {
            logger = new List<string>();
        }

        public static void Finish()
        {
            Add("-------------------------------------------------------------------");
            Add("Log End");
            Add(DateTime.Now.ToString());
            ToFile();
        }
    }
}
