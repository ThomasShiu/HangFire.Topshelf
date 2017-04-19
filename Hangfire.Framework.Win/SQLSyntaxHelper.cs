using System;
using System.IO;
using System.Text;

namespace Hangfire.Framework.Win
{
    /// <summary>
    /// SQL 語法相關工具或是方法
    /// </summary>
    public class SQLSyntaxHelper
    {
        /// <summary>
        /// 讀取SQL File
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.IO.FileNotFoundException">$找不到檔案：{fullpath}</exception>
        public static string ReadSQLFile(string fileName)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var fullpath = $"{baseDir}\\Script\\{fileName}";
            if (!File.Exists(fullpath))
                throw new FileNotFoundException($"找不到檔案：{fullpath}");
            try
            {
                string result;
                using (var sr = new StreamReader(fullpath, Encoding.Default))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                return result;
            } catch (Exception)
            {
                throw;
            }
        }

        public static string AA(string value)
        {
            return $"'{value}'";
        }
    }
}