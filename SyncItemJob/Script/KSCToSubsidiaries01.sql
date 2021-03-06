/*
  從昆山同步料號到寧波 
  找出所有未在寧波系統的料號
 */
 
-- 清除 ##ITEM_Temp
IF OBJECT_ID('tempdb..##ITEM_Temp') is not Null
Drop Table ##ITEM_Temp;


-- 刪除寧波己存在 料名，規格不同者 
DELETE FROM NGB_15.dbo.ITEM WHERE ITEM_NO IN (
SELECT
    MT.ITEM_NO   
FROM
    ITEM MT
    JOIN NGB_15.dbo.ITEM DL
    ON MT.ITEM_NO = DL.ITEM_NO
WHERE
    RTRIM(MT.ITEM_SP) <> RTRIM(DL.ITEM_SP) OR
    RTRIM(MT.ITEM_NM) <> RTRIM(DL.ITEM_NM))

-- 新增資料到暫存區
Select *
into ##ITEM_Temp
from ITEM
where ITEM_NO not In (Select ITEM_NO from NGB_15.dbo.ITEM );

-- 更新為寧波預設值
Update ##ITEM_Temp Set  
         WAHO_NO = 'NA1' ,  -- 儲區
         LOCA_NO = 'XX' ,   -- 儲位         
         VD_NO = NULL ,     -- 廠商編號
         INV_EMP_NO = '' ,  -- 倉庫人員          
         PUR_EMP_NO = '' ,  -- 採購人員
         MOC_EMP_NO = '' ,  -- 生管人員 
         MTR_CST = 0 ,      -- 材料成本             
         LAB_CST = 0 ,      -- 人工成本
         OVH_CST = 0 ,      -- 製費成本
         SBC_CST = 0 ,      -- 加工費用
         LAB_ADD = 0 ,      -- 本階人工
         OVH_ADD = 0 ,      -- 本階製費               
         SBC_ADD = 0 ,      -- 本階加工
         MTR_ADD = 0 ,      -- 本階材料
         STD_PCHPRC = 0 ,   -- 標準進價
         STD_SALPRC = 0 ,   -- 標準售價
         STD_SALEXP = 0 , 
		 MTR_RT = 1 ,       -- 聯產品材料權數  
		 LAB_RT = 1 ,       -- 聯產品人工權數    
		 OVH_RT = 1 ,       -- 聯產品製費權數   
		 SBC_RT = 1  ;       -- 聯產品加工權數 		 
		 
-- 新增到寧波正式資料表
INSERT INTO NGB_15.dbo.ITEM
    (ITEM_NO,ITEM_NM,ITEM_SP,ITEM_NM_E,ITEM_SP_E,ITEM_NO_O,C_STA,GRAT_NO,
     CLAS_NO,CLAS_NO1,CLAS_NO2,CLAS_NO3,CLAS_NO4,CLAS_NO5,CLAS_NO6,CLAS_NO7,
     CLAS_NO8,CLAS_NO9,CLAS_NO10,CLAS_NO11,CLAS_NO12,ACT_NO,INV_TY,FIN_ITEM_NO,
     UNIT,UNIT1,UNIT2,UNIT3,EXCH_RATE1,EXCH_RATE2,EXCH_RATE3,C_AU,A_UNIT,
     W_UNIT,L_UNIT,S_UNIT,V_UNIT,WEIGHT,WEIGHT_DNN,LENGTH,LENGTH_DNN,AREA,
     AREA_DNN,VOLUMN,VOLUMN_DNN,SAFE_QTY,ROR_POT,SUP_POT,MIN_QTY,PCH_QTY,
     ISS_QTY,UNIT_QTY,FIX_LT,LEAD_TIME,INSP_LT,LOT_QTY,C_LT,WAHO_NO,LOCA_NO,
     VD_NO,PLINE_NO,EMP_NO,INV_EMP_NO,PUR_EMP_NO,MOC_EMP_NO,TIN_CODE,LLC_BOM,
     LLC_CST,C_SOURCE,C_BONDED,C_PHANT,C_BCH,C_SR,C_LOCA,C_ROR,C_CYC,C_ISS,C_INSP,
     VLD_DAY,CHK_DAY,C_ABC,MTR_CST,LAB_CST,OVH_CST,SBC_CST,LAB_ADD,OVH_ADD,SBC_ADD,
     MTR_ADD,MTR_RT,LAB_RT,OVH_RT,SBC_RT,STD_PCHPRC,STD_SALPRC,STD_SALEXP,SAL_PRC,C_TAX,
     PACK_NO,C_CTL,C_INV,ITEM_DSCP,ITEM_DSCP1,ITEM_DSCP2,ITEM_DSCP3,REMK,IMG_NO,IMG_NO1,
     IMG_NO2,IMG_NO3,RT_ITEM_NO,RT_NO,DOC_NO,BAR_CODE,EFF_DT,EXP_DT,C_MPS,C_OUT,OUT_RT,C_OVR,
     OVR_RT,QM_NO,PINE_DAY,PURE_DAY,GD_NO,SIZE,CTS_RT,PO_TY,MO_TY,C_COST)
