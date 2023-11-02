using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDC_Ajman_Bank
{
    static class Logger
    {
        static string basePath;

        public static void WriteLog(string strLog)
        {

            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string logFilePath = basePath + "\\Logfiles\\";

            System.IO.Directory.CreateDirectory(logFilePath);
            
            logFilePath = logFilePath + " PDC Ajman Bank General Logs -" + System.DateTime.Today.ToString("dd-MM-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + "|" + strLog);
            log.Close();
        }

        public static void WriteMicrProcessedLogs(string strLog)
        {

            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string logFilePath = basePath + "\\Logfiles\\";

            System.IO.Directory.CreateDirectory(logFilePath);

            logFilePath = logFilePath + " PDC Ajman Bank Successful Micr Logs -" + System.DateTime.Today.ToString("dd-MM-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + "|" + strLog);
            log.Close();
        }

        public static void WriteMicrFailedLogs(string strLog)
        {

            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string logFilePath = basePath + "\\Logfiles\\";

            System.IO.Directory.CreateDirectory(logFilePath);

            logFilePath = logFilePath + " PDC Ajman Bank Failed Micr Logs -" + System.DateTime.Today.ToString("dd-MM-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + "|" + strLog);
            log.Close();
        }
    }
}
