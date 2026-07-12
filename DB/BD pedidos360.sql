--CREAR BD
CREATE DATABASE Pedidos360;
GO

USE Pedidos360;
GO


--ESTADOS

CREATE TABLE ESTADOS(
    ID_ESTADO INT IDENTITY(1,1) NOT NULL,
    DESCRIPCION NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_ESTADOS
        PRIMARY KEY (ID_ESTADO)
);
GO

-- CATEGORIA_PRODUCTO
CREATE TABLE CATEGORIA_PRODUCTO(
    ID_CATEGORIA_PRODUCTO INT IDENTITY(1,1) NOT NULL,
    NOMBRE NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_CATEGORIA_PRODUCTO
        PRIMARY KEY (ID_CATEGORIA_PRODUCTO)
);
GO

--TIPO_ROL
CREATE TABLE TIPO_ROL(
    ID_TIPO_ROL INT IDENTITY(1,1) NOT NULL,
    NOMBRE_TIPO_ROL NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_TIPO_ROL
        PRIMARY KEY (ID_TIPO_ROL)
);
GO

--ROLES
CREATE TABLE ROLES(
    ID_ROL INT IDENTITY(1,1) NOT NULL,
    ID_TIPO_ROL INT NOT NULL,
    ID_ESTADO INT NOT NULL,

    CONSTRAINT PK_ROLES
        PRIMARY KEY (ID_ROL),

    CONSTRAINT FK_ROLES_TIPO_ROL
        FOREIGN KEY (ID_TIPO_ROL)
        REFERENCES TIPO_ROL(ID_TIPO_ROL),

    CONSTRAINT FK_ROLES_ESTADOS
        FOREIGN KEY (ID_ESTADO)
        REFERENCES ESTADOS(ID_ESTADO)
);
GO


-- CLIENTES
CREATE TABLE CLIENTES(
    CEDULA NVARCHAR(20) NOT NULL,
    ID_ESTADO INT NOT NULL,
    NOMBRE NVARCHAR(100) NOT NULL,
    APELLIDO_PATERNO NVARCHAR(100) NOT NULL,
    APELLIDO_MATERNO NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_CLIENTES
        PRIMARY KEY (CEDULA),

    CONSTRAINT FK_CLIENTES_ESTADOS
        FOREIGN KEY (ID_ESTADO)
        REFERENCES ESTADOS(ID_ESTADO)
);
GO

--CORREOS_CLIENTES
CREATE TABLE CORREOS_CLIENTES(
    CEDULA NVARCHAR(20) NOT NULL,
    CORREO NVARCHAR(150) NOT NULL,

    CONSTRAINT PK_CORREOS_CLIENTES
        PRIMARY KEY (CEDULA),

    CONSTRAINT FK_CORREOS_CLIENTES_CLIENTES
        FOREIGN KEY (CEDULA)
        REFERENCES CLIENTES(CEDULA)
);
GO

--TELEFONOS_CLIENTES
CREATE TABLE TELEFONOS_CLIENTES(
    CEDULA NVARCHAR(20) NOT NULL,
    TELEFONO NVARCHAR(20) NOT NULL,

    CONSTRAINT PK_TELEFONOS_CLIENTES
        PRIMARY KEY (CEDULA),

    CONSTRAINT FK_TELEFONOS_CLIENTES_CLIENTES
        FOREIGN KEY (CEDULA)
        REFERENCES CLIENTES(CEDULA)
);
GO

--PROVINCIA
CREATE TABLE PROVINCIA(
    ID_PROVINCIA INT IDENTITY(1,1) NOT NULL,
    NOMBRE_PROVINCIA NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_PROVINCIA
        PRIMARY KEY (ID_PROVINCIA)
);
GO

--CANTON
CREATE TABLE CANTON(
    ID_CANTON INT IDENTITY(1,1) NOT NULL,
    ID_PROVINCIA INT NOT NULL,
    NOMBRE_CANTON NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_CANTON
        PRIMARY KEY (ID_CANTON),

    CONSTRAINT FK_CANTON_PROVINCIA
        FOREIGN KEY (ID_PROVINCIA)
        REFERENCES PROVINCIA(ID_PROVINCIA)
);
GO

--DISTRITO
CREATE TABLE DISTRITO(
    ID_DISTRITO INT IDENTITY(1,1) NOT NULL,
    ID_CANTON INT NOT NULL,
    NOMBRE_DISTRITO NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_DISTRITO
        PRIMARY KEY (ID_DISTRITO),

    CONSTRAINT FK_DISTRITO_CANTON
        FOREIGN KEY (ID_CANTON)
        REFERENCES CANTON(ID_CANTON)
);
GO

