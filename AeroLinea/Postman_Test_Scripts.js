// ========================================
// SCRIPTS DE PRUEBA AUTOMATIZADOS PARA POSTMAN
// AeroLinea StarPeru - Pruebas Funcionales
// ========================================

// ========================================
// FUNCIONES DE UTILIDAD GLOBALES
// ========================================

// Función para validar código de estado HTTP
function validarCodigoEstado(codigoEsperado) {
    pm.test(`Código de estado debe ser ${codigoEsperado}`, function () {
        pm.response.to.have.status(codigoEsperado);
    });
}

// Función para validar tiempo de respuesta
function validarTiempoRespuesta(tiempoMaximo = 2000) {
    pm.test(`Tiempo de respuesta debe ser menor a ${tiempoMaximo}ms`, function () {
        pm.expect(pm.response.responseTime).to.be.below(tiempoMaximo);
    });
}

// Función para validar estructura JSON
function validarEstructuraJSON() {
    pm.test("Respuesta debe ser JSON válido", function () {
        pm.response.to.be.json;
    });
}

// Función para validar que la respuesta no esté vacía
function validarRespuestaNoVacia() {
    pm.test("Respuesta no debe estar vacía", function () {
        pm.expect(pm.response.text()).to.not.be.empty;
    });
}

// Función para validar contenido HTML
function validarContenidoHTML() {
    pm.test("Respuesta debe contener HTML", function () {
        pm.expect(pm.response.text()).to.include("<!DOCTYPE html>");
    });
}

// ========================================
// SCRIPTS PARA AUTENTICACIÓN
// ========================================

// Script para Login exitoso
function scriptLoginExitoso() {
    validarCodigoEstado(302); // Redirect después del login
    validarTiempoRespuesta();
    
    pm.test("Login debe redirigir correctamente", function () {
        pm.expect(pm.response.code).to.be.oneOf([200, 302]);
    });
}

// Script para Login fallido
function scriptLoginFallido() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    
    pm.test("Login fallido debe mostrar error", function () {
        pm.expect(pm.response.text()).to.include("error");
    });
}

// Script para Registro exitoso
function scriptRegistroExitoso() {
    validarCodigoEstado(302);
    validarTiempoRespuesta();
    
    pm.test("Registro debe redirigir correctamente", function () {
        pm.expect(pm.response.code).to.be.oneOf([200, 302]);
    });
}

// Script para Registro fallido
function scriptRegistroFallido() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    
    pm.test("Registro fallido debe mostrar error", function () {
        pm.expect(pm.response.text()).to.include("error");
    });
}

// ========================================
// SCRIPTS PARA ADMINISTRACIÓN
// ========================================

// Script para Panel Administrador
function scriptPanelAdministrador() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarContenidoHTML();
    
    pm.test("Panel debe contener elementos administrativos", function () {
        pm.expect(pm.response.text()).to.include("admin");
    });
}

// Script para Cambiar Rol
function scriptCambiarRol() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Respuesta debe indicar éxito", function () {
        const response = pm.response.json();
        pm.expect(response).to.have.property('success');
    });
}

// Script para Obtener Usuario
function scriptObtenerUsuario() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Usuario debe tener propiedades requeridas", function () {
        const usuario = pm.response.json();
        pm.expect(usuario).to.have.property('idUsuario');
        pm.expect(usuario).to.have.property('nombresUsuario');
        pm.expect(usuario).to.have.property('correoUsuario');
    });
}

// ========================================
// SCRIPTS PARA GESTIÓN DE PILOTOS
// ========================================

// Script para Obtener Pilotos
function scriptObtenerPilotos() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Debe retornar array de pilotos", function () {
        const pilotos = pm.response.json();
        pm.expect(pilotos).to.be.an('array');
    });
}

// Script para Registrar Piloto
function scriptRegistrarPiloto() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Piloto debe registrarse correctamente", function () {
        const response = pm.response.json();
        pm.expect(response).to.have.property('success');
    });
}

// Script para Obtener Piloto específico
function scriptObtenerPiloto() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Piloto debe tener propiedades requeridas", function () {
        const piloto = pm.response.json();
        pm.expect(piloto).to.have.property('idPiloto');
        pm.expect(piloto).to.have.property('nombresPiloto');
        pm.expect(piloto).to.have.property('licenciaPiloto');
    });
}

// ========================================
// SCRIPTS PARA GESTIÓN DE FLOTA
// ========================================

