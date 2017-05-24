/* 
  健保變更單確認時產生對應單資料
  建立日期：2017/05/16
*/

-- 眷屬健保變更明細檔(HR_HISCHD)
 /* 檢查修改模式
    1. 現行沒則新增
       1. 新增為加保                       -- 新增一筆
       2. 新增為退保                       -- 不異動 (原無加保故無需退保)
       3. 調整補助                        -- 不異動 (原無加保故無法調整補助)
    2. 現行己存在
       1. 判斷是否有變更
          1. 現為加保, 新增為加保         -- 不異動
          2. 現為加保, 新增為退保         -- 寫一筆原本到記錄檔 刪除後重新寫入一筆
          3. 現為加保, 新增為調整補助      -- 刪除後重新寫入一筆，寫一筆原本到記錄檔
          4. 現為調整補助, 新增為加保      -- 不異動
          5. 現為調整補助, 新增為退保      -- 寫一筆原本到記錄檔 刪除後重新寫入一筆
          6. 現為調整補助, 新增為調整補助   -- 寫一筆原本到記錄檔 刪除後重新寫入一筆
          7. 現為退保, 新增為加保         -- 寫一筆原本到記錄檔 刪除後重新寫入一筆
          8. 現為退保, 新增為退保         -- 不異動
          9. 現為退保, 新增為調整補助      -- 不異動
 */

if Not EXISTS(SELECT Count(1) FROM HR_FAHIS WHERE EMPLYID ='{0}' AND IDNO = '{1}')  -- 判斷是否存在
BEGIN
-- 無記錄
   if EXISTS(SELECT Count(1) FROM HR_HISCHD WHERE FMNO='{4}' AND IDNO = '{1}' and CHG_TY ='A')  
   BEGIN
        -- 新增記錄到記錄檔去
        INSERT INTO HR_FAHIS                                                                         
              (EMPLYID , IDNO , CHGDT , CHG_TY , NOM , CNAME , BIRTHDT , ADDDT , REMARK , RMK , 
               C_SOURCE, ADD_TYPE , ADD_RATE , ADD_AMT)                                         
        SELECT '{0}'   , IDNO , '{2:yyyy/MM/dd}' , CHG_TY , NOM , CNAME , BIRTHDT , '{3:yyyy/MM/dd}'  ,REMARK  , RMK , 
               '1'     , ADD_TYPE , ADD_RATE , ADD_AMT                                        
          FROM HR_HISCHD                                                                        
         WHERE FMNO = '{4}' and IDNO = '{1}'          
    END
