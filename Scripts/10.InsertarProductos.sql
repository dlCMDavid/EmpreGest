

/*
	David de la Cruz Morán
	Inserta los productos en la tabla productos 
*/

USE DBEmpreGest
GO

INSERT INTO Producto(idCategoria,descripcion,unidadesStock,unidadesPedidas,unidadesReservadas,stockMinimo,reponer,beneficio)
VALUES
	  --Libros
	  (1,'El juego del alma de Javier Castillo',20,0,0,20,1,10),
	  (1,'Reina roja de Juan Gómez-Jurado',20,10,0,30,1,12),
	  (1,'Animal de Leticia Sierra',10,10,0,20,1,14),
	  (1,'Luna Roja de Marta Martín Girón',30,20,0,50,1,9),
	  (1,'El Crítico de Roberto Sánchez Ruiz',20,0,0,20,1,3),

	  --Musica
	  (2,'El Madrileño de C.Tangana',20,10,0,30,1,5),
	  (2,'24K Magic de Bruno Mars',10,40,0,50,1,10),
	  (2,'Thriller de Michael Jackson',20,0,0,20,1,12),
	  (2,'Legend de Bob Marley',30,30,0,60,1,11),
	  (2,'Greatest Hits de Queen',40,0,0,20,1,7),

	  --Videojuegos
	  (3,'Animal Crossing: New Horizons para Nintendo Switch',20,10,0,20,1,10),
	  (3,'NieR Replicant para PS4',60,0,0,20,1,11),
	  (3,'The Last of Us Parte II para PS4',20,30,0,50,1,14),
	  (3,'Ghost of Tsushima para PS4',40,0,0,30,1,13),
	  (3,'It Takes Two para XBOX One',20,20,0,40,1,5),

	  --Alimentacion
	  (4,'Nestlé Jungly',20,20,0,40,1,3),
	  (4,'HARIBO Favoritos Classic',30,0,0,20,1,2),
	  (4,'Nutrisport Barrita',50,0,0,40,1,11),
	  (4,'Nueces de Brasil orgánicas',20,20,0,20,1,12),
	  (4,'Kinder Maxi Lait',20,50,0,30,1,13),

	  --Juguetes
	  (5,'LOL Surprise',20,40,0,60,1,6),
	  (5,'Jenga Refresh',30,0,0,20,1,4),
	  (5,'UNO classic',60,0,0,40,1,10),
	  (5,'Curdle Muñeca',20,10,0,20,1,15),
	  (5,'Barbie Dreamtopia Muñeca',0,20,0,20,1,4),

	  --Ropa
	  (6,'Joma Camiseta',20,20,0,40,1,10),
	  (6,'Calcetines Atléticos',40,10,0,20,1,12),
	  (6,'Viy Noos Pantalones',30,0,10,25,1,14),
	  (6,'Hood Noos Sudadera con Capucha',22,10,0,25,1,4),
	  (6,'BLevi 501 Original Vaqueros',25,10,0,30,1,6),

	  --Informatica
	  (7,'SanDisk Extreme Tarjeta de memoria',20,0,0,20,1,10),
	  (7,'BenQ MOBIUZ EX2510 Monitor',26,10,0,25,1,5),
	  (7,'Toshiba Canvio Basics Disco Duro',20,10,0,30,1,7),
	  (7,'Cable USB tipo C',20,30,0,50,1,11),
	  (7,'Funda de disco duro',10,20,0,20,1,15),

	  --Papeleria
	  (8,'Marcador STABILO BOSS ORIGINAL',20,0,0,20,1,3),
	  (8,'Kathay Bolígrafo Borrable',10,30,0,40,1,6),
	  (8,'Milan 4 gomas de borrar',0,20,0,20,1,10),
	  (8,'Lápices de Madera',6,50,0,40,1,8),
	  (8,'Afilalápices de plástico',40,0,0,30,1,11),

	  --Peliculas
	  (9,'Joker',20,30,0,50,1,10),
	  (9,'Aquaman',0,30,0,20,1,4),
	  (9,'Liga De La Justicia',40,0,0,30,1,6),
	  (9,'El Hombre De Acero',20,50,0,60,1,12),
	  (9,'Harry Potter Colección Completa',60,0,0,30,1,11),

	  --Deporte
	  (10,'Xiaomi Band 5',20,10,0,30,1,2),
	  (10,'Cuerda para Saltar',50,10,0,40,1,13),
	  (10,'Bandas Elásticas',12,15,0,20,1,4),
	  (10,'Pesa Rusa',20,10,0,25,1,5),
	  (10,'Mancuernas',40,10,0,40,1,9)