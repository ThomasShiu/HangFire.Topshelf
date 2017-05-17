/* 
  人事離職單取消確認時產生對應單資料
  建立日期：2017/05/16
*/

UPDATE HR_EMPLYM SET LLFDT =NULL  ,TFMNO = NULL WHERE EMPLYID ='{0}';