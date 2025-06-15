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

    // Comando para escribir en un campo
    if (text.toLowerCase().startsWith('escribir')) {
        const parts = text.split(' en ');
        if (parts.length === 2) {
            const texto = parts[0].replace('escribir', '').trim();
            const nombreCampo = parts[1].trim();
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

    // Comando para seleccionar de una lista desplegable
    if (text.toLowerCase().startsWith('seleccionar')) {
        const parts = text.split(' en ');
        if (parts.length === 2) {
            const opcion = parts[0].replace('seleccionar', '').trim();
            const nombreLista = parts[1].trim();
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

    // Comando para borrar un campo
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

    // Comandos de navegación
    if (text.toLowerCase().includes('ir abajo')) {
        window.scrollBy(0, 300);
        speakText('Desplazando hacia abajo');
        return;
    }

    if (text.toLowerCase().includes('ir arriba')) {
        window.scrollBy(0, -300);
        speakText('Desplazando hacia arriba');
        return;
    }

    if (text.toLowerCase().includes('ir más abajo')) {
        window.scrollTo(0, document.body.scrollHeight);
        speakText('Desplazando hasta el final de la página');
        return;
    }

    if (text.toLowerCase().includes('ir más arriba')) {
        window.scrollTo(0, 0);
        speakText('Desplazando hasta el inicio de la página');
        return;
    }

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
    
    const element = findMatchingElement(text);
    
    if (element) {
        speakText(`Encontrado: ${element.textContent.trim()}`);
        element.click();
    } else {
        speakText('No se encontró ningún elemento similar');
    }
}

// Función para encontrar un campo de entrada por su nombre
function findInputField(nombre) {
    const inputs = document.querySelectorAll('input, textarea');
    for (const input of inputs) {
        const label = input.previousElementSibling;
        const placeholder = input.placeholder;
        const id = input.id;
        const name = input.name;
        
        if (label && label.textContent.toLowerCase().includes(nombre.toLowerCase())) return input;
        if (placeholder && placeholder.toLowerCase().includes(nombre.toLowerCase())) return input;
        if (id && id.toLowerCase().includes(nombre.toLowerCase())) return input;
        if (name && name.toLowerCase().includes(nombre.toLowerCase())) return input;
    }
    return null;
}

// Función para encontrar una lista desplegable por su nombre
function findSelectField(nombre) {
    const selects = document.querySelectorAll('select');
    for (const select of selects) {
        const label = select.previousElementSibling;
        const id = select.id;
        const name = select.name;
        
        if (label && label.textContent.toLowerCase().includes(nombre.toLowerCase())) return select;
        if (id && id.toLowerCase().includes(nombre.toLowerCase())) return select;
        if (name && name.toLowerCase().includes(nombre.toLowerCase())) return select;
    }
    return null;
}

// Función para encontrar una opción en una lista desplegable
function findOptionInSelect(select, opcion) {
    const options = select.options;
    for (const option of options) {
        if (option.text.toLowerCase().includes(opcion.toLowerCase())) {
            return option;
        }
    }
    return null;
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