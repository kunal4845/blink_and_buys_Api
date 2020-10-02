CREATE TABLE [dbo].[ProductImage] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (250) NOT NULL,
    [ImagePath]      NVARCHAR (MAX) NOT NULL,
    [ProductId]      INT            NOT NULL,
    [IsPrimaryImage] BIT            NOT NULL,
    [IsDeleted]      BIT            NOT NULL,
    [CreatedDt]      DATETIME2 (7)  NOT NULL,
    [CreatedBy]      INT            NOT NULL,
    [ModifiedDt]     DATETIME2 (7)  NULL,
    [ModifiedBy]     INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

