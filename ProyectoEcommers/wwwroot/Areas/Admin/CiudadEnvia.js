

$(document).ready(function () {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbGrilla")) {
        $("#tbGrilla").off().DataTable().destroy();
    }
    GetCiudades(0);
  
});

function ConvertirEnString(valor) {// recibe numero float y lo devuelve en formato con puntos y comas y el signo $

    if (valor - Math.trunc(valor) > 0)
        valor = valor.toFixed(2);

    valor = "$ " + parseFloat(valor).toLocaleString('es-CO');
    return valor;
}



//---------------------------------


$("#btnGrabar").click(() => {
    Ins_Marca();
});

function Ins_Marca() {

    var marca = $("#txtMarca").val();
    if (marca == null || marca == "") {
        Swal.fire({
            title: 'Error',
            text: "Por favor registre la ciudad o pais a modificar...",
            type: 'error',
            showCancelButton: false,
            confirmButtonColor: '#032b57',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
        /*   create('error', 'Por favor, registre Marca a Modificar o Crear...', UrlAlertError);*/
        return;
    }
    var _idProducto = 0;
    var valida = $("#txtCodigo").val();
    if (valida == null || valida == "") {
        _idProducto == 0;
    }
    else {
        _idProducto = valida;
    }


    var _Descripcion = $("#txtMarca").val();
    var _Abreviatura = $("#txtAbreviatura").val();
    var _Tipo =$("#txtTipo").val();
    var _Zona = $("#txtZona").val();
    var _LugeCodigo =  $("#txtLuge").val();
    var _Vigente = $("#txtVigente").val();
    var _Indicativo = $("#txtIndicativo").val();
    var _CostoEnvio = $("#txtCostoEnvio").val();


    Swal.fire({
        type: 'warning',
        title: '¡Atención!',
        text: "¿Esta seguro de de guardar la marca?",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then((result) => {
        if (result.value) {

            let data = new FormData();
            data.append('CODIGO', _idProducto);
            data.append('DESCRIPCION', _Descripcion);
            data.append('ABREVIATURA', _Abreviatura);
            data.append('TIPO', _Tipo);
            data.append('ZONA', _Zona);
            data.append('LUGE_CODIGO', _LugeCodigo);
            data.append('VIGENTE', _Vigente);
            data.append('INDICATIVO', _Indicativo);
            data.append('COSTO_ENVIO', _CostoEnvio);


            $.ajax({
                type: 'POST',
                url: UrlAddCiudad,
                dataType: 'json',
                data: data,
                cache: false,
                contentType: false,
                processData: false,
                enctype: 'multipart/form-data',
                success: function (response) {
                    if (response.success == true) {
                        GetCiudades(0);
                        Swal.fire({
                            type: 'success',
                            title: '¡Transacción exitosa!',
                            text: "La pais/ciudad fué guardada exitosamente",
                            showCancelButton: false,
                            confirmButtonText: "¡Adelante!",
                        }).then((result) => {
                            /*  window.location.reload();*/
                        });
                    }
                    else {
                        Swal.fire({
                            type: 'warning',
                            title: 'Señor(a) Usuario (a:)',
                            text: response.message
                        });
                    }
                },
                error: function (ex) {

                    Swal.fire({
                        title: 'Error',
                        text: "Verique Los campos a Insertar",
                        type: 'error',
                        showCancelButton: false,
                        confirmButtonColor: '#032b57',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Aceptar'
                    });
                }
            });
        }
    });

}





//Consulta Grilla Producto
function GetCiudades(_numnero) {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbGrilla")) {
        $("#tbGrilla").off().DataTable().destroy();
    }

    $("#tbGrilla").DataTable({
        "ajax": {
            type: "POST",
            url: UrlConsultaCiudadEnvia,
           async: true,
            datatype: "json",
            cache: false,
            data: { numero: _numnero }
        },
        "initComplete": function (settings, json) {
            if (json.success) {

            }
            else {
                Swal.fire({
                    type: 'warning',
                    title: '¡Estimado(a) Cliente!',
                    text: "No existe informacion del Departamento/Estado"
                });
            }
        },
        language: glOpcionesIdioma,
        responsive: true,
        "columns": [


            {
               
                "title": "Acción", "data": null, className: "celdaCenter celda1", "width": "0%", "render": function (data, type, row) {
                    var inicioBoton = '<div class="dropdown dropend"><button class="btn btn-success" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false"><span class="fas fa-list"></span></button><ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">';
                    var finBoton = '</ul></div>';
                    if (row.vigente == 'SI') {
                        var Ver = `<li style="padding-left: 15px;"><a href="javascript:DesactivarMarca('${row.codigo}','${row.descripcion}')"><i class="far fa-trash-alt green"></i> Desactivar</a></li>`;
                        var Ver2 = `<li style="padding-left: 15px;"><a href="javascript:OpenModalMarca('${row.codigo}','${row.descripcion}','${row.abreviatura}','${row.tipo}','${row.zona}','${row.lugE_CODIGO}','${row.vigente}','${row.indicativo}','${row.costO_ENVIO}')"><i class="fas fa-save blue"></i> Modificar</a></li>`;
                    } else {
                        var Ver = `<li style="padding-left: 15px;"><a href="javascript:ActivarMarca('${row.codigo}','${row.descripcion}')"><i class="far fa-trash-alt red"></i> Activar</a></li>`;
                        var Ver2 = `<li style="padding-left: 15px;"><a href="javascript:OpenModalMarca('${row.codigo}','${row.descripcion}','${row.abreviatura}','${row.tipo}','${row.zona}','${row.lugE_CODIGO}','${row.vigente}','${row.indicativo}','${row.costO_ENVIO}')"><i class="fas fa-save blue"></i> Modificar</a></li>`;
                      
                    }
                    return inicioBoton + Ver + Ver2 + finBoton;
                }
            },

            { "title": "codigo", "data": "codigo", "name": "codigo", className: "celdaCenter celda1" },
            { "title": "Descripcion", "data": "descripcion", "name": "descripcion", className: "celdaCenter celda1" },
            { "title": "abreviatura", "data": "abreviatura", "name": "abreviatura", className: "celdaCenter celda1" },
            { "title": "tipo", "data": "tipo", "name": "tipo", className: "celdaCenter celda1" },
            { "title": "zona", "data": "zona", "name": "zona", className: "celdaCenter celda1" },
            { "title": "luge_codigo", "data": "lugE_CODIGO", "name": "lugE_CODIGO", className: "celdaCenter celda1" },
            { "title": "vigente", "data": "vigente", "name": "vigente", className: "celdaCenter celda1" },
            { "title": "indicativo", "data": "indicativo", "name": "indicativo", className: "celdaCenter celda1" },
            { "title": "costo_envio", "data": "costO_ENVIO", "name": "costO_ENVIO", className: "celdaCenter celda1" },
     

        ],
        lengthMenu: [
            [10, 25, 50, -1],
            ['10 registros', '25 registros', '50 registros', 'Todos']
        ],
        "rowCallback": function (row, data, index) {

            if (data.Cantidad == data.vigente) {
                $(row).find('td:eq(7)').addClass('SemaforoAzul');
            } else {
                $(row).find('td:eq(7)').addClass('SemaforoVerde');
            }

            if (data.Cantidad == data.costO_ENVIO) {
                $(row).find('td:eq(9)').addClass('SemaforoAzul');
            } else {
                $(row).find('td:eq(9)').addClass('SemaforoVerde');
            }
        },
        ordering: true,
        pageLength: 10,
        bLengthChange: true,
        searching: true,
        paging: true,
        info: true

    });


}


//Modificar Categoria y marca
async function OpenModalMarca(_idProducto, _Descripcion, _Abreviatura, _Tipo, _Zona, _LugeCodigo, _Vigente, _Indicativo, _CostoEnvio) {

    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere Activar la Pais 0 Ciudad",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            $("#txtCodigo").val(_idProducto);
            $("#txtMarca").val(_Descripcion);
            $("#txtAbreviatura").val(_Abreviatura);
            $("#txtTipo").val(_Tipo);
            $("#txtZona").val(_Zona);
            $("#txtLuge").val(_LugeCodigo);
            $("#txtVigente").val(_Vigente);
            $("#txtIndicativo").val(_Indicativo);
            $("#txtCostoEnvio").val(_CostoEnvio);       


        }
    });


}

