USE [E_BangPlatformEmail]
GO
/****** Object:  StoredProcedure [HANDLING].[usp_EmailRemover]    Script Date: 09.03.2025 16:16:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [HANDLING].[usp_EmailRemover]
AS
BEGIN 
DECLARE @actProdName NVARCHAR(255) = OBJECT_NAME(@@PROCID)
--CHECK FOR POSSIBLE TRANSACTION 
IF @@TRANCOUNT = 0 
	BEGIN TRAN
ELSE 
	SAVE TRAN @actProdName
	
	--CREATE TMP VARIABLES
	DECLARE @EmailID INT
	DECLARE @EmailAddress NVARCHAR(150)
	DECLARE @EmailBody NVARCHAR(MAX)
	DECLARE @CreatedTime DATETIME

	--DECLARE CURSOR 
	DECLARE EmailCursor CURSOR FOR 
	SELECT EmailID, EmailAddress, EmailBody, CreatedTime FROM Recent.Email

	--OPEN CURSOR
	OPEN EmailCursor

	--FETCH INTO CURSOR 
	FETCH EmailCursor INTO @EmailID, @EmailAddress, @EmailBody, @CreatedTime

	--OPEN LOOP
	WHILE @@FETCH_STATUS = 0 
	BEGIN 
		INSERT INTO History.HistoryEmail(EmailID,EmailTo, EmailBody, SendTime, IsSend)
		VALUES(@EmailID, @EmailAddress, @EmailBody, @CreatedTime, 1)

		DELETE FROM Recent.Email WHERE EmailID = @EmailID 
		FETCH EmailCursor INTO @EmailID, @EmailAddress, @EmailBody, @CreatedTime
	END
	--CLOSE Cursor 
	CLOSE EmailCursor
	DEALLOCATE EmailCursor
COMMIT
END