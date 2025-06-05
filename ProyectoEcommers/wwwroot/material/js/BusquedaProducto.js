

//$("#consultaInput").autocomplete({
//    source: function (request, response) {
//        var consulta = document.getElementById("consultaInput").value;
//        $.ajax({
//            url: UrlSearch,
//            type: "POST",
//            dataType: "json",
//            data: { search: consulta },
//            success: function (data) {
//                response($.map(data, function (item) {
//                    return { label: item.MARCA_ID, value: item.CATEGORIA_ID };
//                }));
//            },
//            error: function (xhr, status, error) {
//                alert("Error")
//            }
//        });
//    },
//    minLength: 10,
//    select: function (event, ui) {
//        MARCA_ID = a.MARCA_ID,
//            CATEGORIA_ID = a.CATEGORIA_ID,
//            $(document.getElementById("MARCA_ID")).val(ui.item.label);
//        $('#MARCA_ID').val(ui.item.value);
//    /*    BuscaFuncionarioVer(ui.item.value);*/
//        return false;
//    }
//});

$("#consultaInput").keypress(function (e) {
    if (e.which == 13) {
        $('#GetidBuscarProducto').click();
    }
});

$("#consultaInput6").keypress(function (e) {
    if (e.which == 13) {
        $('#GetidBuscarProducto6').click();
    }
});
$("#consultaInput2").keypress(function (e) {
    if (e.which == 13) {
        $('#GetidBuscarProducto2').click();
    }
});




function realizarConsulta() {

//    if (PaginaPrincipal != 1) {
//        let data = {};
//;
//        $.post(urlRedireccionaInicio, data).done(function (reult) {

//            return;

//        })
  

//    }

    $("#PanelMarcas1").val(0);
    $("#PanelMarcas1").trigger('change.select2');
    $("#PanelMarcas1").trigger("chosen:updated");
    $("#PanelMarcas1").attr("disabled", false);

    $("#PanelCategoria1").val(0);
    $("#PanelCategoria1").trigger('change.select2');
    $("#PanelCategoria1").trigger("chosen:updated");
    $("#PanelCategoria1").attr("disabled", false);

    var consulta = document.getElementById("consultaInput").value;

    let data = { search: consulta }
    $.post(UrlSearch, data).done(function (result) {

        if (result.data.length > 0) {

            lis = result.data;
            fetchProducts(lis);
            mostrarResultado("");


            const seccionDestino = document.getElementById('seccionDestino');
            const posicionDestino = seccionDestino.offsetTop;

            //const ajuste = 1295; // Ajuste manual en píxeles
            //const nuevaPosicionDestino = posicionDestino + ajuste;
            //seccionDestino.scrollIntoView({ behavior: 'smooth' });
            window.scrollTo({
                top: posicionDestino,
                behavior: 'smooth'
            });
           
        }
        else {
            $("#progrese").css("display", "none");
            ConsultaProductosGeral(0, 0);
            var consulta = document.getElementById("consultaInput").value;
            mostrarResultado("No se encontraron productos con el nombre: " + consulta);
            create('error', 'Los productos seleccionados no se encuentran en Stock, por favor selecione otra Marca o Categoria, Gracias...', UrlAlertError);
        }

    }).fail();
}

function mostrarResultado(resultado) {
    document.getElementById("resultado").innerText = resultado;
}
function mostrarResultado6(resultado) {
    document.getElementById("resultado6").innerText = resultado;
}

function realizarConsulta2() {

    $("#PanelMarcas1").val(0);
    $("#PanelMarcas1").trigger('change.select2');
    $("#PanelMarcas1").trigger("chosen:updated");
    $("#PanelMarcas1").attr("disabled", false);

    $("#PanelCategoria1").val(0);
    $("#PanelCategoria1").trigger('change.select2');
    $("#PanelCategoria1").trigger("chosen:updated");
    $("#PanelCategoria1").attr("disabled", false);
    var consulta = document.getElementById("consultaInput2").value;

    let data = { search: consulta }
    $.post(UrlSearch, data).done(function (result) {

        if (result.data.length > 0) {

            lis = result.data;
            fetchProducts(lis);
            mostrarResultado2("");
            $("#offcanvas__close-btn").click();    

            const seccionDestino = document.getElementById('seccionDestino');
            const posicionDestino = seccionDestino.offsetTop;

            //const ajuste = 1295; // Ajuste manual en píxeles
            //const nuevaPosicionDestino = posicionDestino + ajuste;
            //seccionDestino.scrollIntoView({ behavior: 'smooth' });
            window.scrollTo({
                top: posicionDestino,
                behavior: 'smooth'
            });



        }
        else {
            $("#progrese").css("display", "none");
            ConsultaProductosGeral(0, 0);
            var consulta = document.getElementById("consultaInput2").value;
            mostrarResultado2("No se encontraron productos con el nombre: " + consulta);
            create('error', 'Los productos seleccionados no se encuentran en Stock, por favor selecione otra Marca o Categoria, Gracias...', UrlAlertError);
        }

    }).fail();
}

function mostrarResultado2(resultado) {
    document.getElementById("resultado2").innerText = resultado;
}

