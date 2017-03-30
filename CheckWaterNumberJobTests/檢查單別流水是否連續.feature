#language: zh-TW
功能: 檢查單別流水是否連續
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
場景: 進行單別流水號檢查
	假設 目前製令單中有單別資料如下
	| VCH_TY | VCH_NO    | VCH_DT   |
	| F712   | 170104001 | 2017/1/4 |
	| F712   | 170104002 | 2017/1/4 |
	| F712   | 170104004 | 2017/1/4 |
	| F712   | 170104005 | 2017/1/4 |
	| F712   | 170104006 | 2017/1/4 |	
	當 進行檢查時
	那麼 會有以下資料的產生
	| VCH_TY | VCH_NO    | VCH_DT     |
	| F712   | 170101003 | 2017/01/04 |	
