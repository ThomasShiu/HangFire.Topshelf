// ***********************************************************************
// 程式集(Assembly)         : CallServer
// 作者(Author)             : levi
// 建立日期(Created)          : 02-04-2016
//
// Last Modified By : levi
// Last Modified On : 02-05-2016
// ***********************************************************************
// <copyright file="SS_DBLS_Mod.cs" company="CCM">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
 
namespace Hangfire.Topshelf.Jobs.Model
{ 
    /// <summary>
    /// Interface IDBLS_Mod
    /// </summary>
    public interface IDBLS_Mod
    {
        /// <summary>
        /// 檢測用的標的資料庫連接字串
        /// </summary>
        /// <value>The database connection string.</value>
        string DBConnString { get; }

        /// <summary>
        /// Gets or sets the dbid.
        /// </summary>
        /// <value>The dbid.</value>
        string DBID { get; set; }

        /// <summary>
        /// Gets or sets the act.
        /// </summary>
        /// <value>The act.</value>
        string ACT { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        string HOST { get; set; }

        /// <summary>
        /// Gets or sets the DBNM.
        /// </summary>
        /// <value>The DBNM.</value>
        string DBNM { get; set; }

        /// <summary>
        /// Gets or sets the usr.
        /// </summary>
        /// <value>The usr.</value>
        string USR { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        string PWD { get; set; }

        /// <summary>
        /// Gets or sets the remark.
        /// </summary>
        /// <value>The remark.</value>
        string REMARK { get; set; }
    }

    /// <summary>
    /// SS_DBLS 類別 資料庫清單 
    /// </summary>
    public class SS_DBLS_Mod : IDBLS_Mod
    {
        /// <summary>
        /// 檢測用的標的資料庫連接字串
        /// </summary>
        private const string ConnStrFmt = "Data Source={0};Initial Catalog ={1};Persist Security Info=True; User ID={2}; Password={3}";

        /// <summary>
        /// 檢測用的標的資料庫連接字串
        /// </summary>
        /// <value>The database connection string.</value>
        public string DBConnString
        {
            get
            {
                return String.Format(ConnStrFmt, HOST, DBNM, USR, PWD);
            }
        }

        /// <summary>
        /// 取得或設定資料庫代碼
        /// </summary>
        /// <value>The dbid.</value>
        public string DBID { get; set; }      

        /// <summary>
        /// 取得或設定是否啟用
        /// </summary>
        /// <value>The act.</value>
        public string ACT { get; set; }

        /// <summary>
        /// 取得或設定主機位置
        /// </summary>
        /// <value>The host.</value>
        public string HOST { get; set; }

        /// <summary>
        /// 取得或設定主機位置資料庫
        /// </summary>
        /// <value>The DBNM.</value>
        public string DBNM { get; set; }

        /// <summary>
        /// 取得或設定使用者代號
        /// </summary>
        /// <value>The usr.</value>
        public string USR { get; set; }

        /// <summary>
        /// 取得或設定密碼
        /// </summary>
        /// <value>The password.</value>
        public string PWD { get; set; }     

        /// <summary>
        /// 取得或設定備註
        /// </summary>
        /// <value>The remark.</value>
        public string REMARK { get; set; }
    }
}