using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilityNet.FileIO
{
    public class FileManager
    {
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="virPath">文件相对路径</param>
        /// <returns></returns>
        public static bool DeleteFile(string virPath)
        {
            try
            {
                var file = VirtualPath.GetAbsolutelyPath(virPath);
                if (File.Exists(file))
                    File.Delete(file);
                return true;
            }
            catch
            {
                
            }
            return false;
        }
    }
}
