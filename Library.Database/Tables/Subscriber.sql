CREATE TABLE [dbo].[Subscribers]
(
    [Email] NVARCHAR(64)  NOT NULL,
    [Name] NVARCHAR(64)  NOT NULL,
    [Address] NVARCHAR(128)  NOT NULL,
    [City] NVARCHAR(32)  NOT NULL,
    [State] CHAR(2)  NOT NULL,
    [PostalCode] CHAR(10)  NOT NULL,

	CONSTRAINT [PK_Subscribers]
    PRIMARY KEY CLUSTERED ([Email] ASC),

)
