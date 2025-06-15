// Navegación por voz
let isVoiceNavigationEnabled = localStorage.getItem('voiceNavEnabled') === 'true';
let recognition = null;
let isListening = false;
let lastCommandTime = 0;
const COMMAND_COOLDOWN = 2000; // 2 segundos entre comandos

// Funciones de utilidad
function calculateSimilarity(str1, str2) {
    str1 = str1.toLowerCase().trim();
    str2 = str2.toLowerCase().trim();
    if (str1 === str2) return 1;
    if (str1.includes(str2) || str2.includes(str1)) return 0.9;
    
    const words1 = str1.split(/\s+/);
    const words2 = str2.split(/\s+/);
    return words1.filter(word1 => words2.includes(word1)).length / Math.max(words1.length, words2.length);
}

function speakText(text) {
    const speech = window.speechSynthesis;
    speech.cancel();
    const utterance = new SpeechSynthesisUtterance(text);
    utterance.lang = 'es-ES';
    speech.speak(utterance);
}

// Funciones de búsqueda de elementos
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

function findInputField(nombre) {
    const inputs = document.querySelectorAll('input, textarea');
    return Array.from(inputs).find(input => {
        const label = input.previousElementSibling;
        return (label && label.textContent.toLowerCase().includes(nombre.toLowerCase())) ||
               (input.placeholder && input.placeholder.toLowerCase().includes(nombre.toLowerCase())) ||
               (input.id && input.id.toLowerCase().includes(nombre.toLowerCase())) ||
               (input.name && input.name.toLowerCase().includes(nombre.toLowerCase()));
    });
}

function findSelectField(nombre) {
    const selects = document.querySelectorAll('select');
    return Array.from(selects).find(select => {
        const label = select.previousElementSibling;
        return (label && label.textContent.toLowerCase().includes(nombre.toLowerCase())) ||
               (select.id && select.id.toLowerCase().includes(nombre.toLowerCase())) ||
               (select.name && select.name.toLowerCase().includes(nombre.toLowerCase()));
    });
}

function findOptionInSelect(select, opcion) {
    return Array.from(select.options).find(option => 
        option.text.toLowerCase().includes(opcion.toLowerCase())
    );
}

// Procesamiento de comandos
function processVoiceCommand(text) {
    const now = Date.now();
    if (now - lastCommandTime < COMMAND_COOLDOWN) return;
    lastCommandTime = now;

    // Comandos de navegación
    const navigationCommands = {
        'desactivar': () => {
            const voiceNavToggle = document.getElementById('voiceNavToggle');
            if (voiceNavToggle) {
                voiceNavToggle.checked = false;
                voiceNavToggle.dispatchEvent(new Event('change'));
            }
        },
        'ir abajo': () => {
            window.scrollBy(0, 300);
            speakText('Desplazando hacia abajo');
        },
        'ir arriba': () => {
            window.scrollBy(0, -300);
            speakText('Desplazando hacia arriba');
        },
        'ir más abajo': () => {
            window.scrollTo(0, document.body.scrollHeight);
            speakText('Desplazando hasta el final de la página');
        },
        'ir más arriba': () => {
            window.scrollTo(0, 0);
            speakText('Desplazando hasta el inicio de la página');
        }
    };

    // Verificar comandos de navegación
    for (const [command, action] of Object.entries(navigationCommands)) {
        if (text.toLowerCase().includes(command)) {
            action();
            return;
        }
    }

    // Comando "ir a sección"
    if (text.toLowerCase().startsWith('ir a')) {
        const seccion = text.replace('ir a', '').trim();
        const elementos = document.querySelectorAll('h1, h2, h3, h4, h5, h6, [role="heading"]');
        for (const elemento of elementos) {
            if (elemento.textContent.toLowerCase().includes(seccion.toLowerCase())) {
                elemento.scrollIntoView({ behavior: 'smooth' });
                speakText(`Navegando a ${seccion}`);
                return;
            }
        }
        speakText(`No se encontró la sección ${seccion}`);
        return;
    }

    // Comando escribir
    if (text.toLowerCase().startsWith('escribir')) {
        const [_, texto, nombreCampo] = text.match(/escribir\s+(.*?)\s+en\s+(.*)/i) || [];
        if (texto && nombreCampo) {
            const input = findInputField(nombreCampo);
            if (input) {
                input.value = texto;
                input.dispatchEvent(new Event('input'));
                speakText(`Texto escrito en ${nombreCampo}`);
            } else {
                speakText(`No se encontró el campo ${nombreCampo}`);
            }
            return;
        }
    }

    // Comando seleccionar
    if (text.toLowerCase().startsWith('seleccionar')) {
        const [_, opcion, nombreLista] = text.match(/seleccionar\s+(.*?)\s+en\s+(.*)/i) || [];
        if (opcion && nombreLista) {
            const select = findSelectField(nombreLista);
            if (select) {
                const option = findOptionInSelect(select, opcion);
                if (option) {
                    select.value = option.value;
                    select.dispatchEvent(new Event('change'));
                    speakText(`Seleccionado ${opcion} en ${nombreLista}`);
                } else {
                    speakText(`No se encontró la opción ${opcion} en ${nombreLista}`);
                }
            } else {
                speakText(`No se encontró la lista ${nombreLista}`);
            }
            return;
        }
    }

    // Comando borrar campo
    if (text.toLowerCase().startsWith('borrar campo')) {
        const nombreCampo = text.replace('borrar campo', '').trim();
        const input = findInputField(nombreCampo);
        if (input) {
            input.value = '';
            input.dispatchEvent(new Event('input'));
            speakText(`Campo ${nombreCampo} borrado`);
        } else {
            speakText(`No se encontró el campo ${nombreCampo}`);
        }
        return;
    }

    // Búsqueda general de elementos
    const element = findMatchingElement(text);
    if (element) {
        speakText(`Encontrado: ${element.textContent.trim()}`);
        element.click();
    } else {
        speakText('No se encontró ningún elemento similar');
    }
}

// Inicialización del reconocimiento de voz
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
            speakText('Escuchando');
        };

        recognition.onresult = (event) => {
            const result = event.results[event.results.length - 1];
            if (result.isFinal) {
                processVoiceCommand(result[0].transcript.trim());
            }
        };

        recognition.onerror = (event) => {
            if (event.error === 'no-speech' && isVoiceNavigationEnabled) {
                recognition.stop();
                setTimeout(() => recognition.start(), 1000);
            }
        };

        recognition.onend = () => {
            isListening = false;
            if (isVoiceNavigationEnabled) {
                setTimeout(() => recognition.start(), 1000);
            }
        };

        recognition.start();
    } catch (error) {
        speakText('Error al inicializar el reconocimiento de voz');
    }
}

// Inicialización inicial
function startInitialRecognition() {
    if (!('webkitSpeechRecognition' in window)) return;

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

// Inicialización cuando el documento esté listo
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
            if (recognition) recognition.stop();
            speakText('Reconocimiento de voz desactivado');
            startInitialRecognition();
        }
    });
}); 