
/*
	David de la Cruz Morán
	Crea la funciones que calculan el precio y las añade a la tabla productos
		Se debe tener en cuenta que se debe ir seleccionando y ejecutando uno a uno
*/
USE DBEmpreGest
GO

--Funcion que calcula automaticamente el precio de venta
CREATE FUNCTION fnPrecioCompra(@idProducto INTEGER)
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


--Modificamos la tabla y le añadimos la columna que calcula el precio unitario
ALTER TABLE PRODUCTO
ADD precioCompraUnitario AS dbo.fnPrecioCompra(idProducto)
GO

-- Calcula el precio de venta
CREATE FUNCTION fnPrecioVenta(@idProducto INTEGER)
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

--añadimos la columna precio venta unitario
ALTER TABLE PRODUCTO
ADD precioVentaUnitario AS dbo.fnPrecioVenta(idProducto)
GO