using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAT_Library
{
    public static class LogFile
    {
        #region LOG FILE EDIT        

        public static void OpenLogFile(string fileName, string nameFile, string name)
        {
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(fileName);
            }
            if (!File.Exists(fileName + nameFile))
            {
                using (StreamWriter writer = File.CreateText(fileName + nameFile))
                {
                    writer.WriteLine("=============== LOG File from " + name + " ============");
                    writer.WriteLine("======== This file is created to check the tracking of the app ========");
                    writer.WriteLine("=============== Made on " + DateTime.Now.ToString("dd/MM/yyyy") + " at " + DateTime.Now.ToString("HH:mm") + " ===============\n\n");
                }
            }
        }

        public static void WriteLogFile(string text, string path)
        {
            using (StreamWriter writer = File.AppendText(path))
            {
                writer.WriteLine(text);
            }
        }

        #endregion
    }
}
