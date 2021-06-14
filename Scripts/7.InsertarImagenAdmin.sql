
/*
	David de la Cruz Morán
	Añade la imagen al empleado administrador
*/

USE DBEmpreGest;
GO


--Insertamos una imagen, se debe modificar la ruta según la ubicación de la imagen ya que SQL no permite la inserción de rutas relativas
UPDATE Empleado
SET foto = BulkColumn  FROM Openrowset(Bulk 'C:\Users\WServer\Desktop\Scripts\fotoAdministrador.png', Single_Blob) as Imagen
WHERE usuario = 'daviddcm37'