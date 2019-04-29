cd\
cd windows
cd system32



rmdir /q/s "\\srvktfs\KTBirimlerArasi\BT-Uygulama Gelistirme 3\Abdullah_Beyaztas\BOACardEntityGeneration\Generator"

robocopy "D:\github\WhiteStone\BOACardEntityGenerationWrapper\bin\Debug" "\\srvktfs\KTBirimlerArasi\BT-Uygulama Gelistirme 3\Abdullah_Beyaztas\BOACardEntityGeneration\Generator" /E



if errorlevel 0 goto end1

:end1 echo FILECOPY for $(ProjectName) COMPLETED OK 
exit 0

