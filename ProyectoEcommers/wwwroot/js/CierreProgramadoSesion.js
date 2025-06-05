// Script para finalizar la sesión del usuario y redireccionar al login

// 60 Segundos * 15 minutos * 1000 milisegundos
var tiempoExpiraSesion = 60 * 15 * 1000;
var tiempo = setInterval('FinalizaSesion()', tiempoExpiraSesion);
console.log(tiempo);

$('body').mousemove(function () {
    clearInterval(tiempo);
    tiempo = setInterval('FinalizaSesion()', tiempoExpiraSesion);
});

function FinalizaSesion() {
    window.location = redireccionarLogin;
}