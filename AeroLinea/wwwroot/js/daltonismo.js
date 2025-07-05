// Funcionalidad para Daltonismo
document.addEventListener('DOMContentLoaded', () => {
    const daltonismToggle = document.getElementById('daltonismToggle');
    const daltonismDropdown = document.getElementById('daltonismDropdown');
    const daltonismOptionsDropdown = document.getElementById('daltonismOptionsDropdown');
    const daltonismMenu = document.getElementById('daltonismMenu');
    
    // Estado actual del daltonismo
    let currentDaltonism = localStorage.getItem('daltonismType') || 'default';
    let isDaltonismEnabled = localStorage.getItem('daltonismEnabled') === 'true';
    let isDropdownOpen = false;
    
    console.log('Inicializando daltonismo...');
    console.log('Elementos encontrados:', {
        toggle: daltonismToggle,
        dropdown: daltonismDropdown,
        button: daltonismOptionsDropdown,
        menu: daltonismMenu
    });
    
    // Función para mostrar/ocultar el dropdown de opciones
    function toggleDaltonismDropdown(show) {
        if (daltonismDropdown) {
            if (show) {
                // Mostrar usando clase CSS
                daltonismDropdown.classList.add('show');
                daltonismDropdown.style.display = 'block';
                console.log('Dropdown de daltonismo mostrado');
            } else {
                // Ocultar usando clase CSS
                daltonismDropdown.classList.remove('show');
                daltonismDropdown.style.display = 'none';
                // Cerrar también el menú desplegable si está abierto
                forceCloseDropdown();
                console.log('Dropdown de daltonismo ocultado');
            }
        }
    }
    
    // Establecer estado inicial
    if (daltonismToggle) {
        daltonismToggle.checked = isDaltonismEnabled;
        // Mostrar/ocultar el dropdown según el estado inicial
        toggleDaltonismDropdown(isDaltonismEnabled);
        
        if (isDaltonismEnabled) {
            updateDropdownText(currentDaltonism);
        }
    }
    
    // Función para actualizar el texto del dropdown
    function updateDropdownText(daltonismType) {
        if (daltonismOptionsDropdown) {
            const typeNames = {
                'default': 'Por defecto',
                'protanopia': 'Protanopía',
                'protanomaly': 'Protanomalía',
                'deuteranopia': 'Deuteranopía',
                'deuteranomaly': 'Deuteranomalía',
                'tritanopia': 'Tritanopía',
                'tritanomaly': 'Tritanomalía',
                'achromatopsia': 'Acromatopsia'
            };
            daltonismOptionsDropdown.textContent = typeNames[daltonismType] || 'Seleccionar tipo';
        }
    }
    
    // Función para aplicar filtro de daltonismo
    function applyDaltonismFilter(daltonismType) {
        console.log('Aplicando filtro de daltonismo:', daltonismType);
        
        // Remover todas las clases de daltonismo del body
        document.body.classList.remove(
            'daltonismo-protanopia',
            'daltonismo-protanomaly',
            'daltonismo-deuteranopia',
            'daltonismo-deuteranomaly',
            'daltonismo-tritanopia',
            'daltonismo-tritanomaly',
            'daltonismo-achromatopsia',
            'daltonismo-default'
        );
        
        // Aplicar la clase correspondiente
        if (daltonismType !== 'default') {
            document.body.classList.add(`daltonismo-${daltonismType}`);
        }
        
        // Guardar en localStorage
        localStorage.setItem('daltonismType', daltonismType);
        currentDaltonism = daltonismType;
        
        // Actualizar texto del dropdown
        updateDropdownText(daltonismType);
        
        // Cerrar el dropdown inmediatamente
        forceCloseDropdown();
    }
    
    // Función para forzar el cierre del dropdown - MÁS AGRESIVA
    function forceCloseDropdown() {
        console.log('Forzando cierre del dropdown...');
        
        if (daltonismMenu) {
            // Múltiples métodos para asegurar el cierre
            daltonismMenu.style.display = 'none';
            daltonismMenu.style.visibility = 'hidden';
            daltonismMenu.style.opacity = '0';
            daltonismMenu.style.pointerEvents = 'none';
            daltonismMenu.classList.remove('show');
            daltonismMenu.classList.add('closed');
            
            // Remover cualquier atributo de Bootstrap
            daltonismMenu.removeAttribute('data-bs-popper');
            daltonismMenu.removeAttribute('style');
            daltonismMenu.setAttribute('style', 'display: none !important; visibility: hidden !important; opacity: 0 !important; pointer-events: none !important;');
            
            isDropdownOpen = false;
            console.log('Dropdown cerrado forzadamente');
        }
        
        // También cerrar cualquier dropdown de Bootstrap que pueda estar abierto
        const allDropdowns = document.querySelectorAll('.dropdown-menu.show');
        allDropdowns.forEach(dropdown => {
            if (dropdown !== daltonismMenu) {
                dropdown.classList.remove('show');
                dropdown.style.display = 'none';
            }
        });
    }
    
    // Función para abrir el dropdown
    function openDropdown() {
        console.log('Abriendo dropdown...');
        
        if (daltonismMenu) {
            // Cerrar primero cualquier otro dropdown
            forceCloseDropdown();
            
            // Abrir el dropdown
            daltonismMenu.style.display = 'block';
            daltonismMenu.style.visibility = 'visible';
            daltonismMenu.style.opacity = '1';
            daltonismMenu.style.pointerEvents = 'auto';
            daltonismMenu.classList.add('show');
            daltonismMenu.classList.remove('closed');
            
            isDropdownOpen = true;
            console.log('Dropdown abierto');
        }
    }
    
    // Función para abrir/cerrar el dropdown
    function toggleDropdown() {
        console.log('Toggle dropdown, estado actual:', isDropdownOpen);
        
        if (isDropdownOpen) {
            forceCloseDropdown();
        } else {
            openDropdown();
        }
    }
    
    // Event listener para el toggle principal
    if (daltonismToggle) {
        daltonismToggle.addEventListener('change', (e) => {
            isDaltonismEnabled = e.target.checked;
            localStorage.setItem('daltonismEnabled', isDaltonismEnabled);
            
            console.log('Toggle de daltonismo cambiado:', isDaltonismEnabled);
            
            if (isDaltonismEnabled) {
                // Mostrar el dropdown de opciones
                toggleDaltonismDropdown(true);
                // Aplicar el filtro actual si hay uno seleccionado
                if (currentDaltonism !== 'default') {
                    applyDaltonismFilter(currentDaltonism);
                }
            } else {
                // Ocultar el dropdown de opciones
                toggleDaltonismDropdown(false);
                // Restaurar a por defecto cuando se desactiva
                applyDaltonismFilter('default');
            }
        });
    }
    
    // Event listener para el botón del dropdown
    if (daltonismOptionsDropdown) {
        daltonismOptionsDropdown.addEventListener('click', (e) => {
            e.preventDefault();
            e.stopPropagation();
            console.log('Botón del dropdown clickeado');
            toggleDropdown();
        });
    }
    
    // Event listeners para las opciones del dropdown
    if (daltonismMenu) {
        daltonismMenu.addEventListener('click', (e) => {
            if (e.target.classList.contains('dropdown-item') && e.target.hasAttribute('data-daltonism')) {
                e.preventDefault();
                e.stopPropagation();
                
                const daltonismType = e.target.getAttribute('data-daltonism');
                console.log('Opción seleccionada:', daltonismType);
                
                // Aplicar el filtro
                applyDaltonismFilter(daltonismType);
                
                // Cerrar el dropdown inmediatamente
                forceCloseDropdown();
                
                // Doble verificación después de un pequeño delay
                setTimeout(() => {
                    forceCloseDropdown();
                }, 50);
            }
        });
    }
    
    // Cerrar dropdown al hacer click fuera
    document.addEventListener('click', (e) => {
        if (!isDropdownOpen) return;
        
        const isClickOnButton = daltonismOptionsDropdown && daltonismOptionsDropdown.contains(e.target);
        const isClickInMenu = daltonismMenu && daltonismMenu.contains(e.target);
        
        if (!isClickOnButton && !isClickInMenu) {
            console.log('Click fuera del dropdown, cerrando...');
            forceCloseDropdown();
        }
    });
    
    // Cerrar dropdown con la tecla Escape
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape' && isDropdownOpen) {
            console.log('Tecla Escape presionada, cerrando dropdown...');
            forceCloseDropdown();
        }
    });
    
    // Aplicar filtro inicial si está habilitado
    if (isDaltonismEnabled && currentDaltonism !== 'default') {
        applyDaltonismFilter(currentDaltonism);
    } else if (isDaltonismEnabled) {
        // Si está habilitado pero es default, aplicar la clase default
        document.body.classList.add('daltonismo-default');
    }
    
    // Asegurar que el dropdown esté cerrado al inicio
    setTimeout(() => {
        forceCloseDropdown();
        // Asegurar que el contenedor principal esté oculto si no está activado
        if (!isDaltonismEnabled && daltonismDropdown) {
            daltonismDropdown.classList.remove('show');
            daltonismDropdown.style.display = 'none';
        }
    }, 100);
    
    console.log('Daltonismo inicializado correctamente');
}); 