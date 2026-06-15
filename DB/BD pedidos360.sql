--Crear BD
CREATE DATABASE Pedidos360
-- Categorias
CREATE TABLE Categorias (
    Id      INT IDENTITY(1,1) PRIMARY KEY,
    Nombre  NVARCHAR(100) NOT NULL
);
GO

-- Productos
CREATE TABLE Productos (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Nombre        NVARCHAR(150)   NOT NULL,
    CategoriaId   INT             NOT NULL,
    Precio        DECIMAL(18,2)   NOT NULL,
    ImpuestoPorc  DECIMAL(5,2)    NOT NULL,
    Stock         INT             NOT NULL,
    ImagenUrl     NVARCHAR(500)   NOT NULL,
    Activo        BIT             NOT NULL DEFAULT 1,

    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId)
        REFERENCES Categorias(Id)
);
GO

-- Clientes
CREATE TABLE Clientes (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Nombre    NVARCHAR(150) NOT NULL,
    Cedula    NVARCHAR(20)  NOT NULL,
    Correo    NVARCHAR(100) NOT NULL,
    Telefono  NVARCHAR(20)  NOT NULL,
    Direccion NVARCHAR(250) NOT NULL
);
GO

select * from Clientes
select * from Categorias
select * from Productos