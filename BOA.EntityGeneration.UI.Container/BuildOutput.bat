
:: focus to current directory
CD /D %~dp0

@Echo Off
Setlocal
Set sevenZip="C:\Program Files\7-Zip\7z.exe"

Set sourceDirctory=%CD%\bin\debug\
Set zipFilePath=EntityGenerator.zip


:: Wait one second between each operations for avoid .tmp file locks.

:: create zip file
%sevenZip% a %zipFilePath% %sourceDirctory%BOA.EntityGeneration.UI.Container.exe
timeout 1
:: push files
%sevenZip% u %zipFilePath% %sourceDirctory%BOA.TfsAccess.dll
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%WhiteStone.dll
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%BOA.EntityGeneration.SchemaToEntityExporting.dll
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%BOA.EntityGeneration.CustomSQLExporting.dll
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%BOA.EntityGeneration.ConstantsProjectGeneration.dll
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%BOA.EntityGeneration.dll
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%MahApps.Metro.dll
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%ControlzEx.dll
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%System.Windows.Interactivity.dll
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%WpfControls.dll
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%Exporters
timeout 1
%sevenZip% u %zipFilePath% %sourceDirctory%FileExporters
timeout 1

@echo Success

pause