# �إ߱��y�M�סA�|�b�ؿ��U�s�W `.sonarqube` ��Ƨ�
# /k �� SonarQube �M�� Key
# /n �� SonarQube �M�צW��
# /v �� SonarQube �M�ת���
MSBuild.SonarQube.Runner begin /k:"Hangfirekey" /n:"CCM.Hangfire.Topshelf.sln" /v:"1.0" -X
# ���� MSBuild �sĶ
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /t:Rebuild
# ���� SonarQube �~�豽�y�{��
MSBuild.SonarQube.Runner end