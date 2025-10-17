CREATE TRIGGER Recent.trg_EmailSend 
    ON Recent.Email AFTER INSERT 
    AS
        BEGIN
        DECLARE @EmailID INT;
        DECLARE @IsSend BIT;
        SELECT @EmailID = INSERTED.EmailID, @IsSend = INSERTED.IsSend FROM INSERTED;
        IF @IsSend = 1
            BEGIN
                INSERT INTO History.Emails (EmailID, Recipient, SentDate)
                SELECT EmailID, EmailAddress, SendTime FROM inserted;
            END
    END;
