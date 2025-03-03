-- Creacion de base de datos restaurante_catracho_db
CREATE DATABASE restaurante_catracho_db;
USE restaurante_catracho_db;
GO

-- Creacion de tablas
CREATE TABLE Usuarios (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(255) NOT NULL,
    correo NVARCHAR(255) UNIQUE NOT NULL,
    clave NVARCHAR(MAX) NOT NULL,
    rol NVARCHAR(50) CHECK (rol IN ('1', '2', '3', '4')) NOT NULL,
    telefono NVARCHAR(20) UNIQUE NOT NULL,
    direccion NVARCHAR(MAX)
);

CREATE TABLE Productos (
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(255) NOT NULL,
    descripcion NVARCHAR(MAX),
    precio DECIMAL(10,2) NOT NULL,
    categoria NVARCHAR(100),
    disponible BIT DEFAULT 1
);

CREATE TABLE Pedidos (
    id_pedido INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    numero_pedido VARCHAR(255) UNIQUE NOT NULL,
    estado NVARCHAR(50) CHECK (estado IN ('1', '2', '3', '4')) NOT NULL,
    fecha_creacion DATETIME DEFAULT GETDATE(),
    fecha_entrega_estimada DATETIME,
    monto_total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario) ON DELETE CASCADE
);

CREATE TABLE Detalles_Pedidos (
    id_detalle INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido INT NOT NULL,
    id_producto INT NOT NULL,
    numero_pedido VARCHAR(255),
    cantidad INT CHECK (cantidad > 0) NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (id_pedido) REFERENCES Pedidos(id_pedido) ON DELETE CASCADE,
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto) ON DELETE CASCADE
);

CREATE TABLE Pagos (
    id_pago INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido INT NOT NULL,
    numero_pedido VARCHAR(255),
    monto DECIMAL(10,2) NOT NULL,
    metodo_pago NVARCHAR(50) CHECK (metodo_pago IN ('1', '2', '3')) NOT NULL,
    fecha_pago DATETIME DEFAULT GETDATE(),
    estado_pago NVARCHAR(50) CHECK (estado_pago IN ('1', '2', '3')) NOT NULL,
    FOREIGN KEY (id_pedido) REFERENCES Pedidos(id_pedido) ON DELETE CASCADE
);

