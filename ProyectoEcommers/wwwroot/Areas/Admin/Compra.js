
if (PaginaPrincipal == 2) {
    ValidarDepartamento();
  
}

//function TotalPrecio() {
//    let Resultado = 0;
//    let Total = 0;
//    let PrecioEnvio = $("#IdCostoEnvio").html();
//    lista = JSON.parse(localStorage.getItem("ListaCarrito"));
//    $("#idtotalCarrito2 ul li").remove();
//    lista.forEach((item) => {
//        let sumar = item.precioTotal;
//        Resultado += sumar;
//    });
//    Total = Resultado + PrecioEnvio;


//    $("#idtotalCarrito2 ul").append(`<li>Subtotal <span>${ConvertirEnString(Resultado)}</span></li>
//     <li>Total <span>${ConvertirEnString(Total)}</span></li>`);
//    $("#IdCostoEnvio label").append(`Precio de Envío: <strong class="">${ConvertirEnString(PrecioEnvio)}</strong>`);


//}
function ValidarDepartamento() {
    /*   $("#selectDepartamento").empty();*/
    var pais = 10;
    var Id_envio = 0;
    if (pais == 10) {
        $("#idDepartamento1").css("display", "block");
        $("#idDepartamento2").css("display", "none");
        $("#IdCodigoPostal").css("display", "none");

    } else {
        $("#idDepartamento1").css("display", "none");
        $("#idDepartamento2").css("display", "block");
        $("#IdCodigoPostal").css("display", "block");
    }
    $.ajax({
        type: 'POST',
        url: UrlGetDepartamentoDominios,
        dataType: 'json',
        data: { IdDto: pais },
        success: function success(response) {
            $.each(response.data, function (i, dato) {
                $("#selectDepartamento").append('<option value="' + dato.idDominio + '">' + dato.descripcion + '</option>');
        /*        $("#selectDepartamento").append('<option value="' + dato.idDominio + '" data-extra="' + dato.costoEnvio + '">' + dato.descripcion + '</option>');*/
              
                if (pais == 10) {

                
                    $("#selectDepartamento").val(1227);
                    $("#selectDepartamento").trigger('change.select2');
                    $("#selectDepartamento").trigger("chosen:updated");



                }
                else {
                
                    $("#selectDepartamento").val(0);
                    $("#selectDepartamento").trigger('change.select2');
                    $("#selectDepartamento").trigger("chosen:updated");

                    $("#selectCiudad").val(0);
                    $("#selectCiudad").trigger('change.select2');
                    $("#selectCiudad").trigger("chosen:updated");
                }
       
              

            });
         
            if (pais == 10) {
                ciudad(1227);
            }
            $("#selectDepartamento").trigger("chosen:updated");
        },
        error: function error(ex) {
            Swal.fire({
                type: 'error',
                title: '¡Estimado(a) Cliente!',
                text: 'No es posible cargar los datos, revise'
            });
        }
    });
};
$("#IdPais").change(function () {
    $("#selectDepartamento").empty();
    var pais = $("#IdPais").val();
    CargarListaIndicativo(pais);
    if (pais == 10) {
        $("#idDepartamento1").css("display", "block");
        $("#idDepartamento2").css("display", "none");
        $("#IdCodigoPostal").css("display", "none");
        
    } else {
    
        $("#idDepartamento1").css("display", "none");
        $("#idDepartamento2").css("display", "block");
        $("#IdCodigoPostal").css("display", "block");
    }
    $.ajax({
        type: 'POST',
        url: UrlGetDepartamentoDominios,
        dataType: 'json',
        data: { IdDto: pais },
        success: function success(response) {
            $.each(response.data, function (i, dato) {
                $("#selectDepartamento").append('<option value="' + dato.idDominio + '">' + dato.descripcion + '</option>');

                if (pais == 10) {
                    $("#selectDepartamento").val(1227);
                    $("#selectDepartamento").trigger('change.select2');
                    $("#selectDepartamento").trigger("chosen:updated");



                }
                else {
                    $("#selectDepartamento").val(0);
                    $("#selectDepartamento").trigger('change.select2');
                    $("#selectDepartamento").trigger("chosen:updated");

                    $("#selectCiudad").val(0);
                    $("#selectCiudad").trigger('change.select2');
                    $("#selectCiudad").trigger("chosen:updated");
                }

            });
            if (pais == 10) {
                ciudad(1227);


            }
            else {
                ValidaPrecioEnvioPais(pais) 
            }
            $("#selectDepartamento").trigger("chosen:updated");
        },
        error: function error(ex) {
            Swal.fire({
                type: 'error',
                title: '¡Estimado(a) Cliente!',
                text: 'No es posible cargar los datos, revise'
            });
        }
    });
});


