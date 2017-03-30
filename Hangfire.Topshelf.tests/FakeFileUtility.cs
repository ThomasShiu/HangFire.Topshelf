using System;
using System.IO;

namespace Hangfire.Topshelf.tests
{
    /// <summary>
    /// 產生測試用檔案工具集
    /// </summary>
    internal static class FakeFileUtility
    {
        /// <summary>
        /// 建立一個指定大小的檔案
        /// </summary>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="fileSize">檔案大小</param>
        internal static void CreateFakeFile(string fileName, int fileSize)
        {
            var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None, fileSize, true);
            byte[] dataArray = new byte[fileSize];
            new Random().NextBytes(dataArray);
            using (fs)
            {
                for (int i = 0; i < dataArray.Length; i++)
                {
                    fs.WriteByte(dataArray[i]);
                }
                fs.Seek(0, SeekOrigin.Begin);
                fs.Close();
            }
        }
    }
}