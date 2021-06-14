
/*
	David de la Cruz Morán
	Crea el disparador que repondrá el producto si baja la cantidad disponible por debajo del stock minimo
*/


USE DBEmpreGest
GO

CREATE TRIGGER trgReponerAutomaticamente
ON Producto
AFTER UPDATE AS
BEGIN
	DECLARE @cantidadPedir INTEGER,
			@idProducto INTEGER,
			@precioCompraUnitario MONEY,
			@reponer bit

	SELECT @idProducto = prd.idProducto, @cantidadPedir = (prd.stockMinimo - (prd.unidadesPedidas + prd.unidadesStock)), 
			@precioCompraUnitario = prd.precioCompraUnitario, @reponer = prd.reponer
	FROM Producto as prd, inserted as ins
	WHERE prd.idProducto = ins.IdProducto

	IF(@cantidadPedir > 0 AND  @reponer = 1)
	BEGIN
		UPDATE Producto
			SET unidadesPedidas += ((p.stockMinimo - (p.unidadesPedidas + p.unidadesStock)) + p.stockMinimo/2)
		FROM Producto as p, inserted as ins
		WHERE @idProducto = p.idProducto

		INSERT INTO ProductoRecibir(idProducto,precioUnitario,cantidad)
		VALUES(@idProducto,@precioCompraUnitario,@cantidadPedir)
	END
END
GO