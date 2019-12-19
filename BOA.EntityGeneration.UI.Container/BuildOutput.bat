
CD /D %~dp0

@Echo Off
Setlocal
Set sevenZip="C:\Program Files\7-Zip\7z.exe"

Set sourceDirctory=%~dp0%bin\debug\
Set zipFilePath=EntityGenerator.zip



%sevenZip% u %zipFilePath% %sourceDirctory%BOA.EntityGeneration.UI.Container.exe
%sevenZip% u %zipFilePath% %sourceDirctory%Exporters
%sevenZip% u %zipFilePath% %sourceDirctory%FileExporters





pause