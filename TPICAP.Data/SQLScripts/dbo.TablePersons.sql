CREATE TABLE [dbo].[Persons] (
    [Id]         INT           NOT NULL IDENTITY,
    [FirstName]  VARCHAR (100) NULL,
    [LastName]   VARCHAR (100) NULL,
    [DOB]        DATETIME      NULL,
    [Salutation] VARCHAR (150) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

SET IDENTITY_INSERT [dbo].[Persons] ON