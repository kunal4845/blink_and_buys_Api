CREATE TABLE [dbo].[ProductCategory]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[Name] nvarchar(500) not null,
	IsActive bit not null,
	IsDeleted bit not null,
	CreatedDt datetime not null,
	CreatedBy int not null,
	ModifiedDt datetime null,
	ModifiedBy int null
)
