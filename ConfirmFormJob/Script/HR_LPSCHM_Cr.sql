
/* 
  勞保變更單確認時產生對應單資料
  建立日期：2017/05/23
   勞退變更檔(HR_LPSCHM)
*/
-- 更新結案日期及狀態 FMSTS(單況):OP:待確認;CF:己確認;CC:作廢;CL:結案
Update HR_LPSCHM Set CLDT = GetDate() , FMSTS = 'CL' where FMNO = '{1}';

-- 將現行資料移到異動記錄檔去                                                                           
INSERT INTO HR_LPSREG(CHGDT , EMPLYID , LTP , TITM , AMT , REMARK , C_SOURCE ,                      
                      DEPID , YER_RT , YEE_RT	)                                                     
     SELECT CHGDT , EMPLYID , LTP , TITM , AMT , REMARK , C_SOURCE ,                                
                      DEPID , YER_RT , YEE_RT                                                       
       FROM HR_LPSCHG M                                                                             
      WHERE EMPLYID ='{0}'                                                                           
        AND (SELECT COUNT(1) FROM HR_LPSREG WHERE EMPLYID =M.EMPLYID AND CHGDT =M.CHGDT) = 0        
-- 清除記錄記錄檔                                                                                     
DELETE FROM HR_LPSCHG WHERE EMPLYID ='{0}'
-- 新增一筆記錄到記錄檔去                                                                             
INSERT INTO HR_LPSCHG (CHGDT , EMPLYID , LTP , TITM , AMT , C_SOURCE ,                              
                       YER_RT , YEE_RT, REMARK, SFMNO )                                             
     SELECT CRDT,EMPLYID,LTP,TITM,AMT,'1',YER_RT,YEE_RT, REMARK,FMNO                              
       FROM HR_LPSCHM                                                                               
      WHERE FMNO='{1}';    
      
