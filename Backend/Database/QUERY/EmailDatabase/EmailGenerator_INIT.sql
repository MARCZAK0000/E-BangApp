CREATE DATABASE 

CREATE SCHEMA Generator

GO

CREATE TABLE Generator.AssemblyType 
    (AssemblyTypeID INT IDENTITY(1,1),
     AssemblyName NVARCHAR(50) NOT NULL, 
     AssemblyPath NVARCHAR(MAX) NOT NULL,
     LastUpdateDate DATETIME2);

ALTER TABLE Generator.AssemblyType 
    ADD CONSTRAINT PK_AssemblyType PRIMARY KEY CLUSTERED (AssemblyTypeID);

ALTER TABLE Generator.AssemblyType ADD CONSTRAINT 
    DEF_AssemblyType_LastUpdateDate DEFAULT GETDATE() FOR LastUpdateDate;

CREATE NONCLUSTERED INDEX IX_AssemblyType_AssemblyName 
 ON Generator.AssemblyType(AssemblyName)
GO


CREATE TABLE Generator.AssemblyParametersEntity 
    (AssemblyParametersEntityID INT IDENTITY(1,1),
    AssemblyTypeID INT NOT NULL,
    EntityName NVARCHAR(100) NOT NULL,
    EntityDefinition NVARCHAR(MAX) NOT NULL,
    LastUpdateDate DATETIME2)
ALTER TABLE Generator.AssemblyParametersEntity ADD CONSTRAINT 
    DEF_AssemblyParametersEntity_LastUpdateDate DEFAULT GETDATE() FOR LastUpdateDate

ALTER TABLE Generator.AssemblyParametersEntity ADD CONSTRAINT 
    PK_AssemblyParametersEntity PRIMARY KEY (AssemblyParametersEntityID)

ALTER TABLE Generator.AssemblyParametersEntity ADD CONSTRAINT 
    FK_AssemblyParametersEntity_AssemblyType FOREIGN KEY (AssemblyTypeID) REFERENCES Generator.AssemblyType(AssemblyTypeID)

CREATE NONCLUSTERED INDEX IX_AssemblyParametersEntity_EntityName
ON Generator.AssemblyParametersEntity (EntityName)

GO

CREATE Table Generator.AssemblyComponentEntity
    (AssemblyComponentEntityID INT IDENTITY(1,1),
    AssemblyTypeID INT NOT NULL,
    ComponentName NVARCHAR(100) NOT NULL,
    ComponentDefinition NVARCHAR(MAX) NOT NULL,
    AssemblyComponentTypeID INT NOT NULL,
    AssemblyEntityType INT NOT NULL, 
    LastUpdateDate DATETIME2)
ALTER TABLE Generator.AssemblyComponentEntity ADD CONSTRAINT 
    DEF_AssemblyComponentEntity_LastUpdateDate DEFAULT GETDATE() FOR LastUpdateDate

ALTER TABLE Generator.AssemblyComponentEntity ADD CONSTRAINT 
    PK_AssemblyComponentEntity PRIMARY KEY (AssemblyComponentEntityID)

ALTER TABLE Generator.AssemblyComponentEntity ADD CONSTRAINT 
    FK_AssemblyComponentEntity_AssemblyType FOREIGN KEY (AssemblyTypeID) REFERENCES Generator.AssemblyType(AssemblyTypeID)

CREATE NONCLUSTERED INDEX IX_AssemblyComponentEntity_ComponentName
ON Generator.AssemblyComponentEntity (ComponentName)
GO


CREATE TABLE Generator.EmailType
    (EmailTypeID INT IDENTITY(1,1),
    EmailTypeName NVARCHAR(50) NOT NULL,
    LastUpdateDate DATETIME2)
ALTER TABLE Generator.EmailType ADD CONSTRAINT 
    PK_EmailType PRIMARY KEY (EmailTypeID)
ALTER TABLE Generator.EmailType ADD CONSTRAINT 
    DEF_EmailType_LastUpdateDate DEFAULT GETDATE() FOR LastUpdateDate
GO

CREATE TABLE Generator.EmailTemplateStrategy
    (EmailTemplateStrategyID INT IDENTITY(1,1),
    StrategyName NVARCHAR(100) NOT NULL,
    EmailTypeID INT NOT NULL,
    HeaderParametersEntityID INT NOT NULL,
    HeaderComponentEntityID INT NOT NULL,
    BodyParametersEntityID INT NOT NULL,
    BodyComponentEntityID INT NOT NULL,
    FooterParametersEntityID INT NOT NULL,
    FooterComponentEntityID INT NOT NULL, 
    LastUpdateDate DATETIME2)
ALTER TABLE Generator.EmailTemplateStrategy ADD CONSTRAINT 
    DEF_EmailTemplateStrategy_LastUpdateDate DEFAULT GETDATE() FOR LastUpdateDate

ALTER TABLE Generator.EmailTemplateStrategy ADD CONSTRAINT 
    PK_EmailTemplateStrategy PRIMARY KEY (EmailTemplateStrategyID)

ALTER TABLE Generator.EmailTemplateStrategy ADD CONSTRAINT 
    FK_EmailTemplateStrategy_EmailType FOREIGN KEY (EmailTypeID) REFERENCES Generator.EmailType(EmailTypeID)

ALTER TABLE Generator.EmailTemplateStrategy ADD CONSTRAINT 
    FK_EmailTemplateStrategy_HeaderParametersEntity FOREIGN KEY (HeaderParametersEntityID) REFERENCES Generator.AssemblyParametersEntity(AssemblyParametersEntityID)

ALTER TABLE Generator.EmailTemplateStrategy ADD CONSTRAINT 
    FK_EmailTemplateStrategy_HeaderComponentEntity FOREIGN KEY (HeaderComponentEntityID) REFERENCES Generator.AssemblyComponentEntity(AssemblyComponentEntityID)

ALTER TABLE Generator.EmailTemplateStrategy ADD CONSTRAINT
    FK_EmailTemplateStrategy_BodyParametersEntity FOREIGN KEY (BodyParametersEntityID) REFERENCES Generator.AssemblyParametersEntity(AssemblyParametersEntityID)

ALTER TABLE Generator.EmailTemplateStrategy ADD CONSTRAINT
    FK_EmailTemplateStrategy_BodyComponentEntity FOREIGN KEY (BodyComponentEntityID) REFERENCES Generator.AssemblyComponentEntity(AssemblyComponentEntityID)

ALTER TABLE Generator.EmailTemplateStrategy ADD CONSTRAINT
    FK_EmailTemplateStrategy_FooterParametersEntity FOREIGN KEY (FooterParametersEntityID) REFERENCES Generator.AssemblyParametersEntity(AssemblyParametersEntityID)

ALTER TABLE Generator.EmailTemplateStrategy ADD CONSTRAINT
    FK_EmailTemplateStrategy_FooterComponentEntity FOREIGN KEY (FooterComponentEntityID) REFERENCES Generator.AssemblyComponentEntity(AssemblyComponentEntityID)

CREATE NONCLUSTERED INDEX IX_EmailTemplateStrategy_StrategyName
ON Generator.EmailTemplateStrategy (StrategyName)

CREATE NONCLUSTERED INDEX IX_EmailTemplateStrategy_EmailTypeID
ON Generator.EmailTemplateStrategy (EmailTypeID)


GO