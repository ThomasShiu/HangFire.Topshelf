/* 
  勞保變更單確認時產生對應單資料
  建立日期：2017/05/23
  勞保變更檔(HR_LISCHM)
*/
                                     
-- 將現行資料移到異動記錄檔去                                                                      
INSERT INTO HR_LISREG                                                                          
			( CHGDT , EMPLYID , DEPID , LTP , LITM , AMT , ADDDT , RELDT , LFEE ,                    
			EIS_AMT , ELFEE , EIS_EAMT , LBINSNO , RMK ,                                             
			REMARK , C_SOURCE , ADD_TYPE , ADD_RATE , ADD_AMT , SFMNO , CPS_AMT )                    
	 SELECT CHGDT , EMPLYID , DEPID , LTP , LITM , AMT , ADDDT , RELDT , LFEE ,                  
		    EIS_AMT , ELFEE , EIS_EAMT , LBINSNO , RMK ,                                           
	        REMARK , C_SOURCE , ADD_TYPE , ADD_RATE , ADD_AMT , SFMNO , CPS_AMT                  
	   FROM HR_LISCHG M                                                                          
	  WHERE EMPLYID ='{0}'                                                                        
        AND (SELECT COUNT(1) FROM HR_LISREG WHERE EMPLYID =M.EMPLYID AND CHGDT =M.CHGDT) = 0   
-- 清除記錄記錄檔                                                                                
DELETE FROM HR_LISCHG WHERE EMPLYID ='{0}';  
-- 新增一筆記錄到記錄檔去                                                                         
INSERT INTO HR_LISCHG                                                                          
            (CHGDT , EMPLYID , LTP , LITM , AMT , ADDDT , RELDT , LFEE , EIS_AMT ,             
            ELFEE , EIS_EAMT , RMK , REMARK , C_SOURCE , ADD_TYPE ,                            
            ADD_RATE , ADD_AMT, SFMNO , CPS_AMT )                                              
     SELECT CRDT , EMPLYID  , LTP , LITM , AMT , DDT   ,  NULL , LFEE , EIS_AMT ,              
            ELFEE , EIS_EAMT , RMK , REMARK ,'1'       , ADD_TYPE ,                          
            ADD_RATE , ADD_AMT ,FMNO  , AMT                                                    
       FROM HR_LISCHM                                                                          
      WHERE FMNO='{1}';                                                                         

     
-- 當為退保狀態時將日期移到退保日
IF EXISTS (SELECT FMNO FROM HR_LPSCHM WHERE FMNO='{1}' and LTP='B') 
   UPDATE HR_LISCHG SET RELDT = ADDDT, ADDDT= NULL  WHERE EMPLYID ='{0}' and CHGDT ='{2}'       