/* 
  人事離職單確認時產生對應單資料
  建立日期：2017/05/16
*/
-- 更新結案日期及狀態 FMSTS(單況):A:待確認;B:已確認;C:已作廢;D:已結案;E:變更申請中
Update HR_CHGTOR Set CLDT = GetDate() , FMSTS = 'D' where FMNO = '{1}';

-- 更新人事資料狀態
UPDATE HR_EMPLYM SET LLFDT = '{0:yyyy/MM/dd}'  ,TFMNO = '{1}' ,C_STA ='{2}'   WHERE EMPLYID ='{3}';

-- 更新EIP 帳號狀態
UPDATE EIP.dbo.Sys_User SET F_EnabledMark = 0 WHERE F_Account = '{3}';

-- 更新ERP 帳號失效
UPDATE [192.168.100.19].CCM_Main.dbo.USRNO SET EXP_DT = '{0:yyyy/MM/dd}' WHERE USR_NO = '{3}';

-- 更新BPM 帳號失效
UPDATE [192.168.100.18].WebBPM.dbo.FSe7en_Org_MemberStruct SET Enabled = 0 WHERE AccountID = '{3}';
UPDATE [192.168.100.18].WebBPM_Test.dbo.FSe7en_Org_MemberStruct SET Enabled = 0 WHERE AccountID = '{3}';

-- 更新WEB EIP 帳號失效
UPDATE [192.168.100.18].WebEIP5.dbo.FSe7en_Org_MemberStruct  SET Enabled = 0 WHERE AccountID = '{3}';
UPDATE [192.168.100.18].WebEIP5_Test.dbo.FSe7en_Org_MemberStruct  SET Enabled = 0 WHERE AccountID = '{3}';

