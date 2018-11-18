USE GDDPrueba
GO

CREATE SCHEMA RAGNAR
GO

--/ StoredProcedure para cargar la fecha del archivo config y funcion para obtener esa fecha /--

CREATE PROCEDURE RAGNAR.SP_CargarEnLaBaseFechaDelConfig
AS
BEGIN
	IF(EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Fecha_config'))
		DROP TABLE RAGNAR.Fecha_config
	CREATE TABLE RAGNAR.Fecha_config (fecha smalldatetime)
	BULK INSERT RAGNAR.Fecha_config FROM 'C:\Gestion De Datos\TP2C2018\Aplicacion Desktop\PalcoNet\config.txt' WITH (FIRSTROW = 1,FIELDTERMINATOR = ',',ROWTERMINATOR = '') --Cambiar la ruta
END
GO
--/ RECORDAR QUE LUEGO DE UN CAMBIO EN EL CONFIG SE DEBE SI O SI LLAMAR AL STORED PROCEDURE PARA ACTUALIZAR LA FECHA EN LA BASE

--/ Funcion para usar la fecha del config /--

CREATE FUNCTION RAGNAR.F_ObtenerFechaDelConfig ()
RETURNS smalldatetime
AS
BEGIN
	DECLARE @Fecha smalldatetime
	SET @Fecha = (SELECT * FROM RAGNAR.Fecha_config)
	RETURN @Fecha
END
GO

--/ Creacion de tablas /--

CREATE TABLE RAGNAR.Funcionalidad				(	id_funcionalidad int identity PRIMARY KEY,
													descripcion varchar(255) NOT NULL)

CREATE TABLE RAGNAR.Rol							(	id_rol int identity PRIMARY KEY,
													nombre varchar(255) NOT NULL,
													habilitado bit NOT NULL DEFAULT 1)

CREATE TABLE RAGNAR.Funcionalidad_rol			(	id_rol int FOREIGN KEY references RAGNAR.Rol(id_rol),
													id_funcionalidad int FOREIGN KEY references RAGNAR.Funcionalidad(id_funcionalidad),
													PRIMARY KEY(id_rol,id_funcionalidad))

CREATE TABLE RAGNAR.Usuario						(	id_usuario bigint identity PRIMARY KEY,
													usuario	varchar(50) NOT NULL unique,
													clave	varchar(32) NOT NULL,
													habilitado bit NOT NULL DEFAULT 1,
													es_nuevo tinyint NOT NULL DEFAULT 0)

CREATE TABLE RAGNAR.Usuario_rol					(	id_usuario bigint FOREIGN KEY references RAGNAR.Usuario(id_usuario),
													id_rol int FOREIGN KEY references RAGNAR.Rol(id_rol),
													PRIMARY KEY(id_rol,id_usuario))
													

CREATE TABLE RAGNAR.Login_fallido				(	id_usuario bigint FOREIGN KEY references RAGNAR.Usuario(id_usuario),
													nro_intento tinyint NOT NULL,
													PRIMARY KEY(id_usuario))

CREATE TABLE RAGNAR.Cliente						(	id_usuario bigint FOREIGN KEY references RAGNAR.Usuario(id_usuario) PRIMARY KEY,
													nombre nvarchar(255) NOT NULL,
													apellido nvarchar(255) NOT NULL,
													tipo_documento varchar(3) NOT NULL DEFAULT 'DNI',
													numero_documento numeric(18,0) NOT NULL,
													cuil nvarchar(255),
													mail nvarchar(255) NOT NULL,
													telefono varchar(20),
													calle nvarchar(255) NOT NULL,
													portal numeric(18,0) NOT NULL,
													piso numeric(18,0) NOT NULL,
													departamento nvarchar(255) NOT NULL,
													localidad nvarchar(255),
													codigo_postal nvarchar(255) NOT NULL,
													fecha_nacimiento datetime NOT NULL,
													fecha_creacion datetime, /*Refiere a la fecha de creacion del cliente en el sistema, se puede definir un default a futuro*/
													tarjeta_credito varchar(10),
													CONSTRAINT [Documento] UNIQUE NONCLUSTERED
														 ([tipo_documento], [numero_documento]))

CREATE TABLE RAGNAR.Empresa						(	id_usuario bigint FOREIGN KEY references RAGNAR.Usuario(id_usuario) PRIMARY KEY,
													razon_social nvarchar(255) NOT NULL UNIQUE,
													cuit nvarchar(255) NOT NULL UNIQUE,
													mail nvarchar(50) NOT NULL,
													telefono varchar(20),
													ciudad nvarchar(255),
													calle nvarchar(50) NOT NULL,
													portal numeric(18,0) NOT NULL,
													piso numeric(18,0) NOT NULL,
													departamento nvarchar(50) NOT NULL,
													localidad nvarchar(255),
													codigo_postal nvarchar(50) NOT NULL,
													fecha_creacion datetime NOT NULL)

CREATE TABLE RAGNAR.Puntos_cliente				(	id_puntaje bigint identity PRIMARY KEY,
													id_cliente bigint FOREIGN KEY references RAGNAR.Cliente(id_usuario),
													puntos int,
													vencimiento date NOT NULL)

CREATE TABLE RAGNAR.Grado_publicacion			(	id_grado int identity PRIMARY KEY,
													descripcion nvarchar(255) NOT NULL,
													comision numeric(4,3) NOT NULL,
													habilitado bit NOT NULL DEFAULT 1)

CREATE TABLE RAGNAR.Rubro						(	id_rubro int identity PRIMARY KEY,
													descripcion nvarchar(255) NOT NULL,
													habilitado bit NOT NULL DEFAULT 1)

CREATE TABLE RAGNAR.Estado_publicacion			(	id_estado int identity PRIMARY KEY,
													descripcion nvarchar(255) NOT NULL)

CREATE TABLE RAGNAR.Publicacion					(	id_publicacion bigint identity PRIMARY KEY,
													codigo_publicacion numeric(18,0), /*Resolver como vamos a implementar estos 2 campos*/
													descripcion nvarchar(255) NOT NULL,
													stock int, /*No existe en la maestra o es producto de la suma de registros?*/
													fecha_publicacion datetime, /*Agregar Default?*/
													id_rubro int FOREIGN KEY references RAGNAR.Rubro(id_rubro) NOT NULL, 
													direccion varchar(255),
													id_grado int FOREIGN KEY references RAGNAR.Grado_publicacion(id_grado),
													id_empresa bigint FOREIGN KEY references RAGNAR.Empresa(id_usuario),
													fecha_vencimiento datetime NOT NULL,
													fecha_espectaculo datetime NOT NULL,
													id_estado int FOREIGN KEY references RAGNAR.Estado_publicacion(id_estado) NOT NULL)

CREATE TABLE RAGNAR.Tipo_ubicacion				(	id_tipo_ubicacion int identity PRIMARY KEY,
													codigo numeric(18,0), /*De la maestra, resolver con la linea de arriba*/
													descripcion nvarchar(255) NOT NULL)

CREATE TABLE RAGNAR.Compra						(	id_compra bigint identity PRIMARY KEY,
													id_cliente bigint FOREIGN KEY references RAGNAR.Cliente(id_usuario) NOT NULL,
													fecha datetime NOT NULL, --/Agregar default
													tarjeta_utilizada varchar(10))

CREATE TABLE RAGNAR.Ubicacion_publicacion		(	id_ubicacion bigint identity PRIMARY KEY,
													id_publicacion bigint FOREIGN KEY references RAGNAR.Publicacion(id_publicacion),
													fila varchar(3),
													asiento numeric(18,0),
													sin_numerar bit NOT NULL, /*No seteo el default porque pueder ser 1 o 0*/
													precio numeric(18,0) NOT NULL,
													id_tipo	int FOREIGN KEY references RAGNAR.Tipo_ubicacion(id_tipo_ubicacion) NOT NULL,
													id_compra bigint FOREIGN KEY references RAGNAR.Compra(id_compra),
													compra_cantidad numeric(18,0))

CREATE TABLE RAGNAR.Factura						(	id_factura bigint identity PRIMARY KEY,
													numero numeric(18,0) NOT NULL,
													fecha datetime NOT NULL,
													total numeric(18,2) NOT NULL,
													forma_pago nvarchar(255))

CREATE TABLE RAGNAR.Item_factura				(	id_item bigint identity PRIMARY KEY,
													id_ubicacion bigint FOREIGN KEY references RAGNAR.Ubicacion_publicacion(id_ubicacion),
													id_factura bigint FOREIGN KEY references RAGNAR.Factura(id_factura),
													descripcion nvarchar(60) NOT NULL,
													monto numeric(18,2) NOT NULL,
													cantidad numeric(18,0) NOT NULL DEFAULT 1) --/Saque la PK, se puede llegar a crear una usando id_ubicacion de ser necesario

CREATE TABLE RAGNAR.Premio						(	id_premio int identity PRIMARY KEY,
													puntos_necesarios int NOT NULL,
													descripcion varchar(255) NOT NULL)

CREATE TABLE RAGNAR.Canje_premio				(	id_premio int FOREIGN KEY references RAGNAR.Premio(id_premio),
													id_cliente bigint FOREIGN KEY references RAGNAR.Cliente(id_usuario))

--/ Fin de creacion de tablas /--

GO

--/ Funcion para encriptar la contraseña /--

CREATE FUNCTION RAGNAR.F_HasheoDeClave (@Clave varchar(32))
RETURNS varchar(32)
AS
BEGIN
	RETURN CONVERT(varchar(32),HASHBYTES('SHA2_256',@Clave))
END
GO

--/ Trigger para encriptar la contraseña ingresada /--

CREATE TRIGGER RAGNAR.HasheoDeClaveDeUsuario ON RAGNAR.Usuario INSTEAD OF INSERT
AS
BEGIN
	DECLARE @Usuario varchar(50), @Clave varchar(32), @Habilitado bit
	DECLARE CUsuarios CURSOR FOR (SELECT usuario, clave, habilitado FROM INSERTED)
	OPEN CUsuarios
	FETCH NEXT FROM CUsuarios INTO @Usuario, @Clave, @Habilitado
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO RAGNAR.Usuario(usuario, clave, habilitado) VALUES (@Usuario, RAGNAR.F_HasheoDeClave(@Clave), @Habilitado)
		FETCH NEXT FROM CUsuarios INTO @Usuario, @Clave, @Habilitado
	END
	CLOSE CUsuarios
	DEALLOCATE CUsuarios
END
GO

--/ Migracion de la tabla maestra /--

--/ Inserts de usuarios /--

INSERT INTO RAGNAR.Usuario(usuario,clave,habilitado) VALUES ('admin','admin',1) /*Usuario administrador, cambiar la pass con el metodo de encriptacion*/

INSERT INTO RAGNAR.Usuario(usuario,clave,habilitado) 
	SELECT DISTINCT Cli_Dni, Cli_dni, 1
	FROM gd_esquema.Maestra
	WHERE Cli_Dni IS NOT NULL
	
INSERT INTO RAGNAR.Usuario(usuario,clave,habilitado)
	SELECT DISTINCT Espec_Empresa_Cuit, Espec_Empresa_Cuit, 1
	FROM gd_esquema.Maestra
	WHERE Espec_Empresa_Cuit IS NOT NULL
	
INSERT INTO RAGNAR.Cliente(id_usuario,tipo_documento, numero_documento, apellido, nombre, fecha_nacimiento, mail, calle, portal, piso, departamento, codigo_postal)
	SELECT DISTINCT U.id_usuario,'DNI', M.Cli_dni, M.Cli_Apeliido, M.Cli_Nombre, M.Cli_Fecha_Nac, M.Cli_Mail, M.Cli_Dom_Calle, M.Cli_Nro_Calle, M.Cli_Piso, M.Cli_Depto, M.Cli_Cod_Postal
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Usuario as U ON (CONVERT(varchar,M.Cli_Dni) = U.usuario)
	WHERE Cli_Dni IS NOT NULL
	
INSERT INTO RAGNAR.Empresa(id_usuario, razon_social, cuit, fecha_creacion, mail, calle, portal, piso, departamento, codigo_postal)
	SELECT DISTINCT U.id_usuario, M.Espec_Empresa_Razon_Social, M.Espec_Empresa_Cuit, M.Espec_Empresa_Fecha_Creacion, M.Espec_Empresa_Mail, M.Espec_Empresa_Dom_Calle, M.Espec_Empresa_Nro_Calle, M.Espec_Empresa_Piso, M.Espec_Empresa_Depto, M.Espec_Empresa_Cod_Postal
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Usuario as U ON (M.Espec_Empresa_Cuit = U.usuario)
	WHERE Espec_Empresa_Cuit IS NOT NULL

--/ Inserts de Espectaculos /--

INSERT INTO RAGNAR.Rubro(descripcion)
	SELECT DISTINCT Espectaculo_Rubro_Descripcion
	FROM gd_esquema.Maestra
	WHERE Espectaculo_Rubro_Descripcion IS NOT NULL

INSERT INTO RAGNAR.Estado_publicacion(descripcion)
	SELECT DISTINCT Espectaculo_Estado
	FROM gd_esquema.Maestra
	WHERE Espectaculo_Estado IS NOT NULL
	
INSERT INTO RAGNAR.Publicacion(codigo_publicacion, descripcion, fecha_vencimiento, id_rubro , fecha_espectaculo, id_estado)
	SELECT DISTINCT M.Espectaculo_Cod, M.Espectaculo_Descripcion, M.Espectaculo_Fecha_Venc, R.id_rubro, M.Espectaculo_Fecha, E.id_estado
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Rubro as R ON (M.Espectaculo_Rubro_Descripcion = R.descripcion) JOIN RAGNAR.Estado_publicacion as E ON (M.Espectaculo_Estado = E.descripcion)
	WHERE M.Espectaculo_Cod IS NOT NULL

--/ Inserts de Ubicaciones y Compras /--
	
INSERT INTO RAGNAR.Tipo_ubicacion(codigo, descripcion)
	SELECT DISTINCT Ubicacion_Tipo_Codigo, Ubicacion_Tipo_Descripcion
	FROM gd_esquema.Maestra
	WHERE Ubicacion_Tipo_Codigo IS NOT NULL
	
INSERT INTO RAGNAR.Compra(id_cliente, fecha)
	SELECT DISTINCT C.id_usuario, M.Compra_Fecha
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Cliente as C ON (M.Cli_Dni = C.numero_documento)
	WHERE M.Compra_Fecha IS NOT NULL
	
INSERT INTO RAGNAR.Ubicacion_publicacion(id_publicacion, id_tipo, fila, asiento, sin_numerar, precio, id_compra, compra_cantidad)
	SELECT DISTINCT P.id_publicacion, T.id_tipo_ubicacion, M.Ubicacion_Fila, M.Ubicacion_Asiento, M.Ubicacion_Sin_Numerar, M.Ubicacion_Precio,COM.id_compra, M.Compra_Cantidad
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Publicacion as P ON (M.Espectaculo_Cod = P.codigo_publicacion) JOIN RAGNAR.Tipo_ubicacion as T ON (M.Ubicacion_Tipo_Codigo = T.codigo AND M.Ubicacion_Tipo_Descripcion = T.descripcion) JOIN  RAGNAR.Cliente as C ON (M.Cli_Dni = C.numero_documento) JOIN RAGNAR.Compra as COM ON (COM.id_cliente = C.id_usuario AND COM.fecha = M.Compra_Fecha)
	WHERE M.Ubicacion_Precio IS NOT NULL AND M.Compra_Cantidad IS NOT NULL

INSERT INTO RAGNAR.Ubicacion_publicacion(id_publicacion, id_tipo, fila, asiento, sin_numerar, precio)
	SELECT DISTINCT P.id_publicacion, T.id_tipo_ubicacion, M.Ubicacion_Fila, M.Ubicacion_Asiento, M.Ubicacion_Sin_Numerar, M.Ubicacion_Precio
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Publicacion as P ON (M.Espectaculo_Cod = P.codigo_publicacion) JOIN RAGNAR.Tipo_ubicacion as T ON (M.Ubicacion_Tipo_Codigo = T.codigo AND M.Ubicacion_Tipo_Descripcion = T.descripcion)
	WHERE M.Ubicacion_Precio IS NOT NULL
	GROUP BY P.id_publicacion, T.id_tipo_ubicacion, M.Ubicacion_Fila, M.Ubicacion_Asiento, M.Ubicacion_Sin_Numerar, M.Ubicacion_Precio
	HAVING SUM(ISNULL(M.Compra_Cantidad, 0 )) = 0

--/ Inserts de Facturas /--

INSERT INTO RAGNAR.Factura(numero, fecha, total, forma_pago)
	SELECT DISTINCT Factura_Nro, Factura_Fecha, Factura_Total, Forma_Pago_Desc
	FROM gd_esquema.Maestra
	WHERE Factura_Nro IS NOT NULL

INSERT INTO RAGNAR.Item_factura(id_factura, id_ubicacion, cantidad, descripcion, monto)
	SELECT F.id_factura, U.id_ubicacion, M.Item_Factura_Cantidad, M.Item_Factura_Descripcion, M.Item_Factura_Monto
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Factura as F ON (M.Factura_Nro = F.numero) JOIN RAGNAR.Publicacion as P ON (M.Espectaculo_Cod = P.codigo_publicacion) JOIN RAGNAR.Ubicacion_publicacion as U ON (P.id_publicacion = U.id_publicacion AND M.Ubicacion_Asiento = U.asiento AND M.Ubicacion_Fila = U.fila)
	WHERE M.Item_Factura_Monto IS NOT NULL

--/ Inserts Manuales para el funcionamiento del sistema /--

--/ Inserts de Estados de Publicacion /--

INSERT INTO RAGNAR.Estado_publicacion(descripcion) VALUES ('Borrador')
INSERT INTO RAGNAR.Estado_publicacion(descripcion) VALUES ('Finalizada')

--/ Inserts de Roles /--

INSERT INTO RAGNAR.Rol(nombre, habilitado) VALUES ('Empresa',1)
INSERT INTO RAGNAR.Rol(nombre, habilitado) VALUES ('Administrativo',1)
INSERT INTO RAGNAR.Rol(nombre, habilitado) VALUES ('Cliente',1)

--/ Inserts de Funcionalidades /--

INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('ABM de Rol')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('Registro de Usuario')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('ABM de Cliente')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('ABM de Empresa de Espectaculos')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('ABM de Rubro')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('ABM de Grado de Publicacion')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('Generar Publicacion')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('Editar Publicacion')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('Comprar')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('Historial de Cliente')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('Canje y Administracion de Puntos')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('Generar rendicion de comisiones')
INSERT INTO RAGNAR.Funcionalidad(descripcion) VALUES ('Listado estadistico')

--/ Inserts de Funcionalidad_rol /--

INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'ABM de Rol'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Administrativo'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Registro de Usuario'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Cliente'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Registro de Usuario'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Empresa'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'ABM de Cliente'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Administrativo'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'ABM de Empresa de Espectaculos'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Administrativo'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'ABM de Rubro'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Administrativo'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'ABM de Grado de Publicacion'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Empresa'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Generar Publicacion'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Empresa'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Editar Publicacion'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Empresa'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Comprar'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Cliente'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Historial de Cliente'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Cliente'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Canje y Administracion de Puntos'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Cliente'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Generar rendicion de comisiones'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Administrativo'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Listado estadistico'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Administrativo'))

--/ Fin de Inserts /--

GO

--/ STORED PROCEDURES /--

--/ StoredProcedure para el login de usuarios/--

CREATE PROCEDURE RAGNAR.SP_LoginDeUsuario (@Usuario varchar(50), @Clave varchar(32))
AS
BEGIN
	DECLARE @ClaveEncriptada varchar(32), @ID bigint
	SET @ClaveEncriptada = RAGNAR.F_HasheoDeClave(@Clave)
	SET @ID = (SELECT id_usuario FROM RAGNAR.Usuario WHERE usuario = @Usuario)
	IF (@ID IS NULL)
	BEGIN
		PRINT('El usuario ingresado no existe') --Se puede cambiar a un RAISEERROR?
		RETURN 0
	END
	ELSE
	BEGIN
		IF (NOT EXISTS (SELECT * FROM RAGNAR.Usuario WHERE id_usuario = @ID AND clave = @ClaveEncriptada)) --La contraseña ingresada no es correcta
		BEGIN
			PRINT('La contraseña ingresada no es correcta')
			IF(NOT EXISTS (SELECT * FROM RAGNAR.Login_fallido WHERE id_usuario = @ID))
				INSERT INTO RAGNAR.Login_fallido(id_usuario, nro_intento) VALUES (@ID, 1)
			ELSE
				UPDATE RAGNAR.Login_fallido SET nro_intento = nro_intento + 1 WHERE id_usuario = @ID
			RETURN 2
		END
		ELSE
			RETURN 1 --El login fue exitoso
	END
END
GO

--/ TRIGGERS /--

--/ Trigger de la tabla Login_fallido /--

CREATE TRIGGER RAGNAR.LoginFallido ON RAGNAR.Login_fallido AFTER UPDATE
AS
BEGIN
	DECLARE @ID bigint, @Intento tinyint
	SELECT @ID = id_usuario, @Intento = nro_intento FROM INSERTED
	IF(@Intento >= 3)
	BEGIN
		UPDATE RAGNAR.Usuario SET habilitado = 0 WHERE id_usuario = @ID
		DELETE FROM RAGNAR.Login_fallido WHERE id_usuario = @ID
	END
END
GO

--/ Trigger para quitar a todos los usuarios los roles que hayan sido inhabilitados /--

CREATE TRIGGER RAGNAR.QuitarRolInhabilitadoAUsuario ON RAGNAR.Rol AFTER UPDATE
AS
BEGIN
	DECLARE @Rol int
	DECLARE CRoles CURSOR FOR (SELECT id_rol FROM INSERTED WHERE habilitado = 0)
	OPEN CRoles
	FETCH NEXT FROM CRoles INTO @Rol
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DELETE FROM RAGNAR.Usuario_rol WHERE id_rol = @Rol
		FETCH NEXT FROM CRoles INTO @Rol
	END
END
GO

/*CREATE de Premios a canjear*/

/*ALTER TABLE UNIQUE PUBLICACION*/

/* Store Procedures para los reportes

/* TODO: resolver el tema del trimestre y año ingresado por UI */

-- empresas con > localidades no vendidas (que es visibilidad?)
-- la fecha de quien es la que se ordena?
SELECT TOP 5 DISTINCT em.razon_social, COUNT(up.id_ubicacion) - SUM(uco.cantidad)
	FROM empresa em
		JOIN publicacion p ON em.id_usuario = p.id_empresa
		JOIN ubicacion_publicacion up ON p.id_publicacion = up.id_publicacion
		JOIN espectaculo e ON p.id_publicacion = e.id_publicacion
		JOIN compra co ON e.id_espectaculo = co.id_espectaculo
		JOIN ubicacion_compra uco ON co.id_compra = uco.id_compra
WHERE co.fecha FUERA DEL TRIMESTRE Y AÑO INGRESADO AND co.id_grado = :id_grado
GROUP BY em.razon_social
ORDER BY COUNT(up.id_ubicacion) - SUM(uco.cantidad), fecha, id_grado

-- clientes con puntos no vencidos
SELECT TOP 5 c.id_usuario, c.nombre, c.apellido, SUM(puntos) puntosNoVencidos
	FROM cliente c
		LEFT JOIN puntos_cliente pc ON c.id_usuario = pc.id_cliente
	WHERE vencimiento FUERA DEL TRIMESTRE Y AÑO INGRESADO
GROUP BY c.id_usuario, c.nombre, c.apellido
ORDER BY SUM(puntos) DESC

-- clientes con mayor cantidad de compras
SELECT TOP 5 c.id_usuario, COUNT(id_compra), p.id_empresa
	FROM cliente c
		JOIN compra co ON c.id_usuario = co.id_cliente
		JOIN espectaculo e ON co.id_espectaculo = e.id_espectaculo
		JOIN publicacion p ON e.id_publicacion = p.id_publicacion
WHERE co.fecha FUERA DEL TRIMESTRE Y AÑO INGRESADO
GROUP BY p.id_empresa, c.id_usuario
ORDER BY count(id_compra) DESC

*/

/* Triggers

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

*/



