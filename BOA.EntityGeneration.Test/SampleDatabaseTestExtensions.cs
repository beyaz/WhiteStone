﻿using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    static class SampleDatabaseTestExtensions
    {
        public static void CreateTables(this IDataContext context)
        {
            var database = context.Get(Data.Database);

            database.BeginTransaction();

            database.CommandText = "CREATE SCHEMA ERP";
            database.ExecuteNonQuery();

            database.CommandText =
                @"

CREATE TABLE ERP.SAMPLE_TABLE
(
    SAMPLE_TABLE_ID INT PRIMARY KEY IDENTITY (1, 1),

    FIELD_VARCHAR_50            VARCHAR (50) NOT NULL,
    FIELD_VARCHAR_50_NULLABLE   VARCHAR (50) NULL,

    FIELD_DATETIME              DATETIME NOT NULL,
    FIELD_DATETIME_NULLABLE     DATETIME NULL,

    FIELD_NUMERIC_27_0              NUMERIC(27,0) NOT NULL,
    FIELD_NUMERIC_27_0_NULLABLE     NUMERIC(27,0) NULL,

    FIELD_INT                   INT NOT NULL,
    FIELD_INT_NULLABLE          INT NULL,

    FIELD_MONEY                   MONEY NOT NULL,
    FIELD_MONEY_NULLABLE          MONEY NULL,

    FIELD_NVARCHAR                   NVARCHAR(7) NOT NULL,
    FIELD_NVARCHAR_NULLABLE          NVARCHAR(7) NULL,

    FIELD_NCHAR                   NCHAR(7) NOT NULL,
    FIELD_NCHAR_NULLABLE          NCHAR(7) NULL,

    FIELD_SMALLDATETIME                   SMALLDATETIME NOT NULL,
    FIELD_SMALLDATETIME_NULLABLE          SMALLDATETIME NULL,

    FIELD_SMALLINT                   SMALLINT NOT NULL,
    FIELD_SMALLINT_NULLABLE          SMALLINT NULL,

    FIELD_TINYINT                   TINYINT NOT NULL,
    FIELD_TINYINT_NULLABLE          TINYINT NULL,

    FIELD_CHAR                   CHAR NOT NULL,
    FIELD_CHAR_NULLABLE          CHAR NULL,

    FIELD_BIGINT                   BIGINT NOT NULL,
    FIELD_BIGINT_NULLABLE          BIGINT NULL,

    FIELD_BIT                   BIT NOT NULL,
    FIELD_BIT_NULLABLE          BIT NULL,

    FIELD_DECIMAL                   DECIMAL NOT NULL,
    FIELD_DECIMAL_NULLABLE          DECIMAL NULL,

    FIELD_UNIQUEIDENTIFIER                   UNIQUEIDENTIFIER NOT NULL,
    FIELD_UNIQUEIDENTIFIER_NULLABLE          UNIQUEIDENTIFIER NULL,

    FIELD_VARBINARY                  VARBINARY NOT NULL,
    FIELD_VARBINARY_NULLABLE          VARBINARY NULL,

    FIELD_INDEX_1          INT NOT NULL,
    FIELD_INDEX_2_1        INT NOT NULL,
    FIELD_INDEX_2_2        INT NOT NULL,
    
FIELD_INDEX_3_1        INT NOT NULL,
FIELD_INDEX_3_2        INT NOT NULL,
FIELD_INDEX_3_3        INT NULL
)

CREATE INDEX index_on_erp_sample_1 ON ERP.SAMPLE_TABLE(FIELD_INDEX_1);
CREATE INDEX index_on_erp_sample_2 ON ERP.SAMPLE_TABLE(FIELD_INDEX_2_1,FIELD_INDEX_2_2);
CREATE INDEX index_on_erp_sample_3 ON ERP.SAMPLE_TABLE(FIELD_INDEX_3_1,FIELD_INDEX_3_2,FIELD_INDEX_3_3);

";

            database.ExecuteNonQuery();
        }
    }
}