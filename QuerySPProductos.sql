
-- Procedimientos CRUD para la tabla Productos

-- Crear un producto
CREATE PROCEDURE sp_InsertProducto
    @nombre NVARCHAR(255),
    @descripcion NVARCHAR(MAX),
    @precio DECIMAL(10,2),
    @categoria NVARCHAR(100),
    @disponible BIT = 1
AS
BEGIN
    INSERT INTO Productos (nombre, descripcion, precio, categoria, disponible)
    VALUES (@nombre, @descripcion, @precio, @categoria, @disponible);
END;

-- Leer todos los productos
CREATE PROCEDURE sp_GetProductos
AS
BEGIN
    SELECT * FROM Productos;
END;

-- Leer un producto por ID
CREATE PROCEDURE sp_GetProductoById
    @id_producto INT
AS
BEGIN
    SELECT * FROM Productos WHERE id_producto = @id_producto;
END;

-- Actualizar un producto
CREATE PROCEDURE sp_UpdateProducto
    @id_producto INT,
    @nombre NVARCHAR(255),
    @descripcion NVARCHAR(MAX),
    @precio DECIMAL(10,2),
    @categoria NVARCHAR(100),
    @disponible BIT
AS
BEGIN
    UPDATE Productos
    SET nombre = @nombre, descripcion = @descripcion, precio = @precio, categoria = @categoria, disponible = @disponible
    WHERE id_producto = @id_producto;
END;

-- Eliminar un producto
CREATE PROCEDURE sp_DeleteProducto
    @id_producto INT
AS
BEGIN
    DELETE FROM Productos WHERE id_producto = @id_producto;
END;