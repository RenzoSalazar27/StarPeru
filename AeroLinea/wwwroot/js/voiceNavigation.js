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
    
    // Si son exactamente iguales
    if (str1 === str2) return 1;
    
    // Si uno contiene al otro
    if (str1.includes(str2) || str2.includes(str1)) return 0.9;
    
    // Calcular similitud por palabras
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
    let bestScore = 0.3; // Umbral mínimo de similitud
    
    elements.forEach(element => {
        // Obtener texto del elemento
        const elementText = element.textContent.trim() || element.value || element.placeholder || element.title || element.alt;
        if (!elementText) return;
        
        // Calcular similitud
        const score = calculateSimilarity(text, elementText);
        
        // Si es la mejor coincidencia hasta ahora
        if (score > bestScore) {
            bestScore = score;
            bestMatch = element;
        }
    });
    
    return bestMatch;
}

// Función para hablar el texto
function speakText(text) {
    if (!isVoiceNavigationEnabled) return;
    
    const now = Date.now();
    if (now - lastSpokenTime < 1000) return;
    lastSpokenTime = now;
    
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
    
    // Buscar elemento coincidente
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
    console.log('Iniciando reconocimiento de voz...');
    
    if (!('webkitSpeechRecognition' in window)) {
        console.error('El reconocimiento de voz no está soportado en este navegador');
        speakText('El reconocimiento de voz no está soportado en este navegador');
        return;
    }

    try {
        recognition = new webkitSpeechRecognition();
        recognition.continuous = true;
        recognition.interimResults = true;
        recognition.lang = 'es-ES';

        recognition.onstart = () => {
            console.log('Reconocimiento de voz iniciado');
            isListening = true;
            silenceCount = 0;
            speakText('Escuchando');
        };

        recognition.onresult = (event) => {
            console.log('Resultado de voz detectado');
            const result = event.results[event.results.length - 1];
            const text = result[0].transcript.trim();
            console.log('Texto detectado:', text);
            
            if (result.isFinal) {
                processVoiceCommand(text);
            }
        };

        recognition.onerror = (event) => {
            console.error('Error en el reconocimiento de voz:', event.error);
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
            console.log('Reconocimiento de voz finalizado');
            isListening = false;
            if (isVoiceNavigationEnabled) {
                console.log('Reiniciando reconocimiento de voz...');
                setTimeout(() => {
                    recognition.start();
                }, 1000);
            }
        };

        // Iniciar reconocimiento
        recognition.start();
        console.log('Reconocimiento de voz iniciado correctamente');
    } catch (error) {
        console.error('Error al inicializar el reconocimiento de voz:', error);
        speakText('Error al inicializar el reconocimiento de voz');
    }
}

// Inicializar cuando el documento esté listo
document.addEventListener('DOMContentLoaded', () => {
    console.log('Documento cargado, inicializando navegación por voz...');
    const voiceNavToggle = document.getElementById('voiceNavToggle');
    
    if (!voiceNavToggle) {
        console.error('No se encontró el elemento voiceNavToggle');
        return;
    }
    
    // Establecer el estado inicial del switch
    voiceNavToggle.checked = isVoiceNavigationEnabled;
    
    // Si está activado, inicializar el reconocimiento
    if (isVoiceNavigationEnabled) {
        initVoiceRecognition();
    }
    
    voiceNavToggle.addEventListener('change', (e) => {
        console.log('Cambio en el switch de navegación por voz:', e.target.checked);
        isVoiceNavigationEnabled = e.target.checked;
        // Guardar el estado en localStorage
        localStorage.setItem('voiceNavEnabled', isVoiceNavigationEnabled);
        
        if (isVoiceNavigationEnabled) {
            initVoiceRecognition();
            speakText('Navegación por voz activada. Di cualquier texto para encontrar y activar elementos similares.');
        } else {
            if (recognition) {
                recognition.stop();
            }
            speakText('Navegación por voz desactivada');
        }
    });
}); 