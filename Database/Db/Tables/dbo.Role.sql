CREATE TABLE [dbo].[City] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [StateId]  INT            NOT NULL,
    [CityName] NVARCHAR (150) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

