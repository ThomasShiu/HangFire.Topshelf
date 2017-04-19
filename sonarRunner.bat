# 建立掃描專案，會在目錄下新增 `.sonarqube` 資料夾
# /k 為 SonarQube 專案 Key
# /n 為 SonarQube 專案名稱
# /v 為 SonarQube 專案版本
MSBuild.SonarQube.Runner begin /k:"Hangfirekey" /n:"CCM.Hangfire.Topshelf.sln" /v:"1.0" -X
# 執行 MSBuild 編譯
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /t:Rebuild
# 執行 SonarQube 品質掃描程式
MSBuild.SonarQube.Runner end