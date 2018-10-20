-- Usuarios y Roles
ALTER TABLE usuario_rol
	ADD FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario),
	ADD FOREIGN KEY (id_rol) REFERENCES rol(id_rol);

ALTER TABLE funcionalidad_rol
	ADD FOREIGN KEY (id_rol) REFERENCES rol(id_rol),
	ADD FOREIGN KEY (id_funcionalidad) REFERENCES funcionalidad(id_funcionalidad);	

ALTER TABLE login_fallido
	ADD FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario);

-- Clientes y Empresas
ALTER TABLE cliente
	ADD FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario);

ALTER TABLE puntos_cliente
	ADD FOREIGN KEY (id_cliente) REFERENCES cliente(id_usuario);

ALTER TABLE empresa
	ADD FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario);

-- Publicaciones y Espectaculos
ALTER TABLE publicacion
	ADD FOREIGN KEY (id_rubro) REFERENCES rubro(codigo),
	ADD FOREIGN KEY (id_grado) REFERENCES grado(id_grado),
	ADD FOREIGN KEY (id_empresa) REFERENCES empresa(id_usuario);

ALTER TABLE espectaculo
	ADD FOREIGN KEY (id_publicacion) REFERENCES publicacion(id_publicacion),
	ADD FOREIGN KEY (id_estado) REFERENCES estado_publicacion(id_estado);

ALTER TABLE ubicacion_publicacion
	ADD FOREIGN KEY (id_publicacion) REFERENCES publicacion(id_publicacion),
	ADD FOREIGN KEY (id_tipo) REFERENCES tipo_ubicacion(id_tipo_ubicacion);

-- Compras y Facturas
ALTER TABLE compra
	ADD FOREIGN KEY (id_espectaculo) REFERENCES espectaculo(id_espectaculo),
	ADD FOREIGN KEY (id_cliente) REFERENCES cliente(id_usuario);

ALTER TABLE ubicacion_compra
	ADD FOREIGN KEY (id_ubicacion) REFERENCES ubicacion_publicacion(id_ubicacion),
	ADD FOREIGN KEY (id_compra) REFERENCES compra(id_compra);

ALTER TABLE item_factura
	ADD FOREIGN KEY (id_compra) REFERENCES compra(id_compra),
	ADD FOREIGN KEY (id_factura) REFERENCES factura(id_factura);