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

CREATE TRIGGER FinalizarEspectaculo ON compras
AFTER INSERT
AS BEGIN
	IF (noHayMasUbicaciones) BEGIN
		UPDATE espectaculo SET id_estado = finalizado 
			WHERE id_espectaculo = (SELECT id_espectaculo FROM inserted)
	END
END