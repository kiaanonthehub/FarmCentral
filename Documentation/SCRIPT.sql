/****** Object:  Table [dbo].[Product]    Script Date: 6/17/2022 4:31:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[productID] [int] IDENTITY(1,1) NOT NULL,
	[product_name] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[productID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product_Type]    Script Date: 6/17/2022 4:31:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_Type](
	[productTypeID] [int] IDENTITY(1,1) NOT NULL,
	[productType] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[productTypeID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 6/17/2022 4:31:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[userID] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[surname] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[password] [varchar](100) NOT NULL,
	[role] [varchar](25) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users_Product]    Script Date: 6/17/2022 4:31:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users_Product](
	[usersProductID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NOT NULL,
	[productID] [int] NOT NULL,
	[productTypeID] [int] NOT NULL,
	[quantity] [int] NULL,
	[product_type] [varchar](50) NOT NULL,
	[product_date] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[usersProductID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users_Product]  WITH CHECK ADD FOREIGN KEY([productID])
REFERENCES [dbo].[Product] ([productID])
GO
ALTER TABLE [dbo].[Users_Product]  WITH CHECK ADD FOREIGN KEY([productTypeID])
REFERENCES [dbo].[Product_Type] ([productTypeID])
GO
ALTER TABLE [dbo].[Users_Product]  WITH CHECK ADD FOREIGN KEY([userID])
REFERENCES [dbo].[Users] ([userID])
GO
