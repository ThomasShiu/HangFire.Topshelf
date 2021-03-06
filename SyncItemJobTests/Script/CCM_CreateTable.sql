﻿--IF EXISTS( SELECT * FROM sys.objects WHERE name = 'PK_ITEM' )
--   DROP INDEX PK_ITEM on ITEM
IF EXISTS( SELECT * FROM sys.tables WHERE name = 'ITEM' )
           Drop Table ITEM;
CREATE TABLE ITEM(
	[ITEM_NO] [nchar](24) NOT NULL,
	[ITEM_NM] [nchar](30) NULL,
	[ITEM_SP] [nchar](30) NULL,
	[ITEM_NM_E] [nvarchar](128) NULL,
	[ITEM_SP_E] [nvarchar](128) NULL,
	[ITEM_NO_O] [nchar](24) NULL,
	[C_STA] [nchar](1) NULL,
	[GRAT_NO] [nchar](24) NULL,
	[CLAS_NO] [nchar](10) NULL,
	[CLAS_NO1] [nchar](10) NULL,
	[CLAS_NO2] [nchar](10) NULL,
	[CLAS_NO3] [nchar](10) NULL,
	[CLAS_NO4] [nchar](10) NULL,
	[CLAS_NO5] [nchar](10) NULL,
	[CLAS_NO6] [nchar](10) NULL,
	[CLAS_NO7] [nchar](10) NULL,
	[CLAS_NO8] [nchar](10) NULL,
	[CLAS_NO9] [nchar](10) NULL,
	[CLAS_NO10] [nchar](10) NULL,
	[CLAS_NO11] [nchar](10) NULL,
	[CLAS_NO12] [nchar](10) NULL,
	[ACT_NO] [nchar](20) NULL,
	[INV_TY] [nchar](1) NULL,
	[FIN_ITEM_NO] [nchar](24) NULL,
	[UNIT] [nchar](4) NOT NULL,
	[UNIT1] [nchar](4) NULL,
	[UNIT2] [nchar](4) NULL,
	[UNIT3] [nchar](4) NULL,
	[EXCH_RATE1] [decimal](15, 5) NULL,
	[EXCH_RATE2] [decimal](15, 5) NULL,
	[EXCH_RATE3] [decimal](15, 5) NULL,
	[C_AU] [nchar](1) NULL,
	[A_UNIT] [nchar](4) NULL,
	[W_UNIT] [nchar](4) NULL,
	[L_UNIT] [nchar](4) NULL,
	[S_UNIT] [nchar](4) NULL,
	[V_UNIT] [nchar](4) NULL,
	[WEIGHT] [decimal](15, 5) NULL,
	[WEIGHT_DNN] [decimal](15, 5) NULL,
	[LENGTH] [decimal](15, 5) NULL,
	[LENGTH_DNN] [decimal](15, 5) NULL,
	[AREA] [decimal](15, 5) NULL,
	[AREA_DNN] [decimal](15, 5) NULL,
	[VOLUMN] [decimal](15, 5) NULL,
	[VOLUMN_DNN] [decimal](15, 5) NULL,
	[SAFE_QTY] [decimal](15, 4) NULL,
	[ROR_POT] [decimal](15, 4) NULL,
	[SUP_POT] [decimal](15, 4) NULL,
	[MIN_QTY] [decimal](15, 4) NULL,
	[PCH_QTY] [decimal](15, 4) NULL,
	[ISS_QTY] [decimal](15, 4) NULL,
	[UNIT_QTY] [decimal](15, 4) NULL,
	[FIX_LT] [int] NULL,
	[LEAD_TIME] [int] NULL,
	[INSP_LT] [int] NULL,
	[LOT_QTY] [decimal](15, 4) NULL,
	[C_LT] [nchar](1) NULL,
	[WAHO_NO] [nchar](10) NULL,
	[LOCA_NO] [nchar](24) NULL,
	[VD_NO] [nchar](10) NULL,
	[PLINE_NO] [nchar](10) NULL,
	[EMP_NO] [nchar](10) NULL,
	[INV_EMP_NO] [nchar](10) NULL,
	[PUR_EMP_NO] [nchar](10) NULL,
	[MOC_EMP_NO] [nchar](10) NULL,
	[TIN_CODE] [nchar](10) NULL,
	[LLC_BOM] [int] NULL,
	[LLC_CST] [int] NULL,
	[C_SOURCE] [nchar](1) NOT NULL,
	[C_BONDED] [nchar](1) NOT NULL,
	[C_PHANT] [nchar](1) NOT NULL,
	[C_BCH] [nchar](1) NOT NULL,
	[C_SR] [nchar](1) NOT NULL,
	[C_LOCA] [nchar](1) NULL,
	[C_ROR] [nchar](1) NOT NULL,
	[C_CYC] [nchar](1) NOT NULL,
	[C_ISS] [nchar](1) NOT NULL,
	[C_INSP] [nchar](1) NULL,
	[VLD_DAY] [int] NULL,
	[CHK_DAY] [int] NULL,
	[C_ABC] [nchar](1) NULL,
	[MTR_CST] [decimal](15, 6) NULL,
	[LAB_CST] [decimal](15, 6) NULL,
	[OVH_CST] [decimal](15, 6) NULL,
	[SBC_CST] [decimal](15, 6) NULL,
	[LAB_ADD] [decimal](15, 6) NULL,
	[OVH_ADD] [decimal](15, 6) NULL,
	[SBC_ADD] [decimal](15, 6) NULL,
	[MTR_ADD] [decimal](15, 6) NULL,
	[MTR_RT] [decimal](15, 6) NULL,
	[LAB_RT] [decimal](15, 6) NULL,
	[OVH_RT] [decimal](15, 6) NULL,
	[SBC_RT] [decimal](15, 6) NULL,
	[STD_PCHPRC] [decimal](15, 6) NULL,
	[STD_SALPRC] [decimal](15, 6) NULL,
	[STD_SALEXP] [decimal](15, 6) NULL,
	[SAL_PRC] [decimal](15, 6) NULL,
	[C_TAX] [nchar](1) NULL,
	[PACK_NO] [nchar](10) NULL,
	[C_CTL] [nchar](1) NULL,
	[C_INV] [nchar](1) NULL,
	[ITEM_DSCP] [nvarchar](255) NULL,
	[ITEM_DSCP1] [nvarchar](255) NULL,
	[ITEM_DSCP2] [nvarchar](255) NULL,
	[ITEM_DSCP3] [nvarchar](255) NULL,
	[REMK] [nvarchar](255) NULL,
	[IMG_NO] [nchar](24) NULL,
	[IMG_NO1] [nchar](24) NULL,
	[IMG_NO2] [nchar](24) NULL,
	[IMG_NO3] [nchar](24) NULL,
	[RT_ITEM_NO] [nchar](24) NULL,
	[RT_NO] [nchar](10) NULL,
	[DOC_NO] [nchar](24) NULL,
	[BAR_CODE] [nchar](24) NULL,
	[EFF_DT] [smalldatetime] NULL,
	[EXP_DT] [smalldatetime] NULL,
	[C_MPS] [nchar](1) NULL,
	[C_OUT] [nchar](1) NULL,
	[OUT_RT] [decimal](15, 4) NULL,
	[C_OVR] [nchar](1) NULL,
	[OVR_RT] [decimal](15, 4) NULL,
	[QM_NO] [nchar](24) NULL,
	[PINE_DAY] [int] NULL,
	[PURE_DAY] [int] NULL,
	[GD_NO] [nchar](24) NULL,
	[SIZE] [nchar](10) NULL,
	[CTS_RT] [decimal](15, 4) NULL,
	[PO_TY] [nchar](4) NULL,
	[MO_TY] [nchar](4) NULL,
	[C_COST] [nchar](1) NULL,
	[OWNER_USR_NO] [nchar](10) NULL,
	[OWNER_GRP_NO] [nchar](10) NULL,
	[ADD_DT] [smalldatetime] NULL,
	[MDY_USR_NO] [nchar](10) NULL,
	[MDY_DT] [smalldatetime] NULL,
	[IP_NM] [nchar](30) NULL,
	[CP_NM] [nchar](30) NULL,
 CONSTRAINT [PK_ITEM] PRIMARY KEY CLUSTERED 
(
	[ITEM_NO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]