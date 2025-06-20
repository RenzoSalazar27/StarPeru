# Pruebas Funcionales con Postman - AeroLinea StarPeru

## 📋 Descripción General

Esta colección de Postman contiene pruebas funcionales completas para la aplicación web de aerolínea StarPeru. La colección está organizada en carpetas que cubren todas las funcionalidades principales del sistema.

## 🚀 Configuración Inicial

### 1. Importar la Colección

1. Abre Postman
2. Haz clic en "Import" en la esquina superior izquierda
3. Selecciona el archivo `AeroLinea_Postman_Collection.json`
4. La colección se importará automáticamente

### 2. Configurar Variables de Entorno

#### Variables Globales
- **baseUrl**: URL base de tu aplicación (por defecto: `https://localhost:7001`)

#### Variables de Entorno (Opcional)
Puedes crear un entorno con las siguientes variables:
- `baseUrl`: URL de tu aplicación
- `userEmail`: Email de usuario para pruebas
- `userPassword`: Contraseña de usuario para pruebas
- `adminEmail`: Email de administrador para pruebas
- `adminPassword`: Contraseña de administrador para pruebas

### 3. Verificar la Aplicación

Antes de ejecutar las pruebas, asegúrate de que:
- Tu aplicación ASP.NET Core esté ejecutándose
- La base de datos esté configurada y accesible
- El puerto configurado en `baseUrl` sea correcto

## 📁 Estructura de la Colección

### 1. 🔐 Autenticación
Pruebas relacionadas con el sistema de autenticación y gestión de usuarios.

**Endpoints incluidos:**
- `POST /Home/Login` - Iniciar sesión
- `POST /Home/registroPasajero` - Registrar nuevo usuario
- `POST /Autenticacion/RecuperarContrasena` - Recuperar contraseña
- `GET /Home/CerrarSesion` - Cerrar sesión

### 2. 👨‍💼 Administración
Pruebas para funcionalidades administrativas del sistema.

**Endpoints incluidos:**
- `GET /Home/panelAdministrador` - Panel de administrador
- `POST /Home/CambiarRol` - Cambiar rol de usuario
- `POST /Home/EliminarUsuario` - Eliminar usuario
- `GET /Home/ObtenerUsuario` - Obtener datos de usuario
- `POST /Home/ActualizarUsuario` - Actualizar usuario
- `POST /Home/RegistrarUsuario` - Registrar usuario (admin)

### 3. ✈️ Gestión de Pilotos
Pruebas para la gestión completa de pilotos.

**Endpoints incluidos:**
- `GET /Home/ObtenerPilotos` - Listar todos los pilotos
- `POST /Home/RegistrarPiloto` - Registrar nuevo piloto
- `GET /Home/ObtenerPiloto` - Obtener piloto específico
- `PUT /Home/ActualizarPiloto/{id}` - Actualizar piloto
- `POST /Home/EliminarPiloto/{id}` - Eliminar piloto

### 4. 🛩️ Gestión de Flota
Pruebas para la gestión de la flota de aviones.

**Endpoints incluidos:**
- `GET /Home/ObtenerFlota` - Listar toda la flota
- `POST /Home/RegistrarAvion` - Registrar nuevo avión
- `GET /Home/ObtenerAvion` - Obtener avión específico
- `POST /Home/ActualizarAvion/{id}` - Actualizar avión
- `POST /Home/EliminarAvion/{id}` - Eliminar avión

### 5. 🎫 Gestión de Vuelos
Pruebas para la gestión completa de vuelos.

**Endpoints incluidos:**
- `GET /Home/ObtenerVuelos` - Listar todos los vuelos
- `POST /Home/RegistrarVuelo` - Registrar nuevo vuelo
- `GET /Home/ObtenerVuelo` - Obtener vuelo específico
- `POST /Home/ActualizarVuelo/{id}` - Actualizar vuelo
- `POST /Home/EliminarVuelo/{id}` - Eliminar vuelo
- `GET /Home/VuelosPorDestino` - Filtrar vuelos por destino
- `GET /Home/VuelosEconomicos` - Vuelos económicos
- `GET /Home/ObtenerDetallesVuelo` - Detalles de vuelo

### 6. 📅 Reservas
Pruebas para el sistema de reservas de vuelos.

**Endpoints incluidos:**
- `GET /Home/ReservarVuelo` - Formulario de reserva
- `POST /Home/ReservarVuelo` - Crear reserva
- `GET /Home/DetalleReserva` - Ver detalle de reserva
- `GET /Home/Reservas` - Listar reservas del usuario

### 7. 💬 Consultas y Soporte
Pruebas para el sistema de consultas y soporte al cliente.

**Endpoints incluidos:**
- `POST /Home/RegistrarConsulta` - Registrar nueva consulta
- `POST /Home/ActualizarEstadoConsulta` - Actualizar estado de consulta
- `POST /Home/EliminarConsulta` - Eliminar consulta

### 8. 👤 Perfil y Pagos
Pruebas para gestión de perfil y sistema de pagos.

**Endpoints incluidos:**
- `GET /Home/perfilPasajero` - Ver perfil de usuario
- `POST /Home/ActualizarPerfil` - Actualizar perfil
- `GET /Home/RealizarPago/{idReserva}` - Iniciar proceso de pago
- `GET /Home/PagoExitoso` - Pago exitoso
- `GET /Home/PagoCancelado` - Pago cancelado

### 9. 📄 Páginas Estáticas
Pruebas para páginas estáticas y navegación.

