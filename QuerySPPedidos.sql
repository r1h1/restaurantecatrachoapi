-- Procedimientos CRUD para la tabla Pedidos con los campos numero_pedido, direccion e indicaciones

-- Crear un pedido y devolver el ID insertado
CREATE PROCEDURE sp_InsertPedido
    @id_usuario INT,
    @numero_pedido VARCHAR(255),
    @estado NVARCHAR(50),
    @fecha_entrega_estimada DATETIME = NULL,
    @monto_total DECIMAL(10,2),
    @direccion VARCHAR(255),
    @indicaciones VARCHAR(255) = NULL,
    @id_pedido INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Pedidos (id_usuario, numero_pedido, estado, fecha_creacion, fecha_entrega_estimada, monto_total, direccion, indicaciones)
    VALUES (@id_usuario, @numero_pedido, @estado, GETDATE(), @fecha_entrega_estimada, @monto_total, @direccion, @indicaciones);

    -- Obtener el ID insertado y asignarlo al parámetro de salida
    SET @id_pedido = SCOPE_IDENTITY();
END;

-- Leer todos los pedidos
CREATE PROCEDURE sp_GetPedidos
AS
BEGIN
    SELECT id_pedido, id_usuario, numero_pedido, estado, fecha_creacion, fecha_entrega_estimada, monto_total, direccion, indicaciones 
    FROM Pedidos
	ORDER BY id_pedido ASC;
END;

-- Leer un pedido por ID
CREATE PROCEDURE sp_GetPedidoById
    @id_pedido INT
AS
BEGIN
    SELECT id_pedido, id_usuario, numero_pedido, estado, fecha_creacion, fecha_entrega_estimada, monto_total, direccion, indicaciones 
    FROM Pedidos 
    WHERE id_pedido = @id_pedido;
END;

-- Leer un pedido por número de pedido
CREATE PROCEDURE sp_GetPedidoByNumeroPedido
    @numero_pedido VARCHAR(255)
AS
BEGIN
    SELECT id_pedido, id_usuario, numero_pedido, estado, fecha_creacion, fecha_entrega_estimada, monto_total, direccion, indicaciones 
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
    @monto_total DECIMAL(10,2),
    @direccion VARCHAR(255),
    @indicaciones VARCHAR(255) = NULL
AS
BEGIN
    UPDATE Pedidos
    SET id_usuario = @id_usuario, 
        numero_pedido = @numero_pedido, 
        estado = @estado, 
        fecha_entrega_estimada = @fecha_entrega_estimada, 
        monto_total = @monto_total,
        direccion = @direccion,
        indicaciones = @indicaciones
    WHERE id_pedido = @id_pedido;
END;

-- Eliminar un pedido
CREATE PROCEDURE sp_DeletePedido
    @id_pedido INT
AS
BEGIN
    DELETE FROM Pedidos WHERE id_pedido = @id_pedido;
END;