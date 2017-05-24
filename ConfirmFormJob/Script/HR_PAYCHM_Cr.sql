/* 
  薪資變更單確認時產生對應單資料
  建立日期：2017/05/16
  薪資變更檔(HR_PAYCHM)
*/

-- 更新結案日期及狀態 C_STA(單況):OP:待確認;CF:己確認;CC:作廢;CL:結案
Update HR_PAYCHM Set CLDT = GetDate() , FMSTS = 'CL' where FMNO = '{9}';

-- 員工薪資設定檔(HR_PAYSET)
IF Not EXISTS (SELECT EMPLYID FROM HR_PAYSET WHERE EMPLYID ='{0}') -- 新增
 INSERT INTO HR_PAYSET                                                                                            
         ( EMPLYID ,  BAS_SLY , ALP_RWD , C_OVT , C_LPS_TY , IID_NO , PAYTP , PAYROLLTP , OT_TY , SFMNO , ISLPS ) 
 VALUES  ( '{0}'   , '{1}'    ,'{2}'    ,'{3}'  , '{4}'    ,'{5}'   , '{6}' , '{7}'     , '{8}' , '{9}' , 'N' );
else -- 更新
  UPDATE HR_PAYSET 
                 SET BAS_SLY ='{1}' ,
                     ALP_RWD ='{2}',
                     C_OVT ='{3}',
                     C_LPS_TY='{4}',
                     IID_NO='{5}',
                     PAYTP='{6}',
                     PAYROLLTP='{7}',
                     OT_TY='{8}',
                     SFMNO ='{9}' 
               WHERE EMPLYID ='{0}'; 