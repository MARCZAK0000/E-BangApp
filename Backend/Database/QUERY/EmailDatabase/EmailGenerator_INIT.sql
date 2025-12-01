CREATE SCHEMA Generator

GO;

CREATE TABLE Generator.AssemblyType 
	(AssemblyTypeID INT IDENTITY(1,1),
	AssemblyName NVARCHAR(50) NOT NULL, 
	AssemblyPath NVARCHAR(MAX) NOT NULL,
	LastUpdateDate DATETIME2)

ALTER TABLE Generator.AssemblyType ADD CONSTRAINT 
    DEF_AssemblyType_LastUpdateDate DEFAULT GETDATE() FOR LastUpdateDate

CREATE NONCLUSTERED INDEX IX_AssemblyType_AssemblyName 
 ON Generator.AssemblyType(AssemblyName)
GO;

CREATE TABLE Generator.AssemblyEntityType 
    (AssemblyEntityTypeID INT IDENTITY(1,1),
    EntityTypeName NVARCHAR(100) NOT NULL,
    LastUpdateDate DATETIME2)

ALTER TABLE Generator.AssemblyEntityType ADD CONSTRAINT 
    DEF_AssemblyEntityType_LastUpdateDate DEFAULT GETDATE() FOR LastUpdateDate
GO;

CREATE TABLE Generator.AssemblyParametersEntity 
    (AssemblyParametersEntityID INT IDENTITY(1,1),
    AssemblyTypeID INT NOT NULL,
    EntityName NVARCHAR(100) NOT NULL,
    EntityDefinition NVARCHAR(MAX) NOT NULL,
    AssemblyEntityTypeID INT NOT NULL,
    LastUpdateDate DATETIME2)
ALTER TABLE Generator.AssemblyParametersEntity ADD CONSTRAINT 
    DEF_AssemblyParametersEntity_LastUpdateDate DEFAULT GETDATE() FOR LastUpdateDate

ALTER TABLE Generator.AssemblyParametersEntity ADD CONSTRAINT 
    FK_AssemblyParametersEntity_AssemblyType FOREIGN KEY (AssemblyTypeID) REFERENCES Generator.AssemblyType(AssemblyTypeID)

ALTER TABLE Generator.AssemblyParametersEntity ADD CONSTRAINT 
    FK_AssemblyParametersEntity_AssemblyEntityType FOREIGN KEY (AssemblyEntityTypeID) REFERENCES Generator.AssemblyEntityType(AssemblyEntityTypeID)

CREATE NONCLUSTERED INDEX IX_AssemblyParametersEntity_EntityName
ON Generator.AssemblyParametersEntity (EntityName)

GO; 

CREATE Table Generator.AssemblyComponentEntity
    (AssemblyComponentEntityID INT IDENTITY(1,1),
    AssemblyTypeID INT NOT NULL,
    ComponentName NVARCHAR(100) NOT NULL,
    ComponentDefinition NVARCHAR(MAX) NOT NULL,
    AssemblyComponentTypeID INT NOT NULL,
    LastUpdateDate DATETIME2)
ALTER TABLE Generator.AssemblyComponentEntity ADD CONSTRAINT 
    DEF_AssemblyComponentEntity_LastUpdateDate DEFAULT GETDATE() FOR LastUpdateDate
ALTER TABLE Generator.AssemblyComponentEntity ADD CONSTRAINT 
    FK_AssemblyComponentEntity_AssemblyType FOREIGN KEY (AssemblyTypeID) REFERENCES Generator.AssemblyType(AssemblyTypeID)

ALTER TABLE Generator.AssemblyComponentEntity ADD CONSTRAINT 
    FK_AssemblyComponentEntity_AssemblyEntityType FOREIGN KEY (AssemblyComponentTypeID) REFERENCES Generator.AssemblyEntityType(AssemblyEntityTypeID)
CREATE NONCLUSTERED INDEX IX_AssemblyComponentEntity_ComponentName
ON Generator.AssemblyComponentEntity (ComponentName)
GO;


CREATE TABLE Generator.EmailType
    (EmailTypeID INT IDENTITY(1,1),
    EmailTypeName NVARCHAR(50) NOT NULL,
    LastUpdateDate DATETIME2)
ALTER TABLE Generator.EmailType ADD CONSTRAINT 
    DEF_EmailType_LastUpdateDate DEFAULT GETDATE() FOR LastUpdateDate
GO;

