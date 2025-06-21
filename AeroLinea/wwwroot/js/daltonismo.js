// Funcionalidad para Daltonismo
document.addEventListener('DOMContentLoaded', () => {
    const daltonismToggle = document.getElementById('daltonismToggle');
    const daltonismDropdown = document.getElementById('daltonismDropdown');
    const daltonismOptionsDropdown = document.getElementById('daltonismOptionsDropdown');
    
    // Estado actual del daltonismo
    let currentDaltonism = localStorage.getItem('daltonismType') || 'default';
    let isDaltonismEnabled = localStorage.getItem('daltonismEnabled') === 'true';
    let isDropdownOpen = false;
    
    // Establecer estado inicial
    if (daltonismToggle) {
        daltonismToggle.checked = isDaltonismEnabled;
        if (isDaltonismEnabled) {
            daltonismDropdown.style.display = 'block';
            updateDropdownText(currentDaltonism);
        }
    }
    
    // Función para actualizar el texto del dropdown
    function updateDropdownText(daltonismType) {
        const dropdownButton = document.getElementById('daltonismOptionsDropdown');
        if (dropdownButton) {
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
            dropdownButton.textContent = typeNames[daltonismType] || 'Seleccionar tipo';
        }
    }
    
    // Función para aplicar filtro de daltonismo
    function applyDaltonismFilter(daltonismType) {
        console.log('Tipo de daltonismo seleccionado:', daltonismType);
        
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
        
        // Cerrar el dropdown después de la selección
        closeDropdown();
    }
    
    // Función para abrir/cerrar el dropdown manualmente
    function toggleDropdown() {
        const dropdownMenu = document.getElementById('daltonismMenu');
        if (dropdownMenu) {
            if (isDropdownOpen) {
                dropdownMenu.style.display = 'none';
                isDropdownOpen = false;
            } else {
                dropdownMenu.style.display = 'block';
                isDropdownOpen = true;
            }
        }
    }
    
    function closeDropdown() {
        const dropdownMenu = document.getElementById('daltonismMenu');
        if (dropdownMenu) {
            dropdownMenu.style.display = 'none';
            isDropdownOpen = false;
        }
    }
    
    // Event listener para el toggle principal
    if (daltonismToggle) {
        daltonismToggle.addEventListener('change', (e) => {
            isDaltonismEnabled = e.target.checked;
            localStorage.setItem('daltonismEnabled', isDaltonismEnabled);
            
            if (isDaltonismEnabled) {
                daltonismDropdown.style.display = 'block';
                // Aplicar el filtro actual si hay uno seleccionado
                if (currentDaltonism !== 'default') {
                    applyDaltonismFilter(currentDaltonism);
                }
            } else {
                daltonismDropdown.style.display = 'none';
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
            toggleDropdown();
        });
    }
    
    // Event listeners para las opciones del dropdown
    document.addEventListener('click', (e) => {
        if (e.target.classList.contains('dropdown-item') && e.target.hasAttribute('data-daltonism')) {
            e.preventDefault();
            e.stopPropagation();
            const daltonismType = e.target.getAttribute('data-daltonism');
            applyDaltonismFilter(daltonismType);
        }
    });
    
    // Cerrar dropdown al hacer click fuera
    document.addEventListener('click', (e) => {
        if (!daltonismOptionsDropdown.contains(e.target) && !e.target.classList.contains('dropdown-item')) {
            closeDropdown();
        }
    });
    
    // Aplicar filtro inicial si está habilitado
    if (isDaltonismEnabled && currentDaltonism !== 'default') {
        applyDaltonismFilter(currentDaltonism);
    } else if (isDaltonismEnabled) {
        // Si está habilitado pero es default, aplicar la clase default
        document.body.classList.add('daltonismo-default');
    }
    
    // Debug: Verificar que los elementos existen
    console.log('Daltonism toggle:', daltonismToggle);
    console.log('Daltonism dropdown:', daltonismDropdown);
    console.log('Daltonism options dropdown:', daltonismOptionsDropdown);
}); 