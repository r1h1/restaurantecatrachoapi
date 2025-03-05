-- Procedimientos CRUD para la tabla Detalles_Pedidos con el nuevo campo numero_pedido

-- Crear un detalle de pedido
CREATE PROCEDURE sp_InsertDetallePedido
    @id_pedido INT,
    @id_producto INT,
    @numero_pedido VARCHAR(255),
    @cantidad INT,
    @precio_unitario DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Detalles_Pedidos (id_pedido, id_producto, numero_pedido, cantidad, precio_unitario)
    VALUES (@id_pedido, @id_producto, @numero_pedido, @cantidad, @precio_unitario);
END;

-- Leer todos los detalles de pedidos
CREATE PROCEDURE sp_GetDetallesPedidos
AS
BEGIN
    SELECT id_detalle, id_pedido, id_producto, numero_pedido, cantidad, precio_unitario 
    FROM Detalles_Pedidos;
END;

-- Leer un detalle de pedido por ID
CREATE PROCEDURE sp_GetDetallePedidoById
    @id_pedido INT
AS
BEGIN
    SELECT dp.id_detalle, 
           dp.id_pedido, 
           dp.id_producto, 
           p.nombre AS nombre_producto,  -- Nuevo campo agregado
           dp.numero_pedido, 
           dp.cantidad, 
           dp.precio_unitario
    FROM Detalles_Pedidos dp
    INNER JOIN Productos p ON dp.id_producto = p.id_producto
    WHERE dp.id_pedido = @id_pedido;
END;

-- Leer detalles de pedido por número de pedido (nuevo SP)
CREATE PROCEDURE sp_GetDetallesPedidoByNumeroPedido
    @numero_pedido VARCHAR(255)
AS
BEGIN
    SELECT id_detalle, id_pedido, id_producto, numero_pedido, cantidad, precio_unitario 
    FROM Detalles_Pedidos 
    WHERE numero_pedido = @numero_pedido;
END;

-- Actualizar un detalle de pedido
CREATE PROCEDURE sp_UpdateDetallePedido
    @id_detalle INT,
    @id_pedido INT,
    @id_producto INT,
    @numero_pedido VARCHAR(255),
    @cantidad INT,
    @precio_unitario DECIMAL(10,2)
AS
BEGIN
    UPDATE Detalles_Pedidos
    SET id_pedido = @id_pedido, 
        id_producto = @id_producto, 
        numero_pedido = @numero_pedido, 
        cantidad = @cantidad, 
        precio_unitario = @precio_unitario
    WHERE id_detalle = @id_detalle;
END;

-- Eliminar un detalle de pedido
CREATE PROCEDURE sp_DeleteDetallePedido
    @id_detalle INT
AS
BEGIN
    DELETE FROM Detalles_Pedidos WHERE id_detalle = @id_detalle;
END;
