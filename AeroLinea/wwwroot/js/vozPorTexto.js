document.addEventListener('DOMContentLoaded', () => {
    const voiceToggle = document.getElementById('voiceToggle');
    let isVoiceEnabled = localStorage.getItem('voiceEnabled') === 'true';
    const speech = window.speechSynthesis;

    // Establecer el estado inicial del switch
    if (voiceToggle) {
        voiceToggle.checked = isVoiceEnabled;
        if (isVoiceEnabled) {
            handleTextHover();
        }
    }

    function speakText(text) {
        const utterance = new SpeechSynthesisUtterance(text);
        utterance.lang = 'es-ES';
        speech.speak(utterance);
    }

    function handleTextHover() {
        const textElements = document.querySelectorAll('p, h1, h2, h3, h4, h5, h6, span, a, button, label');
        
        textElements.forEach(element => {
            element.addEventListener('mouseenter', () => {
                if (isVoiceEnabled && element.textContent.trim()) {
                    speakText(element.textContent.trim());
                }
            });
        });
    }

    if (voiceToggle) {
        voiceToggle.addEventListener('change', (e) => {
            isVoiceEnabled = e.target.checked;
            localStorage.setItem('voiceEnabled', isVoiceEnabled);
            
            if (isVoiceEnabled) {
                handleTextHover();
                speakText('Lector por texto activado, pon el puntero encima del texto para leerlo');
            } else {
                speech.cancel();
                speakText('Lector por texto desactivado');
            }
        });

        // Atajo de teclado Alt + V
        document.addEventListener('keydown', (e) => {
            if (e.altKey && e.key === 'v') {
                e.preventDefault();
                voiceToggle.checked = !voiceToggle.checked;
                voiceToggle.dispatchEvent(new Event('change'));
            }
        });
    }
}); 