--DIRECCIONES
CREATE TABLE DIRECCIONES(
    CEDULA NVARCHAR(20) NOT NULL,
    ID_PROVINCIA INT NOT NULL,
    ID_CANTON INT NOT NULL,
    ID_DISTRITO INT NOT NULL,
    OTRAS_SENAS NVARCHAR(300) NOT NULL,

    CONSTRAINT PK_DIRECCIONES
        PRIMARY KEY (CEDULA),

    CONSTRAINT FK_DIRECCIONES_CLIENTES
        FOREIGN KEY (CEDULA)
        REFERENCES CLIENTES(CEDULA),

    CONSTRAINT FK_DIRECCIONES_PROVINCIA
        FOREIGN KEY (ID_PROVINCIA)
        REFERENCES PROVINCIA(ID_PROVINCIA),

    CONSTRAINT FK_DIRECCIONES_CANTON
        FOREIGN KEY (ID_CANTON)
        REFERENCES CANTON(ID_CANTON),

    CONSTRAINT FK_DIRECCIONES_DISTRITO
        FOREIGN KEY (ID_DISTRITO)
        REFERENCES DISTRITO(ID_DISTRITO)
);
GO

-- PRODUCTOS
CREATE TABLE PRODUCTOS(
    ID_PRODUCTO INT IDENTITY(1,1) NOT NULL,
    ID_CATEGORIA_PRODUCTO INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    NOMBRE NVARCHAR(150) NOT NULL,
    PRECIO_UNITARIO DECIMAL(18,2) NOT NULL,
    IMPUESTO DECIMAL(5,2) NOT NULL,
    STOCK INT NOT NULL,
    IMAGEN NVARCHAR(500) NULL,

    CONSTRAINT PK_PRODUCTOS
        PRIMARY KEY (ID_PRODUCTO),

    CONSTRAINT FK_PRODUCTOS_CATEGORIA_PRODUCTO
        FOREIGN KEY (ID_CATEGORIA_PRODUCTO)
        REFERENCES CATEGORIA_PRODUCTO(ID_CATEGORIA_PRODUCTO),

    CONSTRAINT FK_PRODUCTOS_ESTADOS
        FOREIGN KEY (ID_ESTADO)
        REFERENCES ESTADOS(ID_ESTADO),

    CONSTRAINT CK_PRODUCTOS_PRECIO
        CHECK (PRECIO_UNITARIO >= 0),

    CONSTRAINT CK_PRODUCTOS_IMPUESTO
        CHECK (IMPUESTO >= 0),

    CONSTRAINT CK_PRODUCTOS_STOCK
        CHECK (STOCK >= 0)
);
GO

--PEDIDOS
CREATE TABLE PEDIDOS(
    ID_PEDIDO INT IDENTITY(1,1) NOT NULL,
    ID_CLIENTE NVARCHAR(20) NOT NULL,
    ID_ESTADO INT NOT NULL,
    ID_ROL INT NOT NULL,
    FECHA DATETIME2 NOT NULL,
    SUBTOTAL DECIMAL(18,2) NOT NULL,
    IMPUESTO DECIMAL(18,2) NOT NULL,
    DESCUENTO DECIMAL(18,2) NOT NULL,
    TOTAL DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_PEDIDOS
        PRIMARY KEY (ID_PEDIDO),

    CONSTRAINT FK_PEDIDOS_CLIENTES
        FOREIGN KEY (ID_CLIENTE)
        REFERENCES CLIENTES(CEDULA),

    CONSTRAINT FK_PEDIDOS_ESTADOS
        FOREIGN KEY (ID_ESTADO)
        REFERENCES ESTADOS(ID_ESTADO),

    CONSTRAINT FK_PEDIDOS_ROLES
        FOREIGN KEY (ID_ROL)
        REFERENCES ROLES(ID_ROL),

    CONSTRAINT CK_PEDIDOS_SUBTOTAL
        CHECK (SUBTOTAL >= 0),

    CONSTRAINT CK_PEDIDOS_IMPUESTO
        CHECK (IMPUESTO >= 0),

    CONSTRAINT CK_PEDIDOS_DESCUENTO
        CHECK (DESCUENTO >= 0),

    CONSTRAINT CK_PEDIDOS_TOTAL
        CHECK (TOTAL >= 0)
);
GO

--DETALLE_PEDIDO
CREATE TABLE DETALLE_PEDIDO(
    ID_DETALLE_PEDIDO INT IDENTITY(1,1) NOT NULL,
    ID_PEDIDO INT NOT NULL,
    ID_PRODUCTO INT NOT NULL,
    CANTIDAD INT NOT NULL,
    PRECIO_UNITARIO DECIMAL(18,2) NOT NULL,
    DESCUENTO DECIMAL(18,2) NOT NULL,
    IMPUESTO_PORCENTAJE DECIMAL(5,2) NOT NULL,
    TOTAL_LINEA DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_DETALLE_PEDIDO
        PRIMARY KEY (ID_DETALLE_PEDIDO),

    CONSTRAINT FK_DETALLE_PEDIDO_PEDIDOS
        FOREIGN KEY (ID_PEDIDO)
        REFERENCES PEDIDOS(ID_PEDIDO),

    CONSTRAINT FK_DETALLE_PEDIDO_PRODUCTOS
        FOREIGN KEY (ID_PRODUCTO)
        REFERENCES PRODUCTOS(ID_PRODUCTO),

    CONSTRAINT CK_DETALLE_PEDIDO_CANTIDAD
        CHECK (CANTIDAD > 0),

    CONSTRAINT CK_DETALLE_PEDIDO_PRECIO
        CHECK (PRECIO_UNITARIO >= 0),

    CONSTRAINT CK_DETALLE_PEDIDO_DESCUENTO
        CHECK (DESCUENTO >= 0),

    CONSTRAINT CK_DETALLE_PEDIDO_IMPUESTO
        CHECK (IMPUESTO_PORCENTAJE >= 0),

    CONSTRAINT CK_DETALLE_PEDIDO_TOTAL
        CHECK (TOTAL_LINEA >= 0)
);
GO


