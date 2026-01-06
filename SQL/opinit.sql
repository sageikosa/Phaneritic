IF OBJECT_ID(N'[op].[__MigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'op') IS NULL EXEC(N'CREATE SCHEMA [op];');
    CREATE TABLE [op].[__MigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___MigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF SCHEMA_ID(N'op') IS NULL EXEC(N'CREATE SCHEMA [op];');

CREATE SEQUENCE [op].[AccessMechanismID] START WITH 10000 INCREMENT BY 10 NO CYCLE;

CREATE SEQUENCE [op].[AccessorID] START WITH 10000 INCREMENT BY 10 NO CYCLE;

CREATE SEQUENCE [op].[AccessSessionID] START WITH 100000 INCREMENT BY 100 NO CYCLE;

CREATE SEQUENCE [op].[OperationID] START WITH 100000 INCREMENT BY 100 NO CYCLE;

CREATE SEQUENCE [op].[OperationLogID] START WITH 100000 INCREMENT BY 100 NO CYCLE;

CREATE TABLE [op].[AccessGroups] (
    [AccessGroupKey] varchar(32) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_AccessGroups] PRIMARY KEY ([AccessGroupKey])
);

CREATE TABLE [op].[AccessorCredentialTypes] (
    [AccessorCredentialTypeKey] varchar(8) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_AccessorCredentialTypes] PRIMARY KEY ([AccessorCredentialTypeKey])
);

CREATE TABLE [op].[AccessorTypes] (
    [AccessorTypeKey] varchar(16) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_AccessorTypes] PRIMARY KEY ([AccessorTypeKey])
);

CREATE TABLE [op].[Methods] (
    [MethodKey] varchar(16) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    [IsTransient] bit NOT NULL,
    [StayWithAccessMechanism] bit NOT NULL,
    [StayWithAccessor] bit NOT NULL,
    CONSTRAINT [PK_Methods] PRIMARY KEY ([MethodKey])
);

CREATE TABLE [op].[OperationLogs] (
    [OperationLogID] bigint NOT NULL,
    [AccessSessionID] bigint NOT NULL,
    [OperationID] bigint NULL,
    [MethodKey] varchar(16) NULL,
    [LogTime] datetimeoffset NOT NULL,
    [AccessMechanismID] int NOT NULL,
    [AccessorID] int NOT NULL,
    [IsComplete] bit NOT NULL,
    CONSTRAINT [PK_OperationLogs] PRIMARY KEY ([OperationLogID])
);

CREATE TABLE [op].[OptionGroups] (
    [OptionGroupKey] varchar(16) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_OptionGroups] PRIMARY KEY ([OptionGroupKey])
);

CREATE TABLE [op].[OptionTypes] (
    [OptionTypeKey] varchar(32) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_OptionTypes] PRIMARY KEY ([OptionTypeKey])
);

CREATE TABLE [op].[ProcessNodeTypes] (
    [ProcessNodeTypeKey] varchar(16) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_ProcessNodeTypes] PRIMARY KEY ([ProcessNodeTypeKey])
);

CREATE TABLE [op].[Accessors] (
    [AccessorID] int NOT NULL,
    [AccessorKey] varchar(64) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    [AccessorTypeKey] varchar(16) NOT NULL,
    CONSTRAINT [PK_Accessors] PRIMARY KEY ([AccessorID]),
    CONSTRAINT [FK_Accessors_AccessorTypes_AccessorTypeKey] FOREIGN KEY ([AccessorTypeKey]) REFERENCES [op].[AccessorTypes] ([AccessorTypeKey]) ON DELETE CASCADE
);

