-- Usuarios y Roles
CREATE TABLE usuario (
	id_usuario BIGINT PRIMARY KEY IDENTITY(1, 1),
	usuario VARCHAR(50) UNIQUE NOT NULL,
	clave VARCHAR(32) NOT NULL,
	habilitado BIT
);

CREATE TABLE usuario_rol (
	id_usuario BIGINT,
	id_rol INT,
	PRIMARY KEY(id_usuario, id_rol)
);

CREATE TABLE rol (
	id_rol INT PRIMARY KEY IDENTITY(1, 1),
	nombre VARCHAR(50) NOT NULL,
	habilitado BIT
);

CREATE TABLE funcionalidad_rol (
	id_rol INT,
	id_funcionalidad INT,
	PRIMARY KEY(id_rol, id_funcionalidad)
);

CREATE TABLE funcionalidad (
	id_funcionalidad INT PRIMARY KEY IDENTITY(1, 1),
	descripcion VARCHAR(255) NOT NULL
);

CREATE TABLE login_fallido (
	id_usuario BIGINT PRIMARY KEY,
	nro_INTento tinyINT
);

-- Clientes y Empresas
CREATE TABLE cliente (
	id_usuario BIGINT PRIMARY KEY,
	nombre NVARCHAR(255),
	apellido NVARCHAR(255),
	tipo_documento VARCHAR(3) CHECK(tipo_documento IN ('LC', 'LE', 'DNI')),
	numero_documento NUMERIC(18, 0),
	cuil BIGINT,
	mail NVARCHAR(255),
	telefono VARCHAR(20),
	calle NVARCHAR(255),
	portal NUMERIC(18, 0),
	piso NUMERIC(18, 0),
	departamento NVARCHAR(255),
	localidad NVARCHAR(255),
	codigo_postal NVARCHAR(255),
	fecha_nacimiento DATETIME,
	fecha_creacion DATETIME DEFAULT GETDATE(),
	tarjeta_credito VARCHAR(10),
	UNIQUE(tipo_documento, numero_documento)
);

CREATE TABLE puntos_cliente (
	id_puntaje BIGINT PRIMARY KEY IDENTITY(1, 1),
	id_cliente BIGINT,
	puntos INT,
	vencimiento DATE
);

CREATE TABLE empresa (
	id_usuario BIGINT PRIMARY KEY,
	razon_social NVARCHAR(255) UNIQUE,
	cuit NVARCHAR(255) UNIQUE,
	mail NVARCHAR(255),
	telefono VARCHAR(20),
	ciudad NVARCHAR(255),
	calle NVARCHAR(255),
	portal NUMERIC(18, 0),
	piso NUMERIC(18, 0),
	departamento NVARCHAR(255),
	localidad NVARCHAR(255),
	codigo_postal NVARCHAR(255),
	fecha_creacion DATETIME DEFAULT GETDATE()
);

-- Publicaciones y Espectaculos
CREATE TABLE publicacion (
	id_publicacion BIGINT PRIMARY KEY IDENTITY(1, 1),
	codigo_publicacion NUMERIC(18 ,0),
	descripcion NVARCHAR(255),
	stock INT,
	fecha_publicacion DATETIME,
	id_rubro INT,
	direccion_espectaculo VARCHAR(255),
	id_grado INT,
	id_empresa BIGINT,
	comision NUMERIC(4, 3),
	fecha_vencimiento DATETIME
);

CREATE TABLE espectaculo (
	id_espectaculo BIGINT PRIMARY KEY IDENTITY(1, 1),
	id_publicacion BIGINT,
	fecha DATETIME,
	id_estado INT
);

CREATE TABLE rubro (
	codigo INT PRIMARY KEY,
	descripcion NVARCHAR(255),
	habilitado BIT
);

CREATE TABLE grado_publicacion (
	id_grado INT PRIMARY KEY IDENTITY(1, 1),
	descripcion NVARCHAR(255),
	comision NUMERIC(4, 3),
	habilitado BIT
);

CREATE TABLE estado_publicacion (
	id_estado INT PRIMARY KEY IDENTITY(1, 1),
	descripcion NVARCHAR(255)
);

CREATE TABLE ubicacion_publicacion (
	id_ubicacion BIGINT PRIMARY KEY IDENTITY(1, 1),
	id_publicacion BIGINT,
	fila VARCHAR(3),
	asiento NUMERIC(18 ,0),
	precio NUMERIC(18 ,0),
	id_tipo INT,
	sin_numerar BIT
);

CREATE TABLE tipo_ubicacion (
	id_tipo_ubicacion INT PRIMARY KEY IDENTITY(1, 1),
	codigo NUMERIC(18, 0),
	descripcion NVARCHAR(255)
);

-- Compras y Facturas
CREATE TABLE compra (
	id_compra BIGINT PRIMARY KEY IDENTITY(1, 1),
	id_espectaculo BIGINT,
	id_cliente BIGINT,
	fecha DATETIME, --DEFAULT GETDATE()
	tarjeta_utilizada VARCHAR(10)
);

CREATE TABLE ubicacion_compra (
	id_ubicacion BIGINT,
	id_compra BIGINT,
	precio NUMERIC(18, 0),
	cantidad NUMERIC(18, 0),
	PRIMARY KEY(id_ubicacion, id_compra)
);

CREATE TABLE factura (
	id_factura BIGINT PRIMARY KEY IDENTITY(1, 1),
	numero NUMERIC(18, 0),
	fecha DATETIME, --DEFAULT GETDATE()
	total NUMERIC(18, 2),
	forma_pago NVARCHAR(255)
);

CREATE TABLE item_factura (
	id_compra BIGINT,
	id_factura BIGINT,
	descripcion NVARCHAR(255),
	monto NUMERIC(18, 2),
	cantidad NUMERIC(18, 0) DEFAULT 1,
	PRIMARY KEY(id_compra, id_factura)
);

-- Premios
CREATE TABLE premio (
	id_premio INT PRIMARY KEY IDENTITY(1, 1),
	puntos_necesarios INT,
	descripcion VARCHAR(255)
);