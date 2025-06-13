// Navegación por voz
let isVoiceNavigationEnabled = false;
let recognition = null;
let isListening = false;
let lastMessageTime = 0;
let hasSaidListening = false;
let silenceCount = 0;
let lastCommandTime = 0;
let isSpeaking = false;
const COMMAND_COOLDOWN = 2000; // 2 segundos entre comandos

// Función para detener la voz
function stopSpeaking() {
    if (window.speechSynthesis) {
        window.speechSynthesis.cancel();
        isSpeaking = false;
    }
}

// Verificar permisos del micrófono
async function checkMicrophonePermission() {
    try {
        const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
        stream.getTracks().forEach(track => track.stop());
        console.log('Permiso de micrófono concedido');
        return true;
    } catch (error) {
        console.error('Error al solicitar permiso del micrófono:', error);
        return false;
    }
}

// Función para hablar
function speakText(text, force = false) {
    console.log('Intentando hablar:', text);
    if (!isVoiceNavigationEnabled && !force) return;
    
    const now = Date.now();
    if (now - lastMessageTime < 1000 && !force) return;
    
    stopSpeaking();
    
    if (window.speechSynthesis) {
        const utterance = new SpeechSynthesisUtterance(text);
        utterance.lang = 'es-ES';
        utterance.rate = 1.0;
        utterance.pitch = 1.0;
        utterance.volume = 1.0;
        
        isSpeaking = true;
        utterance.onend = () => {
            isSpeaking = false;
            lastMessageTime = Date.now();
        };
        
        window.speechSynthesis.speak(utterance);
    }
}

// Inicializar el reconocimiento de voz
async function initVoiceRecognition() {
    console.log('Iniciando reconocimiento de voz...');
    
    // Verificar soporte del navegador
    if (!('webkitSpeechRecognition' in window)) {
        console.error('El reconocimiento de voz no está soportado en este navegador');
        speakText('El reconocimiento de voz no está soportado en este navegador', true);
        return false;
    }

    // Verificar permisos del micrófono
    const hasPermission = await checkMicrophonePermission();
    if (!hasPermission) {
        console.error('No se pudo obtener permiso del micrófono');
        speakText('Por favor, permite el acceso al micrófono para usar la navegación por voz', true);
        return false;
    }

    try {
        // Crear nueva instancia de reconocimiento
        recognition = new webkitSpeechRecognition();
        recognition.continuous = true;
        recognition.interimResults = true;
        recognition.lang = 'es-ES';
        recognition.maxAlternatives = 1;

        // Evento cuando inicia el reconocimiento
        recognition.onstart = function() {
            console.log('Reconocimiento de voz iniciado');
            isListening = true;
            if (!hasSaidListening) {
                speakText('Escuchando...', true);
                hasSaidListening = true;
            }
        };

        // Evento cuando se detecta voz
        recognition.onresult = function(event) {
            console.log('Resultado de voz detectado');
            const result = event.results[event.results.length - 1];
            const command = result[0].transcript.toLowerCase();
            console.log('Comando detectado:', command);
            
            if (result.isFinal) {
                const now = Date.now();
                if (now - lastCommandTime < COMMAND_COOLDOWN) {
                    console.log('Ignorando comando por cooldown');
                    return;
                }
                
                lastCommandTime = now;
                silenceCount = 0;
                processVoiceCommand(command);
            }
        };

        // Evento de error
        recognition.onerror = function(event) {
            console.error('Error en el reconocimiento de voz:', event.error);
            isListening = false;
            
            switch(event.error) {
                case 'no-speech':
                    console.log('No se detectó voz');
                    silenceCount++;
                    if (silenceCount > 5) {
                        speakText('Di ayuda para ver los comandos disponibles', true);
                        silenceCount = 0;
                    }
                    break;
                case 'audio-capture':
                    speakText('No se pudo acceder al micrófono. Por favor, verifica que esté conectado y funcionando', true);
                    break;
                case 'not-allowed':
                    speakText('Permiso de micrófono denegado. Por favor, permite el acceso al micrófono', true);
                    break;
                default:
                    console.error('Error no manejado:', event.error);
            }
        };

        // Evento cuando termina el reconocimiento
        recognition.onend = function() {
            console.log('Reconocimiento de voz finalizado');
            isListening = false;
            
            if (isVoiceNavigationEnabled) {
                console.log('Reiniciando reconocimiento de voz...');
                setTimeout(() => {
                    if (isVoiceNavigationEnabled && !isListening) {
                        recognition.start();
                    }
                }, 1000);
            }
        };

        // Iniciar reconocimiento
        console.log('Iniciando reconocimiento...');
        recognition.start();
        return true;
    } catch (error) {
        console.error('Error al inicializar el reconocimiento de voz:', error);
        speakText('Error al inicializar el reconocimiento de voz', true);
        return false;
    }
}

