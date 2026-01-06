IF OBJECT_ID(N'[ldgr].[__MigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'ldgr') IS NULL EXEC(N'CREATE SCHEMA [ldgr];');
    CREATE TABLE [ldgr].[__MigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___MigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF SCHEMA_ID(N'ldgr') IS NULL EXEC(N'CREATE SCHEMA [ldgr];');

CREATE SEQUENCE [ldgr].[ActivityID] START WITH 10000 INCREMENT BY 100 NO CYCLE;

CREATE TABLE [ldgr].[ActivityTypes] (
    [ActivityTypeKey] varchar(16) NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    [Category] varchar(16) NOT NULL,
    CONSTRAINT [PK_ActivityTypes] PRIMARY KEY ([ActivityTypeKey])
);

CREATE TABLE [ldgr].[Activities] (
    [ActivityID] bigint NOT NULL,
    [AccessorID] int NOT NULL,
    [AccessMechanismID] int NOT NULL,
    [AccessSessionID] bigint NOT NULL,
    [ActivityTypeKey] varchar(16) NOT NULL,
    [MethodKey] varchar(16) NOT NULL,
    [OperationID] bigint NOT NULL,
    [StartAt] datetimeoffset NOT NULL,
    [EndAt] datetimeoffset NOT NULL,
    [DurationMicroSeconds] bigint NOT NULL,
    [DurationMilliSeconds] bigint NOT NULL,
    [IsSuccessful] bit NOT NULL,
    [EntryCount] int NOT NULL,
    [InfoEntryCount] int NOT NULL,
    [ExceptionEntryCount] int NOT NULL,
    CONSTRAINT [PK_Activities] PRIMARY KEY ([ActivityID]),
    CONSTRAINT [FK_Activities_ActivityTypes_ActivityTypeKey] FOREIGN KEY ([ActivityTypeKey]) REFERENCES [ldgr].[ActivityTypes] ([ActivityTypeKey]) ON DELETE CASCADE
);

CREATE TABLE [ldgr].[ExceptionEntries] (
    [ActivityID] bigint NOT NULL,
    [EntryIndex] int NOT NULL,
    [ExceptionName] nvarchar(64) NOT NULL,
    [Message] nvarchar(256) NOT NULL,
    [StackTrace] nvarchar(max) NULL,
    [AccessorID] int NOT NULL,
    [AccessMechanismID] int NOT NULL,
    [AccessSessionID] bigint NOT NULL,
    [ActivityTypeKey] varchar(16) NOT NULL,
    [MethodKey] varchar(16) NOT NULL,
    [OperationID] bigint NOT NULL,
    [RecordedAt] datetimeoffset NOT NULL,
    [OffsetMicroSeconds] bigint NOT NULL,
    [OffsetMilliSeconds] bigint NOT NULL,
    CONSTRAINT [PK_ExceptionEntries] PRIMARY KEY ([ActivityID], [EntryIndex]),
    CONSTRAINT [FK_ExceptionEntries_Activities_ActivityID] FOREIGN KEY ([ActivityID]) REFERENCES [ldgr].[Activities] ([ActivityID]) ON DELETE CASCADE
);

CREATE TABLE [ldgr].[InfoEntries] (
    [ActivityID] bigint NOT NULL,
    [EntryIndex] int NOT NULL,
    [InfoEntryKey] varchar(16) NOT NULL,
    [InfoEntryValue] nvarchar(256) NOT NULL,
    [AccessorID] int NOT NULL,
    [AccessMechanismID] int NOT NULL,
    [AccessSessionID] bigint NOT NULL,
    [ActivityTypeKey] varchar(16) NOT NULL,
    [MethodKey] varchar(16) NOT NULL,
    [OperationID] bigint NOT NULL,
    [RecordedAt] datetimeoffset NOT NULL,
    [OffsetMicroSeconds] bigint NOT NULL,
    [OffsetMilliSeconds] bigint NOT NULL,
    CONSTRAINT [PK_InfoEntries] PRIMARY KEY ([ActivityID], [EntryIndex], [InfoEntryKey]),
    CONSTRAINT [FK_InfoEntries_Activities_ActivityID] FOREIGN KEY ([ActivityID]) REFERENCES [ldgr].[Activities] ([ActivityID]) ON DELETE CASCADE
);

CREATE INDEX [IX_Activities_ActivityTypeKey] ON [ldgr].[Activities] ([ActivityTypeKey]);

INSERT INTO [ldgr].[__MigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260106185555_Initialize', N'10.0.0');

COMMIT;
GO

