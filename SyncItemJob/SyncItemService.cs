using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;

namespace Hangfire.Topshelf.Jobs
{
    public class SyncItemService
    {
        #region SQL 字串

        #endregion


        private readonly string _connectionstring;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncItemService" /> class.
        /// </summary>
        /// <param name="connnstring">The connnstring.</param>
        public SyncItemService(string connnstring)
        {
            _connectionstring = connnstring;
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
        public void Execute(string pama1, Callback proc)
        {
            /* 執行項目
             * 1. 同步台灣料號到昆山(要先轉換繁簡體)
             * 2. 同步昆山料號到寧波及東莞
             * 3. 比較昆山跟寧波及東莞的不同在同步特定料號
             */
            SyncCCM2KSC(proc);
            SyncKSCto(proc);
        }

        private void SyncCCM2KSC(Callback proc)
        {
            // 由於有要作繁簡體轉換 步驟如下
            // 1. 先從來原資料庫 Copy 到目的資料庫去
            // 2. 找出該欄位所有字元型態的欄位取出來後組合成Update SQL
            // 3. 在更新回資料庫
            proc?.Invoke("");
            
        }

        private void SyncKSCto(Callback proc)
        {
            // 1. 同步不足的部份
            // 2. 判斷己存在但特定欄位不足部份
        }
    }
}