$("#selectCiudad").change(function () {
    var _idCiudad = $("#selectCiudad").val();
    ValidaPrecioEnvio(_idCiudad);
});




function ciudad(_idCiudad) {


    $("#selectCiudad").empty();
    $.ajax({
        type: 'POST',
        url: UrlGetDepartamentoDominios,
        dataType: 'json',
        data: { IdDto: _idCiudad },
        success: function success(response) {
            $.each(response.data, function (i, dato) {
                $("#selectCiudad").append('<option value="' + dato.idDominio + '">' + dato.descripcion + '</option>');



            });
            if (_idCiudad == 1227) {
                ValidaPrecioEnvio(1228);
                $("#selectCiudad").val(1228);
                $("#selectCiudad").trigger('change.select2');
                $("#selectCiudad").trigger("chosen:updated");
            } else {
                ValidaPrecioEnvio(_idCiudad);
            }

    
        },
        error: function error(ex) {
            Swal.fire({
                type: 'error',
                title: '¡Estimado(a) Cliente!',
                text: 'No es posible cargar los datos, revise'
            });
        }
    });
}
$("#selectDepartamento").change(function () {

    let departamento = $("#selectDepartamento").val();
    $("#selectCiudad").empty();
    $.ajax({
        type: 'POST',
        url: UrlGetDepartamentoDominios,
        dataType: 'json',
        data: { IdDto: departamento },
        success: function success(response) {
            $.each(response.data, function (i, dato) {
                $("#selectCiudad").append('<option value="' + dato.idDominio + '">' + dato.descripcion + '</option>');
            
            });

         
            ValidaPrecioEnvioDepartamento(departamento);
         
         
        },
        error: function error(ex) {
            Swal.fire({
                type: 'error',
                title: '¡Estimado(a) Cliente!',
                text: 'No es posible cargar los datos, revise'
            });
        }
    });
});





function ValidaPrecioEnvio(Ciudad) {
    $("#IdCostoEnvio label").html('');
    let idEnvio = 0;
    $.ajax({
        type: 'POST',
        url: UrlGetCostoEnvio,
        dataType: 'json',
        data: { IdDto: Ciudad },
        success: function success(response) {
            $.each(response.data, function (i, dato) {
              
                $("#IdCostoEnvio label").append(`Precio de Envío: <strong class="">${ConvertirEnString(dato.costoEnvio)}</strong>`);
                idEnvio = dato.costoEnvio;
            });
            TotalPrecioEnvio(idEnvio);
        },
        error: function error(ex) {
            Swal.fire({
                type: 'error',
                title: '¡Estimado(a) Cliente!',
                text: 'No es posible visualizar el departamento, revise'
            });
        }
    });
};
function ValidaPrecioEnvioDepartamento(Departamento) {
    $("#IdCostoEnvio label").html('');
    let idEnvio = 0;
    $.ajax({
        type: 'POST',
        url: UrlGetCostoEnvioDepartamento,
        dataType: 'json',
        data: { IdDto: Departamento },
        success: function success(respuesta) {  

            if (respuesta.success) {
                $("#IdCostoEnvio label").append(`Precio de Envío: <strong class="">${ConvertirEnString(respuesta.data.costoEnvio)}</strong>`);
                idEnvio = respuesta.data.costoEnvio;

                TotalPrecioEnvio(idEnvio);
            }
        },
        error: function error(ex) {
            Swal.fire({
                type: 'error',
                title: '¡Estimado(a) Cliente!',
                text: 'No es posible visualizar el departamento, revise'
            });
        }
    });
};


