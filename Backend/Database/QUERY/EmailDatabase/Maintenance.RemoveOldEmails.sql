SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Maintenance].[RemoveOldEmails]
    @Date DATE,
    @UpdatedRows INT OUTPUT
AS
BEGIN
    DECLARE @Message NVARCHAR(100); 
    DECLARE @ResultCode INT;    

    DELETE FROM History.Emails
        WHERE CAST(HistoryProcessedDate AS DATE) <= @Date;
    SET @UpdatedRows = @@ROWCOUNT;

    SET @Message = CAST(@UpdatedRows AS nvarchar(10)) + ' rows deleted from History.Emails older than ' + CAST(@Date AS nvarchar(10));

    SET @ResultCode = 0;
    
    EXEC Command.InsertLog
        @ResultText = @Message,
        @ResultCode = @ResultCode,
        @ProcedureName = 'Maintenance.RemoveOldEmails';

END
GO
