-- =============================================
-- Generator Database Schema - SQLite
-- Based on GeneratorDbContext
-- =============================================

-- Tabela AssemblyTypes
CREATE TABLE IF NOT EXISTS AssemblyTypes (
    AssemblyTypeId INTEGER PRIMARY KEY AUTOINCREMENT,
    AssemblyName TEXT NOT NULL CHECK(LENGTH(AssemblyName) <= 50),
    AssemblyPath TEXT NOT NULL CHECK(LENGTH(AssemblyPath) <= 255),
    LastUpdateTime TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX IF NOT EXISTS IX_AssemblyTypes_AssemblyName 
    ON AssemblyTypes(AssemblyName);

-- Tabela AssemblyEntityTypes
CREATE TABLE IF NOT EXISTS AssemblyEntityTypes (
    AssemblyEntityTypeId INTEGER PRIMARY KEY AUTOINCREMENT,
    AssemblyEntityTypeName TEXT NOT NULL CHECK(LENGTH(AssemblyEntityTypeName) <= 100),
    LastUpdateTime TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX IF NOT EXISTS IX_AssemblyEntityTypes_AssemblyEntityTypeName 
    ON AssemblyEntityTypes(AssemblyEntityTypeName);

-- Tabela AssemblyParametersEntities
CREATE TABLE IF NOT EXISTS AssemblyParametersEntities (
    AssemblyParametersEntityId INTEGER PRIMARY KEY AUTOINCREMENT,
    AssemblyEntityTypeId INTEGER NOT NULL,
    AssemblyTypeId INTEGER NOT NULL,
    EntityParametersName TEXT NOT NULL CHECK(LENGTH(EntityParametersName) <= 100),
    EntityParametersValue TEXT NOT NULL CHECK(LENGTH(EntityParametersValue) <= 500),
    LastUpdateTime TEXT NOT NULL DEFAULT (datetime('now')),
    CONSTRAINT FK_AssemblyParametersEntities_AssemblyEntityType 
        FOREIGN KEY (AssemblyEntityTypeId) 
        REFERENCES AssemblyEntityTypes(AssemblyEntityTypeId) 
        ON DELETE RESTRICT,
    CONSTRAINT FK_AssemblyParametersEntities_AssemblyType 
        FOREIGN KEY (AssemblyTypeId) 
        REFERENCES AssemblyTypes(AssemblyTypeId) 
        ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS IX_AssemblyParametersEntities_EntityParametersName 
    ON AssemblyParametersEntities(EntityParametersName);

CREATE INDEX IF NOT EXISTS IX_AssemblyParametersEntities_AssemblyEntityTypeId 
    ON AssemblyParametersEntities(AssemblyEntityTypeId);

CREATE INDEX IF NOT EXISTS IX_AssemblyParametersEntities_AssemblyTypeId 
    ON AssemblyParametersEntities(AssemblyTypeId);

-- Tabela AssemblyComponentEntities
CREATE TABLE IF NOT EXISTS AssemblyComponentEntities (
    AssemblyComponentEntityId INTEGER PRIMARY KEY AUTOINCREMENT,
    AssemblyEntityTypeId INTEGER NOT NULL,
    AssemblyTypeId INTEGER NOT NULL,
    ComponentName TEXT NOT NULL CHECK(LENGTH(ComponentName) <= 100),
    ComponentValue TEXT NOT NULL CHECK(LENGTH(ComponentValue) <= 500),
    LastUpdateTime TEXT NOT NULL DEFAULT (datetime('now')),
    CONSTRAINT FK_AssemblyComponentEntities_AssemblyEntityType 
        FOREIGN KEY (AssemblyEntityTypeId) 
        REFERENCES AssemblyEntityTypes(AssemblyEntityTypeId) 
        ON DELETE RESTRICT,
    CONSTRAINT FK_AssemblyComponentEntities_AssemblyType 
        FOREIGN KEY (AssemblyTypeId) 
        REFERENCES AssemblyTypes(AssemblyTypeId) 
        ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS IX_AssemblyComponentEntities_ComponentName 
    ON AssemblyComponentEntities(ComponentName);

CREATE INDEX IF NOT EXISTS IX_AssemblyComponentEntities_AssemblyEntityTypeId 
    ON AssemblyComponentEntities(AssemblyEntityTypeId);

CREATE INDEX IF NOT EXISTS IX_AssemblyComponentEntities_AssemblyTypeId 
    ON AssemblyComponentEntities(AssemblyTypeId);

-- Tabela EmailTypes
CREATE TABLE IF NOT EXISTS EmailTypes (
    EmailTypeId INTEGER PRIMARY KEY AUTOINCREMENT,
    EmailTypeName TEXT NOT NULL CHECK(LENGTH(EmailTypeName) <= 100),
    LastUpdateTime TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX IF NOT EXISTS IX_EmailTypes_EmailTypeName 
    ON EmailTypes(EmailTypeName);

-- Tabela RenderStrategies
CREATE TABLE IF NOT EXISTS RenderStrategies (
    RenderStrategyId INTEGER PRIMARY KEY AUTOINCREMENT,
    RenderStrategyName TEXT NOT NULL CHECK(LENGTH(RenderStrategyName) <= 100),
    LastUpdateTime TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX IF NOT EXISTS IX_RenderStrategies_RenderStrategyName 
    ON RenderStrategies(RenderStrategyName);

-- Tabela EmailRenders (główna tabela z wieloma FK)
CREATE TABLE IF NOT EXISTS EmailRenders (
    EmailRenderId INTEGER PRIMARY KEY AUTOINCREMENT,
    EmailTypeId INTEGER NOT NULL,
    EmailRenderStrategyId INTEGER NOT NULL,
    HeaderParametersId INTEGER NOT NULL,
    BodyParametersId INTEGER NOT NULL,
    FooterParametersId INTEGER NOT NULL,
    HeaderComponenetsId INTEGER NOT NULL,
    BodyComponenetsId INTEGER NOT NULL,
    FooterComponenetsId INTEGER NOT NULL,
    LastUpdateTime TEXT NOT NULL DEFAULT (datetime('now')),
    CONSTRAINT FK_EmailRenders_EmailType 
        FOREIGN KEY (EmailTypeId) 
        REFERENCES EmailTypes(EmailTypeId) 
        ON DELETE RESTRICT,
    CONSTRAINT FK_EmailRenders_RenderStrategy 
        FOREIGN KEY (EmailRenderStrategyId) 
        REFERENCES RenderStrategies(RenderStrategyId) 
        ON DELETE RESTRICT,
    CONSTRAINT FK_EmailRenders_HeaderParameter 
        FOREIGN KEY (HeaderParametersId) 
        REFERENCES AssemblyParametersEntities(AssemblyParametersEntityId) 
        ON DELETE RESTRICT,
    CONSTRAINT FK_EmailRenders_BodyParameter 
        FOREIGN KEY (BodyParametersId) 
        REFERENCES AssemblyParametersEntities(AssemblyParametersEntityId) 
        ON DELETE RESTRICT,
    CONSTRAINT FK_EmailRenders_FooterParameter 
        FOREIGN KEY (FooterParametersId) 
        REFERENCES AssemblyParametersEntities(AssemblyParametersEntityId) 
        ON DELETE RESTRICT,
    CONSTRAINT FK_EmailRenders_HeaderComponent 
        FOREIGN KEY (HeaderComponenetsId) 
        REFERENCES AssemblyComponentEntities(AssemblyComponentEntityId) 
        ON DELETE RESTRICT,
    CONSTRAINT FK_EmailRenders_BodyComponent 
        FOREIGN KEY (BodyComponenetsId) 
        REFERENCES AssemblyComponentEntities(AssemblyComponentEntityId) 
        ON DELETE RESTRICT,
    CONSTRAINT FK_EmailRenders_FooterComponent 
        FOREIGN KEY (FooterComponenetsId) 
        REFERENCES AssemblyComponentEntities(AssemblyComponentEntityId) 
        ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS IX_EmailRenders_EmailTypeId 
    ON EmailRenders(EmailTypeId);

CREATE INDEX IF NOT EXISTS IX_EmailRenders_EmailRenderStrategyId 
    ON EmailRenders(EmailRenderStrategyId);

CREATE INDEX IF NOT EXISTS IX_EmailRenders_HeaderParametersId 
    ON EmailRenders(HeaderParametersId);

CREATE INDEX IF NOT EXISTS IX_EmailRenders_FooterParametersId 
    ON EmailRenders(FooterParametersId);

CREATE INDEX IF NOT EXISTS IX_EmailRenders_BodyParametersId 
    ON EmailRenders(BodyParametersId);

CREATE INDEX IF NOT EXISTS IX_EmailRenders_HeaderComponenetsId 
    ON EmailRenders(HeaderComponenetsId);

CREATE INDEX IF NOT EXISTS IX_EmailRenders_FooterComponenetsId 
    ON EmailRenders(FooterComponenetsId);

CREATE INDEX IF NOT EXISTS IX_EmailRenders_BodyComponenetsId 
    ON EmailRenders(BodyComponenetsId);