function Limpiar() {
   
    $("#txtMarca").val("");
    $("#txtCodigo").val("");
    $("#txtMarca").val("");
    $("#txtAbreviatura").val("");
    $("#txtTipo").val("");
    $("#txtZona").val("");
    $("#txtLuge").val("");
    $("#txtVigente").val("");
    $("#txtIndicativo").val("");
    $("#txtCostoEnvio").val("");


    $("#IdPais").val(0);
    $("#IdPais").trigger('change.select2');
    $("#IdPais").trigger("chosen:updated");

    $("#selectDepartamento").val("Seleccione");
    $("#selectDepartamento").trigger('change.select2');
    $("#selectDepartamento").trigger("chosen:updated");
    GetCiudades(0);

    

 
}


//Update Marca
function DesactivarMarca(_idProducto, _descripcion) {
    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere desactivar la marca",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                CODIGO: _idProducto,
                DESCRIPCION: _descripcion,
                VIGENTE: "NO"
            }
            let Usuario = await UpdCiudad(dto);
       /*     GetMarca();*/
            /*  window.location.reload();*/
        }
    });
}

async function UpdCiudad(dto) {

    let UpdMarca = await $.ajax({
        type: "POST",
        url: UrlUpdateCiudad,
        async: true,
        data: dto,
        dataType: 'json',
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                Swal.fire({
                    type: 'success',
                    title: '¡Estimado(a) Cliente!',
                    text: respuesta.message
                });

            }
            else {
                Swal.fire({
                    type: 'warning',
                    title: '¡Estimado(a) Cliente!',
                    text: respuesta.message
                });

            }
        },
        error: function () {
            Swal.fire({
                type: 'error',
                title: '¡Estimado(a) Cliente!',
                text: 'Error al validar Usuario!!'
            });
        }
    });
    return UpdMarca;

}

