/* 
  人事離職單確認時產生對應單資料
  建立日期：2017/05/16
*/
-- 更新結案日期及狀態 FMSTS(單況):A:待確認;B:已確認;C:已作廢;D:已結案;E:變更申請中
Update HR_CHGTOR Set CLDT = GetDate() , FMSTS = 'D' where FMNO = '{1}';

-- 更新人事資料狀態
UPDATE HR_EMPLYM SET LLFDT = '{0:yyyy/MM/dd}'  ,TFMNO = '{1}' ,C_STA ='{2}'   WHERE EMPLYID ='{3}';