CREATE TABLE [op].[AccessorTypeAccessorCredentialTypes] (
    [AccessorTypeKey] varchar(16) NOT NULL,
    [AccessorCredentialTypeKey] varchar(8) NOT NULL,
    CONSTRAINT [PK_AccessorTypeAccessorCredentialTypes] PRIMARY KEY ([AccessorTypeKey], [AccessorCredentialTypeKey]),
    CONSTRAINT [FK_AccessorTypeAccessorCredentialTypes_AccessorCredentialTypes_AccessorCredentialTypeKey] FOREIGN KEY ([AccessorCredentialTypeKey]) REFERENCES [op].[AccessorCredentialTypes] ([AccessorCredentialTypeKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccessorTypeAccessorCredentialTypes_AccessorTypes_AccessorTypeKey] FOREIGN KEY ([AccessorTypeKey]) REFERENCES [op].[AccessorTypes] ([AccessorTypeKey]) ON DELETE CASCADE
);

CREATE TABLE [op].[AccessGroupMethods] (
    [AccessGroupKey] varchar(32) NOT NULL,
    [MethodKey] varchar(16) NOT NULL,
    CONSTRAINT [PK_AccessGroupMethods] PRIMARY KEY ([AccessGroupKey], [MethodKey]),
    CONSTRAINT [FK_AccessGroupMethods_AccessGroups_AccessGroupKey] FOREIGN KEY ([AccessGroupKey]) REFERENCES [op].[AccessGroups] ([AccessGroupKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccessGroupMethods_Methods_MethodKey] FOREIGN KEY ([MethodKey]) REFERENCES [op].[Methods] ([MethodKey]) ON DELETE CASCADE
);

CREATE TABLE [op].[OptionGroupOptionTypes] (
    [OptionGroupKey] varchar(16) NOT NULL,
    [OptionTypeKey] varchar(32) NOT NULL,
    CONSTRAINT [PK_OptionGroupOptionTypes] PRIMARY KEY ([OptionGroupKey], [OptionTypeKey]),
    CONSTRAINT [FK_OptionGroupOptionTypes_OptionGroups_OptionGroupKey] FOREIGN KEY ([OptionGroupKey]) REFERENCES [op].[OptionGroups] ([OptionGroupKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_OptionGroupOptionTypes_OptionTypes_OptionTypeKey] FOREIGN KEY ([OptionTypeKey]) REFERENCES [op].[OptionTypes] ([OptionTypeKey]) ON DELETE CASCADE
);

CREATE TABLE [op].[AccessMechanismTypes] (
    [AccessMechanismTypeKey] varchar(16) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    [IsUserAccess] bit NOT NULL,
    [IsRoamingAccess] bit NOT NULL,
    [IsPoolable] bit NOT NULL,
    [IsValidatedIPAddress] bit NOT NULL,
    [ProcessNodeTypeKey] varchar(16) NULL,
    CONSTRAINT [PK_AccessMechanismTypes] PRIMARY KEY ([AccessMechanismTypeKey]),
    CONSTRAINT [FK_AccessMechanismTypes_ProcessNodeTypes_ProcessNodeTypeKey] FOREIGN KEY ([ProcessNodeTypeKey]) REFERENCES [op].[ProcessNodeTypes] ([ProcessNodeTypeKey])
);

CREATE TABLE [op].[ProcessNodes] (
    [ProcessNodeKey] varchar(32) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    [ProcessNodeTypeKey] varchar(16) NOT NULL,
    [ParentNodeKey] varchar(32) NULL,
    CONSTRAINT [PK_ProcessNodes] PRIMARY KEY ([ProcessNodeKey]),
    CONSTRAINT [FK_ProcessNodes_ProcessNodeTypes_ProcessNodeTypeKey] FOREIGN KEY ([ProcessNodeTypeKey]) REFERENCES [op].[ProcessNodeTypes] ([ProcessNodeTypeKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProcessNodes_ProcessNodes_ParentNodeKey] FOREIGN KEY ([ParentNodeKey]) REFERENCES [op].[ProcessNodes] ([ProcessNodeKey])
);

CREATE TABLE [op].[ProcessNodeTypeOptionGroups] (
    [ProcessNodeTypeKey] varchar(16) NOT NULL,
    [OptionGroupKey] varchar(16) NOT NULL,
    CONSTRAINT [PK_ProcessNodeTypeOptionGroups] PRIMARY KEY ([ProcessNodeTypeKey], [OptionGroupKey]),
    CONSTRAINT [FK_ProcessNodeTypeOptionGroups_OptionGroups_OptionGroupKey] FOREIGN KEY ([OptionGroupKey]) REFERENCES [op].[OptionGroups] ([OptionGroupKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProcessNodeTypeOptionGroups_ProcessNodeTypes_ProcessNodeTypeKey] FOREIGN KEY ([ProcessNodeTypeKey]) REFERENCES [op].[ProcessNodeTypes] ([ProcessNodeTypeKey]) ON DELETE CASCADE
);

CREATE TABLE [op].[AccessorAccessGroups] (
    [AccessorID] int NOT NULL,
    [AccessGroupKey] varchar(32) NOT NULL,
    CONSTRAINT [PK_AccessorAccessGroups] PRIMARY KEY ([AccessorID], [AccessGroupKey]),
    CONSTRAINT [FK_AccessorAccessGroups_AccessGroups_AccessGroupKey] FOREIGN KEY ([AccessGroupKey]) REFERENCES [op].[AccessGroups] ([AccessGroupKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccessorAccessGroups_Accessors_AccessorID] FOREIGN KEY ([AccessorID]) REFERENCES [op].[Accessors] ([AccessorID]) ON DELETE CASCADE
);

CREATE TABLE [op].[AccessorCredentials] (
    [AccessorID] int NOT NULL,
    [AccessorCredentialTypeKey] varchar(8) NOT NULL,
    [CredentialValue] nvarchar(512) NOT NULL,
    [IsEnabled] bit NOT NULL,
    CONSTRAINT [PK_AccessorCredentials] PRIMARY KEY ([AccessorID], [AccessorCredentialTypeKey]),
    CONSTRAINT [FK_AccessorCredentials_AccessorCredentialTypes_AccessorCredentialTypeKey] FOREIGN KEY ([AccessorCredentialTypeKey]) REFERENCES [op].[AccessorCredentialTypes] ([AccessorCredentialTypeKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccessorCredentials_Accessors_AccessorID] FOREIGN KEY ([AccessorID]) REFERENCES [op].[Accessors] ([AccessorID]) ON DELETE CASCADE
);

CREATE TABLE [op].[AccessorTypeAccessMechanismTypes] (
    [AccessorTypeKey] varchar(16) NOT NULL,
    [AccessMechanismTypeKey] varchar(16) NOT NULL,
    CONSTRAINT [PK_AccessorTypeAccessMechanismTypes] PRIMARY KEY ([AccessorTypeKey], [AccessMechanismTypeKey]),
    CONSTRAINT [FK_AccessorTypeAccessMechanismTypes_AccessMechanismTypes_AccessMechanismTypeKey] FOREIGN KEY ([AccessMechanismTypeKey]) REFERENCES [op].[AccessMechanismTypes] ([AccessMechanismTypeKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccessorTypeAccessMechanismTypes_AccessorTypes_AccessorTypeKey] FOREIGN KEY ([AccessorTypeKey]) REFERENCES [op].[AccessorTypes] ([AccessorTypeKey]) ON DELETE CASCADE
);

CREATE TABLE [op].[MethodAccessMechanismTypes] (
    [MethodKey] varchar(16) NOT NULL,
    [AccessMechanismTypeKey] varchar(16) NOT NULL,
    CONSTRAINT [PK_MethodAccessMechanismTypes] PRIMARY KEY ([MethodKey], [AccessMechanismTypeKey]),
    CONSTRAINT [FK_MethodAccessMechanismTypes_AccessMechanismTypes_AccessMechanismTypeKey] FOREIGN KEY ([AccessMechanismTypeKey]) REFERENCES [op].[AccessMechanismTypes] ([AccessMechanismTypeKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_MethodAccessMechanismTypes_Methods_MethodKey] FOREIGN KEY ([MethodKey]) REFERENCES [op].[Methods] ([MethodKey]) ON DELETE CASCADE
);

CREATE TABLE [op].[AccessMechanisms] (
    [AccessMechanismID] int NOT NULL,
    [AccessMechanismKey] varchar(64) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    [ProcessNodeKey] varchar(32) NULL,
    [IsEnabled] bit NOT NULL,
    [AccessMechanismTypeKey] varchar(16) NOT NULL,
    CONSTRAINT [PK_AccessMechanisms] PRIMARY KEY ([AccessMechanismID]),
    CONSTRAINT [FK_AccessMechanisms_AccessMechanismTypes_AccessMechanismTypeKey] FOREIGN KEY ([AccessMechanismTypeKey]) REFERENCES [op].[AccessMechanismTypes] ([AccessMechanismTypeKey]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccessMechanisms_ProcessNodes_ProcessNodeKey] FOREIGN KEY ([ProcessNodeKey]) REFERENCES [op].[ProcessNodes] ([ProcessNodeKey])
);

CREATE TABLE [op].[Options] (
    [ProcessNodeKey] varchar(32) NOT NULL,
    [OptionTypeKey] varchar(32) NOT NULL,
    [OptionValue] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_Options] PRIMARY KEY ([ProcessNodeKey], [OptionTypeKey]),
    CONSTRAINT [FK_Options_ProcessNodes_ProcessNodeKey] FOREIGN KEY ([ProcessNodeKey]) REFERENCES [op].[ProcessNodes] ([ProcessNodeKey]) ON DELETE CASCADE
);

CREATE TABLE [op].[AccessSessions] (
    [AccessSessionID] bigint NOT NULL,
    [StartedAt] datetimeoffset NOT NULL,
    [AccessorID] int NOT NULL,
    [AccessMechanismID] int NOT NULL,
    CONSTRAINT [PK_AccessSessions] PRIMARY KEY ([AccessSessionID]),
    CONSTRAINT [FK_AccessSessions_AccessMechanisms_AccessMechanismID] FOREIGN KEY ([AccessMechanismID]) REFERENCES [op].[AccessMechanisms] ([AccessMechanismID]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccessSessions_Accessors_AccessorID] FOREIGN KEY ([AccessorID]) REFERENCES [op].[Accessors] ([AccessorID]) ON DELETE CASCADE
);

CREATE TABLE [op].[Operations] (
    [OperationID] bigint NOT NULL,
    [AccessSessionID] bigint NOT NULL,
    [MethodKey] varchar(16) NOT NULL,
    [StartedAt] datetimeoffset NOT NULL,
    [AccessMechanismID] int NOT NULL,
    [AccessorID] int NOT NULL,
    CONSTRAINT [PK_Operations] PRIMARY KEY ([OperationID]),
    CONSTRAINT [FK_Operations_AccessMechanisms_AccessMechanismID] FOREIGN KEY ([AccessMechanismID]) REFERENCES [op].[AccessMechanisms] ([AccessMechanismID]),
    CONSTRAINT [FK_Operations_AccessSessions_AccessSessionID] FOREIGN KEY ([AccessSessionID]) REFERENCES [op].[AccessSessions] ([AccessSessionID]),
    CONSTRAINT [FK_Operations_Accessors_AccessorID] FOREIGN KEY ([AccessorID]) REFERENCES [op].[Accessors] ([AccessorID]),
    CONSTRAINT [FK_Operations_Methods_MethodKey] FOREIGN KEY ([MethodKey]) REFERENCES [op].[Methods] ([MethodKey])
);

CREATE INDEX [IX_AccessGroupMethods_MethodKey] ON [op].[AccessGroupMethods] ([MethodKey]);

CREATE UNIQUE INDEX [IX_AccessGroups_AccessGroupKey] ON [op].[AccessGroups] ([AccessGroupKey]);

CREATE UNIQUE INDEX [IX_AccessMechanisms_AccessMechanismKey] ON [op].[AccessMechanisms] ([AccessMechanismKey]);

CREATE INDEX [IX_AccessMechanisms_AccessMechanismTypeKey] ON [op].[AccessMechanisms] ([AccessMechanismTypeKey]);

CREATE INDEX [IX_AccessMechanisms_ProcessNodeKey] ON [op].[AccessMechanisms] ([ProcessNodeKey]);

CREATE INDEX [IX_AccessMechanismTypes_ProcessNodeTypeKey] ON [op].[AccessMechanismTypes] ([ProcessNodeTypeKey]);

CREATE UNIQUE INDEX [IX_AccessorAccessGroups_AccessGroupKey] ON [op].[AccessorAccessGroups] ([AccessGroupKey]);

CREATE INDEX [IX_AccessorCredentials_AccessorCredentialTypeKey] ON [op].[AccessorCredentials] ([AccessorCredentialTypeKey]);

CREATE INDEX [IX_Accessors_AccessorKey] ON [op].[Accessors] ([AccessorKey]);

CREATE INDEX [IX_Accessors_AccessorTypeKey] ON [op].[Accessors] ([AccessorTypeKey]);

CREATE INDEX [IX_AccessorTypeAccessMechanismTypes_AccessMechanismTypeKey] ON [op].[AccessorTypeAccessMechanismTypes] ([AccessMechanismTypeKey]);

CREATE INDEX [IX_AccessorTypeAccessorCredentialTypes_AccessorCredentialTypeKey] ON [op].[AccessorTypeAccessorCredentialTypes] ([AccessorCredentialTypeKey]);

CREATE INDEX [IX_AccessSessions_AccessMechanismID] ON [op].[AccessSessions] ([AccessMechanismID]);

CREATE INDEX [IX_AccessSessions_AccessorID] ON [op].[AccessSessions] ([AccessorID]);

CREATE INDEX [IX_MethodAccessMechanismTypes_AccessMechanismTypeKey] ON [op].[MethodAccessMechanismTypes] ([AccessMechanismTypeKey]);

CREATE INDEX [IX_Operations_AccessMechanismID_AccessorID] ON [op].[Operations] ([AccessMechanismID], [AccessorID]);

CREATE INDEX [IX_Operations_AccessorID] ON [op].[Operations] ([AccessorID]);

CREATE INDEX [IX_Operations_AccessSessionID] ON [op].[Operations] ([AccessSessionID]);

CREATE INDEX [IX_Operations_MethodKey] ON [op].[Operations] ([MethodKey]);

CREATE INDEX [IX_OptionGroupOptionTypes_OptionTypeKey] ON [op].[OptionGroupOptionTypes] ([OptionTypeKey]);

CREATE INDEX [IX_ProcessNodes_ParentNodeKey] ON [op].[ProcessNodes] ([ParentNodeKey]);

CREATE INDEX [IX_ProcessNodes_ProcessNodeTypeKey] ON [op].[ProcessNodes] ([ProcessNodeTypeKey]);

CREATE INDEX [IX_ProcessNodeTypeOptionGroups_OptionGroupKey] ON [op].[ProcessNodeTypeOptionGroups] ([OptionGroupKey]);

INSERT INTO [op].[__MigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260106185553_Initialize', N'10.0.0');

COMMIT;
GO

