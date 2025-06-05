//document.addEventListener('DOMContentLoaded', function () {
//    const optionsList = document.querySelector('.options-listPersonalizado');

//    optionsList.addEventListener('change', function () {
//        let marca = $("#PanelMarcas1").val();
//        let categoria = $("#PanelCategoria1").val();
//        ConsultaProductosGeral(marca, categoria);
//    });


//    //const optionsList = document.querySelector('.options-listPersonalizado');

//    //optionsList.addEventListener('change', function () {
//    //    let marca = $("#PanelMarcas2").val();
//    //    let categoria = $("#PanelCategoria2").val();
//    //    ConsultaProductosGeral(marca, categoria);

//    //});

//    //const optionsList3 = document.querySelector('.options-listPersonalizado3');

//    //optionsList3.addEventListener('change', function () {
//    //    let marca = $("#PanelMarcas3").val();
//    //    let categoria = $("#PanelCategoria3").val();
//    //    ConsultaProductosGeral(marca, categoria);

//    //});
//});
function RedirecionaConsulta() {
    const seccionDestino = document.getElementById('seccionDestino');
    const posicionDestino = seccionDestino.offsetTop;
    window.scrollTo({
        top: posicionDestino,
        behavior: 'smooth'
    });
}



$("#PanelMarcas1").change(function () {

    $("#consultaInput").val("");
    $("#consultaInput2").val("");
    let objeto = $("#PanelMarcas1");
    let texto = $('#PanelMarcas1 option:selected').html()

    objeto.parent()[0].parentNode.style.width = (texto.length * 8) + "px";
    objeto.parent()[0].style.width = (texto.length * 8) + "px";
    objeto[0].style.width = (texto.length * 9) + "px";

    let marca = $("#PanelMarcas1").val();
    let categoria = $("#PanelCategoria1").val();

    var nombre = ConsultaProductosGeral(marca, categoria);

    if (nombre === true) {
        RedirecionaConsulta();
    } else {      
    }
 
});

$("#PanelCategoria1").change(function () {

    $("#consultaInput").val("");
    $("#consultaInput2").val("");
    let objeto = $("#PanelCategoria1");
    let texto = $('#PanelCategoria1 option:selected').html()

    objeto.parent()[0].parentNode.style.width = (texto.length * 8) + "px";
    objeto.parent()[0].style.width = (texto.length * 8) + "px";
    objeto[0].style.width = (texto.length * 9) + "px";

    let marca = $("#PanelMarcas1").val();
    let categoria = $("#PanelCategoria1").val();
    ConsultaProductosGeral(marca, categoria);
});



$("#PanelMarcas2").change(function () {

    $("#consultaInput").val("");
    $("#consultaInput2").val("");
    let marca = $("#PanelMarcas2").val();
    let categoria = $("#PanelCategoria2").val();
    ConsultaProductosGeral(marca, categoria);
   
});
$("#PanelCategoria2").change(function () {

    $("#consultaInput").val("");
    $("#consultaInput2").val("");
    let marca = $("#PanelMarcas2").val();
    let categoria = $("#PanelCategoria2").val();
    ConsultaProductosGeral(marca, categoria);
});


$("#PanelMarcas3").change(function () {

    $("#consultaInput").val("");
    $("#consultaInput2").val("");
    let marca = $("#PanelMarcas3").val();
    let categoria = $("#PanelCategoria3").val();
    ConsultaProductosGeral(marca, categoria);
});


$("#PanelCategoria3").change(function () {

    $("#consultaInput").val("");
    $("#consultaInput2").val("");
    let marca = $("#PanelMarcas3").val();
    let categoria = $("#PanelCategoria3").val();
    ConsultaProductosGeral(marca, categoria);
});


// Función para ajustar dinámicamente el tamaño del select
//function ajustarTamañoSelectM() {
//    var select = document.getElementById('PanelMarcas1');
//    var longitudSeleccionada = select.options[select.selectedIndex].text.length + 4;

//    // Ajustar el tamaño del select según la longitud del texto seleccionado
//    select.style.width = Math.max(Math.min(longitudSeleccionada * 10, 300), 100) + 'px';
//}

// Función para ajustar dinámicamente el tamaño del select
//function ajustarTamañoSelectC() {
//    var select = document.getElementById('PanelCategoria1');
//    var longitudSeleccionada = select.options[select.selectedIndex].text.length + 4;

//    // Ajustar el tamaño del select según la longitud del texto seleccionado
//    select.style.width = Math.max(Math.min(longitudSeleccionada * 10, 300), 100) + 'px';
//}

// Llamar a la función al cargar la página y en eventos que puedan cambiar la selección del select
//document.addEventListener('DOMContentLoaded', ajustarTamañoSelect);
//document.getElementById('miSelect').addEventListener('change', ajustarTamañoSelect);

