-- Procedimientos CRUD para la tabla Usuarios

-- Crear un usuario
ALTER PROCEDURE sp_InsertUsuario
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
    @clave NVARCHAR(MAX),
    @rol NVARCHAR(50),
    @telefono NVARCHAR(20) = NULL,
    @direccion NVARCHAR(MAX) = NULL
AS
BEGIN
    UPDATE Usuarios
    SET nombre = @nombre, correo = @correo, clave = @clave, rol = @rol, telefono = @telefono, direccion = @direccion
    WHERE id_usuario = @id_usuario;
END;

-- Eliminar un usuario
CREATE PROCEDURE sp_DeleteUsuario
    @id_usuario INT
AS
BEGIN
    DELETE FROM Usuarios WHERE id_usuario = @id_usuario;
END;


-- Actualizar contrase�a
ALTER PROCEDURE sp_UpdatePassword
    @id_usuario INT,
    @nueva_clave NVARCHAR(MAX)
AS
BEGIN
    -- Verificar si el usuario existe antes de actualizar
    IF EXISTS (SELECT 1 FROM Usuarios WHERE id_usuario = @id_usuario)
    BEGIN
        UPDATE Usuarios
        SET clave = @nueva_clave
        WHERE id_usuario = @id_usuario;
        PRINT 'Clave actualizada correctamente.';
    END
    ELSE
    BEGIN
        PRINT 'Error: El usuario no existe.';
    END
END;
