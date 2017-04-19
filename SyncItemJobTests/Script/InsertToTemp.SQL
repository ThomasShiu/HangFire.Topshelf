-- 找出未在昆山的料號
IF OBJECT_ID('ITEM_TEMP') IS NOT NULL                                                        
   DROP TABLE ITEM_TEMP                                                                                 
 select * into ITEM_TEMP                                                                                 
   from [192.168.100.19].[CCM_Main].[dbo].ITEM                                                                        
  Where ITEM_NO Not Like 'E%'                                                                                             
    And ITEM_NO Not Like '22R%'                                                               
    And ITEM_NO Not Like '22S%'   
    And ITEM_NO ='2AMMU0165'                                                              
    And ITEM_NO Not In(Select ITEM_NO From ITEM)

-- 預設值的變更
Update ITEM_TEMP Set  
         WAHO_NO = 'KA1' ,  
         LOCA_NO = 'XX' ,   
         VD_NO = NULL ,     
         INV_EMP_NO = '' ,      
         PUR_EMP_NO = '' ,  
         MOC_EMP_NO = '' ,  
         MTR_CST = 0 ,             
         LAB_CST = 0 ,      
         OVH_CST = 0 ,      
         SBC_CST = 0 ,      
         LAB_ADD = 0 ,      
         OVH_ADD = 0 ,               
         SBC_ADD = 0 ,      
         MTR_ADD = 0 ,      
         STD_PCHPRC = 0 ,   
         STD_SALPRC = 0 ,   
         STD_SALEXP = 0 , 
		 MTR_RT = 1 ,         
		 LAB_RT = 1 ,           
		 OVH_RT = 1 ,          
		 SBC_RT = 1  ;

-- 組合需要繁簡轉換的部份				   
select 'Update ITEM_TEMP Set '
        +'ITEM_NM =N'''+Rtrim(ITEM_NM)
		+''' , ITEM_SP = N''' + Rtrim(ITEM_SP)+''''
		+' where ITEM_NO=''' + Rtrim(ITEM_NO)+''';' 
		as SQLSTRING 
 from ITEM_TEMP
 