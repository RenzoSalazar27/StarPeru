USE starperu;
DROP DATABASE starperu;

select * from pagos;
INSERT INTO Usuarios (nombresUsuario, apellidosUsuario, nacimientoUsuario, telefonoUsuario, identificacionUsuario, correoUsuario, contrasenaUsuario, esAdmin)
VALUES ('Admin', 'Sistema', '1990-01-01', '999999999', '12345678', 'admin@gmail.com', 'Admin123*', 1);

INSERT INTO Usuarios (nombresUsuario, apellidosUsuario, nacimientoUsuario, telefonoUsuario, identificacionUsuario, correoUsuario, contrasenaUsuario, esAdmin)
VALUES ('Renzo Nicolas', 'Salazar Valenzuela', '2004-02-27', '999999999', '12345671', 'renzo@gmail.com', 'Renzo123*', 0);

INSERT INTO Pilotos (nombrePiloto, apellidoPiloto, nacimientoPiloto, telefonoPiloto, dniPiloto, licenciaPiloto, tipoLicPiloto, fechaEmiLic)
VALUES 
('Carlos', 'Rodríguez', '1985-06-20', '987654321', '12345678', 'LIC001', 'Comercial', '2010-01-01'),
('María', 'González', '1988-03-15', '987654322', '87654321', 'LIC002', 'Comercial', '2012-01-01'),
('Juan', 'Martínez', '1990-09-10', '987654323', '23456789', 'LIC003', 'Comercial', '2015-01-01');

INSERT INTO Flota (modeloAvion, fabricanteAvion, matriculaAvion, capacidadAvion, claseAvion)
VALUES 
('Boeing 737', 'Boeing', 'N12345', 150, 'Económica'),
('Airbus A320', 'Airbus', 'N67890', 180, 'Ejecutiva'),
('Boeing 787', 'Boeing', 'N54321', 250, 'Primera clase');

INSERT INTO Vuelo (origenVuelo, destinoVuelo, fechaVuelo, precioVuelo, idPiloto, idAvion)
VALUES 
('Lima', 'Chiclayo', '2025-09-20 08:00:00', 250.00, 1, 1),
('Lima', 'Cajamarca', '2025-10-20 09:30:00', 280.00, 2, 2),
('Lima', 'Huanuco', '2025-08-20 10:00:00', 220.00, 1, 3),
('Lima', 'Iquitos', '2025-06-20 11:30:00', 350.00, 2, 1),
('Lima', 'Tarapoto', '2025-08-20 13:00:00', 300.00, 1, 2),
('Lima', 'Pucallpa', '2025-07-20 14:30:00', 320.00, 2, 3),
('Lima', 'Lima', '2025-10-20 16:00:00', 200.00, 1, 1);

INSERT INTO Consultas (idUsuario, motivoConsulta, descripcionConsultas, urgencia, estado)
VALUES (2, 'Información', '¿Cuál es el límite de equipaje permitido en vuelos nacionales?', 'Alto', 'Pendiente');



TRUNCATE TABLE Pagos;
TRUNCATE TABLE Pasajeros;
TRUNCATE TABLE reservaVuelos;
TRUNCATE TABLE Consultas;
TRUNCATE TABLE Vuelo;
TRUNCATE TABLE Flota;
TRUNCATE TABLE Pilotos;
TRUNCATE TABLE Usuarios;