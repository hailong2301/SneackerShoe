CREATE DATABASE [SneakerShoeStore]

CREATE TABLE [dbo].[Brand](
	[BrandId] [int] primary key IDENTITY(1,1) NOT NULL,
	[BrandName] [nvarchar](250) NULL,
)

CREATE TABLE [dbo].[Product](
	[ProductId] [int] primary key IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](250),
	[Price] [float],
	[Discount] [float],
	[Image] [nvarchar](50),
	[Description] [nvarchar](500),
	[BrandId] [int] NOT NULL foreign key references [dbo].[Brand]([BrandId]),
	[CreatedDate] [datetime]
)

CREATE TABLE [dbo].[Size](
	[SizeId] [int] primary key IDENTITY(1,1) NOT NULL,
	[SizeNumber] [int]
)

CREATE TABLE [dbo].[ProductSize](
	[ProductId] [int] NOT NULL foreign key references [dbo].[Product]([ProductId]),
	[SizeId] [int] NOT NULL foreign key references [dbo].[Size]([SizeId]),
	[Quantity] [int] NULL
)

CREATE TABLE [dbo].[User](
	[UserId] int IDENTITY(1,1) NOT NULL primary key,
	[Email] varchar(255) NOT NULL,
	[Password] varchar(30) NOT NULL,
	[Avatar] varchar(255),
	[Name] nvarchar(255),
	[Gender] bit,
	[Phone] varchar(30),
	[Address] nvarchar(max),
	[Role] varchar(30),
	[UserStatus] bit
)
CREATE TABLE [dbo].[Order](
	[OrderId] [int] primary key IDENTITY(1,1) NOT NULL,
	[UserId] int  NOT NULL foreign key references [dbo].[User]([UserId]),
	[Quantity] [int] NULL,
	[TotalPrice] [float] NULL,
	[OrderDate] [datetime] NULL,
)

CREATE TABLE [dbo].[OrderDetail](
	[OrderDetailId] [int] primary key IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL foreign key references [dbo].[Order]([OrderId]),
	[ProductId] [int] NOT NULL foreign key references [dbo].[Product]([ProductId]),
	[SizeId] [int] NOT NULL foreign key references [dbo].[Size]([SizeId]),
	[Quantity] [int] NOT NULL,
	[Price] [float] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Amount] [float] NOT NULL
)

CREATE TABLE [dbo].[Cart](
	[RecordId] [int] primary key IDENTITY(1,1) NOT NULL,
	[CartId] [int] NOT NULL,
	[ProductId] [int] NOT NULL foreign key references [dbo].[Product]([ProductId]),
	[SizeId] [int] NOT NULL foreign key references [dbo].[Size]([SizeId]),
	[Quantity] [int] NOT NULL,
)

alter table ProductSize
add ProductSizeId int primary key IDENTITY(1,1);

ALTER TABLE OrderDetail
ADD CONSTRAINT orderdetailprimary PRIMARY KEY (OrderDetailId);

drop table OrderDetail