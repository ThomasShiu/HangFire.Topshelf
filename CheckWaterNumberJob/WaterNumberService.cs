using System;
using Hangfire.Samples.Framework;

namespace Hangfire.Topshelf.Jobs
{
    public class WaterNumberService:BaseAppService,IWaterNumberService
    {
        public string VCHTY { get; set; }
        public string Master { get; set; }
        public string Detail { get; set; }


        public bool Check()
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
             */ 
            return false;
        }

    }
}

/* 撿查規格
SELECT
    CONVERT(DATE , VCH_DT) , MAX(VCH_NO) , COUNT(1) , CONVERT(INT , SUBSTRING(MAX(VCH_NO) , 7 , 3)) - COUNT(1) AS '不等數'
FROM
    MOMT
WHERE
    VCH_TY = 'F712'
GROUP BY
    CONVERT(DATE , VCH_DT)
HAVING
    ( CONVERT(INT , SUBSTRING(MAX(VCH_NO) , 7 , 3)) - COUNT(1) ) <> 0
 */
