//<sopcce.com>
//--------------------------------------------------------------
//<version>V0.1</verion>
//<createdate>2018-1-23</createdate>
//<author>guojq</author>
//<email>sopcce@qq.com</email>
//<log date="2018-2-23" version="0.5">创建</log>
//--------------------------------------------------------------
//<sopcce.com>

using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;

namespace Sop.Common.Helper.XML
{
    static class PathRoute
    {
        public static string DataFolder = string.Empty;

        public static string GetXmlPath<T>()
        {
            string dataFolder = DataFolder;
            if (string.IsNullOrEmpty(dataFolder))
            {
                dataFolder = Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data");
            }
            return Path.ChangeExtension(Combine(dataFolder, Combine(typeof(T).FullName?.Split('.'))), ".xml");
        }


        /// <summary>
        /// System.IO.Path.Combine 扩展，主要是用于路径拼接
        /// 1、当path2 是相对路径的时候，返回的是path2，path1会被丢弃
        /// 2、路径是驱动器，返回的结果不正确
        /// 3、无法连接http路径 
        /// </summary>
        /// <example>
        /// <code> Combine(p1, "p2", "\\p3/", p4));</code>
        /// </example>
        /// <see>
        /// <![CDATA[
        /// http://www.jb51.net/article/36744.htm
        /// http://www.cnblogs.com/LoveJenny/archive/2012/03/05/2381094.html]]>
        /// </see>
        /// <param name="paths">拼接路径</param>
        /// <returns>返回拼接之后的路径</returns>
        public static string Combine(params string[] paths)
        {
            if (paths.Length == 0)
                throw new ArgumentException("please input path");
            var builder = new StringBuilder();
            var spliter = "\\";
            var firstPath = paths[0];
            if (firstPath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                spliter = "/";
            if (!firstPath.EndsWith(spliter))
                firstPath = firstPath + spliter;
            builder.Append(firstPath);
            for (var i = 1; i < paths.Length; i++)
            {
                var nextPath = paths[i];
                if (nextPath.StartsWith("/") || nextPath.StartsWith("\\"))
                    nextPath = nextPath.Substring(1);
                if (i != paths.Length - 1)
                {
                    if (nextPath.EndsWith("/") || nextPath.EndsWith("\\"))
                    {
                        nextPath = nextPath.Substring(0, nextPath.Length - 1) + spliter;
                    }
                    else
                    {
                        nextPath = nextPath + spliter;
                    }
                }

                builder.Append(nextPath);
            }

            return builder.ToString();
        }


    }

}