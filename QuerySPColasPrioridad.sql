
-- Procedimientos CRUD para la tabla Colas_Prioridad

-- Crear una cola de prioridad
CREATE PROCEDURE sp_InsertColaPrioridad
    @id_pedido INT,
    @prioridad NVARCHAR(50)
AS
BEGIN
    INSERT INTO Colas_Prioridad (id_pedido, hora_solicitud, prioridad)
    VALUES (@id_pedido, GETDATE(), @prioridad);
END;

-- Leer todas las colas de prioridad
CREATE PROCEDURE sp_GetColasPrioridad
AS
BEGIN
    SELECT * FROM Colas_Prioridad;
END;

-- Leer una cola de prioridad por ID
CREATE PROCEDURE sp_GetColaPrioridadById
    @id_cola INT
AS
BEGIN
    SELECT * FROM Colas_Prioridad WHERE id_cola = @id_cola;
END;

-- Actualizar una cola de prioridad
CREATE PROCEDURE sp_UpdateColaPrioridad
    @id_cola INT,
    @id_pedido INT,
    @prioridad NVARCHAR(50)
AS
BEGIN
    UPDATE Colas_Prioridad
    SET id_pedido = @id_pedido, prioridad = @prioridad
    WHERE id_cola = @id_cola;
END;

-- Eliminar una cola de prioridad
CREATE PROCEDURE sp_DeleteColaPrioridad
    @id_cola INT
AS
BEGIN
    DELETE FROM Colas_Prioridad WHERE id_cola = @id_cola;
END;