
/*
	David de la Cruz Mor�n
	A�ade la imagen al empleado administrador
*/

USE DBEmpreGest;
GO


--Insertamos una imagen, se debe modificar la ruta seg�n la ubicaci�n de la imagen ya que SQL no permite la inserci�n de rutas relativas
UPDATE Empleado
SET foto = BulkColumn  FROM Openrowset(Bulk 'C:\Users\WServer\Desktop\Scripts\fotoAdministrador.png', Single_Blob) as Imagen
WHERE usuario = 'daviddcm37'