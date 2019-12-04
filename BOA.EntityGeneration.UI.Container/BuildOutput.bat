
:: focus to current directory
CD /D %~dp0

@Echo Off
Setlocal
Set sevenZip="C:\Program Files\7-Zip\7z.exe"

Set sourceDirctory=%CD%\bin\debug\
Set zipFilePath=EntityGenerator.zip


:: create zip file
%sevenZip% a %zipFilePath% %sourceDirctory%BOA.EntityGeneration.UI.Container.exe
:: push files
%sevenZip% u %zipFilePath% %sourceDirctory%BOA.TfsAccess.dll
%sevenZip% u %zipFilePath% %sourceDirctory%WhiteStone.dll
%sevenZip% u %zipFilePath% %sourceDirctory%BOA.EntityGeneration.SchemaToEntityExporting.dll
%sevenZip% u %zipFilePath% %sourceDirctory%BOA.EntityGeneration.CustomSQLExporting.dll
%sevenZip% u %zipFilePath% %sourceDirctory%BOA.EntityGeneration.ConstantsProjectGeneration.dll
%sevenZip% u %zipFilePath% %sourceDirctory%BOA.EntityGeneration.dll
%sevenZip% u %zipFilePath% %sourceDirctory%MahApps.Metro.dll
%sevenZip% u %zipFilePath% %sourceDirctory%ControlzEx.dll
%sevenZip% u %zipFilePath% %sourceDirctory%System.Windows.Interactivity.dll
%sevenZip% u %zipFilePath% %sourceDirctory%WpfControls.dll
%sevenZip% u %zipFilePath% %sourceDirctory%Exporters
%sevenZip% u %zipFilePath% %sourceDirctory%FileExporters

@echo Success
@echo Success
@echo Success
pause