function realizarConsulta3() {
    $("#PanelMarcas1").val(0);
    $("#PanelMarcas1").trigger('change.select2');
    $("#PanelMarcas1").trigger("chosen:updated");
    $("#PanelMarcas1").attr("disabled", false);

    $("#PanelCategoria1").val(0);
    $("#PanelCategoria1").trigger('change.select2');
    $("#PanelCategoria1").trigger("chosen:updated");
    $("#PanelCategoria1").attr("disabled", false);
    var consulta = "COMBOS";

    let data = { search: consulta }
    $.post(UrlSearch, data).done(function (result) {

        if (result.data.length > 0) {

            lis = result.data;
            fetchProducts(lis);
          

            const seccionDestino = document.getElementById('seccionDestino');
            const posicionDestino = seccionDestino.offsetTop;
            window.scrollTo({
                top: posicionDestino,
                behavior: 'smooth'
            });



        }
        else {
            $("#progrese").css("display", "none");
            ConsultaProductosGeral(0, 0);
           /* create('error', 'Los productos seleccionados no se encuentran en Stock, por favor selecione otra Marca o Categoria, Gracias...', UrlAlertError);*/
        }

    }).fail();
}

function realizarConsulta4() {
    $("#PanelMarcas1").val(0);
    $("#PanelMarcas1").trigger('change.select2');
    $("#PanelMarcas1").trigger("chosen:updated");
    $("#PanelMarcas1").attr("disabled", false);

    $("#PanelCategoria1").val(0);
    $("#PanelCategoria1").trigger('change.select2');
    $("#PanelCategoria1").trigger("chosen:updated");
    $("#PanelCategoria1").attr("disabled", false);
    var consulta = "COMBOS";

    let data = { search: consulta }
    $.post(UrlSearch, data).done(function (result) {

        if (result.data.length > 0) {

            lis = result.data;
            fetchProducts(lis);
       

            const seccionDestino = document.getElementById('seccionDestino');
            const posicionDestino = seccionDestino.offsetTop;
            window.scrollTo({
                top: posicionDestino,
                behavior: 'smooth'
            });



        }
        else {
            $("#progrese").css("display", "none");
            ConsultaProductosGeral(0, 0);
          /*  create('error', 'No hay combos o kit disponibles en el momento, Gracias...', UrlAlertError);*/
        }

    }).fail();
}

function realizarConsulta5() {
    $("#PanelMarcas1").val(0);
    $("#PanelMarcas1").trigger('change.select2');
    $("#PanelMarcas1").trigger("chosen:updated");
    $("#PanelMarcas1").attr("disabled", false);

    $("#PanelCategoria1").val(0);
    $("#PanelCategoria1").trigger('change.select2');
    $("#PanelCategoria1").trigger("chosen:updated");
    $("#PanelCategoria1").attr("disabled", false);
    var consulta = "COMBOS";

    let data = { search: consulta }
    $.post(UrlSearch, data).done(function (result) {

        if (result.data.length > 0) {

            lis = result.data;
            fetchProducts(lis);


            const seccionDestino = document.getElementById('seccionDestino');
            const posicionDestino = seccionDestino.offsetTop;
            window.scrollTo({
                top: posicionDestino,
                behavior: 'smooth'
            });



        }
        else {
            $("#progrese").css("display", "none");
            ConsultaProductosGeral(0, 0);
            /*  create('error', 'No hay combos o kit disponibles en el momento, Gracias...', UrlAlertError);*/
        }

    }).fail();
}

function realizarConsulta6() {

    //    if (PaginaPrincipal != 1) {
    //        let data = {};
    //;
    //        $.post(urlRedireccionaInicio, data).done(function (reult) {

    //            return;

    //        })


    //    }

    $("#PanelMarcas1").val(0);
    $("#PanelMarcas1").trigger('change.select2');
    $("#PanelMarcas1").trigger("chosen:updated");
    $("#PanelMarcas1").attr("disabled", false);

    $("#PanelCategoria1").val(0);
    $("#PanelCategoria1").trigger('change.select2');
    $("#PanelCategoria1").trigger("chosen:updated");
    $("#PanelCategoria1").attr("disabled", false);

    var consulta = document.getElementById("consultaInput6").value;

    let data = { search: consulta }
    $.post(UrlSearch, data).done(function (result) {

        if (result.data.length > 0) {

            lis = result.data;
            fetchProducts(lis);
            mostrarResultado6("");



        }
        else {
            $("#progrese").css("display", "none");
            ConsultaProductosGeral(0, 0);
            var consulta = document.getElementById("consultaInput6").value;
            mostrarResultado6("No se encontraron productos con el nombre: " + consulta);
            create('error', 'Los productos seleccionados no se encuentran en Stock, por favor selecione otra Marca o Categoria, Gracias...', UrlAlertError);
        }

    }).fail();
}

//document.getElementById('botonScroll').addEventListener('click', function () {
//    // Obtener la posición de la sección destino
//    const seccionDestino = document.getElementById('seccionDestino');
//    const posicionDestino = seccionDestino.offsetTop;

//    // Hacer scroll suave hacia la sección destino

//});


function validarCampoConsulta() {
    var valorCampo = document.getElementById("consultaInput").value;

    if (valorCampo === "") {
  
        ConsultaProductosGeral(0, 0);
        mostrarResultado("");
        mostrarResultado2("");
/*        alert("El campo ya está vacío.");*/
    }
}


function validarCampoConsulta2() {
    var valorCampo2 = document.getElementById("consultaInput2").value;

    if (valorCampo2 === "") {
        ConsultaProductosGeral(0, 0);
        mostrarResultado("");
        mostrarResultado2("");
        /*        alert("El campo ya está vacío.");*/
    }
}