SELECT ITEM_NO,ITEM_NM,ITEM_SP,ITEM_NM_E,ITEM_SP_E,ITEM_NO_O,C_STA,GRAT_NO,
     CLAS_NO,CLAS_NO1,CLAS_NO2,CLAS_NO3,CLAS_NO4,CLAS_NO5,CLAS_NO6,CLAS_NO7,
     CLAS_NO8,CLAS_NO9,CLAS_NO10,CLAS_NO11,CLAS_NO12,ACT_NO,INV_TY,FIN_ITEM_NO,
     UNIT,UNIT1,UNIT2,UNIT3,EXCH_RATE1,EXCH_RATE2,EXCH_RATE3,C_AU,A_UNIT,
     W_UNIT,L_UNIT,S_UNIT,V_UNIT,WEIGHT,WEIGHT_DNN,LENGTH,LENGTH_DNN,AREA,
     AREA_DNN,VOLUMN,VOLUMN_DNN,SAFE_QTY,ROR_POT,SUP_POT,MIN_QTY,PCH_QTY,
     ISS_QTY,UNIT_QTY,FIX_LT,LEAD_TIME,INSP_LT,LOT_QTY,C_LT,WAHO_NO,LOCA_NO,
     VD_NO,PLINE_NO,EMP_NO,INV_EMP_NO,PUR_EMP_NO,MOC_EMP_NO,TIN_CODE,LLC_BOM,
     LLC_CST,C_SOURCE,C_BONDED,C_PHANT,C_BCH,C_SR,C_LOCA,C_ROR,C_CYC,C_ISS,C_INSP,
     VLD_DAY,CHK_DAY,C_ABC,MTR_CST,LAB_CST,OVH_CST,SBC_CST,LAB_ADD,OVH_ADD,SBC_ADD,
     MTR_ADD,MTR_RT,LAB_RT,OVH_RT,SBC_RT,STD_PCHPRC,STD_SALPRC,STD_SALEXP,SAL_PRC,C_TAX,
     PACK_NO,C_CTL,C_INV,ITEM_DSCP,ITEM_DSCP1,ITEM_DSCP2,ITEM_DSCP3,REMK,IMG_NO,IMG_NO1,
     IMG_NO2,IMG_NO3,RT_ITEM_NO,RT_NO,DOC_NO,BAR_CODE,EFF_DT,EXP_DT,C_MPS,C_OUT,OUT_RT,C_OVR,
     OVR_RT,QM_NO,PINE_DAY,PURE_DAY,GD_NO,SIZE,CTS_RT,PO_TY,MO_TY,C_COST
FROM tempdb.dbo.##ITEM_Temp;

