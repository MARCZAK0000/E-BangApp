CREATE PROCEDURE Command.InsertLog
    @ResultText NVARCHAR(MAX),
    @ResultCode SMALLINT,
    @ProcedureName NVARCHAR(255) 
AS
BEGIN
    DECLARE @Status NVARCHAR(50) = CASE 
        WHEN @ResultCode = 0 THEN 'Success'
        WHEN @ResultCode = 1 THEN 'Warning'
        WHEN @ResultCode = 2 THEN 'Error'
        ELSE 'Unknown'
    END;
    
    INSERT INTO Command.ProcedureLog (ProcedureName, ExecutionTime, Status, Message)
    VALUES (@ProcedureName, GETDATE(), @Status, @ResultText);
END