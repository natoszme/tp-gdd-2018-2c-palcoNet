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