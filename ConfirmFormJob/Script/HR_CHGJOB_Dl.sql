/* 
  人事異動單取消確認時產生對應單資料
  建立日期：2017/05/16
*/

-- 更新人事主檔
UPDATE HR_EMPLYM SET WBSID ='{0}',COMID='{1}',FA_NO='{2}',DEPID='{3}',JOBID='{4}',RANKID='{5}'  WHERE EMPLYID ='{6}';

-- 判斷異動別(HRCTP)若是D:留職停薪 或是R:複職時需要另外變更人員狀態
-- HRCTP(異動別):A:部門調動;B:升遷;C:降級;D:留職停薪;M:職務異動;R:複職

-- D:留職停薪
IF  EXISTS (SELECT FMNO FROM HR_CHGJOB WHERE FMNO ='{7}' and HRCTP ='D')
UPDATE HR_EMPLYM SET C_STA = 'A'  WHERE EMPLYID ='{6}';

-- R:複職
IF  EXISTS (SELECT FMNO FROM HR_CHGJOB WHERE FMNO ='{7}' and HRCTP ='R')
UPDATE HR_EMPLYM SET C_STA = 'B'  WHERE EMPLYID ='{6}';