/*
	從昆山同步料號到東莞
	找出所有未在東莞系統的料號
*/
-- 先資料存到暫存區去
IF OBJECT_ID('tempdb..##ITEM_Temp') is not Null
Drop Table ##ITEM_Temp;
-- 先資料存到暫存區去
IF OBJECT_ID('tempdb..##ITEM_Temp2') is not Null
Drop Table ##ITEM_Temp2;

-- 刪除東莞波己存在 料名，規格不同者 
DELETE FROM DAC_15.dbo.ITEM WHERE ITEM_NO IN (
SELECT
    MT.ITEM_NO   
FROM
    ITEM MT
    JOIN DAC_15.dbo.ITEM DL
    ON MT.ITEM_NO = DL.ITEM_NO
WHERE
    RTRIM(MT.ITEM_SP) <> RTRIM(DL.ITEM_SP) OR
    RTRIM(MT.ITEM_NM) <> RTRIM(DL.ITEM_NM));

-- 更新為東莞預設值
Select *
into ##ITEM_Temp2
from ITEM
where ITEM_NO not In (Select ITEM_NO from DAC_15.dbo.ITEM );

-- 更新為東莞預設值
Update ##ITEM_Temp2 Set  
         WAHO_NO = 'DA1' ,  -- 儲區
         LOCA_NO = 'XX' ,   -- 儲位         
         VD_NO = NULL ,     -- 廠商編號
         INV_EMP_NO = '' ,  -- 倉庫人員          
         PUR_EMP_NO = '' ,  -- 採購人員
         MOC_EMP_NO = '' ,  -- 生管人員 
         MTR_CST = 0 ,      -- 材料成本             
         LAB_CST = 0 ,      -- 人工成本
         OVH_CST = 0 ,      -- 製費成本
         SBC_CST = 0 ,      -- 加工費用
         LAB_ADD = 0 ,      -- 本階人工
         OVH_ADD = 0 ,      -- 本階製費               
         SBC_ADD = 0 ,      -- 本階加工
         MTR_ADD = 0 ,      -- 本階材料
         STD_PCHPRC = 0 ,   -- 標準進價
         STD_SALPRC = 0 ,   -- 標準售價
         STD_SALEXP = 0 , 
		 MTR_RT = 1 ,       -- 聯產品材料權數  
		 LAB_RT = 1 ,       -- 聯產品人工權數    
		 OVH_RT = 1 ,       -- 聯產品製費權數   
		 SBC_RT = 1         -- 聯產品加工權數 	
		 
-- 新增到東莞正式資料表
INSERT INTO DAC_15.dbo.ITEM
    (ITEM_NO,ITEM_NM,ITEM_SP,ITEM_NM_E,ITEM_SP_E,ITEM_NO_O,C_STA,GRAT_NO,
     CLAS_NO,CLAS_NO1,CLAS_NO2,CLAS_NO3,CLAS_NO4,CLAS_NO5,CLAS_NO6,CLAS_NO7,
     CLAS_NO8,CLAS_NO9,CLAS_NO10,CLAS_NO11,CLAS_NO12,ACT_NO,INV_TY,FIN_ITEM_NO,
     UNIT,UNIT1,UNIT2,UNIT3,EXCH_RATE1,EXCH_RATE2,EXCH_RATE3,C_AU,A_UNIT,
     W_UNIT,L_UNIT,S_UNIT,V_UNIT,WEIGHT,WEIGHT_DNN,LENGTH,LENGTH_DNN,AREA,
     AREA_DNN,VOLUMN,VOLUMN_DNN,SAFE_QTY,ROR_POT,SUP_POT,MIN_QTY,PCH_QTY,
     ISS_QTY,UNIT_QTY,FIX_LT,LEAD_TIME,INSP_LT,LOT_QTY,C_LT,WAHO_NO,LOCA_NO,
     VD_NO,PLINE_NO,EMP_NO,INV_EMP_NO,PUR_EMP_NO,MOC_EMP_NO,TIN_CODE,LLC_BOM,
     LLC_CST,C_SOURCE,C_BONDED,C_PHANT,C_BCH,C_SR,C_LOCA,C_ROR,C_CYC,C_ISS,C_INSP,
     VLD_DAY,CHK_DAY,C_ABC,MTR_CST,LAB_CST,OVH_CST,SBC_CST,LAB_ADD,OVH_ADD,SBC_ADD,
     MTR_ADD,MTR_RT,LAB_RT,OVH_RT,SBC_RT,STD_PCHPRC,STD_SALPRC,STD_SALEXP,SAL_PRC,C_TAX,
     PACK_NO,C_CTL,C_INV,ITEM_DSCP,ITEM_DSCP1,ITEM_DSCP2,ITEM_DSCP3,REMK,IMG_NO,IMG_NO1,
     IMG_NO2,IMG_NO3,RT_ITEM_NO,RT_NO,DOC_NO,BAR_CODE,EFF_DT,EXP_DT,C_MPS,C_OUT,OUT_RT,C_OVR,
     OVR_RT,QM_NO,PINE_DAY,PURE_DAY,GD_NO,SIZE,CTS_RT,PO_TY,MO_TY,C_COST)