function ActivarMarca(_idProducto, _descripcion) {
    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere Activar la Pais 0 Ciudad",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                CODIGO: _idProducto,
                DESCRIPCION: _descripcion,
                VIGENTE: "SI"
            }
            let Usuario = await UpdCiudad(dto);
        /*    GetMarca();*/
            /*  window.location.reload();*/
        }
    });
}




//    ValidarDepartamento();

//function ValidarDepartamento() {
//    /*   $("#selectDepartamento").empty();*/
//    var pais = 10;

//    if (pais == 10) {
//        $("#idDepartamento1").css("display", "block");
//        $("#idDepartamento2").css("display", "block");
      

//    } else {
//        $("#idDepartamento1").css("display", "block");
//        $("#idDepartamento2").css("display", "block");
 
//    }
//    $.ajax({
//        type: 'POST',
//        url: UrlGetDepartamentoDominios,
//        dataType: 'json',
//        data: { IdDto: pais },
//        success: function success(response) {
//            $.each(response.data, function (i, dato) {
//                $("#selectDepartamento").append('<option value="' + dato.idDominio + '">' + dato.descripcion + '</option>');

//                if (pais == 10) {
//                    $("#selectDepartamento").val(1227);
//                    $("#selectDepartamento").trigger('change.select2');
//                    $("#selectDepartamento").trigger("chosen:updated");



//                }
//                else {
//                    $("#selectDepartamento").val(0);
//                    $("#selectDepartamento").trigger('change.select2');
//                    $("#selectDepartamento").trigger("chosen:updated");

//                    $("#selectCiudad").val(0);
//                    $("#selectCiudad").trigger('change.select2');
//                    $("#selectCiudad").trigger("chosen:updated");
//                }

