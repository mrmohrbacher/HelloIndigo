-- Creating table 'Books'
CREATE TABLE [dbo].[Books] (
    [ISBN] NVARCHAR(64)  NOT NULL,
    [Title] NVARCHAR(128)  NOT NULL,
    [Publisher] NVARCHAR(64)  NOT NULL,
    [Author] NVARCHAR(64)  NOT NULL,
    [Synopsis] NVARCHAR(max)  NULL,

	CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([ISBN] ASC)
);
GO
