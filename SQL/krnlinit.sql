IF OBJECT_ID(N'[krnl].[__MigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'krnl') IS NULL EXEC(N'CREATE SCHEMA [krnl];');
    CREATE TABLE [krnl].[__MigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___MigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF SCHEMA_ID(N'krnl') IS NULL EXEC(N'CREATE SCHEMA [krnl];');

CREATE TABLE [krnl].[TableFreshness] (
    [TableKey] varchar(64) NOT NULL,
    [LastUpdate] datetimeoffset NOT NULL,
    [ConcurrencyCheck] rowversion NOT NULL,
    CONSTRAINT [PK_TableFreshness] PRIMARY KEY ([TableKey])
);

INSERT INTO [krnl].[__MigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260106185550_Initialize', N'10.0.0');

COMMIT;
GO