INSERT INTO ESTADOS (DESCRIPCION)
VALUES
    ('Activo'),
    ('Inactivo');
GO

INSERT INTO TIPO_ROL (NOMBRE_TIPO_ROL)
VALUES
    ('Administrador'),
    ('Ventas'),
    ('Operaciones');
GO

INSERT INTO ROLES (ID_TIPO_ROL, ID_ESTADO)
VALUES
    (1, 1), -- Administrador activo
    (2, 1), -- Ventas activo
    (3, 1); -- Operaciones activo


-- INSERTAR PROVINCIAS CONSERVANDO EL CODIGO TERRITORIAL OFICIAL
SET IDENTITY_INSERT PROVINCIA ON;
INSERT INTO PROVINCIA (ID_PROVINCIA, NOMBRE_PROVINCIA)
VALUES
    (1, N'San José'),
    (2, N'Alajuela'),
    (3, N'Cartago'),
    (4, N'Heredia'),
    (5, N'Guanacaste'),
    (6, N'Puntarenas'),
    (7, N'Limón');
SET IDENTITY_INSERT PROVINCIA OFF;
GO

-- INSERTAR CANTONES CONSERVANDO EL CODIGO TERRITORIAL OFICIAL
SET IDENTITY_INSERT CANTON ON;
INSERT INTO CANTON (ID_CANTON, ID_PROVINCIA, NOMBRE_CANTON)
VALUES
    (101, 1, N'San José'),
    (102, 1, N'Escazú'),
    (103, 1, N'Desamparados'),
    (104, 1, N'Puriscal'),
    (105, 1, N'Tarrazú'),
    (106, 1, N'Aserrí'),
    (107, 1, N'Mora'),
    (108, 1, N'Goicoechea'),
    (109, 1, N'Santa Ana'),
    (110, 1, N'Alajuelita'),
    (111, 1, N'Vázquez de Coronado'),
    (112, 1, N'Acosta'),
    (113, 1, N'Tibás'),
    (114, 1, N'Moravia'),
    (115, 1, N'Montes de Oca'),
    (116, 1, N'Turrubares'),
    (117, 1, N'Dota'),
    (118, 1, N'Curridabat'),
    (119, 1, N'Pérez Zeledón'),
    (120, 1, N'León Cortés Castro'),
    (201, 2, N'Alajuela'),
    (202, 2, N'San Ramón'),
    (203, 2, N'Grecia'),
    (204, 2, N'San Mateo'),
    (205, 2, N'Atenas'),
    (206, 2, N'Naranjo'),
    (207, 2, N'Palmares'),
    (208, 2, N'Poás'),
    (209, 2, N'Orotina'),
    (210, 2, N'San Carlos'),
    (211, 2, N'Zarcero'),
    (212, 2, N'Sarchí'),
    (213, 2, N'Upala'),
    (214, 2, N'Los Chiles'),
    (215, 2, N'Guatuso'),
    (216, 2, N'Río Cuarto'),
    (301, 3, N'Cartago'),
    (302, 3, N'Paraíso'),
    (303, 3, N'La Unión'),
    (304, 3, N'Jiménez'),
    (305, 3, N'Turrialba'),
    (306, 3, N'Alvarado'),
    (307, 3, N'Oreamuno'),
    (308, 3, N'El Guarco'),
    (401, 4, N'Heredia'),
    (402, 4, N'Barva'),
    (403, 4, N'Santo Domingo'),
    (404, 4, N'Santa Bárbara'),
    (405, 4, N'San Rafael'),
    (406, 4, N'San Isidro'),
    (407, 4, N'Belén'),
    (408, 4, N'Flores'),
    (409, 4, N'San Pablo'),
    (410, 4, N'Sarapiquí'),
    (501, 5, N'Liberia'),
    (502, 5, N'Nicoya'),
    (503, 5, N'Santa Cruz'),
    (504, 5, N'Bagaces'),
    (505, 5, N'Carrillo'),
    (506, 5, N'Cañas'),
    (507, 5, N'Abangares'),
    (508, 5, N'Tilarán'),
    (509, 5, N'Nandayure'),
    (510, 5, N'La Cruz'),
    (511, 5, N'Hojancha'),
    (601, 6, N'Puntarenas'),
    (602, 6, N'Esparza'),
    (603, 6, N'Buenos Aires'),
    (604, 6, N'Montes de Oro'),
    (605, 6, N'Osa'),
    (606, 6, N'Quepos'),
    (607, 6, N'Golfito'),
    (608, 6, N'Coto Brus'),
    (609, 6, N'Parrita'),
    (610, 6, N'Corredores'),
    (611, 6, N'Garabito'),
    (612, 6, N'Monteverde'),
    (613, 6, N'Puerto Jiménez'),
    (701, 7, N'Limón'),
    (702, 7, N'Pococí'),
    (703, 7, N'Siquirres'),
    (704, 7, N'Talamanca'),
    (705, 7, N'Matina'),
    (706, 7, N'Guácimo');
