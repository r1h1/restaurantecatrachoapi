-- Procedimientos CRUD para la tabla Pedidos con el nuevo campo numero_pedido

-- Crear un pedido
CREATE PROCEDURE sp_InsertPedido
    @id_usuario INT,
    @numero_pedido VARCHAR(255),
    @estado NVARCHAR(50),
    @fecha_entrega_estimada DATETIME = NULL,
    @monto_total DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Pedidos (id_usuario, numero_pedido, estado, fecha_creacion, fecha_entrega_estimada, monto_total)
    VALUES (@id_usuario, @numero_pedido, @estado, GETDATE(), @fecha_entrega_estimada, @monto_total);
END;

-- Leer todos los pedidos
CREATE PROCEDURE sp_GetPedidos
AS
BEGIN
    SELECT id_pedido, id_usuario, numero_pedido, estado, fecha_creacion, fecha_entrega_estimada, monto_total 
    FROM Pedidos;
END;

-- Leer un pedido por ID
CREATE PROCEDURE sp_GetPedidoById
    @id_pedido INT
AS
BEGIN
    SELECT id_pedido, id_usuario, numero_pedido, estado, fecha_creacion, fecha_entrega_estimada, monto_total 
    FROM Pedidos 
    WHERE id_pedido = @id_pedido;
END;

-- Leer un pedido por número de pedido (nuevo SP)
CREATE PROCEDURE sp_GetPedidoByNumeroPedido
    @numero_pedido VARCHAR(255)
AS
BEGIN
    SELECT id_pedido, id_usuario, numero_pedido, estado, fecha_creacion, fecha_entrega_estimada, monto_total 
    FROM Pedidos 
    WHERE numero_pedido = @numero_pedido;
END;

-- Actualizar un pedido
CREATE PROCEDURE sp_UpdatePedido
    @id_pedido INT,
    @id_usuario INT,
    @numero_pedido VARCHAR(255),
    @estado NVARCHAR(50),
    @fecha_entrega_estimada DATETIME = NULL,
    @monto_total DECIMAL(10,2)
AS
BEGIN
    UPDATE Pedidos
    SET id_usuario = @id_usuario, 
        numero_pedido = @numero_pedido, 
        estado = @estado, 
        fecha_entrega_estimada = @fecha_entrega_estimada, 
        monto_total = @monto_total
    WHERE id_pedido = @id_pedido;
END;

-- Eliminar un pedido
CREATE PROCEDURE sp_DeletePedido
    @id_pedido INT
AS
BEGIN
    DELETE FROM Pedidos WHERE id_pedido = @id_pedido;
END;
