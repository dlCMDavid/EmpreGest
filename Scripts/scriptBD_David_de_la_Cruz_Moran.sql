USE [master]
GO
/****** Object:  Database [DBEmpreGest]    Script Date: 11/06/2021 0:07:40 ******/
CREATE DATABASE [DBEmpreGest]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EmpreGest_dat', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EmpreGest_dat.mdf' , SIZE = 204800KB , MAXSIZE = 4194304KB , FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EmpreGest_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EmpreGest_log.ldf' , SIZE = 20480KB , MAXSIZE = 81920KB , FILEGROWTH = 10240KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [DBEmpreGest] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DBEmpreGest].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DBEmpreGest] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DBEmpreGest] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DBEmpreGest] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DBEmpreGest] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DBEmpreGest] SET ARITHABORT OFF 
GO
ALTER DATABASE [DBEmpreGest] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DBEmpreGest] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DBEmpreGest] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DBEmpreGest] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DBEmpreGest] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DBEmpreGest] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DBEmpreGest] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DBEmpreGest] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DBEmpreGest] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DBEmpreGest] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DBEmpreGest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DBEmpreGest] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DBEmpreGest] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DBEmpreGest] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DBEmpreGest] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DBEmpreGest] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DBEmpreGest] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DBEmpreGest] SET RECOVERY FULL 
GO
ALTER DATABASE [DBEmpreGest] SET  MULTI_USER 
GO
ALTER DATABASE [DBEmpreGest] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DBEmpreGest] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DBEmpreGest] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DBEmpreGest] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DBEmpreGest] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DBEmpreGest] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'DBEmpreGest', N'ON'
GO
ALTER DATABASE [DBEmpreGest] SET QUERY_STORE = OFF
GO
USE [DBEmpreGest]
GO
/****** Object:  User [CuentaEntity]    Script Date: 11/06/2021 0:07:40 ******/
CREATE USER [CuentaEntity] FOR LOGIN [CuentaEntity] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [CuentaEntity]
GO
/****** Object:  UserDefinedFunction [dbo].[fnPrecioCompra]    Script Date: 11/06/2021 0:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Funcion que calcula automaticamente el precio de venta
CREATE FUNCTION [dbo].[fnPrecioCompra](@idProducto INTEGER)
RETURNS MONEY
AS 
BEGIN
	DECLARE
		@precioTotal			MONEY,
		@unidades				INTEGER

		SELECT @unidades = SUM(cantidad), @precioTotal = SUM(precioTotal)
		FROM ProductoRecibir
		WHERE idProducto = @idProducto

		RETURN @precioTotal/@unidades
END
GO
/****** Object:  UserDefinedFunction [dbo].[fnPrecioVenta]    Script Date: 11/06/2021 0:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Calcula el precio de venta
CREATE FUNCTION [dbo].[fnPrecioVenta](@idProducto INTEGER)
RETURNS MONEY
AS 
BEGIN
	DECLARE
		@precioTotal	MONEY

		SELECT @precioTotal = dbo.fnPrecioCompra(@idProducto) * (100 + beneficio)/100
		FROM Producto AS PR
		WHERE idProducto = @idProducto

		RETURN @precioTotal
END
GO
/****** Object:  Table [dbo].[Categoria]    Script Date: 11/06/2021 0:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categoria](
	[idCategoria] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](35) NULL,
PRIMARY KEY CLUSTERED 
(
	[idCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 11/06/2021 0:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[idCliente] [int] IDENTITY(1,1) NOT NULL,
	[correo] [varchar](100) NULL,
	[nombre] [varchar](35) NOT NULL,
	[apellido1] [varchar](35) NOT NULL,
	[apellido2] [varchar](35) NOT NULL,
	[direccion] [varchar](50) NOT NULL,
	[ciudad] [varchar](35) NOT NULL,
	[pais] [varchar](35) NOT NULL,
	[telefono] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departamento]    Script Date: 11/06/2021 0:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departamento](
	[idDepartamento] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [nvarchar](35) NULL,
PRIMARY KEY CLUSTERED 
(
	[idDepartamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetallePedido]    Script Date: 11/06/2021 0:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetallePedido](
	[idPedido] [int] NOT NULL,
	[idProducto] [int] NOT NULL,
	[cantidad] [int] NOT NULL,
	[precioUnitario] [money] NOT NULL,
	[precioFinal] [money] NOT NULL,
 CONSTRAINT [PK_DetallePedido] PRIMARY KEY CLUSTERED 
(
	[idPedido] ASC,
	[idProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empleado]    Script Date: 11/06/2021 0:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado](
	[idEmpleado] [int] IDENTITY(1,1) NOT NULL,
	[idDepartamento] [int] NOT NULL,
	[activo] [bit] NOT NULL,
	[correo] [varchar](100) NULL,
	[usuario] [varchar](30) NULL,
	[contraseña] [varchar](64) NOT NULL,
	[nombre] [varchar](35) NOT NULL,
	[apellido1] [varchar](35) NOT NULL,
	[apellido2] [varchar](35) NOT NULL,
	[fechaNacimiento] [date] NOT NULL,
	[direccion] [varchar](50) NOT NULL,
	[ciudad] [varchar](35) NOT NULL,
	[pais] [varchar](35) NOT NULL,
	[telefono] [int] NOT NULL,
	[foto] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[idEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pedido]    Script Date: 11/06/2021 0:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedido](
	[idPedido] [int] IDENTITY(1,1) NOT NULL,
	[idEmpleado] [int] NOT NULL,
	[idCliente] [int] NOT NULL,
	[fechaPedido] [datetime] NOT NULL,
	[fechaEntrega] [datetime] NULL,
	[estado] [varchar](15) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producto]    Script Date: 11/06/2021 0:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[idProducto] [int] IDENTITY(1,1) NOT NULL,
	[idCategoria] [int] NOT NULL,
	[descripcion] [varchar](50) NOT NULL,
	[unidadesStock] [int] NOT NULL,
	[unidadesPedidas] [int] NOT NULL,
	[unidadesReservadas] [int] NOT NULL,
	[stockMinimo] [int] NOT NULL,
	[reponer] [bit] NOT NULL,
	[beneficio] [int] NOT NULL,
	[precioCompraUnitario]  AS ([dbo].[fnPrecioCompra]([idProducto])),
	[precioVentaUnitario]  AS ([dbo].[fnPrecioVenta]([idProducto])),
PRIMARY KEY CLUSTERED 
(
	[idProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [FK_idProdDescripcion] UNIQUE NONCLUSTERED 
(
	[descripcion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductoRecibir]    Script Date: 11/06/2021 0:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoRecibir](
	[idPedido] [int] IDENTITY(1,1) NOT NULL,
	[idProducto] [int] NOT NULL,
	[precioUnitario] [money] NOT NULL,
	[cantidad] [int] NOT NULL,
	[precioTotal]  AS ([precioUnitario]*[cantidad]),
	[fechaPedido] [datetime] NULL,
	[fechaEntregado] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[idPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [idx_catDescripcion_notnull]    Script Date: 11/06/2021 0:07:40 ******/
