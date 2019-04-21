












IF NOT EXISTS ( SELECT  *
                FROM    sys.schemas
                WHERE   name = N'TST' ) 
    EXEC('CREATE SCHEMA [TST] AUTHORIZATION [dbo]');
GO


IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'TST' 
                 AND  TABLE_NAME = 'Table1'))
BEGIN
   drop TABLE TST.Table1
END



CREATE TABLE TST.Table1
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	Value0 NVARCHAR(500),
	Value1 VARCHAR(MAX),
	Value2 DECIMAL (18, 2) NULL,
	Value3 DECIMAL (18, 2),
	Value4 DATETIME NULL,
	Value5 DATETIME,
	Value6 INT NULL,
	Value7 INT,
	Value8 INT
);


CREATE UNIQUE INDEX index_1
ON TST.Table1 (Value7);





CREATE INDEX index_2
ON TST.Table1 (Value0);


CREATE UNIQUE INDEX index_3
ON TST.Table1 (Value3,Value4);


CREATE INDEX index_4
ON TST.Table1 (Value7,Value8);