// Navegación por voz
let isVoiceNavigationEnabled = localStorage.getItem('voiceNavEnabled') === 'true';
let recognition = null;
let isListening = false;
let lastSpokenTime = 0;
let silenceCount = 0;
let lastCommandTime = 0;
const COMMAND_COOLDOWN = 2000; // 2 segundos entre comandos

// Función para calcular la similitud entre dos cadenas
function calculateSimilarity(str1, str2) {
    str1 = str1.toLowerCase().trim();
    str2 = str2.toLowerCase().trim();
    
    if (str1 === str2) return 1;
    if (str1.includes(str2) || str2.includes(str1)) return 0.9;
    
    const words1 = str1.split(/\s+/);
    const words2 = str2.split(/\s+/);
    
    let matches = 0;
    for (const word1 of words1) {
        for (const word2 of words2) {
            if (word1 === word2) matches++;
        }
    }
    
    return matches / Math.max(words1.length, words2.length);
}

// Función para encontrar el elemento más similar al texto
function findMatchingElement(text) {
    const elements = document.querySelectorAll('a, button, input[type="submit"], [role="button"], [role="link"]');
    let bestMatch = null;
    let bestScore = 0.3;
    
    elements.forEach(element => {
        const elementText = element.textContent.trim() || element.value || element.placeholder || element.title || element.alt;
        if (!elementText) return;
        
        const score = calculateSimilarity(text, elementText);
        if (score > bestScore) {
            bestScore = score;
            bestMatch = element;
        }
    });
    
    return bestMatch;
}

// Función para hablar el texto
function speakText(text) {
    const speechSynthesis = window.speechSynthesis;
    speechSynthesis.cancel();
    
    const utterance = new SpeechSynthesisUtterance(text);
    utterance.lang = 'es-ES';
    utterance.rate = 1.0;
    utterance.pitch = 1.0;
    utterance.volume = 1.0;
    
    speechSynthesis.speak(utterance);
}

// Función para procesar el comando de voz
function processVoiceCommand(text) {
    const now = Date.now();
    if (now - lastCommandTime < COMMAND_COOLDOWN) return;
    lastCommandTime = now;
    
    // Verificar si el comando es "desactivar"
    if (text.toLowerCase().includes('desactivar')) {
        const voiceNavToggle = document.getElementById('voiceNavToggle');
        if (voiceNavToggle) {
            voiceNavToggle.checked = false;
            voiceNavToggle.dispatchEvent(new Event('change'));
        }
        return;
    }
    
    const element = findMatchingElement(text);
    
    if (element) {
        speakText(`Encontrado: ${element.textContent.trim()}`);
        element.click();
    } else {
        speakText('No se encontró ningún elemento similar');
    }
}

// Función para inicializar el reconocimiento de voz
function initVoiceRecognition() {
    if (!('webkitSpeechRecognition' in window)) {
        speakText('El reconocimiento de voz no está soportado en este navegador');
        return;
    }

    try {
        recognition = new webkitSpeechRecognition();
        recognition.continuous = true;
        recognition.interimResults = true;
        recognition.lang = 'es-ES';

        recognition.onstart = () => {
            isListening = true;
            silenceCount = 0;
            speakText('Escuchando');
        };

        recognition.onresult = (event) => {
            const result = event.results[event.results.length - 1];
            const text = result[0].transcript.trim();
            
            if (result.isFinal) {
                processVoiceCommand(text);
            }
        };

        recognition.onerror = (event) => {
            if (event.error === 'no-speech') {
                silenceCount++;
                if (silenceCount > 3) {
                    recognition.stop();
                    setTimeout(() => {
                        if (isVoiceNavigationEnabled) {
                            recognition.start();
                        }
                    }, 1000);
                }
            }
        };

        recognition.onend = () => {
            isListening = false;
            if (isVoiceNavigationEnabled) {
                setTimeout(() => {
                    recognition.start();
                }, 1000);
            }
        };

        recognition.start();
    } catch (error) {
        speakText('Error al inicializar el reconocimiento de voz');
    }
}

// Función para iniciar el reconocimiento inicial
function startInitialRecognition() {
    if (!('webkitSpeechRecognition' in window)) {
        return;
    }

    try {
        const initialRecognition = new webkitSpeechRecognition();
        initialRecognition.continuous = false;
        initialRecognition.interimResults = false;
        initialRecognition.lang = 'es-ES';

        initialRecognition.onresult = (event) => {
            const text = event.results[0][0].transcript.toLowerCase().trim();
            if (text.includes('activar')) {
                const voiceNavToggle = document.getElementById('voiceNavToggle');
                if (voiceNavToggle) {
                    voiceNavToggle.checked = true;
                    voiceNavToggle.dispatchEvent(new Event('change'));
                }
            }
        };

        initialRecognition.onend = () => {
            if (!isVoiceNavigationEnabled) {
                initialRecognition.start();
            }
        };

        initialRecognition.start();
    } catch (error) {
        console.error('Error al inicializar el reconocimiento inicial:', error);
    }
}

// Inicializar cuando el documento esté listo
document.addEventListener('DOMContentLoaded', () => {
    const voiceNavToggle = document.getElementById('voiceNavToggle');
    
    if (!voiceNavToggle) return;
    
    voiceNavToggle.checked = isVoiceNavigationEnabled;
    
    if (isVoiceNavigationEnabled) {
        initVoiceRecognition();
        speakText('Reconocimiento de voz activado');
    } else {
        startInitialRecognition();
    }
    
    voiceNavToggle.addEventListener('change', (e) => {
        isVoiceNavigationEnabled = e.target.checked;
        localStorage.setItem('voiceNavEnabled', isVoiceNavigationEnabled);
        
        if (isVoiceNavigationEnabled) {
            initVoiceRecognition();
            speakText('Reconocimiento de voz activado');
        } else {
            if (recognition) {
                recognition.stop();
            }
            speakText('Reconocimiento de voz desactivado');
            startInitialRecognition();
        }
    });
}); 