SELECT ITEM_NO,ITEM_NM,ITEM_SP,ITEM_NM_E,ITEM_SP_E,ITEM_NO_O,C_STA,GRAT_NO,
     CLAS_NO,CLAS_NO1,CLAS_NO2,CLAS_NO3,CLAS_NO4,CLAS_NO5,CLAS_NO6,CLAS_NO7,
     CLAS_NO8,CLAS_NO9,CLAS_NO10,CLAS_NO11,CLAS_NO12,ACT_NO,INV_TY,FIN_ITEM_NO,
     UNIT,UNIT1,UNIT2,UNIT3,EXCH_RATE1,EXCH_RATE2,EXCH_RATE3,C_AU,A_UNIT,
     W_UNIT,L_UNIT,S_UNIT,V_UNIT,WEIGHT,WEIGHT_DNN,LENGTH,LENGTH_DNN,AREA,
     AREA_DNN,VOLUMN,VOLUMN_DNN,SAFE_QTY,ROR_POT,SUP_POT,MIN_QTY,PCH_QTY,
     ISS_QTY,UNIT_QTY,FIX_LT,LEAD_TIME,INSP_LT,LOT_QTY,C_LT,WAHO_NO,LOCA_NO,
     VD_NO,PLINE_NO,EMP_NO,INV_EMP_NO,PUR_EMP_NO,MOC_EMP_NO,TIN_CODE,LLC_BOM,
     LLC_CST,C_SOURCE,C_BONDED,C_PHANT,C_BCH,C_SR,C_LOCA,C_ROR,C_CYC,C_ISS,C_INSP,
     VLD_DAY,CHK_DAY,C_ABC,MTR_CST,LAB_CST,OVH_CST,SBC_CST,LAB_ADD,OVH_ADD,SBC_ADD,
     MTR_ADD,MTR_RT,LAB_RT,OVH_RT,SBC_RT,STD_PCHPRC,STD_SALPRC,STD_SALEXP,SAL_PRC,C_TAX,
     PACK_NO,C_CTL,C_INV,ITEM_DSCP,ITEM_DSCP1,ITEM_DSCP2,ITEM_DSCP3,REMK,IMG_NO,IMG_NO1,
     IMG_NO2,IMG_NO3,RT_ITEM_NO,RT_NO,DOC_NO,BAR_CODE,EFF_DT,EXP_DT,C_MPS,C_OUT,OUT_RT,C_OVR,
     OVR_RT,QM_NO,PINE_DAY,PURE_DAY,GD_NO,SIZE,CTS_RT,PO_TY,MO_TY,C_COST
FROM ##ITEM_Temp2;

IF OBJECT_ID('tempdb..##ITEM_Temp2') is not Null
Drop Table ##ITEM_Temp2;
