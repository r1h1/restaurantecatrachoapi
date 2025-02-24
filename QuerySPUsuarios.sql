-- Procedimientos CRUD para la tabla Usuarios

-- Crear un usuario
CREATE PROCEDURE sp_InsertUsuario
    @nombre NVARCHAR(255),
    @correo NVARCHAR(255),
    @contraseña NVARCHAR(MAX),
    @rol NVARCHAR(50),
    @telefono NVARCHAR(20) = NULL,
    @direccion NVARCHAR(MAX) = NULL
AS
BEGIN
    INSERT INTO Usuarios (nombre, correo, contraseña, rol, telefono, direccion)
    VALUES (@nombre, @correo, @contraseña, @rol, @telefono, @direccion);
END;
GO

-- Leer todos los usuarios
CREATE PROCEDURE sp_GetUsuarios
AS
BEGIN
    SELECT * FROM Usuarios;
END;

-- Leer un usuario por ID
CREATE PROCEDURE sp_GetUsuarioById
    @id_usuario INT
AS
BEGIN
    SELECT * FROM Usuarios WHERE id_usuario = @id_usuario;
END;

-- Actualizar un usuario
ALTER PROCEDURE sp_UpdateUsuario
    @id_usuario INT,
    @nombre NVARCHAR(255),
    @correo NVARCHAR(255),
    @rol NVARCHAR(50),
    @telefono NVARCHAR(20) = NULL,
    @direccion NVARCHAR(MAX) = NULL
AS
BEGIN
    UPDATE Usuarios
    SET nombre = @nombre, correo = @correo, rol = @rol, telefono = @telefono, direccion = @direccion
    WHERE id_usuario = @id_usuario;
END;


-- Actualizar contraseña solo si coinciden todos los datos
ALTER PROCEDURE sp_UpdatePassword
    @correo NVARCHAR(100),
    @telefono NVARCHAR(50),
    @direccion NVARCHAR(200),
    @nueva_clave NVARCHAR(MAX)
AS
BEGIN
    DECLARE @id_usuario INT;

    -- Buscar al usuario con los datos coincidentes
    SELECT @id_usuario = id_usuario
    FROM Usuarios 
    WHERE correo = @correo 
      AND telefono = @telefono 
      AND direccion = @direccion;

    -- Verificar si se encontró un usuario con los datos proporcionados
    IF @id_usuario IS NOT NULL
    BEGIN
        -- Si se encontró, actualizar la clave
        UPDATE Usuarios
        SET clave = @nueva_clave
        WHERE id_usuario = @id_usuario;
        
        PRINT 'Clave actualizada correctamente.';
    END
    ELSE
    BEGIN
        -- Si no se encontró ningún usuario, mostrar mensaje de error
        PRINT 'Error: Datos no coinciden o el usuario no existe.';
    END
END;


-- Leer un usuario por correo y clave
CREATE PROCEDURE sp_GetUsuarioByEmailAndPass
    @correo NVARCHAR(255)
AS
BEGIN
    SELECT * FROM Usuarios WHERE correo = @correo;
END;
