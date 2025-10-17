CREATE OR ALTER PROCEDURE Maintenance.RemoveOldEmails
    @Date DATE
AS
BEGIN
    DELETE FROM History.Emails
    WHERE CAST(HistoryProcessedDate AS DATE) <= @Date;
END