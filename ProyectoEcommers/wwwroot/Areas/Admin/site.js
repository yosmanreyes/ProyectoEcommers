// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var objLadda;

$(document).ajaxStart(function () {
    $("#Process").show();
});
$(document).ajaxStop(function () {
    $("#Process").hide();

});

function ModalExitoso(titulo, texto) {
    $("#TituloMensaje").html("<h3><img src='/img/AlertSucess.png' width='50' height='50' />&nbsp" + titulo + "</h3>");
    $("#ContenidoCambio").html("<div class='alert alert-success' role='alert' style='text-align: justify; font-size:20px;'><span class='fas fa-info'></span>&nbsp" + texto + "</div>");
    $('#Modal_Transacciones').modal("show");
}
function ModalError(titulo, texto) {
    $("#TituloMensaje").html("<h3><img src='/img/AlertError.png' width='50' height='50' />&nbsp" + titulo + "</h3>");
    $("#ContenidoCambio").html("<div class='alert alert-danger' role='alert' style='text-align: justify; font-size:20px;'><span class='fas fa-info'></span>&nbsp" + texto + "</div>");
    $('#Modal_Transacciones').modal("show");
}

function create(Type, Mensaje, Icon) {
    VanillaToasts.create({
        title: 'Estimado(a) Cliente...',
        text: Mensaje,
        type: Type,
        icon: Icon,
        timeout: '6000'
    });
}

