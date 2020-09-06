CREATE TABLE [dbo].[Account] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (250)  Not NULL,
	[CompanyName] NVARCHAR (500)  NULL,
    [Email]      NVARCHAR (MAX) NOT NULL,
    [Password]   NVARCHAR (MAX) NOT NULL,
    [Image]      NVARCHAR (MAX) NULL,
    [IsDeleted]  BIT            NOT NULL,
    [IsActive]   BIT            NOT NULL,
    [IsVerified] BIT            NOT NULL,
    [RoleId]     INT            NOT NULL,
    [CreatedDt]  DATETIME       NOT NULL,
    [ModifiedBy] INT            NULL,
    [ModifiedDt] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