function ValidaPrecioEnvioPais(pais) {
    $("#IdCostoEnvio label").html('');
    let idEnvio = 0;
    $.ajax({
        type: 'POST',
        url: UrlGetCostoEnvioPais,
        dataType: 'json',
        data: { IdDto: pais },
        success: function success(respuesta) {

            if (respuesta.success) {
                $("#IdCostoEnvio label").append(`Precio de Envío: <strong class="">${ConvertirEnString(respuesta.data.costoEnvio)}</strong>`);
                idEnvio = respuesta.data.costoEnvio;

                TotalPrecioEnvio(idEnvio);
            }
        },
        error: function error(ex) {
            Swal.fire({
                type: 'error',
                title: '¡Estimado(a) Cliente!',
                text: 'No es posible visualizar el departamento, revise'
            });
        }
    });
};



function TotalPrecioEnvio(IdCostoEnvio) {
    let Resultado = 0;
    let Total = 0;
    let PrecioEnvio = IdCostoEnvio;
    lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    $("#idtotalCarrito2 ul li").remove();
    lista.forEach((item) => {
        let sumar = item.precioTotal;
        Resultado += sumar;
    });
    Total = Resultado + PrecioEnvio;


    $("#idtotalCarrito2 ul").append(`<li>Subtotal <span>${ConvertirEnString(Resultado)}</span></li>
     <li>Total <span>${ConvertirEnString(Total)}</span></li>`);
    /*  $("#IdCostoEnvio label").append(`Precio de Envío: <strong class="">${ConvertirEnString(PrecioEnvio)}</strong>`);*/


}
function limpiarCarrito() {


    $("#txtNombre").val("");
    $("#txtApellidos").val("");
    $("#txtNombreEmpresa").val("");
    $("#txtDireccion").val("");
    $("#txtoptionalDireccion").val("");
    $("#txtCodigoPostal").val("");
    $("#txtCelular").val("");
    $("#txtCorreoElectronico").val("");
    $("#txtCorreoElectronicoConfirmacion").val("");
    $("#txtComentario").val("");
    $("#txtDepartamento2").val("");
    $("#IdPais").val(0);
    $("#IdPais").trigger('change.select2');
    $("#IdPais").trigger("chosen:updated");
    $("#selectDepartamento").val(0);
    $("#selectDepartamento").trigger('change.select2');
    $("#selectDepartamento").trigger("chosen:updated");
    $("#selectCiudad").val(0);
    $("#selectCiudad").trigger('change.select2');
    $("#selectCiudad").trigger("chosen:updated");
    $("#IdtablaCarrito tbody tr").remove();
    $("#IdtablaCarrito2 tbody tr").remove();
    $("#IdListaCarrito").html('');
    localStorage.removeItem("ListaCarrito");
    listaProductosAgregados = [];
    $("#idtotalCarrito ul li").remove();
    $("#idtotalCarrito2 ul li").remove();
    $("#idNumero").html('0');
    $("#IdPaisIndicativo").val(0);
    $("#IdPaisIndicativo").trigger('change.select2');
    $("#IdPaisIndicativo").trigger("chosen:updated");

}
/*GetConsultapaisesDominios();*/
var listaPais = [];
//function GetConsultapaisesDominios() {

//    let data = {}
//    $.post(UrlGetPaisesDominiosX, data).done(function (result) {

//        if (result.data.length > 0) {
//            listaPais = result.data;
//            localStorage.removeItem("listaPaises");
//            localStorage.setItem("listaPaises", JSON.stringify(listaPais));
//        } else { }
//    }).fail();

//}

