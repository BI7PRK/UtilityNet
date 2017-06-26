using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Web;

namespace UtilityNet.FileIO
{
    /// <summary>
    /// 虚拟路径处理的一些方法，其实 4.0已经有 VirtualPathUtility 这个类。
    /// </summary>
    public abstract class VirtualPath
    {
        /// <summary>
        /// 相对路径转成绝对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAbsolutelyPath(string path)
        {
            var fullPath = HttpContext.Current.Server.MapPath(path);
            return fullPath;
        }
        /// <summary>
        /// 绝对根路径
        /// </summary>
        public static string AbsolutelyRootPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/");
            }
        }
        /// <summary>
        /// 将绝对路径转成相对路径
        /// </summary>
        /// <param name="absolute"></param>
        /// <returns></returns>
        public static string GetRelativePath(string absolute)
        {
            var rootPathLength = AbsolutelyRootPath.Length;
            if (absolute.Length <= rootPathLength) return "/";
            string path = absolute.Substring(rootPathLength - 1);
            return path.Replace("\\", "/");
        }
        /// <summary>
        /// 获取当前所在的相对路径
        /// </summary>
        public static string CurrentRelativePath
        {
            get
            {
                var absolute = HttpContext.Current.Request.Url.AbsolutePath;
                if (absolute.Length < 2) return "/";
                return absolute.Substring(0, absolute.LastIndexOf('/')) + "/";
            }
        }
        /// <summary>
        /// 获取当前绝对路径
        /// </summary>
        public static string CurrentAbsolutePath
        {
            get
            {
                return GetAbsolutelyPath(CurrentRelativePath);
            }
        }
        /// <summary>
        /// 传入目录，创建一个绝对物理路径，返回绝对物理路径
        /// </summary>
        /// <param name="path">目录</param>
        /// <returns>绝对物理路径</returns>
        public static string CreateAbsolutePath(string path)
        {
            path = path.Replace("/", "\\");
            var newPath = string.Empty;
            if (path.StartsWith("\\"))
            {
                newPath = GetAbsolutelyPath(path);
            }
            else
            {
                newPath = Path.Combine(CurrentAbsolutePath, path);
            }
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            return newPath + (newPath.EndsWith("\\") ? "" : "\\");
        }
        /// <summary>
        /// 传入目录，创建一个绝对物理路径。返回相对虚拟路径
        /// </summary>
        /// <param name="path">相对虚拟路径</param>
        /// <returns>相对虚拟路径</returns>
        public static string CreateRelativePath(string path)
        {
            var newPath = CreateAbsolutePath(path);
            return GetRelativePath(newPath);
        }
        /// <summary>
        ///创建文件物理路径，并返文件相对路径。
        /// </summary>
        /// <param name="file">文件相对路径</param>
        /// <returns></returns>
        public static string CreateFileRelativePath(string file)
        {
            file = file.Replace("/", "\\");
            var newPath = string.Empty;
            if (file.StartsWith("\\"))
            {
                newPath = Path.Combine(AbsolutelyRootPath, file.TrimStart('\\'));
            }
            else
            {
                newPath = Path.Combine(CurrentAbsolutePath, file);
            }
            var fileInfo = new FileInfo(newPath);
            var dir = fileInfo.DirectoryName;
            var fileName = fileInfo.Name;
            var virPath = CreateRelativePath(dir);
            return string.Concat(virPath, fileName);
        
        }
        /// <summary>
        /// 创建文件物理路径，并返文件绝对路径。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string CreateFileAbsolutePath(string file)
        {
            file = file.Replace("/", "\\");
            var newPath = string.Empty;
            if (file.StartsWith("\\"))
            {
                newPath = Path.Combine(AbsolutelyRootPath, file.TrimStart('\\'));
            }
            else
            {
                newPath = Path.Combine(CurrentAbsolutePath, file);
            }
            var dir = new FileInfo(newPath).DirectoryName;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return newPath;

        }

        /// <summary>
        /// 获取文件的完整目录
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetDirectoryPath(string file)
        {
            var fileInfo = new FileInfo(file);
            if (!fileInfo.Exists) return "";
            return fileInfo.DirectoryName;
        }

      
    }
}
