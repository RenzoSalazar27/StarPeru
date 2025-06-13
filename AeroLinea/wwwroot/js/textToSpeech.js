let isVoiceEnabled = localStorage.getItem('voiceEnabled') === 'true';
const speechSynthesis = window.speechSynthesis;

// Función para hablar el texto
function speakText(text) {
    // Permitir que se reproduzca el mensaje de desactivación incluso si el modo está desactivado
    if (!isVoiceEnabled && text !== 'Lector de texto desactivado') return;
    
    // Cancelar cualquier síntesis de voz en curso
    speechSynthesis.cancel();

    const utterance = new SpeechSynthesisUtterance(text);
    utterance.lang = 'es-ES'; // Idioma español
    utterance.rate = 1.0; // Velocidad normal
    utterance.pitch = 1.0; // Tono normal
    utterance.volume = 1.0; // Volumen máximo

    speechSynthesis.speak(utterance);
}

// Función para manejar el hover sobre elementos de texto
function handleTextHover() {
    const textElements = document.querySelectorAll('p, h1, h2, h3, h4, h5, h6, a, span, li');
    
    textElements.forEach(element => {
        element.addEventListener('mouseenter', () => {
            if (isVoiceEnabled) {
                speakText(element.textContent.trim());
            }
        });
    });
}

// Función para alternar el modo voz
function toggleVoiceMode() {
    const voiceToggle = document.getElementById('voiceToggle');
    if (voiceToggle) {
        // Cancelar cualquier síntesis de voz en curso antes de cambiar el estado
        speechSynthesis.cancel();
        
        // Alternar el estado
        voiceToggle.checked = !voiceToggle.checked;
        isVoiceEnabled = voiceToggle.checked;
        localStorage.setItem('voiceEnabled', isVoiceEnabled);
        
        if (isVoiceEnabled) {
            handleTextHover();
            speakText('Lector de texto activado. Pase el cursor sobre el texto para escucharlo.');
        } else {
            // Asegurarse de que no quede ninguna síntesis de voz activa
            speechSynthesis.cancel();
            // Esperar un momento antes de decir que está desactivado
            setTimeout(() => {
                speakText('Lector de texto desactivado');
            }, 100);
        }
    }
}

// Inicializar cuando el documento esté listo
document.addEventListener('DOMContentLoaded', () => {
    const voiceToggle = document.getElementById('voiceToggle');
    
    // Establecer el estado inicial del switch
    voiceToggle.checked = isVoiceEnabled;
    
    // Si está activado, inicializar los eventos de hover
    if (isVoiceEnabled) {
        handleTextHover();
    }
    
    // Agregar evento para el atajo de teclado (Alt + V)
    document.addEventListener('keydown', (e) => {
        if (e.altKey && e.key.toLowerCase() === 'v') {
            e.preventDefault(); // Prevenir el comportamiento predeterminado
            toggleVoiceMode();
        }
    });
    
    voiceToggle.addEventListener('change', (e) => {
        // Cancelar cualquier síntesis de voz en curso antes de cambiar el estado
        speechSynthesis.cancel();
        
        isVoiceEnabled = e.target.checked;
        // Guardar el estado en localStorage
        localStorage.setItem('voiceEnabled', isVoiceEnabled);
        
        if (isVoiceEnabled) {
            handleTextHover();
            speakText('Lector de texto activado. Pase el cursor sobre el texto para escucharlo.');
        } else {
            // Asegurarse de que no quede ninguna síntesis de voz activa
            speechSynthesis.cancel();
            // Esperar un momento antes de decir que está desactivado
            setTimeout(() => {
                speakText('Lector de texto desactivado');
            }, 100);
        }
    });
}); 