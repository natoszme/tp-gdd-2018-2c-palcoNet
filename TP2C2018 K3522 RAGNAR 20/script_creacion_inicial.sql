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
	CREATE TABLE RAGNAR.Fecha_config (fecha datetime)
	BULK INSERT RAGNAR.Fecha_config FROM 'C:\TP2C2018 K3522 RAGNAR 20\src\Config.txt' WITH (FIRSTROW = 1,FIELDTERMINATOR = ',',ROWTERMINATOR = '') --Cambiar la ruta
END
GO

EXEC [RAGNAR].[SP_CargarEnLaBaseFechaDelConfig]
GO
--/ RECORDAR QUE LUEGO DE UN CAMBIO EN EL CONFIG SE DEBE SI O SI LLAMAR AL STORED PROCEDURE PARA ACTUALIZAR LA FECHA EN LA BASE

--/ Funcion para usar la fecha del config /--

CREATE FUNCTION RAGNAR.F_ObtenerFechaDelConfig()
RETURNS datetime
AS
BEGIN
	DECLARE @Fecha datetime
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
													PRIMARY KEY(id_rol,id_usuario),
													habilitado bit NOT NULL DEFAULT 1)
													

CREATE TABLE RAGNAR.Login_fallido				(	id_usuario bigint PRIMARY KEY FOREIGN KEY references RAGNAR.Usuario(id_usuario),
													nro_intento tinyint NOT NULL)

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
													fecha_creacion datetime NOT NULL DEFAULT RAGNAR.F_ObtenerFechaDelConfig(), 
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
													fecha_creacion datetime)

CREATE TABLE RAGNAR.Puntos_cliente				(	id_puntaje bigint identity PRIMARY KEY,
													id_cliente bigint FOREIGN KEY references RAGNAR.Cliente(id_usuario),
													puntos int NOT NULL,
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
													codigo_publicacion numeric(18,0),
													descripcion nvarchar(255) NOT NULL,
													stock int NOT NULL DEFAULT 0,
													fecha_publicacion datetime DEFAULT RAGNAR.F_ObtenerFechaDelConfig(),
													id_rubro int FOREIGN KEY references RAGNAR.Rubro(id_rubro) NOT NULL, 
													direccion varchar(255),
													id_grado int FOREIGN KEY references RAGNAR.Grado_publicacion(id_grado),
													id_empresa bigint FOREIGN KEY references RAGNAR.Empresa(id_usuario),
													fecha_vencimiento datetime NOT NULL,
													fecha_espectaculo datetime NOT NULL,
													id_estado int FOREIGN KEY references RAGNAR.Estado_publicacion(id_estado) NOT NULL,
													CONSTRAINT [Espectaculo] UNIQUE NONCLUSTERED
														 ([descripcion], [fecha_espectaculo]))

CREATE TABLE RAGNAR.Tipo_ubicacion				(	id_tipo_ubicacion int identity PRIMARY KEY,
													codigo numeric(18,0), /*De la maestra, resolver con la linea de arriba*/
													descripcion nvarchar(255) NOT NULL)

CREATE TABLE RAGNAR.Compra						(	id_compra bigint identity PRIMARY KEY,
													id_cliente bigint FOREIGN KEY references RAGNAR.Cliente(id_usuario) NOT NULL,
													fecha datetime NOT NULL DEFAULT RAGNAR.F_ObtenerFechaDelConfig(),
													tarjeta_utilizada varchar(10))

CREATE TABLE RAGNAR.Ubicacion_publicacion		(	id_ubicacion bigint identity PRIMARY KEY,
													id_publicacion bigint FOREIGN KEY references RAGNAR.Publicacion(id_publicacion),
													fila varchar(3),
													asiento numeric(18,0),
													sin_numerar bit NOT NULL,
													precio numeric(18,0) NOT NULL,
													id_tipo	int FOREIGN KEY references RAGNAR.Tipo_ubicacion(id_tipo_ubicacion) NOT NULL,
													id_compra bigint FOREIGN KEY references RAGNAR.Compra(id_compra),
													compra_cantidad numeric(18,0),
													habilitado bit NOT NULL DEFAULT 1)

