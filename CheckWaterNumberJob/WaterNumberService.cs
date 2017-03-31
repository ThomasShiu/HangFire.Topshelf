using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class WaterNumberService.
    /// </summary>
    public class WaterNumberService : IWaterNumberService
    {
        #region SQL 字串
        private readonly string sql1= @"
                                SELECT  '{0}' as 'TableName',
	                                    VCH_TY, MAX(VCH_NO)AS 'MaxVCHNo', COUNT(1) AS 'ToCount',
                                    	CONVERT(INT, SUBSTRING(MAX(VCH_NO), 7, 3)) AS  'MAXNO',
	                                    CONVERT(INT, SUBSTRING(MAX(VCH_NO), 7, 3)) - COUNT(1) AS 'Diff'
                                  FROM
                                       {0}   
							      Where VCH_TY <> 'B911' 
								    AND VCH_DT > '{1:yyyy/MM/dd}'
                              GROUP BY
                                        SUBSTRING(VCH_NO, 1, 6), VCH_TY
                                HAVING
	                                    (CONVERT(INT, SUBSTRING(MAX(VCH_NO), 7, 3)) - COUNT(1)) <> 0
                                    ";
        private readonly string sql2 = @"
					            SELECT
					                MT.VCH_TY , MT.VCH_NO AS MT_VCH_NO ,
					                SUBSTRING(MT.VCH_NO,1,6)+RIGHT('000'+ CONVERT(NVARCHAR(10),CONVERT(INT, SUBSTRING(MT.VCH_NO,7,3))+1),3) AS expected,
					                DL.VCH_NO AS DL_VCH_NO , MT.VCH_DT 
					            FROM
					                {0} MT LEFT JOIN {0} DL
					                ON MT.VCH_TY = DL.VCH_TY  
					                AND SUBSTRING(MT.VCH_NO,1,6)+RIGHT('000'+ CONVERT(NVARCHAR(10),CONVERT(INT, SUBSTRING(MT.VCH_NO,7,3))+1),3) =  DL.VCH_NO   
					            WHERE   
					                MT. VCH_NO LIKE '{2}%' AND MT.VCH_TY = '{1}'
					                AND MT.VCH_NO <> '{3}'
					                AND DL.VCH_NO IS NULL
					                ORDER BY MT.VCH_NO
	                             ";
        private readonly string tableList = @"SELECT DISTINCT TABLE_NM FROM EXCHTYPE";
        #endregion

        private readonly string _connectionstring;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaterNumberService"/> class.
        /// </summary>
        /// <param name="connnstring">The connnstring.</param>
        public WaterNumberService(string connnstring)
        {
            _connectionstring = connnstring;
        }
        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>IEnumerable&lt;COMT&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ResuleData Execute(DateTime startDate)
        {
            /* 1. 使用SQL 統計筆數跟最後單號不同者
             *    應加入判斷單號前筆數不同者             *    
             * 2. 逐筆判斷不同者的錯誤原因
             *     己知問題
             *     1. 少單號 (打單時發生錯誤沒有存到)
             *     2. 用日期群組化時會有可能發生先存檔後改單據日期的情況 變成單號為 F712-141215032 但日期為2015/12/15
             * 3. 可能的例外 
             *     1. 有一張不是當日單又剛好有少一張 最後一張又剛好符合張數
             * 4. 若改用單號前6碼來群組化結果會不同    
             * 5. 用For 去跑找出每一筆 
             * 6. 有欠很多筆的 超過5筆以上應該人工判讀
             */

            using (IDbConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                var list = conn.Query<string>(tableList);

                var listAll = new List<Incomplete>();

                foreach (var table in list)
                {
                    var sql = string.Format(sql1, table, startDate);
                    var value = conn.Query<Incomplete>(sql);
                    var incompletes = value as IList<Incomplete> ?? value.ToList();
                    if (incompletes.Count() != 0)
                        listAll.AddRange(incompletes);
                }

                var vchlist = new List<MForm>();

                foreach (var item in listAll)
                {
                    var sql = string.Format(sql2, item.TableName, item.VCH_TY, item.MaxVCHNo.Substring(0, 6), item.MaxVCHNo);
                  
                    var resule = conn.Query<MForm>(sql);
                    var mForms = resule as IList<MForm> ?? resule.ToList();
                    if (mForms.Count() != 0)
                        vchlist.AddRange(mForms);
                }
                return new ResuleData()
                {
                    Incompletes = listAll,
                    MForms = vchlist
                };

            }
        }
    }

    /// <summary>
    /// Class ResuleData.
    /// </summary>
    public class ResuleData
    {
        /// <summary>
        /// Gets or sets the incompletes.
        /// </summary>
        /// <value>The incompletes.</value>
        public IEnumerable<Incomplete> Incompletes { get; set; }
        /// <summary>
        /// Gets or sets the m forms.
        /// </summary>
        /// <value>The m forms.</value>
        public IEnumerable<MForm> MForms { get; set; }
    }

    /// <summary>
    /// Class Incomplete.
    /// </summary>
    public class Incomplete
    {
        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName { get; set; }
        /// <summary>
        /// Gets or sets the vc h_ ty.
        /// </summary>
        /// <value>The vc h_ ty.</value>
        public string VCH_TY { get; set; }
        /// <summary>
        /// Gets or sets the maximum VCH no.
        /// </summary>
        /// <value>The maximum VCH no.</value>
        public string MaxVCHNo { get; set; }
        /// <summary>
        /// Gets or sets to count.
        /// </summary>
        /// <value>To count.</value>
        public int? ToCount { get; set; }
        /// <summary>
        /// Gets or sets the maxno.
        /// </summary>
        /// <value>The maxno.</value>
        public int? MAXNO { get; set; }
        /// <summary>
        /// Gets or sets the difference.
        /// </summary>
        /// <value>The difference.</value>
        public int? Diff { get; set; }

    }

    /// <summary>
    /// Class MForm.
    /// </summary>
    public class MForm
    {
        /// <summary>
        /// Gets or sets the vc h_ ty.
        /// </summary>
        /// <value>The vc h_ ty.</value>
        public string VCH_TY { get; set; }
        /// <summary>
        /// Gets or sets the m t_ vc h_ no.
        /// </summary>
        /// <value>The m t_ vc h_ no.</value>
        public string MT_VCH_NO { get; set; }
        /// <summary>
        /// Gets or sets the expected.
        /// </summary>
        /// <value>The expected.</value>
        public string expected { get; set; }
        /// <summary>
        /// Gets or sets the d l_ vc h_ no.
        /// </summary>
        /// <value>The d l_ vc h_ no.</value>
        public string DL_VCH_NO { get; set; }
        /// <summary>
        /// Gets or sets the vc h_ dt.
        /// </summary>
        /// <value>The vc h_ dt.</value>
        public DateTime? VCH_DT { get; set; }
    }
}

/* 撿查規格
SELECT
    VCH_TY, MAX(VCH_NO) , COUNT(1) ,  
    CONVERT(INT , SUBSTRING(MAX(VCH_NO) , 7 ,3)) AS  ,  
    CONVERT(INT , SUBSTRING(MAX(VCH_NO) , 7 , 3)) - COUNT(1) AS 'Diff'
FROM
    ACTVCHMT                                                
GROUP BY
  SUBSTRING(VCH_NO , 1 , 6) , VCH_TY
HAVING
    ( CONVERT(INT , SUBSTRING(MAX(VCH_NO) , 7 , 3)) - COUNT(1) ) <> 0
 */