// Script para Obtener Flota
function scriptObtenerFlota() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Debe retornar array de aviones", function () {
        const flota = pm.response.json();
        pm.expect(flota).to.be.an('array');
    });
}

// Script para Registrar Avión
function scriptRegistrarAvion() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Avión debe registrarse correctamente", function () {
        const response = pm.response.json();
        pm.expect(response).to.have.property('success');
    });
}

// Script para Obtener Avión específico
function scriptObtenerAvion() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Avión debe tener propiedades requeridas", function () {
        const avion = pm.response.json();
        pm.expect(avion).to.have.property('idAvion');
        pm.expect(avion).to.have.property('modeloAvion');
        pm.expect(avion).to.have.property('capacidadAvion');
    });
}

// ========================================
// SCRIPTS PARA GESTIÓN DE VUELOS
// ========================================

// Script para Obtener Vuelos
function scriptObtenerVuelos() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Debe retornar array de vuelos", function () {
        const vuelos = pm.response.json();
        pm.expect(vuelos).to.be.an('array');
    });
}

// Script para Registrar Vuelo
function scriptRegistrarVuelo() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Vuelo debe registrarse correctamente", function () {
        const response = pm.response.json();
        pm.expect(response).to.have.property('success');
    });
}

// Script para Obtener Vuelo específico
function scriptObtenerVuelo() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Vuelo debe tener propiedades requeridas", function () {
        const vuelo = pm.response.json();
        pm.expect(vuelo).to.have.property('idVuelo');
        pm.expect(vuelo).to.have.property('origenVuelo');
        pm.expect(vuelo).to.have.property('destinoVuelo');
        pm.expect(vuelo).to.have.property('precioVuelo');
    });
}

// Script para Vuelos por Destino
function scriptVuelosPorDestino() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarContenidoHTML();
    
    pm.test("Debe mostrar vuelos del destino especificado", function () {
        pm.expect(pm.response.text()).to.not.be.empty;
    });
}

// ========================================
// SCRIPTS PARA RESERVAS
// ========================================

// Script para Reservar Vuelo (GET)
function scriptReservarVueloGET() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarContenidoHTML();
    
    pm.test("Debe mostrar formulario de reserva", function () {
        pm.expect(pm.response.text()).to.include("reserva");
    });
}

// Script para Reservar Vuelo (POST)
function scriptReservarVueloPOST() {
    validarCodigoEstado(302);
    validarTiempoRespuesta();
    
    pm.test("Reserva debe procesarse correctamente", function () {
        pm.expect(pm.response.code).to.be.oneOf([200, 302]);
    });
}

// Script para Detalle de Reserva
function scriptDetalleReserva() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarContenidoHTML();
    
    pm.test("Debe mostrar detalles de la reserva", function () {
        pm.expect(pm.response.text()).to.not.be.empty;
    });
}

// ========================================
// SCRIPTS PARA CONSULTAS Y SOPORTE
// ========================================

// Script para Registrar Consulta
function scriptRegistrarConsulta() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    
    pm.test("Consulta debe registrarse correctamente", function () {
        pm.expect(pm.response.text()).to.include("éxito");
    });
}

// Script para Actualizar Estado Consulta
function scriptActualizarEstadoConsulta() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarEstructuraJSON();
    
    pm.test("Estado debe actualizarse correctamente", function () {
        const response = pm.response.json();
        pm.expect(response).to.have.property('success');
    });
}

// ========================================
// SCRIPTS PARA PERFIL Y PAGOS
// ========================================

// Script para Perfil Pasajero
function scriptPerfilPasajero() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarContenidoHTML();
    
    pm.test("Debe mostrar perfil del usuario", function () {
        pm.expect(pm.response.text()).to.include("perfil");
    });
}

// Script para Actualizar Perfil
function scriptActualizarPerfil() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    
    pm.test("Perfil debe actualizarse correctamente", function () {
        pm.expect(pm.response.text()).to.include("éxito");
    });
}

// Script para Realizar Pago
function scriptRealizarPago() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarContenidoHTML();
    
    pm.test("Debe mostrar formulario de pago", function () {
        pm.expect(pm.response.text()).to.include("pago");
    });
}

// ========================================
// SCRIPTS PARA PÁGINAS ESTÁTICAS
// ========================================

// Script genérico para páginas estáticas
function scriptPaginaEstatica() {
    validarCodigoEstado(200);
    validarTiempoRespuesta();
    validarContenidoHTML();
    
    pm.test("Página debe cargar correctamente", function () {
        pm.expect(pm.response.text()).to.not.be.empty;
    });
}

