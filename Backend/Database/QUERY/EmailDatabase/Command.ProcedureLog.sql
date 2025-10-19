CREATE SCHEMA Command 

GO 

CREATE TABLE Command.ProcedureLog 
( 
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProcedureName NVARCHAR(128) NOT NULL, 
    ExecutionTime DATETIME NOT NULL, 
    Status NVARCHAR(50) NOT NULL, 
    Message NVARCHAR(4000) NULL 
)

CREATE INDEX IX_ProcedureLog_ProcedureName ON Command.ProcedureLog(ProcedureName)

GO 

ALTER TABLE Command.ProcedureLog ADD CONSTRAINT CHK_Status CHECK (Status IN ('Success', 'Error', 'Warning', 'Unknown'));
ALTER TABLE Command.ProcedureLog ADD CONSTRAINT DF_ExecutionTime DEFAULT GETDATE() FOR ExecutionTime