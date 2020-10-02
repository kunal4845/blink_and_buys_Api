CREATE TABLE [dbo].[ProductCategory] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (500) NOT NULL,
    [IsActive]   BIT            NOT NULL,
    [IsDeleted]  BIT            NOT NULL,
    [CreatedDt]  DATETIME       NOT NULL,
    [CreatedBy]  INT            NOT NULL,
    [ModifiedDt] DATETIME       NULL,
    [ModifiedBy] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

