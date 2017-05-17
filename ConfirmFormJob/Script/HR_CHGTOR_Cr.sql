/* 
  人事離職單確認時產生對應單資料
  建立日期：2017/05/16
*/

UPDATE HR_EMPLYM SET LLFDT = '{0:yyyy/MM/dd}'  ,TFMNO = '{1}' ,C_STA ='{2}'   WHERE EMPLYID ='{3}';