CREATE UNIQUE NONCLUSTERED INDEX [idx_catDescripcion_notnull] ON [dbo].[Categoria]
(
	[descripcion] ASC
)
WHERE ([descripcion] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [idx_clieCorreo_notnull]    Script Date: 11/06/2021 0:07:40 ******/
CREATE UNIQUE NONCLUSTERED INDEX [idx_clieCorreo_notnull] ON [dbo].[Cliente]
(
	[correo] ASC
)
WHERE ([correo] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [idx_depCorreo_notnull]    Script Date: 11/06/2021 0:07:40 ******/
CREATE UNIQUE NONCLUSTERED INDEX [idx_depCorreo_notnull] ON [dbo].[Departamento]
(
	[descripcion] ASC
)
WHERE ([descripcion] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [idx_EmplCorreo_notnull]    Script Date: 11/06/2021 0:07:40 ******/
CREATE UNIQUE NONCLUSTERED INDEX [idx_EmplCorreo_notnull] ON [dbo].[Empleado]
(
	[correo] ASC
)
WHERE ([correo] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [idx_EmplUsuario_notnull]    Script Date: 11/06/2021 0:07:40 ******/
CREATE UNIQUE NONCLUSTERED INDEX [idx_EmplUsuario_notnull] ON [dbo].[Empleado]
(
	[usuario] ASC
)
WHERE ([usuario] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DetallePedido] ADD  DEFAULT ((0)) FOR [cantidad]
GO
ALTER TABLE [dbo].[DetallePedido] ADD  DEFAULT ((0.00)) FOR [precioUnitario]
GO
ALTER TABLE [dbo].[DetallePedido] ADD  DEFAULT ((0.00)) FOR [precioFinal]
GO
ALTER TABLE [dbo].[Empleado] ADD  DEFAULT ((1)) FOR [activo]
GO
ALTER TABLE [dbo].[Pedido] ADD  DEFAULT (getdate()) FOR [fechaPedido]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((0)) FOR [unidadesStock]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((0)) FOR [unidadesPedidas]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((0)) FOR [unidadesReservadas]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((0)) FOR [stockMinimo]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((1)) FOR [reponer]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((0)) FOR [beneficio]
GO
ALTER TABLE [dbo].[ProductoRecibir] ADD  DEFAULT ((0.00)) FOR [precioUnitario]
GO
ALTER TABLE [dbo].[ProductoRecibir] ADD  DEFAULT ((0)) FOR [cantidad]
GO
ALTER TABLE [dbo].[ProductoRecibir] ADD  DEFAULT (getdate()) FOR [fechaPedido]
GO
ALTER TABLE [dbo].[DetallePedido]  WITH CHECK ADD  CONSTRAINT [FK_idPedido] FOREIGN KEY([idPedido])
REFERENCES [dbo].[Pedido] ([idPedido])
GO
ALTER TABLE [dbo].[DetallePedido] CHECK CONSTRAINT [FK_idPedido]
GO
ALTER TABLE [dbo].[DetallePedido]  WITH CHECK ADD  CONSTRAINT [FK_idProducto] FOREIGN KEY([idProducto])
REFERENCES [dbo].[Producto] ([idProducto])
GO
ALTER TABLE [dbo].[DetallePedido] CHECK CONSTRAINT [FK_idProducto]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK_idDepartamento] FOREIGN KEY([idDepartamento])
REFERENCES [dbo].[Departamento] ([idDepartamento])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_idDepartamento]
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_idCliente] FOREIGN KEY([idCliente])
REFERENCES [dbo].[Cliente] ([idCliente])
GO
ALTER TABLE [dbo].[Pedido] CHECK CONSTRAINT [FK_idCliente]
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_idEmpleado] FOREIGN KEY([idEmpleado])
REFERENCES [dbo].[Empleado] ([idEmpleado])
GO
ALTER TABLE [dbo].[Pedido] CHECK CONSTRAINT [FK_idEmpleado]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_idCategoria] FOREIGN KEY([idCategoria])
REFERENCES [dbo].[Categoria] ([idCategoria])
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_idCategoria]
GO
ALTER TABLE [dbo].[ProductoRecibir]  WITH CHECK ADD  CONSTRAINT [FK_idProductoRecibir] FOREIGN KEY([idProducto])
REFERENCES [dbo].[Producto] ([idProducto])
GO
ALTER TABLE [dbo].[ProductoRecibir] CHECK CONSTRAINT [FK_idProductoRecibir]
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD CHECK  (([estado]='Entregado' OR [estado]='Enviado' OR [estado]='Preparando'))
GO
USE [master]
GO
ALTER DATABASE [DBEmpreGest] SET  READ_WRITE 
GO
