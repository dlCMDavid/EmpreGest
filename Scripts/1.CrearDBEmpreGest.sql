
/* 
	David de la Cruz Morán
	Crea la base de datos con todas sus restricciones 
*/


USE master;  
GO  
-- Crea la base de datos DBEmpreGest
CREATE DATABASE DBEmpreGest  
ON   
( NAME = EmpreGest_dat,  
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EmpreGest_dat.mdf',  
    SIZE = 200MB,  
    MAXSIZE = 4GB,  
    FILEGROWTH = 64MB )  
LOG ON  
( NAME = EmpreGest_log,  
    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EmpreGest_log.ldf',  
    SIZE = 20MB,  
    MAXSIZE = 80MB,  
    FILEGROWTH = 10MB ) ;  
GO

-- Cambiamos el nivel de compatiblidad para que permita ser compatible al menos con SQL Server 2014
ALTER DATABASE DBEmpreGest SET COMPATIBILITY_LEVEL = 120
GO



-- Crearemos las tablas con sus relaciones
USE DBEmpreGest;
GO


-- Tabla Departamento
CREATE TABLE Departamento
(
	idDepartamento		INTEGER			IDENTITY		PRIMARY KEY,
	descripcion			NVARCHAR(35),
);

-- Restriccion que impide que se introduzcan dos departamentos iguales
CREATE UNIQUE NONCLUSTERED INDEX idx_depCorreo_notnull ON Departamento(descripcion) WHERE descripcion IS NOT NULL;

-- Tabla Trabajador
CREATE TABLE Empleado
(
	idEmpleado			INTEGER			IDENTITY		PRIMARY KEY,
	idDepartamento		INTEGER			NOT NULL,
	activo				BIT				NOT NULL		DEFAULT 1,
	--Permitiremos nulos para que en caso de eliminar un usuario podamos dejar libre el usuario y correo
	correo				VARCHAR(100),
	usuario				VARCHAR(30),
	--Almacenará una contraseña encriptada en SHA256 que corresponde a 32 bytes por lo que crearemos un campo de 64 
	contraseña			VARCHAR(64)		NOT NULL,		
	nombre				VARCHAR(35)		NOT NULL,
	apellido1			VARCHAR(35)		NOT NULL,
	apellido2			VARCHAR(35)		NOT NULL,
	fechaNacimiento		DATE			NOT NULL,
	direccion			VARCHAR(50)		NOT NULL,
	ciudad				VARCHAR(35)		NOT NULL,
	pais				VARCHAR(35)		NOT NULL,
	telefono			INTEGER			NOT NULL,
	foto				VARBINARY(MAX),
	CONSTRAINT FK_idDepartamento FOREIGN KEY(idDepartamento) REFERENCES Departamento(idDepartamento),
);

-- Creamos la restriccion unique de forma que no se puede introducir dos correos o usuarios iguales pero si permite introduccir diversos campos nulos para cuando se borra el usuario
CREATE UNIQUE NONCLUSTERED INDEX idx_EmplCorreo_notnull ON Empleado(correo) WHERE correo IS NOT NULL;
CREATE UNIQUE NONCLUSTERED INDEX idx_EmplUsuario_notnull ON Empleado(usuario) WHERE usuario IS NOT NULL;

-- Tabla Cliente
CREATE TABLE Cliente
(
	idCliente			INTEGER			IDENTITY		PRIMARY KEY,
	correo				varchar(100),
	nombre				VARCHAR(35)		NOT NULL,
	apellido1			VARCHAR(35)		NOT NULL,
	apellido2			VARCHAR(35)		NOT NULL,
	direccion			VARCHAR(50)		NOT NULL,
	ciudad				VARCHAR(35)		NOT NULL,
	pais				VARCHAR(35)		NOT NULL,
	telefono			INTEGER			NOT NULL,
);

--- Restricion que impide que se introduzcan dos correos iguales
CREATE UNIQUE NONCLUSTERED INDEX idx_clieCorreo_notnull ON Cliente(correo) WHERE correo IS NOT NULL;

-- Tabla Pedido
CREATE TABLE Pedido
(
	idPedido			INTEGER			IDENTITY		PRIMARY KEY,
	idEmpleado			INTEGER			NOT NULL,
	idCliente			INTEGER			NOT NULL,  
	fechaPedido			DATETIME		NOT NULL		DEFAULT GETDATE(),
	fechaEntrega		DATETIME,						
	estado				VARCHAR(15)		NOT NULL		CHECK (estado='Entregado' OR estado='Enviado' OR estado='Preparando'),
	CONSTRAINT FK_idEmpleado FOREIGN KEY(idEmpleado) REFERENCES Empleado(idEmpleado),
	CONSTRAINT FK_idCliente FOREIGN KEY(idCliente) REFERENCES Cliente(idCliente),
);

-- Tabla Categoria
CREATE TABLE Categoria
(
	idCategoria			INTEGER			IDENTITY		PRIMARY KEY,
	descripcion			VARCHAR(35),
);

--- Restricion que impide que se introduzcan dos categorias iguales
CREATE UNIQUE NONCLUSTERED INDEX idx_catDescripcion_notnull ON Categoria(descripcion) WHERE descripcion IS NOT NULL;



-- Tabla Producto
CREATE TABLE Producto
(
	idProducto				INTEGER			IDENTITY		PRIMARY KEY,
	idCategoria				INTEGER			NOT NULL,
	descripcion				VARCHAR(50)		NOT NULL,
	unidadesStock			INTEGER			NOT NULL		DEFAULT 0,
	unidadesPedidas			INTEGER			NOT NULL		DEFAULT 0,
	unidadesReservadas		INTEGER			NOT NULL		DEFAULT 0,
	stockMinimo				INTEGER			NOT NULL		DEFAULT 0,
	reponer					BIT				NOT NULL		DEFAULT 1,	
	beneficio				INTEGER			NOT NULL		DEFAULT 0,
	CONSTRAINT FK_idCategoria FOREIGN KEY(idCategoria) REFERENCES Categoria(idCategoria),
	CONSTRAINT FK_idProdDescripcion UNIQUE (descripcion)
);



-- Tabla Recibir Producto
CREATE TABLE ProductoRecibir
(
	idPedido			INTEGER			IDENTITY		PRIMARY KEY,
	idProducto			INTEGER			NOT NULL,	
	precioUnitario		MONEY			NOT NULL		DEFAULT 0.00,
	cantidad			INTEGER			NOT NULL		DEFAULT 0,
	precioTotal			AS				precioUnitario * cantidad,
	fechaPedido			DATETIME		DEFAULT GETDATE(),
	fechaEntregado		DATETIME,
	
	CONSTRAINT FK_idProductoRecibir FOREIGN KEY(idProducto) REFERENCES Producto(idProducto),
);


-- Tabla DetallesPedidos
CREATE TABLE DetallePedido
(
	idPedido			INTEGER			NOT NULL,
	idProducto			INTEGER			NOT NULL, 
	cantidad			INTEGER			NOT NULL		DEFAULT 0,
	precioUnitario		MONEY			NOT NULL		DEFAULT 0.00,
	precioFinal			MONEY			NOT NULL		DEFAULT 0.00,

	CONSTRAINT PK_DetallePedido PRIMARY KEY CLUSTERED(idPedido,idProducto),
	CONSTRAINT FK_idPedido FOREIGN KEY(idPedido) REFERENCES Pedido(idPedido),
	CONSTRAINT FK_idProducto FOREIGN KEY(idProducto) REFERENCES Producto(idProducto),
);









