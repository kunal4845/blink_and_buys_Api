CREATE TABLE [dbo].[Account] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (250)  NULL,
    [Email]      NVARCHAR (MAX) NOT NULL,
    [Password]   NVARCHAR (MAX) NULL,
    [Image]      NVARCHAR (MAX) NULL,
    [IsDeleted]  BIT            NOT NULL,
    [IsActive]   BIT            NOT NULL,
    [RoleId]     INT            NOT NULL,
    [CreatedDt]  DATETIME       NOT NULL,
    [ModifiedBy] INT            NULL,
    [ModifiedDt] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

