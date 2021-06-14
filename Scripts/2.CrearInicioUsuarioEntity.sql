


/*
	Crea el login, el usuario y le da permisos a la cuenta que se utilizará para conectar el programa y la base de datos a traves de Entity
*/

USE DBEmpreGest
GO

--Crea el inicio de sesión en el sistema de base de datos
CREATE LOGIN CuentaEntity   
    WITH PASSWORD = 'Entity12';  
GO  

-- Crea el usuario de la base de datos
CREATE USER CuentaEntity FOR LOGIN CuentaEntity;  
GO  

-- Asignamos los permisos al usuario de la base de datos para insertar los datos
EXEC sp_addrolemember N'db_owner', N'CuentaEntity'
GO


