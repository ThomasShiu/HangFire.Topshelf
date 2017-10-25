/* 
  健保變更單確認時產生對應單資料
  建立日期：2017/05/16
  健保變更檔(HR_HISCHM)
*/

-- 更新結案日期及狀態 FMSTS(單況):OP:待確認;CF:己確認;CC:作廢;CL:結案
Update HR_HISCHM Set CLDT = GetDate() , FMSTS = 'CL' where FMNO = '{1}';


-- 將現行資料移到異動記錄檔去                                                                                    
INSERT INTO HR_HISREG                                                                              
           (EMPLYID , CHGDT , HITM , CHG_TY , AMT , ADDDT , RELDT ,                                
            HELFEE , EHFEE , REMARK ,C_SOURCE ,                                                    
            ADD_TYPE , ADD_RATE , ADD_AMT , JIANB , SFMNO )                                        
     SELECT EMPLYID, CHGDT  , HITM , CHG_TY , AMT , ADDDT ,RELDT ,                                 
            HELFEE , EHFEE , REMARK ,C_SOURCE ,                                                    
            ADD_TYPE ,ADD_RATE  ,ADD_AMT , JIANB,SFMNO                                             
	   From HR_HISCHG M                                                                              
      WHERE EMPLYID ='{0}'                                                                         
       AND (SELECT COUNT(1) FROM HR_HISREG WHERE EMPLYID =M.EMPLYID AND CHGDT =M.CHGDT) = 0        
-- 清除記錄記錄檔                                                                                    
DELETE FROM HR_HISCHG WHERE EMPLYID ='{0}' ;                                                         
-- 新增一筆記錄到記錄檔去                                                                            
INSERT INTO HR_HISCHG                                                                              
           (EMPLYID , CHGDT , HITM , CHG_TY , AMT , ADDDT , RELDT ,                                
            HELFEE , EHFEE , REMARK ,  C_SOURCE ,                                                  
            ADD_TYPE , ADD_RATE , ADD_AMT , JIANB , SFMNO,C_STA )                                  
     SELECT EMPLYID , CRDT  , HITM , CHG_TY , AMT , DDT ,  NULL,                                   
            HELFEE , EHFEE , REMARK, '1' ,                                                       
            ADD_TYPE , ADD_RATE , ADD_AMT , JIANB , FMNO ,'1'                                    
       FROM HR_HISCHM                                                                              
      WHERE FMNO='{1}';    
      
-- 當為退保狀態時將日期移到退保日
IF EXISTS (SELECT FMNO FROM HR_HISCHM WHERE FMNO='{1}' and CHG_TY='B') 
 UPDATE HR_HISCHG SET RELDT = ADDDT, ADDDT= NULL  WHERE EMPLYID ='{0}' and CHGDT ='{2}' 

