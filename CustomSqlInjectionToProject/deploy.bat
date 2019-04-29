cd\
cd windows
cd system32



rmdir /q/s "\\srvktfs\KTBirimlerArasi\BT-Uygulama Gelistirme 3\Abdullah_Beyaztas\CustomSqlInjectionToProject\Injector"

robocopy "D:\github\WhiteStone\CustomSqlInjectionToProject\bin\Debug" "\\srvktfs\KTBirimlerArasi\BT-Uygulama Gelistirme 3\Abdullah_Beyaztas\CustomSqlInjectionToProject\Injector" /E


if errorlevel 0 goto end1

:end1 echo FILECOPY for $(ProjectName) COMPLETED OK 
exit 0

