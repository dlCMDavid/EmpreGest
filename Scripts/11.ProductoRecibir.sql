

/*
	David de la Cruz Morán
	Inserta el registro de la llega de productos correspondientes al almacén que se van a recibir o ya se recibieron
*/
USE DBEmpreGest
GO

--Libros
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (1, 11.83, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (2, 12.07, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (2, 13.34, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (3, 9.55, 10, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (3, 8.65, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (4, 9.61, 30, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (4, 10.66, 20);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (5, 13.7, 20);

--Musica
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (6, 8.71, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (6, 10.43, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (7, 15.34, 10, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (7, 12.56, 40);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (8, 9.83, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (9, 7.6, 30, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (9, 10.64, 30);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (10, 8.37, 40, GETDATE());

--Videojuegos
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (11, 20.25, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (11, 19.25, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (12, 23.8, 60, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (13, 26.58, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (13, 21.38, 30);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (14, 30.63, 40, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (15, 19.8, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (15, 21.84, 20);

--Alimentacion
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (16, 2.93, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (16, 3.24, 20);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (17, 2.49, 30, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (18, 4.14, 50, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (19, 2.93, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (19, 1.63, 20);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (20, 2.88, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (20, 3.24, 50);

--Juguetes
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (21, 6.85, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (21, 8.23, 40);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (22, 9.96, 30, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (23, 6.18, 60, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (24, 8.16, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (24, 7.23, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (25, 11.43, 20, GETDATE());

--Ropa
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (26, 11.23, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (26, 9.42, 20);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (27, 5.34, 40, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (27, 3.45, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (28, 20.34, 30, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (29, 22.74, 22, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (29, 19.53, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (30, 24.26, 25, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (30, 27.26, 10);

--Informatica
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (31, 24.63, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (32, 80.55, 26, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (32, 72.72, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (33, 24.48, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (33, 31.52, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (34, 9.56, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (34, 11.32, 30);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (35, 10.33, 10, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (35, 8.25, 20);

--Papeleria
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (36, 4.72, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (37, 1.86, 10, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (37, 2.47, 30);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (38, 2.1, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (39, 1.28, 6, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (39, 2.31, 50);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (40, 1.83, 40, GETDATE());

--Peliculas
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (41, 11.48, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (41, 12.21, 30);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (42, 16.14, 30, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (43, 14.82, 40, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (44, 11.43, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (44, 13.61, 50);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (45, 14.14, 60, GETDATE());

--Deporte
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (46, 16.98, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (46, 19.42, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (47, 11.94, 50, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (47, 9.52, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (48, 5.65, 12, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (48, 6.35, 15);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (49, 8.67, 20, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (49, 7.42, 10);
insert into ProductoRecibir (idProducto, precioUnitario, cantidad, fechaEntregado) values (50, 12.25, 40, GETDATE());
insert into ProductoRecibir (idProducto, precioUnitario, cantidad) values (50, 16.52, 10);
