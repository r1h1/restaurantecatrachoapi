-- Procedimientos CRUD para la tabla Pagos con el nuevo campo numero_pedido

-- Crear un pago
CREATE PROCEDURE sp_InsertPago
    @id_pedido INT,
    @numero_pedido VARCHAR(255),
    @monto DECIMAL(10,2),
    @metodo_pago NVARCHAR(50),
    @estado_pago NVARCHAR(50)
AS
BEGIN
    INSERT INTO Pagos (id_pedido, numero_pedido, monto, metodo_pago, fecha_pago, estado_pago)
    VALUES (@id_pedido, @numero_pedido, @monto, @metodo_pago, GETDATE(), @estado_pago);
END;

-- Leer todos los pagos
CREATE PROCEDURE sp_GetPagos
AS
BEGIN
    SELECT id_pago, id_pedido, numero_pedido, monto, metodo_pago, fecha_pago, estado_pago FROM Pagos;
END;

-- Leer un pago por ID
CREATE PROCEDURE sp_GetPagoById
    @id_pago INT
AS
BEGIN
    SELECT id_pago, id_pedido, numero_pedido, monto, metodo_pago, fecha_pago, estado_pago 
    FROM Pagos 
    WHERE id_pago = @id_pago;
END;

-- Leer pagos por número de pedido (nuevo SP opcional)
CREATE PROCEDURE sp_GetPagoByNumeroPedido
    @numero_pedido VARCHAR(255)
AS
BEGIN
    SELECT id_pago, id_pedido, numero_pedido, monto, metodo_pago, fecha_pago, estado_pago 
    FROM Pagos 
    WHERE numero_pedido = @numero_pedido;
END;

-- Actualizar un pago
CREATE PROCEDURE sp_UpdatePago
    @id_pago INT,
    @id_pedido INT,
    @numero_pedido VARCHAR(255),
    @monto DECIMAL(10,2),
    @metodo_pago NVARCHAR(50),
    @estado_pago NVARCHAR(50)
AS
BEGIN
    UPDATE Pagos
    SET id_pedido = @id_pedido, 
        numero_pedido = @numero_pedido, 
        monto = @monto, 
        metodo_pago = @metodo_pago, 
        estado_pago = @estado_pago
    WHERE id_pago = @id_pago;
END;

-- Eliminar un pago
CREATE PROCEDURE sp_DeletePago
    @id_pago INT
AS
BEGIN
    DELETE FROM Pagos WHERE id_pago = @id_pago;
END;