-- Procedimientos CRUD para la tabla Pedidos

-- Crear un pedido
CREATE PROCEDURE sp_InsertPedido
    @id_usuario INT,
    @estado NVARCHAR(50),
    @fecha_entrega_estimada DATETIME = NULL,
    @monto_total DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Pedidos (id_usuario, estado, fecha_creacion, fecha_entrega_estimada, monto_total)
    VALUES (@id_usuario, @estado, GETDATE(), @fecha_entrega_estimada, @monto_total);
END;

-- Leer todos los pedidos
CREATE PROCEDURE sp_GetPedidos
AS
BEGIN
    SELECT * FROM Pedidos;
END;

-- Leer un pedido por ID
CREATE PROCEDURE sp_GetPedidoById
    @id_pedido INT
AS
BEGIN
    SELECT * FROM Pedidos WHERE id_pedido = @id_pedido;
END;

-- Actualizar un pedido
CREATE PROCEDURE sp_UpdatePedido
    @id_pedido INT,
    @id_usuario INT,
    @estado NVARCHAR(50),
    @fecha_entrega_estimada DATETIME = NULL,
    @monto_total DECIMAL(10,2)
AS
BEGIN
    UPDATE Pedidos
    SET id_usuario = @id_usuario, estado = @estado, fecha_entrega_estimada = @fecha_entrega_estimada, monto_total = @monto_total
    WHERE id_pedido = @id_pedido;
END;

-- Eliminar un pedido
CREATE PROCEDURE sp_DeletePedido
    @id_pedido INT
AS
BEGIN
    DELETE FROM Pedidos WHERE id_pedido = @id_pedido;
END;
