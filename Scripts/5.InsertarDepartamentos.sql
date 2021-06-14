

/*
	David de la Cruz Morán
	Inserta los departamentos que funcionarán como roles de usuario en el programa
*/

USE DBEmpreGest;
GO

-- Inserta los departamentos que utilizaremos en la propia aplicacion para separar el personal
INSERT INTO Departamento(descripcion) 
VALUES  ('Administradores'),
		('Recursos Humanos'),
		('Ventas'),
		('Gestores de Almacén')

