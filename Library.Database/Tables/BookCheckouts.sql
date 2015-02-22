CREATE TABLE [dbo].[BookCheckouts] (
    [ISBN] NVARCHAR(64)  NOT NULL,
    [Email] NVARCHAR(64)  NOT NULL,
	[DateOut] DATETIME NOT NULL, 
    [DateIn] DATETIME NULL, 
    [Name] NVARCHAR(64)  NOT NULL,
    [Address] NVARCHAR(128)  NOT NULL,
    [City] NVARCHAR(32)  NOT NULL,
    [State] CHAR(2)  NOT NULL,
    [PostalCode] CHAR(10)  NOT NULL,

	CONSTRAINT [PK_BookCheckouts]
    PRIMARY KEY CLUSTERED ([ISBN], [Email], [DateOut] ASC),

    CONSTRAINT [FK_BookCheckouts_Books]
	FOREIGN KEY ([ISBN]) REFERENCES [Books]([ISBN])
)