// Procesar comandos de voz
function processVoiceCommand(command) {
    console.log('Procesando comando:', command);
    
    // Normalizar el comando
    command = command.toLowerCase().trim();
    
    // Si el comando está vacío o es muy corto, ignorarlo silenciosamente
    if (!command || command.length < 2) {
        console.log('Comando ignorado por ser muy corto o vacío');
        return;
    }

    // Verificar si el comando es válido antes de procesarlo
    const validCommands = [
        'iniciar sesion', 'destinos', 'consultas', 'flota', 'reservas',
        'abrir menu', 'cerrar menu', 'ayuda', 'cerrar', 'salir'
    ];

    // Si el comando no es válido, ignorarlo silenciosamente
    if (!validCommands.some(cmd => command.includes(cmd))) {
        console.log('Comando no válido:', command);
        return;
    }

    // Función para navegar
    function navigateTo(path) {
        console.log('Navegando a:', path);
        speakText('Navegando...', true);
        setTimeout(() => {
            window.location.href = path;
        }, 1000);
    }

    // Función para manejar el menú
    function toggleMenu(shouldOpen) {
        const menuButton = document.querySelector('.navbar-toggler');
        const menu = document.querySelector('.dropdown-menu');
        
        if (menuButton && menu) {
            if (shouldOpen && !menu.classList.contains('show')) {
                menuButton.click();
                speakText('Abriendo menú', true);
            } else if (!shouldOpen && menu.classList.contains('show')) {
                menuButton.click();
                speakText('Cerrando menú', true);
            }
        }
    }

    // Procesar comandos específicos
    if (command.includes('iniciar sesion')) {
        navigateTo('/Autenticacion/Autenticacion');
    } else if (command.includes('destinos')) {
        navigateTo('/Home/Destinos');
    } else if (command.includes('servicio')) {
        navigateTo('/Home/Servicios_al_cliente');
    } else if (command.includes('flota')) {
        navigateTo('/Home/Flota');
    } else if (command.includes('reservas')) {
        navigateTo('/Home/Reservas');
    } else if (command.includes('abrir menu')) {
        toggleMenu(true);
    } else if (command.includes('cerrar menu')) {
        toggleMenu(false);
    } else if (command.includes('ayuda')) {
        speakText('Comandos disponibles: iniciar sesion, destinos, servicio, flota, reservas, abrir menu, cerrar menu, ayuda, cerrar, salir', true);
    } else if (command.includes('cerrar') || command.includes('salir')) {
        speakText('Cerrando navegación por voz', true);
        setTimeout(() => {
            isVoiceNavigationEnabled = false;
            if (recognition) {
                recognition.stop();
            }
            document.getElementById('voiceNavToggle').checked = false;
        }, 1000);
    }
}

// Inicializar cuando el documento esté listo
document.addEventListener('DOMContentLoaded', function() {
    console.log('Documento cargado, inicializando navegación por voz...');
    const voiceNavToggle = document.getElementById('voiceNavToggle');
    
    if (!voiceNavToggle) {
        console.error('No se encontró el elemento voiceNavToggle');
        return;
    }

    voiceNavToggle.addEventListener('change', async function() {
        console.log('Cambio en el switch de navegación por voz:', this.checked);
        
        if (this.checked) {
            isVoiceNavigationEnabled = true;
            hasSaidListening = false;
            const success = await initVoiceRecognition();
            
            if (!success) {
                console.error('No se pudo inicializar el reconocimiento de voz');
                this.checked = false;
                isVoiceNavigationEnabled = false;
            }
        } else {
            isVoiceNavigationEnabled = false;
            if (recognition) {
                recognition.stop();
            }
            stopSpeaking();
        }
    });
}); 