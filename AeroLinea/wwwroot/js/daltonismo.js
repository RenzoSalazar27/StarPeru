// Función para aplicar el modo de color
function applyColorMode(mode) {
    // Remover todas las clases de daltonismo
    document.documentElement.classList.remove('protanopia', 'deuteranopia', 'tritanopia', 'achromatopsia');
    
    // Aplicar la nueva clase
    if (mode !== 'normal') {
        document.documentElement.classList.add(mode);
    }
    
    // Guardar la preferencia
    localStorage.setItem('colorMode', mode);
}

// Cargar preferencias guardadas al iniciar
document.addEventListener('DOMContentLoaded', function() {
    const savedMode = localStorage.getItem('colorMode') || 'normal';
    const selector = document.getElementById('colorModeSelector');
    
    if (selector) {
        selector.value = savedMode;
        applyColorMode(savedMode);
        
        // Agregar evento de cambio
        selector.addEventListener('change', function() {
            applyColorMode(this.value);
        });
    }
}); 