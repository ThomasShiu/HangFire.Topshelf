using System;
using System.IO;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 執行CMD命令，或以進程的形式打開應用程序（d:\*.exe）
    /// </summary>
    public class CmdHelper
    {
        private static readonly string CmdExe = "Cmd.Exe";
        //  public static bool useBatMode = false;  // 是否使用.bat模式運行工具
        //  public static bool singleBat = true;    // 是否使用單個bat文件執行操作

        /// <summary>
        /// 定義委託接口處理函數，用於實時處理cmd輸出信息
        /// </summary>
        public delegate void Method();

        /// <summary>
        /// 以後台進程的形式執行應用程序（d:\*.exe）
        /// </summary>
        public static System.Diagnostics.Process NewProcess(string exe)
        {
            var p = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,  // 不跳出cmd視窗
                    FileName = exe,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };
            //P.StartInfo.WorkingDirectory = @"C:\windows\system32";
            p.Start();
            return p;
        }

        /// <summary>
        /// 執行指令批次檔
        /// </summary>
        public static string Run_bat(string batfile)
        {
            if (!File.Exists(batfile)) return $"找不到Bat檔案({batfile})";
            var p = NewProcess(batfile);
            var outStr = p.StandardOutput.ReadToEnd();
            p.Close();
            return outStr;
        }

        /// <summary>
        /// 執行CMD命令
        /// </summary>
        public static string Run(string cmd, bool useBatMode = true)
        {
            if (useBatMode) return Run_bat(cmd);    // 使用.bat文件模式執行cmd命令
            else
            {
                var process = NewProcess(CmdExe);
                process.StandardInput.WriteLine(cmd);
                process.StandardInput.WriteLine("exit");
                var outStr = process.StandardOutput.ReadToEnd();
                process.Close();
                return outStr;
            }
        }

        /// <summary>
        /// 定義委託接口處理函數，用於實時處理cmd輸出信息
        /// </summary>
        /// <example>
        /// private void Callback1(String line)
        /// {
        ///   textBox1.AppendText(line);
        ///   textBox1.AppendText(Environment.NewLine);
        ///   textBox1.ScrollToCaret();
        /// }
        /// </example>
        public delegate void Callback(string line);

        /// <summary>
        /// 執行CMD語句,實時獲取cmd輸出結果，輸出到call函數中
        /// </summary>
        /// <param name="cmd">要執行的CMD命令</param>
        /// <param name="call">回呼函數</param>
        /// <returns>System.String.</returns>
        public static string Run(string cmd, Callback call)
        {
            var cmdFile = CmdExe; // 使用.bat文件模式執行cmd命令

            var p = NewProcess(cmdFile);
            p.StandardInput.WriteLine(cmd);
            p.StandardInput.WriteLine("exit");

            var outStr = "";
            //  var error = "";
            var baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');

            try
            {
                for (var i = 0; i < 3; i++) p.StandardOutput.ReadLine();

                string line;
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                while ((line = p.StandardOutput.ReadLine()) != null || ((line = p.StandardError.ReadToEnd()) != null && !line.Trim().Equals("")))
                {
                    // cmd運行輸出信息
                    if (line.EndsWith(">exit") || line.Equals("")) continue;
                    if (line.StartsWith(baseDir + ">")) line = line.Replace(baseDir + ">", "cmd>\r\n"); // 識別的cmd命令行信息
                    line = ((line.Contains("[Fatal Error]") || line.Contains("ERROR:") || line.Contains("Exception")) ? "【E】 " : "") + line;
                    call?.Invoke(line);
                    outStr += line + "\r\n";
                }
            } catch (Exception ex)
            {
                call?.Invoke(ex.Message);
            }

            p.WaitForExit();
            p.Close();
            return outStr;
        }

        #region 呼叫執行外部程式

        /// <summary>
        /// 以進程的形式打開應用程序（d:\*.exe）,並執行命令，只負責呼叫不管是否有執行完成 
        /// </summary>
        public static string RunProgram(string programName, string cmd)
        {
            var proc = NewProcess(programName);
            if (cmd.Length != 0)
            {
                proc.StandardInput.WriteLine(cmd);
            }
            var outStr = proc.StandardOutput.ReadToEnd();
            proc.Close();
            return outStr;
        }

        /// <summary>
        /// 以進程的形式打開應用程序（d:\*.exe）,並執行命令
        /// </summary>
        public static string RunProgram(string programName, string cmd, int timeOut = 300)
        {
            if (timeOut <= 0) throw new ArgumentOutOfRangeException(nameof(timeOut));
            var proc = NewProcess(programName);
            string message;
            try
            {
                if (cmd.Length != 0)
                {
                    proc.StandardInput.WriteLine(cmd);
                }
                    proc.WaitForExit(timeOut);
                if (!proc.HasExited)
                {
                    // 如果外部程式沒有結束運行則強行終止之。
                    proc.Kill();
                    message = $"外部程式 {programName} 被強行終止！";
                }
                else
                {
                    message = proc.StandardOutput.ReadToEnd();
                }
            } catch (ArgumentException ex)
            {
                message = $"發生例外：{ex.Message}";
            } finally 
            {
                proc.Close();
            }
            return message;
        }
        /// <summary>
        /// 以進程的形式打開應用程序（d:\*.exe）,並執行命令無限等待該程式執行完成 
        /// </summary>
        public static string RunProgramWaitFor(string programName, string cmd)
        {
            var proc = NewProcess(programName);
            if (cmd.Length != 0)
            {
                proc.StandardInput.WriteLine(cmd);
            }
            proc.WaitForExit();
            var outStr = proc.StandardOutput.ReadToEnd();
            proc.Close();
            return outStr;
        }

        /// <summary>
        /// 正常啟動window應用程序（d:\*.exe）,並傳遞初始命令參數 (不等待其退出)
        /// </summary>
        /// <param name="exe">The executable.</param>
        /// <param name="args">The arguments.</param>
        public static void Open(string exe, string args)
        {
            System.Diagnostics.Process.Start(exe, args);
        }

        #endregion 呼叫執行外部程式
    }
}