END ELSE BEGIN
-- 有記錄存在
   if EXISTS(SELECT Count(1) FROM HR_FAHIS WHERE EMPLYID ='{0}' AND IDNO = '{1}' AND CHG_TY in('A','C'))  -- 判斷現有記錄狀態加保 or 現為調整補助
   BEGIN
     -- 新增為B:退保       -- 寫一筆原本到記錄檔 刪除後重新寫入一筆
     -- 新增為C:調整補助   -- 刪除後重新寫入一筆，寫一筆原本到記錄檔
     if EXISTS(SELECT Count(1) FROM HR_HISCHD WHERE FMNO='{4}' AND IDNO = '{1}' and CHG_TY in ('B','C'))  
     BEGIN
         -- 將現行資料移到異動記錄檔去                                                                                   
         INSERT INTO HR_FAHISREG                                                                            
               (EMPLYID , IDNO , CHGDT , CHG_TY , NOM , CNAME , BIRTHDT ,                              
                ADDDT , RELDT , INSURT , INSFEE , REMARK , RMK ,                                       
                C_SOURCE , ADD_TYPE , ADD_RATE , ADD_AMT )                                             
         SELECT EMPLYID , IDNO , CHGDT , CHG_TY , NOM , CNAME , BIRTHDT ,                              
                ADDDT , RELDT , INSURT , INSFEE , REMARK , RMK ,                                       
                C_SOURCE , ADD_TYPE , ADD_RATE , ADD_AMT                                               
           FROM HR_FAHIS M                                                                             
          WHERE EMPLYID ='{0}' AND IDNO ='{1}' AND CHGDT = '{2:yyyy/MM/dd}'                                               
            AND (SELECT COUNT(1) FROM HR_FAHISREG WHERE EMPLYID =M.EMPLYID AND CHGDT =M.CHGDT) = 0 
          -- 清除記錄記錄檔                                                         
          DELETE FROM HR_FAHIS WHERE EMPLYID ='{0}' AND IDNO ='{1}' AND CHGDT = '{2:yyyy/MM/dd}';
          -- 新增記錄到記錄檔去                                                                                 ' 
          INSERT INTO HR_FAHIS                                                                           
                 (EMPLYID , IDNO , CHGDT , CHG_TY , NOM , CNAME , BIRTHDT , ADDDT , REMARK , RMK ,   
                  C_SOURCE, ADD_TYPE , ADD_RATE , ADD_AMT)                                           
          SELECT '{0}'    , IDNO , '{2:yyyy/MM/dd}' , CHG_TY , NOM , CNAME , BIRTHDT , '{3:yyyy/MM/dd}' ,REMARK  , RMK ,   
                  '1'     , ADD_TYPE , ADD_RATE , ADD_AMT                                          
            FROM HR_HISCHD                                                                          
          WHERE FMNO = '{4}' and IDNO = '{1}'      
          -- 設定退保日期
          if EXISTS(SELECT Count(1) FROM HR_HISCHD WHERE FMNO='{4}' AND IDNO = '{1}' and CHG_TY = 'B')  
             UPDATE HR_FAHIS SET RELDT = ADDDT, ADDDT= NULL  WHERE EMPLYID ='{0}' AND IDNO ='{1}' and CHGDT = '{2:yyyy/MM/dd}'
     END
   END   -- 判斷現有記錄狀態加保 or 現為調整補助
   if EXISTS(SELECT Count(1) FROM HR_FAHIS WHERE EMPLYID ='{0}' AND IDNO = '{1}' AND CHG_TY in('A','C'))  -- 判斷現有記錄狀態退保
   BEGIN
         -- 將現行資料移到異動記錄檔去                                                                                   
         INSERT INTO HR_FAHISREG                                                                            
               (EMPLYID , IDNO , CHGDT , CHG_TY , NOM , CNAME , BIRTHDT ,                              
                ADDDT , RELDT , INSURT , INSFEE , REMARK , RMK ,                                       
                C_SOURCE , ADD_TYPE , ADD_RATE , ADD_AMT )                                             
         SELECT EMPLYID , IDNO , CHGDT , CHG_TY , NOM , CNAME , BIRTHDT ,                              
                ADDDT , RELDT , INSURT , INSFEE , REMARK , RMK ,                                       
                C_SOURCE , ADD_TYPE , ADD_RATE , ADD_AMT                                               
           FROM HR_FAHIS M                                                                             
          WHERE EMPLYID ='{0}' AND IDNO ='{1}' AND CHGDT = '{2:yyyy/MM/dd}'                                               
            AND (SELECT COUNT(1) FROM HR_FAHISREG WHERE EMPLYID =M.EMPLYID AND CHGDT =M.CHGDT) = 0 
          -- 清除記錄記錄檔                                                         
          DELETE FROM HR_FAHIS WHERE EMPLYID ='{0}' AND IDNO ='{1}' AND CHGDT = '{2:yyyy/MM/dd}';
          -- 新增記錄到記錄檔去                                                                                 ' 
          INSERT INTO HR_FAHIS                                                                           
                 (EMPLYID , IDNO , CHGDT , CHG_TY , NOM , CNAME , BIRTHDT , ADDDT , REMARK , RMK ,   
                  C_SOURCE, ADD_TYPE , ADD_RATE , ADD_AMT)                                           
          SELECT '{0}'    , IDNO , '{2:yyyy/MM/dd}' , CHG_TY , NOM , CNAME , BIRTHDT , '{3:yyyy/MM/dd}' ,REMARK  , RMK ,   
                  '1'     , ADD_TYPE , ADD_RATE , ADD_AMT                                          
          FROM HR_HISCHD 
         WHERE FMNO = '{4}' and IDNO = '{1}'
   END
END