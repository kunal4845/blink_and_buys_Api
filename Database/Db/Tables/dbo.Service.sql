CREATE TABLE [dbo].[Service] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [ServiceName] NVARCHAR (MAX) NOT NULL,
    [ServiceIcon] NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [IsActive]    BIT            NOT NULL,
    [IsDeleted]   BIT            NOT NULL,
    [CreatedDt]   DATETIME2 (7)  NOT NULL,
    [CreatedBy]   INT            NOT NULL,
    [ModifiedDt]  DATETIME2 (7)  NULL,
    [ModifiedBy]  INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

