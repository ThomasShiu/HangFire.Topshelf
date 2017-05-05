// ***********************************************************************
// Assembly         : CallServer
// 作者             : levi
// 建立日期          : 12-24-2015
//
// Last Modified By : levi
// Last Modified On : 01-13-2016
// ***********************************************************************
// <copyright file="DBMaintain.cs" company="CCM">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class DBMaintain.
    /// </summary>
    public class DBMaintainService : SQLRuleRun
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DBMaintainService" /> class.
        /// </summary>
        public DBMaintainService(string connnstring)
        {
            _connectionstring = connnstring;
            RuleType = SQLRuleType.Maintain; //RLTP(型態):M:資料庫維護;C:資料檢查
        }      
    }
}