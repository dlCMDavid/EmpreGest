

/*
	David de la Cruz Morán
	Inserta el empleado administrador del programa
*/

USE DBEmpreGest;
GO

-- Insertamos el usuario admin en la base de datos
INSERT INTO Empleado(idDepartamento,correo,usuario,contraseña,nombre,apellido1,apellido2,fechaNacimiento,direccion,ciudad,pais,telefono)
VALUES(1,'daviddcm37@educastur.es','daviddcm37','276f37fa535cdaa335999ea48501a8d782ae237c4d00437d8c7bd5a3771ee11d','David','de la Cruz','Morán','18/04/2000','Contrueces','Gijón','España',691790464);