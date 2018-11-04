USE GDDPrueba
GO
CREATE SCHEMA RAGNAR
GO
--/ CREACION DE TABLAS /--
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
													habilitado bit NOT NULL DEFAULT 1)

CREATE TABLE RAGNAR.Usuario_rol					(	id_usuario bigint FOREIGN KEY references RAGNAR.Usuario(id_usuario),
													id_rol int FOREIGN KEY references RAGNAR.Rol(id_rol),
													PRIMARY KEY(id_rol,id_usuario))
													

CREATE TABLE RAGNAR.Login_fallido				(	id_usuario bigint FOREIGN KEY references RAGNAR.Usuario(id_usuario),
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
													id_empresa bigint FOREIGN KEY references RAGNAR.Empresa(id_usuario) NOT NULL,
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
													id_compra bigint FOREIGN KEY references RAGNAR.Compra(id_compra),
													id_factura bigint FOREIGN KEY references RAGNAR.Factura(id_factura),
													descripcion nvarchar(60) NOT NULL,
													monto numeric(18,2) NOT NULL,
													cantidad numeric(18,0) NOT NULL DEFAULT 1) --/Saque la PK, se puede llegar a crear una usando id_ubicacion de ser necesario

CREATE TABLE RAGNAR.Premio						(	id_premio int identity PRIMARY KEY,
													puntos_necesarios int NOT NULL,
													descripcion varchar(255) NOT NULL)

--/Fin de creacion de tablas/--

--/Migracion de la tabla maestra/--

--/Inserts de usuarios/--

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

--/Inserts de Espectaculos/--

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

--/Inserts de Ubicaciones/--
	
INSERT INTO RAGNAR.Tipo_ubicacion(codigo, descripcion)
	SELECT DISTINCT Ubicacion_Tipo_Codigo, Ubicacion_Tipo_Descripcion
	FROM gd_esquema.Maestra
	WHERE Ubicacion_Tipo_Codigo IS NOT NULL AND Ubicacion_Tipo_Descripcion IS NOT NULL
	
INSERT INTO RAGNAR.Compra(id_cliente, id_empresa, fecha)
	SELECT DISTINCT C.id_usuario,E.id_usuario, M.Compra_Fecha
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Cliente as C ON (M.Cli_Dni = C.numero_documento) JOIN RAGNAR.Empresa as E ON (M.Espec_Empresa_Cuit = E.cuit)
	WHERE M.Compra_Fecha IS NOT NULL
	
INSERT INTO RAGNAR.Ubicacion_publicacion(id_publicacion, id_tipo, fila, asiento, sin_numerar, precio, id_compra, compra_cantidad)
	SELECT DISTINCT P.id_publicacion, T.id_tipo_ubicacion, M.Ubicacion_Fila, M.Ubicacion_Asiento, M.Ubicacion_Sin_Numerar, M.Ubicacion_Precio,COM.id_compra, M.Compra_Cantidad
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Publicacion as P ON (M.Espectaculo_Cod = P.codigo_publicacion) JOIN RAGNAR.Tipo_ubicacion as T ON (M.Ubicacion_Tipo_Codigo = T.codigo AND M.Ubicacion_Tipo_Descripcion = T.descripcion) JOIN  RAGNAR.Cliente as C ON (M.Cli_Dni = C.numero_documento) JOIN RAGNAR.Empresa as E ON (M.Espec_Empresa_Cuit = E.cuit) JOIN RAGNAR.Compra as COM ON (COM.id_cliente = C.id_usuario AND COM.id_empresa = E.id_usuario AND COM.fecha = M.Compra_Fecha)
	WHERE M.Ubicacion_Sin_Numerar IS NOT NULL AND M.Ubicacion_Precio IS NOT NULL AND M.Compra_Cantidad IS NOT NULL

INSERT INTO RAGNAR.Ubicacion_publicacion(id_publicacion, id_tipo, fila, asiento, sin_numerar, precio)
	SELECT DISTINCT P.id_publicacion, T.id_tipo_ubicacion, M.Ubicacion_Fila, M.Ubicacion_Asiento, M.Ubicacion_Sin_Numerar, M.Ubicacion_Precio
	FROM gd_esquema.Maestra as M JOIN RAGNAR.Publicacion as P ON (M.Espectaculo_Cod = P.codigo_publicacion) JOIN RAGNAR.Tipo_ubicacion as T ON (M.Ubicacion_Tipo_Codigo = T.codigo AND M.Ubicacion_Tipo_Descripcion = T.descripcion)
	WHERE M.Ubicacion_Sin_Numerar IS NOT NULL AND M.Ubicacion_Precio IS NOT NULL
	GROUP BY P.id_publicacion, T.id_tipo_ubicacion, M.Ubicacion_Fila, M.Ubicacion_Asiento, M.Ubicacion_Sin_Numerar, M.Ubicacion_Precio
	HAVING COUNT(M.Compra_Cantidad) = 0

