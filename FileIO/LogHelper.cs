using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace UtilityNet.FileIO
{
    
    public abstract class LogHelper
    {
        public static void WriteError(string str)
        {
            var dir = new FileInfo(string.Format("{0}log\\err_{1}.txt", new object[] { AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.Ticks }));
            if (!Directory.Exists(dir.DirectoryName))
            {
                Directory.CreateDirectory(dir.DirectoryName);
            }
            StreamWriter sw = new StreamWriter(dir.FullName, false, Encoding.UTF8);
            sw.WriteLine(str);
            dir = null;
            sw.Close();
        }

        public static void WriteLog(string str)
        {
            var dir = new FileInfo(string.Format("{0}log\\log_{1}.txt", new object[] { AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.Ticks }));
            if (!Directory.Exists(dir.DirectoryName))
            {
                Directory.CreateDirectory(dir.DirectoryName);
            }
            StreamWriter sw = new StreamWriter(dir.FullName, false, Encoding.UTF8);
            sw.WriteLine(str);
            dir = null;
            sw.Close();
        }


    }
}