CREATE TABLE RAGNAR.Factura						(	id_factura bigint identity PRIMARY KEY,
													numero numeric(18,0) NOT NULL,
													fecha datetime NOT NULL DEFAULT RAGNAR.F_ObtenerFechaDelConfig(),
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

--/ Funcion para encriptar la contraseņa /--

CREATE FUNCTION RAGNAR.F_HasheoDeClave (@Clave varchar(32))
RETURNS varchar(32)
AS
BEGIN
	RETURN CONVERT(varchar(32),HASHBYTES('SHA2_256',@Clave))
END
GO

--/ Trigger para encriptar la contraseņa ingresada /--

CREATE TRIGGER RAGNAR.HasheoDeClaveDeUsuario ON RAGNAR.Usuario INSTEAD OF INSERT, UPDATE
AS
BEGIN
	DECLARE @Usuario varchar(50), @Clave varchar(32), @Habilitado bit, @Nuevo tinyint
	IF(EXISTS (SELECT * FROM DELETED)) --/Es un update
	BEGIN
		DECLARE CUsuarios CURSOR FOR (SELECT I.usuario, I.clave, I.habilitado, I.es_nuevo FROM INSERTED as I JOIN DELETED as D ON (I.usuario = D.usuario AND I.habilitado = D.habilitado AND I.es_nuevo = D.es_nuevo))
		OPEN CUsuarios
		FETCH NEXT FROM CUsuarios INTO @Usuario, @Clave, @Habilitado, @Nuevo
		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO RAGNAR.Usuario(usuario, clave, habilitado, es_nuevo) VALUES (@Usuario, RAGNAR.F_HasheoDeClave(@Clave), @Habilitado, @Nuevo)
			FETCH NEXT FROM CUsuarios INTO @Usuario, @Clave, @Habilitado, @Nuevo
		END
		CLOSE CUsuarios
		DEALLOCATE CUsuarios
	END
	ELSE --/Es un insert
	BEGIN
		DECLARE CUsuarios CURSOR FOR (SELECT usuario, clave, habilitado, es_nuevo FROM INSERTED)
		OPEN CUsuarios
		FETCH NEXT FROM CUsuarios INTO @Usuario, @Clave, @Habilitado, @Nuevo
		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO RAGNAR.Usuario(usuario, clave, habilitado, es_nuevo) VALUES (@Usuario, RAGNAR.F_HasheoDeClave(@Clave), @Habilitado, @Nuevo)
			FETCH NEXT FROM CUsuarios INTO @Usuario, @Clave, @Habilitado, @Nuevo
		END
		CLOSE CUsuarios
		DEALLOCATE CUsuarios
	END
END
GO

--/ Triggers para el calculo de stock /--

CREATE TRIGGER RAGNAR.SumaDeStockDePublicacion ON RAGNAR.Ubicacion_publicacion AFTER INSERT
AS
BEGIN
	DECLARE @Publicacion bigint
	DECLARE CUbicacion CURSOR FOR (SELECT I.id_publicacion FROM INSERTED as I WHERE I.id_compra IS NULL)
	OPEN CUbicacion
	FETCH NEXT FROM CUbicacion INTO @Publicacion
	WHILE @@FETCH_STATUS = 0
	BEGIN
		UPDATE RAGNAR.Publicacion SET stock = (stock + 1) WHERE id_publicacion = @Publicacion
		FETCH NEXT FROM CUbicacion INTO @Publicacion
	END
	CLOSE CUbicacion
	DEALLOCATE CUbicacion
END
GO

CREATE TRIGGER RAGNAR.RestaDeStockDePublicacion ON RAGNAR.Ubicacion_publicacion AFTER UPDATE
AS
BEGIN
	DECLARE @Publicacion bigint, @CompraActual numeric(18,0), @CompraPasada numeric(18,0), @HabilitadoActual bit, @HabilitadoAnterior bit
	--/Cuando se vende una ubicacion/--
	DECLARE CUbicacion CURSOR FOR (SELECT I.id_publicacion, I.id_compra, D.id_compra FROM INSERTED as I JOIN DELETED as D ON (I.asiento = D.asiento AND I.fila = D.fila AND I.id_publicacion = D.id_publicacion AND I.id_tipo = D.id_tipo AND I.precio = D.precio AND I.sin_numerar = D.sin_numerar) WHERE I.id_compra IS NOT NULL)
	OPEN CUbicacion
	FETCH NEXT FROM CUbicacion INTO @Publicacion, @CompraActual, @CompraPasada
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF(@CompraActual IS NOT NULL AND @CompraPasada IS NULL)
		BEGIN
		UPDATE RAGNAR.Publicacion SET stock = (stock - 1) WHERE id_publicacion = @Publicacion
		END
		FETCH NEXT FROM CUbicacion INTO @Publicacion, @CompraActual, @CompraPasada
	END
	CLOSE CUbicacion
	DEALLOCATE CUbicacion
	--/Cuando se borran ubicaciones de una publicacion o se rehabilitan ubicaciones/--
	DECLARE CUbicacion CURSOR FOR (SELECT I.id_publicacion, I.habilitado, D.habilitado FROM INSERTED as I JOIN DELETED as D ON (I.asiento = D.asiento AND I.fila = D.fila AND I.id_publicacion = D.id_publicacion AND I.id_tipo = D.id_tipo AND I.precio = D.precio AND I.sin_numerar = D.sin_numerar AND I.id_compra = D.id_compra) WHERE I.id_compra IS NULL)
	OPEN CUbicacion
	FETCH NEXT FROM CUbicacion INTO @Publicacion, @HabilitadoActual, @HabilitadoAnterior
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF(@HabilitadoActual = 0 AND @HabilitadoAnterior = 1)
		BEGIN
		UPDATE RAGNAR.Publicacion SET stock = (stock - 1) WHERE id_publicacion = @Publicacion
		END
		IF(@HabilitadoActual = 1 AND @HabilitadoAnterior = 0)
		BEGIN
		UPDATE RAGNAR.Publicacion SET stock = (stock + 1) WHERE id_publicacion = @Publicacion
		END
		FETCH NEXT FROM CUbicacion INTO @Publicacion, @HabilitadoActual, @HabilitadoAnterior
	END
	CLOSE CUbicacion
	DEALLOCATE CUbicacion
END
GO

--/ Migracion de la tabla maestra /--

--/ Inserts de usuarios /--

INSERT INTO RAGNAR.Usuario(usuario,clave,habilitado) VALUES ('admin','w23e',1) /*Usuario administrador, cambiar la pass con el metodo de encriptacion*/

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

INSERT INTO RAGNAR.Grado_publicacion (descripcion, comision, habilitado) VALUES ('Alta',CONVERT(numeric(4,3),0.25),1)
INSERT INTO RAGNAR.Grado_publicacion (descripcion, comision, habilitado) VALUES ('Media',CONVERT(numeric(4,3),0.1),1)
INSERT INTO RAGNAR.Grado_publicacion (descripcion, comision, habilitado) VALUES ('Baja',CONVERT(numeric(4,3),0.05),1)
	
INSERT INTO RAGNAR.Publicacion(codigo_publicacion, descripcion, fecha_vencimiento, id_rubro , fecha_espectaculo, id_estado, id_empresa, id_grado)
	SELECT DISTINCT M.Espectaculo_Cod, M.Espectaculo_Descripcion, M.Espectaculo_Fecha_Venc, R.id_rubro, M.Espectaculo_Fecha, E.id_estado, EMP.id_usuario, 2
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Rubro as R ON (M.Espectaculo_Rubro_Descripcion = R.descripcion) JOIN RAGNAR.Estado_publicacion as E ON (M.Espectaculo_Estado = E.descripcion) JOIN RAGNAR.Empresa as EMP ON (M.Espec_Empresa_Cuit = EMP.cuit)
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
INSERT INTO RAGNAR.Rol(nombre, habilitado) VALUES ('AdminParaTests',1)

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
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'ABM de Rol'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Registro de Usuario'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'ABM de Cliente'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'ABM de Empresa de Espectaculos'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'ABM de Rubro'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'ABM de Grado de Publicacion'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Generar Publicacion'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Editar Publicacion'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Comprar'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Historial de Cliente'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Canje y Administracion de Puntos'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Generar rendicion de comisiones'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))
INSERT INTO RAGNAR.Funcionalidad_rol (id_funcionalidad, id_rol) VALUES ((SELECT id_funcionalidad FROM RAGNAR.Funcionalidad WHERE descripcion = 'Listado estadistico'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))

--/ Inserts de roles de cada usuario /--

INSERT INTO RAGNAR.Usuario_rol (id_usuario, id_rol) VALUES ((SELECT id_usuario FROM RAGNAR.Usuario WHERE usuario = 'Admin'),(SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'AdminParaTests'))

INSERT INTO RAGNAR.Usuario_rol (id_usuario, id_rol)
	SELECT id_usuario, (SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Cliente')
	FROM RAGNAR.Cliente

INSERT INTO RAGNAR.Usuario_rol (id_usuario, id_rol)
	SELECT id_usuario, (SELECT id_rol FROM RAGNAR.Rol WHERE nombre = 'Empresa')
	FROM RAGNAR.Empresa

--/ Inserts de premios /--

INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('Cafetera',2250)
INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('Licuadora',1000)
INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('Horno electrico',5000)
INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('Aspiradora',2000)
INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('Pelota de futbol',250)
INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('2 dias de estadia en un spa',7500)
INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('Remera de la seleccion argentina',450)
INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('Caja de herramientas',7500)
INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('Viaje a Mar del Plata',10000)
INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('Viaje a Bariloche',20000)
INSERT INTO RAGNAR.Premio (descripcion, puntos_necesarios) VALUES ('Viaje a Miami',50000)

--/ Fin de Inserts /--

GO

--/ VISTAS /--

--/ FUNCIONES /--

--/ Funcion para ver el historial de un cliente /--

CREATE FUNCTION RAGNAR.F_HistorialDeCliente (@Cliente bigint)
RETURNS TABLE
AS
	RETURN (SELECT P.descripcion as Espectaculo, P.fecha_espectaculo as FechaEspectaculo, C.fecha as FechaDeCompra, ISNULL(C.tarjeta_utilizada,'Efectivo') as FormaDePago, U.precio as Precio, U.asiento as Asiento, U.fila as Fila, U.sin_numerar as EsSinNumerar, T.descripcion as TipodeUbicacion FROM RAGNAR.Compra as C JOIN RAGNAR.Ubicacion_publicacion as U ON (C.id_compra = U.id_compra) JOIN RAGNAR.Publicacion as P ON (U.id_publicacion = P.id_publicacion) JOIN RAGNAR.Tipo_ubicacion as T ON (U.id_tipo = T.id_tipo_ubicacion) WHERE C.id_cliente = @Cliente)
GO

--/ Funciones del Listado Estadistico /--

--/ Funcion para listado de empresas con mayor cantidad de localidades no vendidas /--
/*
CREATE FUNCTION RAGNAR.F_EmpresasConMasLocalidadesNoVencidas (@Fecha datetime)
RETURNS TABLE
AS
RETURN (SELECT * FROM RAGNAR.Empresa as E JOIN RAGNAR.Publicacion as P ON (E.id_usuario = P.id_empresa) WHERE YEAR(fecha_publicacion))
GO*/
CREATE FUNCTION RAGNAR.F_EmpresasConMasLocalidadesNoVencidas (@Fecha datetime)
RETURNS TABLE
AS
RETURN (SELECT TOP 5 MONTH(P.fecha_publicacion) as Mes, G.descripcion as GradoDePublicacion, (SELECT TOP 1 EMP.razon_social FROM RAGNAR.Empresa as EMP JOIN RAGNAR.Publicacion as PUB ON (EMP.id_usuario = PUB.id_empresa) WHERE YEAR(PUB.fecha_publicacion) = YEAR(P.fecha_publicacion) AND MONTH(PUB.fecha_publicacion) = MONTH(P.fecha_publicacion) AND PUB.id_grado = P.id_grado GROUP BY EMP.razon_social ORDER BY SUM(PUB.stock) DESC) as EmpresaConMasStock FROM RAGNAR.Publicacion as P JOIN RAGNAR.Grado_publicacion as G ON (G.id_grado = P.id_grado) WHERE YEAR(P.fecha_publicacion) = YEAR(@Fecha) AND (MONTH(@Fecha) - MONTH(P.fecha_publicacion)) < 3 GROUP BY MONTH(P.fecha_publicacion), G.descripcion, P.fecha_publicacion, P.id_grado ORDER BY MONTH(P.fecha_publicacion) ASC, G.descripcion ASC)
GO


--/ Funcion para listado de clientes con mas puntos vencidos /--

CREATE FUNCTION RAGNAR.F_ClientesConMasPuntosVencidos (@Fecha datetime)
RETURNS TABLE
AS
RETURN (SELECT TOP 5 C.nombre as Nombre, C.apellido as Apellido, C.tipo_documento as TipoDeDocumento, C.numero_documento as NumeroDeDocumento, C.cuil as CUIL, SUM(P.puntos) as PuntosVencidos FROM RAGNAR.Puntos_cliente as P JOIN RAGNAR.Cliente as C ON (P.id_cliente = C.id_usuario) WHERE YEAR(P.vencimiento) = YEAR(@Fecha) AND (MONTH(@Fecha) - MONTH(P.vencimiento)) < 3 GROUP BY C.id_usuario, C.nombre, C.apellido, C.tipo_documento, C.numero_documento, C.cuil ORDER BY 6 DESC)
GO

--/ Funcion para listado de clientes con mayor cantidad de compras /--

CREATE FUNCTION RAGNAR.F_ClientesConMasCompras (@Fecha datetime)
RETURNS TABLE
AS
RETURN (SELECT TOP 5 CLI.nombre as Nombre, CLI.apellido as Apellido, CLI.tipo_documento as TipoDeDocumento, CLI.numero_documento as NumeroDeDocumento, CLI.cuil as CUIL, COUNT(*) as CantidadDeCompras FROM RAGNAR.Compra as C JOIN RAGNAR.Cliente as CLI ON (CLI.id_usuario = C.id_cliente) JOIN RAGNAR.Ubicacion_publicacion as U ON (C.id_compra = U.id_compra) JOIN RAGNAR.Publicacion as P ON (U.id_publicacion = P.id_publicacion) WHERE YEAR(C.fecha) = YEAR(@Fecha) AND (MONTH(@Fecha) - MONTH(C.fecha)) < 3 GROUP BY P.id_empresa, CLI.nombre, CLI.apellido, CLI.tipo_documento, CLI.numero_documento, CLI.cuil ORDER BY 6 DESC)
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
		IF (NOT EXISTS (SELECT * FROM RAGNAR.Usuario WHERE id_usuario = @ID AND clave = @ClaveEncriptada)) --La contraseņa ingresada no es correcta
		BEGIN
			PRINT('La contraseņa ingresada no es correcta')
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

--/ StoredProcedure para calcular el stock de una publicacion YA NO SE USA /--

CREATE PROCEDURE RAGNAR.SP_StockDePublicacion (@IdPublicacion bigint)
AS
BEGIN
	DECLARE @Stock int
	SET @Stock = (SELECT COUNT(*) FROM RAGNAR.Ubicacion_publicacion WHERE id_publicacion = @IdPublicacion AND id_compra IS NULL)
	UPDATE RAGNAR.Publicacion SET stock = @Stock WHERE id_publicacion = @IdPublicacion
END
GO

--/ StoredProcedure para rendicion de comisiones /--

CREATE PROCEDURE RAGNAR.SP_RendicionDeComisiones (@CantidadAFacturar int)
AS
BEGIN
	DECLARE @NumeroDeFactura numeric(18,0), @Empresa bigint, @EmpresaAnterior bigint, @IdUbicacion bigint, @PrecioUbicacion numeric(18,0), @PrecioComision numeric(18,2), @Comision numeric(4,3), @Total numeric(18,2)
	DECLARE CUbicacionesAFacturar CURSOR FOR (SELECT * FROM (SELECT TOP (@CantidadAFacturar) U.id_ubicacion, U.precio, G.comision, P.id_empresa FROM RAGNAR.Ubicacion_publicacion as U JOIN RAGNAR.Compra as C ON (C.id_compra = U.id_compra) JOIN RAGNAR.Publicacion as P ON (P.id_publicacion = U.id_publicacion) JOIN RAGNAR.Grado_publicacion as G ON (G.id_grado = P.id_grado) WHERE U.id_ubicacion NOT IN (SELECT I.id_ubicacion FROM RAGNAR.Item_factura as I) ORDER BY C.fecha ASC) as TablaParaUsarOrderBy)
	OPEN CUbicacionesAFacturar
	FETCH NEXT FROM CUbicacionesAFacturar INTO @IdUbicacion, @PrecioUbicacion, @Comision, @Empresa
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @Total = 0
		SET @EmpresaAnterior = @Empresa
		SET @NumeroDeFactura = (SELECT TOP 1 numero FROM RAGNAR.Factura ORDER BY numero DESC) + 1
		INSERT INTO RAGNAR.Factura (numero, fecha) VALUES (@NumeroDeFactura, RAGNAR.F_ObtenerFechaDelConfig())
		WHILE(@Empresa = @EmpresaAnterior) --/Misma factura
		BEGIN
			SET @PrecioComision = @PrecioUbicacion * @Comision
			SET @Total = @Total + @PrecioComision
			INSERT INTO RAGNAR.Item_factura (id_ubicacion, id_factura, monto) VALUES (@IdUbicacion, (SELECT id_factura FROM RAGNAR.Factura WHERE numero = @NumeroDeFactura), @PrecioComision)
			FETCH NEXT FROM CUbicacionesAFacturar INTO @IdUbicacion, @PrecioUbicacion, @Comision, @Empresa
		END
		UPDATE RAGNAR.Factura SET total = @Total WHERE numero = @NumeroDeFactura
	END
	CLOSE CUbicacionesAFacturar
	DEALLOCATE CUbicacionesAFacturar
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
		UPDATE RAGNAR.Login_fallido SET nro_intento = 0 WHERE id_usuario = @ID
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
		UPDATE RAGNAR.Usuario_rol SET habilitado = 0 WHERE id_rol = @Rol
		FETCH NEXT FROM CRoles INTO @Rol
	END
	CLOSE CRoles
	DEALLOCATE CRoles
END
GO

--/ Trigger para suma de puntos ante una compra /--

CREATE TRIGGER RAGNAR.SumarPuntos ON RAGNAR.Compra AFTER INSERT
AS
BEGIN
	DECLARE @Cliente bigint, @Fecha datetime
	DECLARE CCliente CURSOR FOR (SELECT id_cliente, fecha FROM INSERTED)
	OPEN CCliente
	FETCH NEXT FROM CCliente INTO @Cliente
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO RAGNAR.Puntos_cliente (id_cliente,puntos,vencimiento) VALUES (@Cliente, 50, DATEADD(year,1,@Fecha))
		FETCH NEXT FROM CCliente INTO @Cliente, @Fecha
	END
	CLOSE CCliente
	DEALLOCATE CCliente
END
GO

/*ALTER TABLE UNIQUE PUBLICACION*/

/* Triggers

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