INSERT INTO RAGNAR.Factura(numero, fecha, total, forma_pago)
	SELECT DISTINCT Factura_Nro, Factura_Fecha, Factura_Total, Forma_Pago_Desc
	FROM gd_esquema.Maestra
	WHERE Factura_Nro IS NOT NULL AND Factura_Fecha IS NOT NULL AND Factura_Total IS NOT NULL AND Forma_Pago_Desc IS NOT NULL
	/*
/*INSERT INTO GDDRAGNAR.Ubicacion_publicacion(id_publicacion, id_tipo, fila, asiento, sin_numerar, precio)
	SELECT DISTINCT P.id_publicacion, T.id_tipo_ubicacion, M.Ubicacion_Fila, M.Ubicacion_Asiento, M.Ubicacion_Sin_Numerar, M.Ubicacion_Precio
	FROM gd_esquema.Maestra as M JOIN GDDRAGNAR.Publicacion as P ON (M.Espectaculo_Cod = P.codigo_publicacion) JOIN GDDRAGNAR.Tipo_ubicacion as T ON (M.Ubicacion_Tipo_Codigo = T.codigo AND M.Ubicacion_Tipo_Descripcion = T.descripcion)
	WHERE M.Ubicacion_Sin_Numerar IS NOT NULL AND M.Ubicacion_Precio IS NOT NULL*/

--/Inserts de Compras/--

	
/*INSERT INTO GDDRAGNAR.Ubicacion_compra(id_ubicacion,id_compra,precio,cantidad)
	SELECT DISTINCT U.id_ubicacion, C.id_compra, M.Ubicacion_Precio, M.Compra_Cantidad
	FROM gd_esquema.Maestra as M JOIN GDDRAGNAR.Cliente as CLI ON (M.Cli_Dni = CLI.numero_documento) JOIN GDDRAGNAR.Compra as C ON (C.id_cliente = CLI.id_usuario AND M.Compra_Fecha = C.fecha) JOIN GDDRAGNAR.Publicacion as P ON (P.codigo_publicacion = M.Espectaculo_Cod) JOIN GDDRAGNAR.Ubicacion_publicacion as U ON (U.id_publicacion = P.id_publicacion AND U.asiento = M.Ubicacion_Asiento AND U.fila = M.Ubicacion_Fila AND U.sin_numerar = M.Ubicacion_Sin_numerar)
	WHERE M.Compra_Cantidad IS NOT NULL*/

--/Inserts de Facturas/--


	/*
INSERT INTO GDDRAGNAR.Item_factura(cantidad, descripcion, monto, id_factura, id_compra)
	SELECT M.Item_Factura_Cantidad, Item_Factura_Descripcion, Item_Factura_Monto, F.id_factura, C.id_compra
	FROM gd_esquema.Maestra as M JOIN GDDRAGNAR.Factura as F ON (M.Factura_Fecha = F.fecha AND M.Factura_Nro = F.numero AND M.Factura_Total = F.total) JOIN GDDRAGNAR.Cliente as CLI ON (CLI.numero_documento = M.Cli_Dni) JOIN GDDRAGNAR.Compra as C ON (C.id_cliente = CLI.id_usuario AND C.fecha = M.Compra_Fecha)
	WHERE M.Item_Factura_Cantidad IS NOT NULL*/*/

	/*
INSERT INTO GDDRAGNAR.Ubicacion_publicacion(id_publicacion, id_tipo, fila, asiento, sin_numerar, precio, id_compra, compra_cantidad)
	SELECT DISTINCT A.id_publicacion, A.id_tipo_ubicacion, A.Ubicacion_Fila, A.Ubicacion_Asiento, A.Ubicacion_Sin_Numerar, A.Ubicacion_Precio, NULL, NULL
		FROM (SELECT DISTINCT P.id_publicacion, T.id_tipo_ubicacion, M.Ubicacion_Fila, M.Ubicacion_Asiento, M.Ubicacion_Sin_Numerar, M.Ubicacion_Precio, NULL as nulo, COUNT(M.Compra_Cantidad) as nulo2
				FROM gd_esquema.Maestra as M JOIN GDDRAGNAR.Publicacion as P ON (M.Espectaculo_Cod = P.codigo_publicacion) JOIN GDDRAGNAR.Tipo_ubicacion as T ON (M.Ubicacion_Tipo_Codigo = T.codigo AND M.Ubicacion_Tipo_Descripcion = T.descripcion)
				WHERE M.Ubicacion_Sin_Numerar IS NOT NULL AND M.Ubicacion_Precio IS NOT NULL
				GROUP BY P.id_publicacion, T.id_tipo_ubicacion, M.Ubicacion_Fila, M.Ubicacion_Asiento, M.Ubicacion_Sin_Numerar, M.Ubicacion_Precio
				HAVING COUNT(M.Compra_Cantidad) = 0) as A*/