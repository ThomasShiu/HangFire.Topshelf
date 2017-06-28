/* 
  薪資變更單確認時產生對應單資料
  建立日期：2017/05/16
  0 item.CHTY 
  1 item.EMPLYID
  2 item.FITEM
  3 item.PMS_NO
  4 item.AMT                                       
  5 item.C_OVT
  6 item.C_FRL
  7 item.C_TAX
*/

-- 員工津貼扣款設定(HR_PAYPMS)
-- CHTY(變更類別):A:新增;U:修改;D:刪除
if ('{0}'='D')
 Delete From HR_PAYPMS where EMPLYID ='{1}' And PMS_NO ='{3}';  -- D:刪除

if ('{0}'='U')
Update HR_PAYPMS Set AMT = '{4}' Where EMPLYID = '{1}' And PMS_NO ='{3}'; -- U:修改

if ('{0}'='A')
INSERT INTO HR_PAYPMS ( EMPLYID , FITEM , PMS_NO , AMT , C_OVT , C_FRL , C_TAX )  -- A:新增
               VALUES ( '{1}'   , '{2}' , '{3}'  ,'{4}' ,'{5}' ,'{6}'   ,'{7}' );

