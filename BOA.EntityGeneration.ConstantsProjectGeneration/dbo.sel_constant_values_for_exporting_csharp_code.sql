﻿
USE BOACard

IF EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_constant_values_for_exporting_csharp_code]') AND type in (N'P', N'PC'))
    DROP PROCEDURE dbo.sel_constant_values_for_exporting_csharp_code
GO
/**
 *  Returns the list of constant values for exporting to c# code.
 */             
CREATE PROC dbo.sel_constant_values_for_exporting_csharp_code
AS
BEGIN
    IF OBJECT_ID('tempdb..#enumInfo') IS NOT NULL DROP TABLE #enumInfo
      
    ;WITH AlreadyDefinedEnums AS
    (
      SELECT enumclassname, enumitemname, enumvalue, enumsortid, ROW_NUMBER() OVER(PARTITION BY enumitemname ORDER BY enumsortid) AS 'RowNum'
        FROM dbo.enums
    GROUP BY enumclassname,enumitemname,enumvalue,enumsortid  
    )
    SELECT enumclassname AS ClassName, enumitemname AS PropertyName, enumvalue AS StringValue, enumsortid AS NumberValue INTO #enumInfo
    FROM AlreadyDefinedEnums
    WHERE RowNum = 1
    
    create UNIQUE index idx on #enumInfo (ClassName,PropertyName)
    
    INSERT INTO #enumInfo VALUES('DENEME_CLASS_1','PROPERTY_1','A','0')
    INSERT INTO #enumInfo VALUES('DENEME_CLASS_1','PROPERTY_2','B','1')
	INSERT INTO #enumInfo VALUES('DENEME_CLASS_1','PROPERTY_3','C','2')
    
    -- fix same value Never_Active
    DELETE FROM #enumInfo WHERE ClassName = 'ACTIVITY_STATUS' AND PropertyName = 'NeverActive'    
    
    SELECT * FROM #enumInfo
END
GO
EXEC dbo.sel_constant_values_for_exporting_csharp_code