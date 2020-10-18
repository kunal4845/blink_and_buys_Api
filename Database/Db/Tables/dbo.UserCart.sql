CREATE TABLE [dbo].[UserCart] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [UserId]     INT      NOT NULL,
    [ProductId]  INT      NOT NULL,
    [Quantity]   INT      NULL,
    [IsDeleted]  BIT      NOT NULL,
    [CreatedDt]  DATETIME NOT NULL,
    [CreatedBy]  INT      NOT NULL,
    [ModifiedDt] DATETIME NULL,
    [ModifiedBy] INT      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

