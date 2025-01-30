-- Procedimientos CRUD para la tabla Detalles_Pedidos

-- Crear un detalle de pedido
CREATE PROCEDURE sp_InsertDetallePedido
    @id_pedido INT,
    @id_producto INT,
    @cantidad INT,
    @precio_unitario DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Detalles_Pedidos (id_pedido, id_producto, cantidad, precio_unitario)
    VALUES (@id_pedido, @id_producto, @cantidad, @precio_unitario);
END;

-- Leer todos los detalles de pedidos
CREATE PROCEDURE sp_GetDetallesPedidos
AS
BEGIN
    SELECT * FROM Detalles_Pedidos;
END;

-- Leer un detalle de pedido por ID
CREATE PROCEDURE sp_GetDetallePedidoById
    @id_detalle INT
AS
BEGIN
    SELECT * FROM Detalles_Pedidos WHERE id_detalle = @id_detalle;
END;

-- Actualizar un detalle de pedido
CREATE PROCEDURE sp_UpdateDetallePedido
    @id_detalle INT,
    @id_pedido INT,
    @id_producto INT,
    @cantidad INT,
    @precio_unitario DECIMAL(10,2)
AS
BEGIN
    UPDATE Detalles_Pedidos
    SET id_pedido = @id_pedido, id_producto = @id_producto, cantidad = @cantidad, precio_unitario = @precio_unitario
    WHERE id_detalle = @id_detalle;
END;

-- Eliminar un detalle de pedido
CREATE PROCEDURE sp_DeleteDetallePedido
    @id_detalle INT
AS
BEGIN
    DELETE FROM Detalles_Pedidos WHERE id_detalle = @id_detalle;
END;