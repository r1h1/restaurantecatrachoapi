-- Procedimientos CRUD para la tabla Pagos

-- Crear un pago
CREATE PROCEDURE sp_InsertPago
    @id_pedido INT,
    @monto DECIMAL(10,2),
    @metodo_pago NVARCHAR(50),
    @estado_pago NVARCHAR(50)
AS
BEGIN
    INSERT INTO Pagos (id_pedido, monto, metodo_pago, fecha_pago, estado_pago)
    VALUES (@id_pedido, @monto, @metodo_pago, GETDATE(), @estado_pago);
END;

-- Leer todos los pagos
CREATE PROCEDURE sp_GetPagos
AS
BEGIN
    SELECT * FROM Pagos;
END;

-- Leer un pago por ID
CREATE PROCEDURE sp_GetPagoById
    @id_pago INT
AS
BEGIN
    SELECT * FROM Pagos WHERE id_pago = @id_pago;
END;

-- Actualizar un pago
CREATE PROCEDURE sp_UpdatePago
    @id_pago INT,
    @id_pedido INT,
    @monto DECIMAL(10,2),
    @metodo_pago NVARCHAR(50),
    @estado_pago NVARCHAR(50)
AS
BEGIN
    UPDATE Pagos
    SET id_pedido = @id_pedido, monto = @monto, metodo_pago = @metodo_pago, estado_pago = @estado_pago
    WHERE id_pago = @id_pago;
END;

-- Eliminar un pago
CREATE PROCEDURE sp_DeletePago
    @id_pago INT
AS
BEGIN
    DELETE FROM Pagos WHERE id_pago = @id_pago;
END;