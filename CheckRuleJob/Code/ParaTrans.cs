using System;
using System.Text.RegularExpressions;

namespace Hangfire.Topshelf.Jobs.Code
{
    /// <summary>
    /// 格式化字串參數
    /// </summary>
    public class ParaTrans
    {
        /// <summary>
        /// 格式化SQL 參數
        /// </summary>
        /// <param name="tXT">待處理字串</param>
        /// <returns>格式化後內容</returns>
        /// <remarks>
        ///  以 2015/12/15為例 <p/>
        /// 1.{0} --> 當日   --> 2015/12/15
        /// 2.{1} --> 當月初 --> 2015/12/01
        /// 3.{2} --> 當月底 --> 2015/12/31
        /// 4.{3} --> 上月初 --> 2015/11/01
        /// 5.{4} --> 上月底 --> 2015/11/30
        /// 6.{5} --> 今年初 --> 2015/01/01
        /// 7.{6} --> 今年底 --> 2015/12/31
        /// 8.{7} --> 年字串 --> 2015
        /// 9.{8} --> 月字串 --> 12
        /// 10.{9}--> 日字串 --> 15
        /// </remarks>
        public static string Format( string tXT )
        {
            string sql = tXT;
            var regex = new Regex("{.*?}");
            var matches = regex.Matches(tXT);
            DateTime dt = DateTime.Now;
           const string Fmt = "yyyy/MM/dd";
            foreach (var match in matches)
            {
                switch (match.ToString())
                {
                    case "{0}":// 當日
                        sql = sql.Replace("{0}", dt.ToString(Fmt));
                        break;

                    case "{1}": // 本月初
                        sql = sql.Replace("{1}", ( new DateTime(dt.Year, dt.Month, 1) ).ToString(Fmt));
                        break;

                    case "{2}": // 本月底 下月初1號減一天=本月底)
                        sql = sql.Replace("{2}", ( new DateTime(dt.AddMonths(1).Year,
                                      dt.AddMonths(1).Month, 1).AddDays(-1) ).ToString(Fmt));
                        break;

                    case "{3}":// 上月初
                        sql = sql.Replace("{3}", ( new DateTime(dt.AddMonths(-1).Year,
                                dt.AddMonths(-1).Month, 1) ).ToString(Fmt));
                        break;

                    case "{4}": // 上月底 本月初1號減一天=上月底)
                        sql = sql.Replace("{4}", ( new DateTime(dt.AddMonths(-1).Year,
                                 dt.Month, 1).AddDays(-1) ).ToString(Fmt));
                        break;

                    case "{5}": // 今年初
                        sql = sql.Replace("{5}", ( new DateTime(dt.Year, 1, 1) ).ToString(Fmt));
                        break;

                    case "{6}":  // 今年底
                        sql = sql.Replace("{6}", ( new DateTime(dt.Year, 12, 31) ).ToString(Fmt));
                        break;

                    case "{7}": // 年字串 2015
                        sql = sql.Replace("{7}", dt.ToString("yyyy"));
                        break;

                    case "{8}": // 月字串 12
                        sql = sql.Replace("{8}", dt.ToString("MM"));
                        break;

                    case "{9}": // 日字串 12
                        sql = sql.Replace("{8}", dt.ToString("dd"));
                        break;
                }
            }
            return sql;
        }
    }
}