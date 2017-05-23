/* 
  人事任用單確認時產生對應單資料
  建立日期：2017/05/16
*/
-- 更新結案日期及狀態 FMSTS(單況):A:待確認;B:已確認;C:已作廢;D:已結案;E:變更申請中
Update HR_CHGENR Set CLDT = GetDate() , FMSTS = 'D' where FMNO = '{0}';

-- 人事主檔(HR_EMPLYM)
Delete From HR_EMPLYM where EMPLYID ='{1}';
INSERT INTO HR_EMPLYM
        ( EMPLYID , EMPLYNM , PIDNO , SEX , BRTHDT , MARY , REGADRS , MAILADRS ,
          HP , CONTEL , DEPID , REGDT , JOBID , NATION ,
          C_STA , PERD_05 , PERD_06 , SFT_NO , COMID , FA_NO , EFMNO , LBRTP )
Select NEMPLYID, EMPLYNM , PIDNO  , SEX , BRTHDT  , MARY , REGADRS , MAILADRS ,
       HP , CONTEL , DEPID , REGDT , JOBID , NATION ,
       'A' , PERD_05 , PERD_06 , SFT_NO , COMID , FA_NO  , FMNO, ''
  from HR_CHGENR
 where FMNO = '{0}';

-- 員工差勤設定(HR_WRKSET)
Delete From HR_WRKSET where EMPLYID ='{1}';
INSERT INTO HR_WRKSET
  (EMPLYID,IOCTRL,C_LATE)
Select NEMPLYID , 'Y', 'Y'
  from HR_CHGENR
 where FMNO = '{0}';

-- 其他資料(HR_PERD)
Delete From HR_PERD where EMPLYID ='{1}';
INSERT INTO HR_PERD
  ( SITM , EMPLYID ,PERD_04 , PERD_07 , OWNCITY )
Select 1, NEMPLYID , BLOOD, CRDT, OWNCITY
from HR_CHGENR
where FMNO = '{0}';

-- 緊急狀況連絡檔(HR_EMGCNT)
Delete From HR_EMGCNT where EMPLYID ='{1}';
INSERT INTO HR_EMGCNT                                                      
         ( RITM , EMPLYID , ECNTPHONE , ECNTNM , HP  , NOM )  
Select  1,NEMPLYID , ECNTPHONE , ECNTNM , PHPS , NOM 
from HR_CHGENR
where FMNO = '{0}';

-- 職務代理人檔(HR_RPSNT)
Delete From HR_RPSNT where EMPLYID ='{1}';
INSERT INTO HR_RPSNT ( RPSEQ ,EMPLYID , REMPLYID )   
 select 1,NEMPLYID ,EMPLYID 
 from HR_CHGENR
where FMNO = '{0}';

-- 薪資匯款帳戶檔(HR_ACNTNO)
Delete From HR_ACNTNO where EMPLYID ='{1}';
INSERT INTO HR_ACNTNO
  ( ACNTTP , ITEM , EMPLYID , BANKID,SANO , SANM , UDT )
Select  'A' ,  1, NEMPLYID, BANKID , SANO, SANM , CRDT
from HR_CHGENR
where FMNO = '{0}';

-- 員工新版特休記錄(HR_EMPYFRNEW)
Delete from HR_EMPYFRNEW where EMPLYID = '{1}';
INSERT INTO HR_EMPYFRNEW
  ( EMPLYID , WYR , F_DAY , F_HR , U_HR ,STARTDATE,DEADLINE ,SETTLE)
Select NEMPLYID  , 0   , 0 , 0    , 0 ,
  DATEADD(MONTH,6,EFFDT) , DATEADD(DAY,-1, DATEADD(YEAR,1,EFFDT)) , 'N'
from HR_CHGENR
where FMNO = '{0}';

-- 同步菁華員工資料
Delete From [192.168.100.19].CCM_Main.dbo.EMPNO where EMP_NO ='{1}'
INSERT INTO [192.168.100.19].CCM_Main.dbo.EMPNO                                            
         ( EMP_NO , EMP_NM , DEPM_NO , TEL_NO , TEL_NO2 , E_MAIL ,                         
           C_INV , C_PUR , C_COP , C_PPS , C_AST , C_ACT , C_SFC , C_QMS , C_BOM , C_MOC ) 
SELECT NEMPLYID , EMPLYNM , DEPID,'','' ,Rtrim(NEMPLYID)+'@ccm3s.com',
          'N'     ,'N'   ,'N'    ,'N'    ,'Y'    ,'N'    ,'N'    ,'N'    ,'N'    ,'N'
from HR_CHGENR
where FMNO = '{0}';