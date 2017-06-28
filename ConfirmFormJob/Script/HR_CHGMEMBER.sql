/* 
  人事離職單 停用相關帳號
  建立日期：2017/05/16
  0:FLDT, 1:EMPLYID
*/
-- 更新EIP 帳號狀態
UPDATE EIP.dbo.Sys_User SET F_EnabledMark = 0 WHERE F_Account = '{1}';

-- 更新ERP 帳號失效
UPDATE [192.168.100.19].CCM_Main.dbo.USRNO SET EXP_DT = '{0:yyyy/MM/dd}' WHERE USR_NO = '{1}';

-- 更新BPM 帳號失效
UPDATE [192.168.100.18].WebBPM.dbo.FSe7en_Org_MemberStruct SET Enabled = 0 WHERE AccountID = '{1}';
UPDATE [192.168.100.18].WebBPM_Test.dbo.FSe7en_Org_MemberStruct SET Enabled = 0 WHERE AccountID = '{1}';

-- 更新WEB EIP 帳號失效
UPDATE [192.168.100.18].WebEIP5.dbo.FSe7en_Org_MemberStruct  SET Enabled = 0 WHERE AccountID = '{1}';
UPDATE [192.168.100.18].WebEIP5_Test.dbo.FSe7en_Org_MemberStruct  SET Enabled = 0 WHERE AccountID = '{1}';

