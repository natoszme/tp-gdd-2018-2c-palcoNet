CREATE TRIGGER QuitarRolInhabilitado ON rol
AFTER UPDATE
AS BEGIN
	IF ((SELECT habilitado FROM inserted) = 0 AND (SELECT habilitado FROM deleted) = 1) BEGIN
		DELETE FROM usuario_rol WHERE id_rol = (SELECT id_rol FROM inserted)
	END
END

CREATE TRIGGER ValidarPasajeDeEstadoDelEspectaculo ON espectaculo
INSTEAD OF UPDATE
AS BEGIN
	IF (estadoAnterior = activo && estadoActual = borrador ||
			estadoAnterior = finalizado) BEGIN
		RAISE ERROR
	END

	UPDATE espectaculo SET id_estado = (SELECT id_estado FROM INSERTED)
END

CREATE TRIGGER CuilAutomatico ON cliente
AFTER INSERT
AS BEGIN
	DECLARE @id_usuario BIGINT,
			@numero_documento NUMERIC(18, 0),
			@cuil BIGINT

	DECLARE clientes_insertados CURSOR FOR
	SELECT id_usuario, numero_documento, cuil FROM inserted

	OPEN clientes_insertados

	FETCH NEXT FROM clientes_insertados
	INTO @id_usuario, @numero_documento, @cuil

	WHILE @@FETCH_STATUS = 0 BEGIN
		IF @cuil IS NULL BEGIN
			UPDATE cliente SET cuil = @id_usuario + @numero_documento WHERE id_usuario = @id_usuario
		END

		FETCH NEXT FROM clientes_insertados
		INTO @id_usuario, @numero_documento, @cuil
	END
	
	CLOSE clientes_insertados
	DEALLOCATE clientes_insertados
END

CREATE TRIGGER FinalizarEspectaculo ON compras
AFTER INSERT
AS BEGIN
	IF (noHayMasUbicaciones) BEGIN
		UPDATE espectaculo SET id_estado = finalizado 
			WHERE id_espectaculo = (SELECT id_espectaculo FROM inserted)
	END
END