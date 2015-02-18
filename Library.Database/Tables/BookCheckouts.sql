CREATE TABLE [dbo].[BookCheckouts] (
    [ISBN] NVARCHAR(64)  NOT NULL,
    [Name] NVARCHAR(64)  NOT NULL,
    [Address] NVARCHAR(128)  NOT NULL,
    [City] NVARCHAR(32)  NOT NULL,
    [State] CHAR(2)  NOT NULL,
    [ZIP] CHAR(10)  NOT NULL,
    [Email] NVARCHAR(64)  NOT NULL

	CONSTRAINT [PK_BookCheckouts]
    PRIMARY KEY CLUSTERED ([ISBN], [Email] ASC),

	CONSTRAINT [FK_BookCheckouts_Books]
	FOREIGN KEY ([ISBN]) REFERENCES [Books]([ISBN])
)