
/* OPCIONES PARA LA RESERVA */
function showFlightOptions() {
    const panel = document.getElementById('infoPanel');
    panel.innerHTML = `
                        <h5 class="mb-0">Selecciona una opción de vuelo:</h5>
                    `;
    const buttonContainer = document.getElementById('buttonContainer');
    buttonContainer.innerHTML = `
                        
                    `;
}

function updatePanel(option) {
    const panel = document.getElementById('infoPanel');
    const buttonContainer = document.getElementById('buttonContainer');

    switch (option) {
        case 'Web Check-In':
            panel.innerHTML = '<h5 class="mb-0">Explora nuestros destinos más populares!</h5>';
            buttonContainer.innerHTML = `
                                <button class="btn btn-custom mx-2" onclick="resetButtons()">Regresar</button>
                            `;
            break;
        case 'Pagar Reserva':
            panel.innerHTML = '<h5 class="mb-0">Conoce nuestras últimas novedades!</h5>';
            buttonContainer.innerHTML = `
                                <button class="btn btn-custom mx-2" onclick="resetButtons()">Regresar</button>
                            `;
            break;
        case 'E-ticket':
            panel.innerHTML = '<h5 class="mb-0">Contáctanos para más información!</h5>';
            buttonContainer.innerHTML = `
                                <button class="btn btn-custom mx-2" onclick="resetButtons()">Regresar</button>
                            `;
            break;
        case 'Ida y Vuelta':
            panel.innerHTML = '<h5 class="mb-0">Información sobre Ida y Vuelta.</h5>';
            break;
        case 'Vuelta':
            panel.innerHTML = '<h5 class="mb-0">Información sobre Vuelta.</h5>';
            break;
    }
}

function resetButtons() {
    const panel = document.getElementById('infoPanel');
    const buttonContainer = document.getElementById('buttonContainer');
    panel.innerHTML = '<h5 class="mb-0">¡Descubre nuestras mejores ofertas y destinos!</h5>';
    buttonContainer.innerHTML = `
                        <button class="btn btn-custom mx-2" onclick="showFlightOptions()">Vuela</button>
                        <button class="btn btn-custom mx-2" onclick="updatePanel('Web Check-In')">Web Check-In</button>
                        <button class="btn btn-custom mx-2" onclick="updatePanel('Pagar Reserva')">Pagar Reserva</button>
                        <button class="btn btn-custom mx-2" onclick="updatePanel('E-ticket')">E-ticket</button>
                    `;
}