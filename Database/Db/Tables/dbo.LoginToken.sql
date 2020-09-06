CREATE TABLE [dbo].[LoginToken] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Token]      NVARCHAR (MAX) NOT NULL,
    [UserId]     INT            NOT NULL,
    [CreatedDt]  DATETIME       NOT NULL,
    [ModifiedDt] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