**Endpoints incluidos:**
- `GET /Home/Index` - Página principal
- `GET /Home/Destinos` - Página de destinos
- `GET /Home/Equipaje` - Información de equipaje
- `GET /Home/Servicios_al_cliente` - Servicios al cliente
- `GET /Home/Discapacidad` - Información de accesibilidad
- `GET /Home/Flota` - Información de la flota
- `GET /Home/Privacy` - Política de privacidad

## 🧪 Ejecución de Pruebas

### Ejecutar Pruebas Individuales
1. Selecciona la carpeta deseada
2. Haz clic en el endpoint que quieres probar
3. Verifica que los datos de prueba sean correctos
4. Haz clic en "Send"

### Ejecutar Colección Completa
1. Haz clic derecho en la colección "AeroLinea - Pruebas Funcionales"
2. Selecciona "Run collection"
3. Configura las opciones de ejecución:
   - **Iterations**: Número de veces que se ejecutará cada prueba
   - **Delay**: Tiempo de espera entre pruebas (recomendado: 1000ms)
4. Haz clic en "Run AeroLinea - Pruebas Funcionales"

### Ejecutar Carpeta Específica
1. Haz clic derecho en la carpeta deseada
2. Selecciona "Run folder"
3. Configura las opciones y ejecuta

## 📊 Datos de Prueba

### Usuarios de Prueba
```json
{
  "usuario_normal": {
    "email": "usuario@test.com",
    "password": "123456",
    "nombres": "Juan Carlos",
    "apellidos": "Pérez García"
  },
  "administrador": {
    "email": "admin@test.com",
    "password": "admin123",
    "nombres": "Admin Test",
    "apellidos": "Administrador"
  }
}
```

### Datos de Vuelo de Prueba
```json
{
  "origen": "Lima",
  "destino": "Cusco",
  "fecha": "2024-12-25T10:00:00",
  "precio": 250.00,
  "idAvion": 1,
  "idPiloto": 1
}
```

### Datos de Piloto de Prueba
```json
{
  "nombres": "Carlos Alberto",
  "apellidos": "Rodríguez López",
  "licencia": "PIL-2024-001",
  "experiencia": 5,
  "telefono": "987654321",
  "correo": "piloto@test.com"
}
```

### Datos de Avión de Prueba
```json
{
  "modelo": "Boeing 737",
  "capacidad": 150,
  "estado": "Activo",
  "tipo": "Comercial"
}
```

## 🔍 Validaciones Incluidas

### Validaciones de Respuesta
- **Códigos de Estado HTTP**: Verificación de respuestas 200, 201, 400, 401, 404, 500
- **Estructura de Respuesta**: Validación de formato JSON/HTML
- **Tiempo de Respuesta**: Verificación de que las respuestas sean rápidas (< 2000ms)

### Validaciones de Negocio
- **Autenticación**: Verificación de sesiones y permisos
- **Validación de Datos**: Verificación de reglas de negocio
- **Integridad de Datos**: Verificación de relaciones entre entidades

## 🚨 Casos de Prueba Especiales

### 1. Pruebas de Autenticación
- Login con credenciales válidas
- Login con credenciales inválidas
- Registro con datos duplicados
- Recuperación de contraseña

### 2. Pruebas de Autorización
- Acceso a funciones administrativas sin permisos
- Acceso a datos de otros usuarios
- Verificación de roles y permisos

### 3. Pruebas de Validación
- Datos requeridos faltantes
- Formatos de datos incorrectos
- Límites de caracteres
- Validaciones de negocio

### 4. Pruebas de Integración
- Creación de reserva completa
- Proceso de pago completo
- Gestión de consultas

## 📝 Notas Importantes

### Sesiones y Cookies
- La aplicación utiliza sesiones para mantener el estado del usuario
- Algunas pruebas requieren estar autenticado previamente
- Las cookies se manejan automáticamente por Postman

### Base de Datos
- Asegúrate de que la base de datos tenga datos de prueba
- Las pruebas pueden modificar datos existentes
- Considera usar una base de datos de prueba separada

### Dependencias
- Algunas pruebas dependen de datos creados en pruebas anteriores
- Ejecuta las pruebas en el orden recomendado para mejores resultados

## 🛠️ Solución de Problemas

### Error de Conexión
- Verifica que la aplicación esté ejecutándose
- Confirma que la URL base sea correcta
- Revisa los logs de la aplicación

### Errores de Autenticación
- Verifica que las credenciales sean correctas
- Asegúrate de que el usuario exista en la base de datos
- Revisa la configuración de sesiones

### Errores de Validación
- Verifica que los datos de prueba cumplan con las reglas de validación
- Revisa los mensajes de error en la respuesta
- Confirma que los campos requeridos estén presentes

## 📈 Métricas de Prueba

### Métricas Recomendadas
- **Tiempo de Respuesta Promedio**: < 2000ms
- **Tasa de Éxito**: > 95%
- **Cobertura de Funcionalidades**: 100%

### Reportes
Postman genera automáticamente reportes de ejecución que incluyen:
- Tiempo de ejecución total
- Tasa de éxito/fallo
- Detalles de errores
- Métricas de rendimiento

## 🔄 Mantenimiento

### Actualización de Datos de Prueba
- Actualiza los datos de prueba cuando cambien las reglas de negocio
- Mantén las credenciales de prueba actualizadas
- Revisa periódicamente la validez de los datos

### Actualización de Endpoints
- Verifica que las URLs de los endpoints no hayan cambiado
- Actualiza los parámetros cuando sea necesario
- Revisa los cambios en la estructura de respuesta

---

## 📞 Soporte

Para dudas o problemas con las pruebas:
1. Revisa los logs de la aplicación
2. Verifica la configuración de la base de datos
3. Confirma que todos los servicios estén ejecutándose
4. Consulta la documentación de la API

¡Feliz testing! 🚀 