// Funcionalidades de Accesibilidad

// Modo de Click Asistido
function toggleClickAssistMode() {
    document.body.classList.toggle('click-assist-mode');
    const isActive = document.body.classList.contains('click-assist-mode');
    localStorage.setItem('clickAssistMode', isActive);
}

// Ajuste de Tamaño de Fuente
function adjustFontSize(size) {
    const html = document.documentElement;
    const currentSize = parseFloat(getComputedStyle(html).fontSize);
    const newSize = currentSize + size;
    if (newSize >= 12 && newSize <= 24) {
        html.style.fontSize = newSize + 'px';
        localStorage.setItem('fontSize', newSize);
    }
}

// Navegación por Teclado
document.addEventListener('keydown', function(e) {
    // Alt + 1: Activar/Desactivar Modo Click Asistido
    if (e.altKey && e.key === '1') {
        toggleClickAssistMode();
    }
    // Alt + +: Aumentar Tamaño de Fuente
    if (e.altKey && e.key === '+') {
        adjustFontSize(1);
    }
    // Alt + -: Disminuir Tamaño de Fuente
    if (e.altKey && e.key === '-') {
        adjustFontSize(-1);
    }
});

// Inicialización
document.addEventListener('DOMContentLoaded', function() {
    // Restaurar preferencias guardadas
    const clickAssistMode = localStorage.getItem('clickAssistMode') === 'true';
    const fontSize = localStorage.getItem('fontSize');
    
    if (clickAssistMode) {
        document.body.classList.add('click-assist-mode');
    }
    
    if (fontSize) {
        document.documentElement.style.fontSize = fontSize + 'px';
    }

    // Agregar botón de Skip to Main Content
    const skipLink = document.createElement('a');
    skipLink.href = '#main-content';
    skipLink.className = 'skip-to-main';
    skipLink.textContent = 'Saltar al contenido principal';
    document.body.insertBefore(skipLink, document.body.firstChild);

    // Agregar ID al contenido principal
    const mainContent = document.querySelector('main');
    if (mainContent) {
        mainContent.id = 'main-content';
    }
});

// Mejoras para Formularios
document.querySelectorAll('form').forEach(form => {
    form.addEventListener('submit', function(e) {
        const requiredFields = form.querySelectorAll('[required]');
        let isValid = true;

        requiredFields.forEach(field => {
            if (!field.value.trim()) {
                isValid = false;
                field.classList.add('is-invalid');
            } else {
                field.classList.remove('is-invalid');
            }
        });

        if (!isValid) {
            e.preventDefault();
            alert('Por favor, complete todos los campos requeridos.');
        }
    });
});

// Mejoras para Focus
document.querySelectorAll('a, button, input, select, textarea').forEach(element => {
    element.addEventListener('focus', function() {
        this.scrollIntoView({ behavior: 'smooth', block: 'center' });
    });
}); 