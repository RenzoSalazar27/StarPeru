// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    // Manejar el clic en el botón hamburguesa
    $('.hamburger').click(function() {
        $('#menuOptions').collapse('toggle');
    });

    // Cerrar el menú al hacer clic en un enlace
    $('#menuOptions .nav-link').click(function() {
        $('#menuOptions').collapse('hide');
    });
});
