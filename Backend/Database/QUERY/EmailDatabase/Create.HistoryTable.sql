CREATE SCHEMA History;
GO
CREATE TABLE History.Emails
(
    EmailID INT PRIMARY KEY IDENTITY(1,1),
    Recipient NVARCHAR(255) NOT NULL,
    SentDate DATETIME NOT NULL, 
    HistoryProcessedDate DATETIME NOT NULL, 
);

ALTER TABLE History.Emails DROP COLUMN Sender;
CREATE INDEX IDX_SentDate ON History.Emails(SentDate);
ALTER TABLE History.Emails ADD CONSTRAINT DF_HistoryProcessedDate DEFAULT GETDATE() FOR HistoryProcessedDate;


