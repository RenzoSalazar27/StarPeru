let isVoiceEnabled = localStorage.getItem('voiceEnabled') === 'true';
const speechSynthesis = window.speechSynthesis;

// Función para hablar el texto
function speakText(text) {
    if (!isVoiceEnabled) return;
    
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

// Inicializar cuando el documento esté listo
document.addEventListener('DOMContentLoaded', () => {
    const voiceToggle = document.getElementById('voiceToggle');
    
    // Establecer el estado inicial del switch
    voiceToggle.checked = isVoiceEnabled;
    
    // Si está activado, inicializar los eventos de hover
    if (isVoiceEnabled) {
        handleTextHover();
    }
    
    voiceToggle.addEventListener('change', (e) => {
        isVoiceEnabled = e.target.checked;
        // Guardar el estado en localStorage
        localStorage.setItem('voiceEnabled', isVoiceEnabled);
        
        if (isVoiceEnabled) {
            handleTextHover();
        } else {
            speechSynthesis.cancel();
        }
    });
}); 