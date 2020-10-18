CREATE TABLE [dbo].[Product] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [ProductTitle]      NVARCHAR (250)  NOT NULL,
    [ProductName]       NVARCHAR (500)  NOT NULL,
    [ProductCategoryId] INT             NULL,
    [Description]       NVARCHAR (MAX)  NULL,
    [Price]             DECIMAL (18, 2) NOT NULL,
    [Specification]     NVARCHAR (MAX)  NULL,
    [Size]              NVARCHAR (250)  NULL,
    [Quantity]          INT             NULL,
    [Note]              NVARCHAR (MAX)  NULL,
    [IsDeleted]         BIT             NOT NULL,
    [IsActive]          BIT             NOT NULL,
    [IsVerified]        BIT             NOT NULL,
    [CreatedDt]         DATETIME2 (7)   NOT NULL,
    [CreatedBy]         INT             NOT NULL,
    [ModifiedDt]        DATETIME2 (7)   NULL,
    [ModifiedBy]        INT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