// ========================================
// FUNCIÓN PRINCIPAL DE SELECCIÓN DE SCRIPT
// ========================================

// Función que determina qué script ejecutar basado en el nombre de la request
function ejecutarScriptSegunRequest() {
    const requestName = pm.info.requestName.toLowerCase();
    
    // Autenticación
    if (requestName.includes("login")) {
        if (requestName.includes("fallido")) {
            scriptLoginFallido();
        } else {
            scriptLoginExitoso();
        }
    }
    else if (requestName.includes("registro") || requestName.includes("registrar")) {
        if (requestName.includes("fallido")) {
            scriptRegistroFallido();
        } else {
            scriptRegistroExitoso();
        }
    }
    // Administración
    else if (requestName.includes("panel administrador")) {
        scriptPanelAdministrador();
    }
    else if (requestName.includes("cambiar rol")) {
        scriptCambiarRol();
    }
    else if (requestName.includes("obtener usuario")) {
        scriptObtenerUsuario();
    }
    // Pilotos
    else if (requestName.includes("obtener pilotos")) {
        scriptObtenerPilotos();
    }
    else if (requestName.includes("registrar piloto")) {
        scriptRegistrarPiloto();
    }
    else if (requestName.includes("obtener piloto")) {
        scriptObtenerPiloto();
    }
    // Flota
    else if (requestName.includes("obtener flota")) {
        scriptObtenerFlota();
    }
    else if (requestName.includes("registrar avión") || requestName.includes("registrar avion")) {
        scriptRegistrarAvion();
    }
    else if (requestName.includes("obtener avión") || requestName.includes("obtener avion")) {
        scriptObtenerAvion();
    }
    // Vuelos
    else if (requestName.includes("obtener vuelos")) {
        scriptObtenerVuelos();
    }
    else if (requestName.includes("registrar vuelo")) {
        scriptRegistrarVuelo();
    }
    else if (requestName.includes("obtener vuelo")) {
        scriptObtenerVuelo();
    }
    else if (requestName.includes("vuelos por destino")) {
        scriptVuelosPorDestino();
    }
    // Reservas
    else if (requestName.includes("reservar vuelo")) {
        if (requestName.includes("get")) {
            scriptReservarVueloGET();
        } else {
            scriptReservarVueloPOST();
        }
    }
    else if (requestName.includes("detalle reserva")) {
        scriptDetalleReserva();
    }
    // Consultas
    else if (requestName.includes("registrar consulta")) {
        scriptRegistrarConsulta();
    }
    else if (requestName.includes("actualizar estado consulta")) {
        scriptActualizarEstadoConsulta();
    }
    // Perfil y Pagos
    else if (requestName.includes("perfil pasajero")) {
        scriptPerfilPasajero();
    }
    else if (requestName.includes("actualizar perfil")) {
        scriptActualizarPerfil();
    }
    else if (requestName.includes("realizar pago")) {
        scriptRealizarPago();
    }
    // Páginas estáticas
    else {
        scriptPaginaEstatica();
    }
}

// ========================================
// EJECUCIÓN AUTOMÁTICA
// ========================================

// Ejecutar el script correspondiente
ejecutarScriptSegunRequest();

// ========================================
// VALIDACIONES ADICIONALES GLOBALES
// ========================================

// Validar headers de seguridad
pm.test("Headers de seguridad deben estar presentes", function () {
    pm.expect(pm.response.headers).to.have.property('X-Content-Type-Options');
    pm.expect(pm.response.headers).to.have.property('X-Frame-Options');
});

// Validar que no haya errores de servidor
pm.test("No debe haber errores de servidor", function () {
    pm.expect(pm.response.code).to.not.equal(500);
    pm.expect(pm.response.text()).to.not.include("Internal Server Error");
});

// Validar que la respuesta sea consistente
pm.test("Respuesta debe ser consistente", function () {
    pm.expect(pm.response.responseTime).to.be.a('number');
    pm.expect(pm.response.size()).to.be.greaterThan(0);
});

// ========================================
// LOGGING PARA DEBUGGING
// ========================================

// Log de información de la request para debugging
console.log("Request Name:", pm.info.requestName);
console.log("Response Code:", pm.response.code);
console.log("Response Time:", pm.response.responseTime + "ms");
console.log("Response Size:", pm.response.size() + " bytes");

// Log de headers importantes
console.log("Content-Type:", pm.response.headers.get("Content-Type"));
console.log("Server:", pm.response.headers.get("Server")); 