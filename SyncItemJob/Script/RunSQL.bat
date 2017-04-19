@echo off
rem 資料庫IP\資料庫執行個體名稱
set dbIp=192.168.100.18
rem 資料庫名稱
set dbName=KSC_15
rem 登入帳號
set dbUsrAcc=sa
rem 使用者密碼
set dbUsrPwd=6937937
rem 整理完畢的SQL指令集資料夾位置
set batchFilePath="C:\TestDB\db_script\folder"

rem 程式開始執行
cd %batchFilePath%

FOR /f %%i IN ('DIR *.Sql /B') do call :RunScript %%i
GOTO :END

:RunScript
Echo Executing %1
sqlcmd -S %dbIp% -d %dbName% -U %dbUsrAcc% -P %dbUsrPwd% -i %1 >>view.log
Echo Completed %1

:END