SET IDENTITY_INSERT CANTON OFF;
GO

-- INSERTAR DISTRITOS CONSERVANDO EL CODIGO TERRITORIAL OFICIAL
SET IDENTITY_INSERT DISTRITO ON;
INSERT INTO DISTRITO (ID_DISTRITO, ID_CANTON, NOMBRE_DISTRITO)
VALUES
    (10101, 101, N'Carmen'),
    (10102, 101, N'Merced'),
    (10103, 101, N'Hospital'),
    (10104, 101, N'Catedral'),
    (10105, 101, N'Zapote'),
    (10106, 101, N'San Francisco de Dos Ríos'),
    (10107, 101, N'Uruca'),
    (10108, 101, N'Mata Redonda'),
    (10109, 101, N'Pavas'),
    (10110, 101, N'Hatillo'),
    (10111, 101, N'San Sebastián'),
    (10201, 102, N'Escazú'),
    (10202, 102, N'San Antonio'),
    (10203, 102, N'San Rafael'),
    (10301, 103, N'Desamparados'),
    (10302, 103, N'San Miguel'),
    (10303, 103, N'San Juan de Dios'),
    (10304, 103, N'San Rafael Arriba'),
    (10305, 103, N'San Antonio'),
    (10306, 103, N'Frailes'),
    (10307, 103, N'Patarrá'),
    (10308, 103, N'San Cristóbal'),
    (10309, 103, N'Rosario'),
    (10310, 103, N'Damas'),
    (10311, 103, N'San Rafael Abajo'),
    (10312, 103, N'Gravilias'),
    (10313, 103, N'Los Guido'),
    (10401, 104, N'Santiago'),
    (10402, 104, N'Mercedes Sur'),
    (10403, 104, N'Barbacoas'),
    (10404, 104, N'Grifo Alto'),
    (10405, 104, N'San Rafael'),
    (10406, 104, N'Candelarita'),
    (10407, 104, N'Desamparaditos'),
    (10408, 104, N'San Antonio'),
    (10409, 104, N'Chires'),
    (10501, 105, N'San Marcos'),
    (10502, 105, N'San Lorenzo'),
    (10503, 105, N'San Carlos'),
    (10601, 106, N'Aserrí'),
    (10602, 106, N'Tarbaca'),
    (10603, 106, N'Vuelta de Jorco'),
    (10604, 106, N'San Gabriel'),
    (10605, 106, N'Legua'),
    (10606, 106, N'Monterrey'),
    (10607, 106, N'Salitrillos'),
    (10701, 107, N'Colón'),
    (10702, 107, N'Guayabo'),
    (10703, 107, N'Tabarcia'),
    (10704, 107, N'Piedras Negras'),
    (10705, 107, N'Picagres'),
    (10706, 107, N'Jaris'),
    (10707, 107, N'Quitirrisí'),
    (10801, 108, N'Guadalupe'),
    (10802, 108, N'San Francisco'),
    (10803, 108, N'Calle Blancos'),
    (10804, 108, N'Mata de Plátano'),
    (10805, 108, N'Ipís'),
    (10806, 108, N'Rancho Redondo'),
    (10807, 108, N'Purral'),
    (10901, 109, N'Santa Ana'),
    (10902, 109, N'Salitral'),
    (10903, 109, N'Pozos'),
    (10904, 109, N'Uruca'),
    (10905, 109, N'Piedades'),
    (10906, 109, N'Brasil'),
    (11001, 110, N'Alajuelita'),
    (11002, 110, N'San Josecito'),
    (11003, 110, N'San Antonio'),
    (11004, 110, N'Concepción'),
    (11005, 110, N'San Felipe'),
    (11101, 111, N'San Isidro'),
    (11102, 111, N'San Rafael'),
    (11103, 111, N'Dulce Nombre de Jesús'),
    (11104, 111, N'Patalillo'),
    (11105, 111, N'Cascajal'),
    (11201, 112, N'San Ignacio'),
    (11202, 112, N'Guaitil'),
    (11203, 112, N'Palmichal'),
    (11204, 112, N'Cangrejal'),
    (11205, 112, N'Sabanillas'),
    (11301, 113, N'San Juan'),
    (11302, 113, N'Cinco Esquinas'),
    (11303, 113, N'Anselmo Llorente'),
    (11304, 113, N'León XIII'),
    (11305, 113, N'Colima'),
    (11401, 114, N'San Vicente'),
    (11402, 114, N'San Jerónimo'),
    (11403, 114, N'La Trinidad'),
    (11501, 115, N'San Pedro'),
    (11502, 115, N'Sabanilla'),
    (11503, 115, N'Mercedes'),
    (11504, 115, N'San Rafael'),
    (11601, 116, N'San Pablo'),
    (11602, 116, N'San Pedro'),
    (11603, 116, N'San Juan de Mata'),
    (11604, 116, N'San Luis'),
    (11605, 116, N'Carara'),
    (11701, 117, N'Santa María'),
    (11702, 117, N'Jardín'),
    (11703, 117, N'Copey'),
    (11801, 118, N'Curridabat'),
    (11802, 118, N'Granadilla'),
    (11803, 118, N'Sánchez'),
    (11804, 118, N'Tirrases'),
    (11901, 119, N'San Isidro de El General'),
    (11902, 119, N'El General'),
    (11903, 119, N'Daniel Flores'),
    (11904, 119, N'Rivas'),
    (11905, 119, N'San Pedro'),
    (11906, 119, N'Platanares'),
    (11907, 119, N'Pejibaye'),
    (11908, 119, N'Cajón'),
    (11909, 119, N'Barú'),
    (11910, 119, N'Río Nuevo'),
    (11911, 119, N'Páramo'),
    (11912, 119, N'La Amistad'),
    (12001, 120, N'San Pablo'),
    (12002, 120, N'San Andrés'),
    (12003, 120, N'Llano Bonito'),
    (12004, 120, N'San Isidro'),
    (12005, 120, N'Santa Cruz'),
    (12006, 120, N'San Antonio'),
    (20101, 201, N'Alajuela'),
    (20102, 201, N'San José'),
    (20103, 201, N'Carrizal'),
    (20104, 201, N'San Antonio'),
    (20105, 201, N'Guácima'),
    (20106, 201, N'San Isidro'),
    (20107, 201, N'Sabanilla'),
    (20108, 201, N'San Rafael'),
    (20109, 201, N'Río Segundo'),
    (20110, 201, N'Desamparados'),
    (20111, 201, N'Turrúcares'),
    (20112, 201, N'Tambor'),
    (20113, 201, N'Garita'),
    (20114, 201, N'Sarapiquí'),
    (20201, 202, N'San Ramón'),
    (20202, 202, N'Santiago'),
    (20203, 202, N'San Juan'),
    (20204, 202, N'Piedades Norte'),
    (20205, 202, N'Piedades Sur'),
    (20206, 202, N'San Rafael'),
    (20207, 202, N'San Isidro'),
    (20208, 202, N'Ángeles'),
    (20209, 202, N'Alfaro'),
    (20210, 202, N'Volio'),
    (20211, 202, N'Concepción'),
    (20212, 202, N'Zapotal'),
    (20213, 202, N'Peñas Blancas'),
    (20214, 202, N'San Lorenzo'),
    (20301, 203, N'Grecia'),
    (20302, 203, N'San Isidro'),
    (20303, 203, N'San José'),
    (20304, 203, N'San Roque'),
    (20305, 203, N'Tacares'),
    (20307, 203, N'Puente de Piedra'),
    (20308, 203, N'Bolívar'),
    (20401, 204, N'San Mateo'),
    (20402, 204, N'Desmonte'),
    (20403, 204, N'Jesús María'),
    (20404, 204, N'Labrador'),
    (20501, 205, N'Atenas'),
    (20502, 205, N'Jesús'),
    (20503, 205, N'Mercedes'),
    (20504, 205, N'San Isidro'),
    (20505, 205, N'Concepción'),
    (20506, 205, N'San José'),
    (20507, 205, N'Santa Eulalia'),
    (20508, 205, N'Escobal'),
    (20601, 206, N'Naranjo'),
    (20602, 206, N'San Miguel'),
    (20603, 206, N'San José'),
    (20604, 206, N'Cirrí Sur'),
    (20605, 206, N'San Jerónimo'),
    (20606, 206, N'San Juan'),
    (20607, 206, N'El Rosario'),
    (20608, 206, N'Palmitos'),
    (20701, 207, N'Palmares'),
    (20702, 207, N'Zaragoza'),
    (20703, 207, N'Buenos Aires'),
    (20704, 207, N'Santiago'),
    (20705, 207, N'Candelaria'),
    (20706, 207, N'Esquipulas'),
    (20707, 207, N'La Granja'),
    (20801, 208, N'San Pedro'),
    (20802, 208, N'San Juan'),
    (20803, 208, N'San Rafael'),
    (20804, 208, N'Carrillos'),
    (20805, 208, N'Sabana Redonda'),
    (20901, 209, N'Orotina'),
    (20902, 209, N'El Mastate'),
    (20903, 209, N'Hacienda Vieja'),
    (20904, 209, N'Coyolar'),
    (20905, 209, N'La Ceiba'),
    (21001, 210, N'Quesada'),
    (21002, 210, N'Florencia'),
    (21003, 210, N'Buenavista'),
    (21004, 210, N'Aguas Zarcas'),
    (21005, 210, N'Venecia'),
    (21006, 210, N'Pital'),
    (21007, 210, N'La Fortuna'),
    (21008, 210, N'La Tigra'),
    (21009, 210, N'La Palmera'),
    (21010, 210, N'Venado'),
    (21011, 210, N'Cutris'),
    (21012, 210, N'Monterrey'),
    (21013, 210, N'Pocosol'),
    (21101, 211, N'Zarcero'),
    (21102, 211, N'Laguna'),
    (21103, 211, N'Tapesco'),
    (21104, 211, N'Guadalupe'),
    (21105, 211, N'Palmira'),
    (21106, 211, N'Zapote'),
    (21107, 211, N'Brisas'),
    (21201, 212, N'Sarchí Norte'),
    (21202, 212, N'Sarchí Sur'),
    (21203, 212, N'Toro Amarillo'),
    (21204, 212, N'San Pedro'),
    (21205, 212, N'Rodríguez'),
    (21301, 213, N'Upala'),
    (21302, 213, N'Aguas Claras'),
    (21303, 213, N'San José O Pizote'),
    (21304, 213, N'Bijagua'),
    (21305, 213, N'Delicias'),
    (21306, 213, N'Dos Ríos'),
    (21307, 213, N'Yolillal'),
    (21308, 213, N'Canalete'),
    (21401, 214, N'Los Chiles'),
    (21402, 214, N'Caño Negro'),
    (21403, 214, N'El Amparo'),
    (21404, 214, N'San Jorge'),
    (21501, 215, N'San Rafael'),
    (21502, 215, N'Buenavista'),
    (21503, 215, N'Cote'),
    (21504, 215, N'Katira'),
    (21601, 216, N'Río Cuarto'),
    (21602, 216, N'Santa Rita'),
    (21603, 216, N'Santa Isabel'),
    (30101, 301, N'Oriental'),
    (30102, 301, N'Occidental'),
    (30103, 301, N'Carmen'),
    (30104, 301, N'San Nicolás'),
    (30105, 301, N'Aguacaliente o San Francisco'),
    (30106, 301, N'Guadalupe o Arenilla'),
    (30107, 301, N'Corralillo'),
    (30108, 301, N'Tierra Blanca'),
    (30109, 301, N'Dulce Nombre'),
    (30110, 301, N'Llano Grande'),
    (30111, 301, N'Quebradilla'),
    (30201, 302, N'Paraíso'),
    (30202, 302, N'Santiago'),
    (30203, 302, N'Orosi'),
    (30204, 302, N'Cachí'),
    (30205, 302, N'Llanos de Santa Lucía'),
    (30206, 302, N'Birrisito'),
    (30301, 303, N'Tres Ríos'),
    (30302, 303, N'San Diego'),
    (30303, 303, N'San Juan'),
    (30304, 303, N'San Rafael'),
    (30305, 303, N'Concepción'),
    (30306, 303, N'Dulce Nombre'),
    (30307, 303, N'San Ramón'),
    (30308, 303, N'Río Azul'),
    (30401, 304, N'Juan Viñas'),
    (30402, 304, N'Tucurrique'),
    (30403, 304, N'Pejibaye'),
    (30404, 304, N'La Victoria'),
    (30501, 305, N'Turrialba'),
    (30502, 305, N'La Suiza'),
    (30503, 305, N'Peralta'),
    (30504, 305, N'Santa Cruz'),
    (30505, 305, N'Santa Teresita'),
    (30506, 305, N'Pavones'),
    (30507, 305, N'Tuis'),
    (30508, 305, N'Tayutic'),
    (30509, 305, N'Santa Rosa'),
    (30510, 305, N'Tres Equis'),
    (30511, 305, N'La Isabel'),
    (30512, 305, N'Chirripó'),
    (30601, 306, N'Pacayas'),
    (30602, 306, N'Cervantes'),
    (30603, 306, N'Capellades'),
    (30701, 307, N'San Rafael'),
    (30702, 307, N'Cot'),
    (30703, 307, N'Potrero Cerrado'),
    (30704, 307, N'Cipreses'),
    (30705, 307, N'Santa Rosa'),
    (30801, 308, N'El Tejar'),
    (30802, 308, N'San Isidro'),
    (30803, 308, N'Tobosi'),
    (30804, 308, N'Patio de Agua'),
    (40101, 401, N'Heredia'),
    (40102, 401, N'Mercedes'),
    (40103, 401, N'San Francisco'),
    (40104, 401, N'Ulloa'),
    (40105, 401, N'Varablanca'),
    (40201, 402, N'Barva'),
    (40202, 402, N'San Pedro'),
    (40203, 402, N'San Pablo'),
    (40204, 402, N'San Roque'),
    (40205, 402, N'Santa Lucía'),
    (40206, 402, N'San José de la Montaña'),
    (40207, 402, N'Puente Salas'),
    (40301, 403, N'Santo Domingo'),
    (40302, 403, N'San Vicente'),
    (40303, 403, N'San Miguel'),
    (40304, 403, N'Paracito'),
    (40305, 403, N'Santo Tomás'),
    (40306, 403, N'Santa Rosa'),
    (40307, 403, N'Tures'),
    (40308, 403, N'Pará'),
    (40401, 404, N'Santa Bárbara'),
    (40402, 404, N'San Pedro'),
    (40403, 404, N'San Juan'),
    (40404, 404, N'Jesús'),
    (40405, 404, N'Santo Domingo'),
    (40406, 404, N'Purabá'),
    (40501, 405, N'San Rafael'),
    (40502, 405, N'San Josecito'),
    (40503, 405, N'Santiago'),
    (40504, 405, N'Ángeles'),
    (40505, 405, N'Concepción'),
    (40601, 406, N'San Isidro'),
    (40602, 406, N'San José'),
    (40603, 406, N'Concepción'),
    (40604, 406, N'San Francisco'),
    (40701, 407, N'San Antonio'),
    (40702, 407, N'La Ribera'),
    (40703, 407, N'La Asunción'),
    (40801, 408, N'San Joaquín'),
    (40802, 408, N'Barrantes'),
    (40803, 408, N'Llorente'),
    (40901, 409, N'San Pablo'),
    (40902, 409, N'Rincón de Sabanilla'),
    (41001, 410, N'Puerto Viejo'),
    (41002, 410, N'La Virgen'),
    (41003, 410, N'Las Horquetas'),
    (41004, 410, N'Llanuras del Gaspar'),
    (41005, 410, N'Cureña'),
    (50101, 501, N'Liberia'),
    (50102, 501, N'Cañas Dulces'),
    (50103, 501, N'Mayorga'),
    (50104, 501, N'Nacascolo'),
    (50105, 501, N'Curubandé'),
    (50201, 502, N'Nicoya'),
    (50202, 502, N'Mansión'),
    (50203, 502, N'San Antonio'),
    (50204, 502, N'Quebrada Honda'),
    (50205, 502, N'Sámara'),
    (50206, 502, N'Nosara'),
    (50207, 502, N'Belén de Nosarita'),
    (50301, 503, N'Santa Cruz'),
    (50302, 503, N'Bolsón'),
    (50303, 503, N'Veintisiete de Abril'),
    (50304, 503, N'Tempate'),
    (50305, 503, N'Cartagena'),
    (50306, 503, N'Cuajiniquil'),
    (50307, 503, N'Diriá'),
    (50308, 503, N'Cabo Velas'),
    (50309, 503, N'Tamarindo'),
    (50401, 504, N'Bagaces'),
    (50402, 504, N'La Fortuna'),
    (50403, 504, N'Mogote'),
    (50404, 504, N'Río Naranjo'),
    (50501, 505, N'Filadelfia'),
    (50502, 505, N'Palmira'),
    (50503, 505, N'Sardinal'),
    (50504, 505, N'Belén'),
    (50601, 506, N'Cañas'),
    (50602, 506, N'Palmira'),
    (50603, 506, N'San Miguel'),
    (50604, 506, N'Bebedero'),
    (50605, 506, N'Porozal'),
    (50701, 507, N'Las Juntas'),
    (50702, 507, N'Sierra'),
    (50703, 507, N'San Juan'),
    (50704, 507, N'Colorado'),
    (50801, 508, N'Tilarán'),
    (50802, 508, N'Quebrada Grande'),
    (50803, 508, N'Tronadora'),
    (50804, 508, N'Santa Rosa'),
    (50805, 508, N'Líbano'),
    (50806, 508, N'Tierras Morenas'),
    (50807, 508, N'Arenal'),
    (50808, 508, N'Cabeceras'),
    (50901, 509, N'Carmona'),
    (50902, 509, N'Santa Rita'),
    (50903, 509, N'Zapotal'),
    (50904, 509, N'San Pablo'),
    (50905, 509, N'Porvenir'),
    (50906, 509, N'Bejuco'),
    (51001, 510, N'La Cruz'),
    (51002, 510, N'Santa Cecilia'),
    (51003, 510, N'La Garita'),
    (51004, 510, N'Santa Elena'),
    (51101, 511, N'Hojancha'),
    (51102, 511, N'Monte Romo'),
    (51103, 511, N'Puerto Carrillo'),
    (51104, 511, N'Huacas'),
    (51105, 511, N'Matambú'),
    (60101, 601, N'Puntarenas'),
    (60102, 601, N'Pitahaya'),
    (60103, 601, N'Chomes'),
    (60104, 601, N'Lepanto'),
    (60105, 601, N'Paquera'),
    (60106, 601, N'Manzanillo'),
    (60107, 601, N'Guacimal'),
    (60108, 601, N'Barranca'),
    (60110, 601, N'Isla del Coco'),
    (60111, 601, N'Cóbano'),
    (60112, 601, N'Chacarita'),
    (60113, 601, N'Chira'),
    (60114, 601, N'Acapulco'),
    (60115, 601, N'El Roble'),
    (60116, 601, N'Arancibia'),
    (60201, 602, N'Espíritu Santo'),
    (60202, 602, N'San Juan Grande'),
    (60203, 602, N'Macacona'),
    (60204, 602, N'San Rafael'),
    (60205, 602, N'San Jerónimo'),
    (60206, 602, N'Caldera'),
    (60301, 603, N'Buenos Aires'),
    (60302, 603, N'Volcán'),
    (60303, 603, N'Potrero Grande'),
    (60304, 603, N'Boruca'),
    (60305, 603, N'Pilas'),
    (60306, 603, N'Colinas'),
    (60307, 603, N'Chánguena'),
    (60308, 603, N'Biolley'),
    (60309, 603, N'Brunka'),
    (60401, 604, N'Miramar'),
    (60402, 604, N'La Unión'),
    (60403, 604, N'San Isidro'),
    (60501, 605, N'Puerto Cortés'),
    (60502, 605, N'Palmar'),
    (60503, 605, N'Sierpe'),
    (60504, 605, N'Bahía Ballena'),
    (60505, 605, N'Piedras Blancas'),
    (60506, 605, N'Bahía Drake'),
    (60601, 606, N'Quepos'),
    (60602, 606, N'Savegre'),
    (60603, 606, N'Naranjito'),
    (60701, 607, N'Golfito'),
    (60703, 607, N'Guaycará'),
    (60704, 607, N'Pavón'),
    (60801, 608, N'San Vito'),
    (60802, 608, N'Sabalito'),
    (60803, 608, N'Aguabuena'),
    (60804, 608, N'Limoncito'),
    (60805, 608, N'Pittier'),
    (60806, 608, N'Gutiérrez Braun'),
    (60901, 609, N'Parrita'),
    (61001, 610, N'Corredor'),
    (61002, 610, N'La Cuesta'),
    (61003, 610, N'Canoas'),
    (61004, 610, N'Laurel'),
    (61101, 611, N'Jacó'),
    (61102, 611, N'Tárcoles'),
    (61103, 611, N'Lagunillas'),
    (61201, 612, N'Monteverde'),
    (61301, 613, N'Puerto Jiménez'),
    (70101, 701, N'Limón'),
    (70102, 701, N'Valle La Estrella'),
    (70103, 701, N'Río Blanco'),
    (70104, 701, N'Matama'),
    (70201, 702, N'Guápiles'),
    (70202, 702, N'Jiménez'),
    (70203, 702, N'Rita'),
    (70204, 702, N'Roxana'),
    (70205, 702, N'Cariari'),
    (70206, 702, N'Colorado'),
    (70207, 702, N'La Colonia'),
    (70301, 703, N'Siquirres'),
    (70302, 703, N'Pacuarito'),
    (70303, 703, N'Florida'),
    (70304, 703, N'Germania'),
    (70305, 703, N'El Cairo'),
    (70306, 703, N'Alegría'),
    (70307, 703, N'Reventazón'),
    (70401, 704, N'Bratsi'),
    (70402, 704, N'Sixaola'),
    (70403, 704, N'Cahuita'),
    (70404, 704, N'Telire'),
    (70501, 705, N'Matina'),
    (70502, 705, N'Batán'),
    (70503, 705, N'Carrandí'),
    (70601, 706, N'Guácimo'),
    (70602, 706, N'Mercedes'),
    (70603, 706, N'Pocora'),
    (70604, 706, N'Río Jiménez'),
    (70605, 706, N'Duacarí');
SET IDENTITY_INSERT DISTRITO OFF;
GO

--CADA PROVICNIA TIENE SUS CANTONES Y QUE CADA CANTON TIENE SUS DISTRITOS 
SELECT
    P.ID_PROVINCIA,
    P.NOMBRE_PROVINCIA,
    C.ID_CANTON,
    C.NOMBRE_CANTON,
    D.ID_DISTRITO,
    D.NOMBRE_DISTRITO
FROM PROVINCIA P
INNER JOIN CANTON C
    ON P.ID_PROVINCIA = C.ID_PROVINCIA
INNER JOIN DISTRITO D
    ON C.ID_CANTON = D.ID_CANTON
ORDER BY
    P.ID_PROVINCIA,
    C.ID_CANTON,
    D.ID_DISTRITO;