//            });
//            if (pais == 10) {
//                ciudad(1227);
//            }
//            $("#selectDepartamento").trigger("chosen:updated");
//        },
//        error: function error(ex) {
//            Swal.fire({
//                type: 'error',
//                title: '¡Estimado(a) Cliente!',
//                text: 'No es posible cargar los datos, revise'
//            });
//        }
//    });
//};
$("#IdPais").change(function () {
    $("#selectDepartamento").empty();
    var pais = $("#IdPais").val();

    //if (pais == 10) {
    //    $("#idDepartamento1").css("display", "block");
    //    $("#idDepartamento2").css("display", "block");
      

    //} else {
    //    $("#idDepartamento1").css("display", "block");
    //    $("#idDepartamento2").css("display", "block");
       
    //}
    $.ajax({
        type: 'POST',
        url: UrlGetDepartamentoDominios,
        dataType: 'json',
        data: { IdDto: pais },
        success: function success(response) {
            $.each(response.data, function (i, dato) {
                $("#selectDepartamento").append('<option value="' + dato.idDominio + '">' + dato.descripcion + '</option>');

                //if (pais == 10) {
                //    $("#selectDepartamento").val(1227);
                //    $("#selectDepartamento").trigger('change.select2');
                //    $("#selectDepartamento").trigger("chosen:updated");



                //}
                //else {
                //    $("#selectDepartamento").val(0);
                //    $("#selectDepartamento").trigger('change.select2');
                //    $("#selectDepartamento").trigger("chosen:updated");

                //    $("#selectCiudad").val(0);
                //    $("#selectCiudad").trigger('change.select2');
                //    $("#selectCiudad").trigger("chosen:updated");
                //}

            });
            //if (pais == 10) {
            //    ciudad(1227);
            //}
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



//function ciudad(_idCiudad) {


//    $("#selectCiudad").empty();
//    $.ajax({
//        type: 'POST',
//        url: UrlGetDepartamentoDominios,
//        dataType: 'json',
//        data: { IdDto: _idCiudad },
//        success: function success(response) {
//            $.each(response.data, function (i, dato) {
//                $("#selectCiudad").append('<option value="' + dato.idDominio + '">' + dato.descripcion + '</option>');



//            });
//            if (_idCiudad == 1227) {
//                $("#selectCiudad").val(1228);
//                $("#selectCiudad").trigger('change.select2');
//                $("#selectCiudad").trigger("chosen:updated");
//            }
//            $("#selectCiudad").trigger("chosen:updated");
//        },
//        error: function error(ex) {
//            Swal.fire({
//                type: 'error',
//                title: '¡Estimado(a) Cliente!',
//                text: 'No es posible cargar los datos, revise'
//            });
//        }
//    });
//}
$("#selectDepartamento").change(function () {
    var departamento = $("#selectDepartamento").val();
    GetCiudades(departamento);
    //$("#selectCiudad").empty();
    //$.ajax({
    //    type: 'POST',
    //    url: UrlGetDepartamentoDominios,
    //    dataType: 'json',
    //    data: { IdDto: $("#selectDepartamento").val() },
    //    success: function success(response) {
    //        $.each(response.data, function (i, dato) {
    //            $("#selectCiudad").append('<option value="' + dato.idDominio + '">' + dato.descripcion + '</option>');
    //        });
    //        $("#selectCiudad").trigger("chosen:updated");
    //    },
    //    error: function error(ex) {
    //        Swal.fire({
    //            type: 'error',
    //            title: '¡Estimado(a) Cliente!',
    //            text: 'No es posible cargar los datos, revise'
    //        });
    //    }
    //});
});


var listaPais = [];
function GetConsultapaisesDominios() {

    let data = {}
    $.post(UrlGetPaisesDominiosX, data).done(function (result) {

        if (result.data.length > 0) {
            listaPais = result.data;
            localStorage.removeItem("listaPaises");
            localStorage.setItem("listaPaises", JSON.stringify(listaPais));
        } else { }
    }).fail();

}
CargarLista();

var DominioCiudad = [];
function CargarLista() {
    let container = document.querySelector('#IdPais');
    container.innerHTML = "";
    DominioCiudad = JSON.parse(localStorage.getItem("listaPaises"));
    container.innerHTML += `<option value="0" >Seleccione</option>`;
    DominioCiudad.forEach((item) => {
        container.innerHTML += `<option value="${item.idDominio}"> ${item.descripcion}</option>`;

    });

 /*   container.value = "10";*/



}

