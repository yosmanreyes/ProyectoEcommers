
GetMisComentarios();
GetConsultaCorreoes();
//Consulta Grilla
function GetMisComentarios() {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbGrilla")) {
        $("#tbGrilla").DataTable().destroy();
    }
    $("#tbGrilla").DataTable({
        "ajax": {
            type: "POST",
            url: UrlGetConsultaComentarios,
            async: true,
            datatype: "json",
            cache: false
        },
        "initComplete": function (settings, json) {
            if (json.success) {
                listaProductoVenta = json.data;
            }
            else {
                /*   $("#Process").hide();*/
                /*     $("#Process").show();*/
                /*     OcultarPanelListaUsuarios();*/
                Swal.fire({
                    type: 'warning',
                    title: '¡Estimado(a) Cliente!',
                    text: json.message
                });
            }
        },
        language: glOpcionesIdioma,
        responsive: true,

        "columns": [
            {
                data: null, title: "Acción", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div class="dropdown dropend">
                        <button class="btn btn-success" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
                            <span class="fas fa-list"></span>
                        </button>
                        <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">                        
                            <li>
                                <a class="dropdown-item" href="javascript:OpenEnviarCorreo('${row.idComentario}','${row.correoElectronico}')">
                                    <i class="fa fa-envelope-o faa-ring animated green" style="color:green"></i> Enviar Correo
                                </a>
                            </li>     
                                  <li>
                                <a class="dropdown-item" href="javascript:UpdateAtendio('${row.idComentario}')">
                                    <i class="fa fa-pencil-square-o faa-ring animated blue" style="color:green"></i> Revisado
                                </a>
                            </li>  
                        </ul>
                    </div>`
                }
            },
            { "title": "Id Comentario", "data": "idComentario", "name": "idComentario", className: "celdaCenter celda1" },
            {
                data: null, title: "Número de Teléfono", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.idComentario}" data-columna="numeroTelefono">${row.numeroTelefono}</div>`
                }
            },
            {
                data: null, title: "Número de Teléfono con Indicativo del país", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.idComentario}" data-columna="numeroTelefono">${row.numeroTelefonoConIndicativo}</div>`
                }
            },
            {
                data: null, title: "Nombres", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.idComentario}" data-columna="nombres">${row.nombres}</div>`
                }
            },
            {
                data: null, title: "Comentario", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.idComentario}" data-columna="comentarios">${row.comentarios}</div>`
                }
            },
            {
                data: null, title: "Correo Electrónico", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.idComentario}" data-columna="correoElectronico">${row.correoElectronico}</div>`
                }
            },
            {
                data: null, title: "Fecha Creación", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.idComentario}" data-columna="fechaCreacionS">${row.fechaCreacionS}</div>`
                }
            },
            {
                data: null, title: "Vigente", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.idComentario}" data-columna="vigente">${row.vigente}</div>`
                }
            },
            {
                data: null, title: "Visualizado", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.idComentario}" data-columna="atendio">${row.atendio}</div>`
                }
            },
            {
                data: null, title: "Envío Correo", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.idComentario}" data-columna="envioCorreo">${row.envioCorreo}</div>`
                }
            }
        ],
        "rowCallback": function (row, data, index) {

            if (data.vigente == "NO") {
                $(row).find('td:eq(7)').addClass('SemaforoRojo');
                /*      $(row).find('td:eq(6)').addClass('SemaforoAzul');*/
            } else {
                $(row).find('td:eq(7)').addClass('SemaforoAzul');
            }
            if (data.comentarios == "Sin Comentarios") {
                $(row).find('td:eq(4)').addClass('SemaforoRojo');
                /*      $(row).find('td:eq(6)').addClass('SemaforoAzul');*/
            } else {
                $(row).find('td:eq(4)').addClass('SemaforoVerde');
            }
        },




        ordering: true,
        pageLength: 10,
        bLengthChange: true,
        searching: true,
        paging: true,
        info: true

        // Resto del código...
    });

    // Manejador de eventos para guardar los cambios en las celdas editables
    $("#tbGrilla tbody").on('input', '.editable', function () {
        var idVenta = $(this).data("id");
        var columna = $(this).data("columna");
        var nuevoValor = $(this).text();

        /* ModificarVentaProducto(idVenta, columna, nuevoValor);*/

    });
}
//Abrir Modal envio Correo Perosnal
function OpenEnviarCorreo(_idComentario, _correo) {
    $("#btnEnviarCorreo").addClass('hidden');
    $("#btnEnviarCorreoP").removeClass('hidden');
    $('#ModalCorreo').modal("show");
    $("#txtIdComentario").val(_idComentario);
    $("#txtcorreo").val(_correo);
    $("#txtAsunto").val("Distribuidora de Belleza Majas, agradece por sus comentarios");
    $("#txtMensaje").val("Para majas es muy importante por contar con su comentarios y sugerencias, vamos a validar sus caso con los directivos con el fin de mejorar nuestros servicios, Gracias");

}
function LimpiarCorreo() {
    $("#txtAsunto").val("");
    $("#txtMensaje").val("");
    $("#btnEnviarCorreo").attr("disabled", false);
    $("#btnEnviarCorreoP").attr("disabled", false);
}
function CerrarCorreos() {
    $('#ModalCorreo').modal("hide");
    LimpiarCorreo();
}
//Envio Correo Personal
function EnvioCorreosPersonal() {

    var IdComentario = $("#txtIdComentario").val();

    var Correo = $("#txtcorreo").val();
    if (Correo == null || Correo == "") {
        Swal.fire({
            title: 'Error',
            text: "Por favor registre correo electrónico...",
            type: 'error',
            showCancelButton: false,
            confirmButtonColor: '#032b57',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
   /*     create('error', 'Por favor, correo electrónico...', UrlAlertError);*/
        return;
    }


    var Asumnto = $("#txtAsunto").val();
    if (Asumnto == null || Asumnto == "") {
        Swal.fire({
            title: 'Error',
            text: "Por favor registre el asunto...",
            type: 'error',
            showCancelButton: false,
            confirmButtonColor: '#032b57',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
/*        create('error', 'Por favor, registre el asunto...', UrlAlertError);*/
        return;
    }

    var Mensaje = $("#txtMensaje").val();
    if (Mensaje == null || Mensaje == "") {
        Swal.fire({
            title: 'Error',
            text: "Por favor registre el mensaje...",
            type: 'error',
            showCancelButton: false,
            confirmButtonColor: '#032b57',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
      /*  create('error', 'Por favor, registre el mensaje..', UrlAlertError);*/
        return;
    }
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    data = { __RequestVerificationToken: token, _IdComentario: IdComentario, _Correo: Correo, _Asumnto: Asumnto, _Mensaje: Mensaje }
    $.ajax({
        type: "POST",
        url: UrlEnvioCorreoPersonal,
        async: true,
        data: data,
        dataType: 'json',
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                /*     GetMisVenta();*/
                Swal.fire({
                    type: 'success',
                    title: '¡Transacción exitosa!',
                    text: respuesta.mensaje,
                    showCancelButton: false,
                    confirmButtonText: "¡Adelante!",
                }).then((result) => {
                    /*  window.location.reload();*/
                });
            } else {
                Swal.fire({
                    title: 'Error',
                    text: respuesta.mensaje,
                    type: 'error',
                    showCancelButton: false,
                    confirmButtonColor: '#032b57',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Aceptar'
                });

            }
        },
        error: function () {
            //Swal.fire({
            //    type: 'error',
            //    title: '¡Estimado(a) Cliente!',
            //    text: 'Error al validar Usuario!!'
            //});
        }
    });
}
//Generar Excel 
function RptExcel() {
    $("#Process").show();
    UrlReporte = UrlReporteExcel;
    setTimeout(function () {
        document.location = UrlReporte;
        $("#Process").hide();
    }, 2000);
}
//function BuscaFormularioExcel(_FechaInicio, _FechaFin) {

//    var FechaInicio = _FechaInicio;
//    var FechaFin = _FechaFin;

//    $("#Process").show();



//    if (FechaInicio === 0 || FechaInicio === "") {
//        create('error', 'Por favor seleccione la fecha inicio de la consulta', UrlAlertError);
//        $("#Process").hide();
//        return;
//    }


//    if (FechaFin === 0 || FechaFin === "") {
//        create('error', 'Por favor seleccione la fecha Final de la consulta', UrlAlertError);
//        $("#Process").hide();
//        return;
//    }

//    var data = {
//        'V_FechaInicio': FechaInicio,
//        'V_FechaFin': FechaFin,
//    };
//    /*    var opcionDetalleAuto = `<a class="dropdown-item" href="${urlPasarGraduados}?_idCodigoCivica=${data.CODIGO_CIVICA}&&_idEstado=${data.ESTADO}"><i class="fas fa-bars"></i> Editar</a>`;*/


//    var _TipoEncuesta = encodeQueryData(data);
//    /*    alert('https://programacionextrema.com?' + querystring);*/



//    UrlReporte = UrlReporteExcel + "?" + _TipoEncuesta;
//    setTimeout(function () {
//        document.location = UrlReporte;
//        $("#Process").hide();
//    }, 2000);
//}

//Abrir Modal envio Correo Masivo

//Abrir Correo Personal y Masivo


//Envio Correo Personal Masivo
function EnvioCorreosPersonalM() {



    var Correo = $("#txtcorreoMasivo").val();
    if (Correo == null || Correo == "") {
        Swal.fire({
            title: 'Error',
            text: "Por favor registre el correo electrónico...",
            type: 'error',
            showCancelButton: false,
            confirmButtonColor: '#032b57',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
     /*   create('error', 'Por favor, correo electrónico...', UrlAlertError);*/
        return;
    }


    var Asumnto = $("#txtAsuntoMasivo").val();
    if (Asumnto == null || Asumnto == "") {
        Swal.fire({
            title: 'Error',
            text: "Por favor registre el asunto...",
            type: 'error',
            showCancelButton: false,
            confirmButtonColor: '#032b57',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
/*        create('error', 'Por favor, registre el asunto...', UrlAlertError);*/
        return;
    }

    var Mensaje = $("#txtMensajeMasivo").val();
    if (Mensaje == null || Mensaje == "") {
        Swal.fire({
            title: 'Error',
            text: "Por favor registre el mensaje...",
            type: 'error',
            showCancelButton: false,
            confirmButtonColor: '#032b57',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
  /*      create('error', 'Por favor, registre el mensaje..', UrlAlertError);*/
        return;
    }
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    data = { __RequestVerificationToken: token, _Correo: Correo, _Asumnto: Asumnto, _Mensaje: Mensaje }
    $.ajax({
        type: "POST",
        url: UrlEnvioCorreoPersonal,
        async: true,
        data: data,
        dataType: 'json',
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                /*     GetMisVenta();*/
                Swal.fire({
                    type: 'success',
                    title: '¡Transacción exitosa!',
                    text: respuesta.mensaje,
                    showCancelButton: false,
                    confirmButtonText: "¡Adelante!",
                }).then((result) => {
                    /*  window.location.reload();*/
                });
            } else {
                Swal.fire({
                    title: 'Error',
                    text: respuesta.mensaje,
                    type: 'error',
                    showCancelButton: false,
                    confirmButtonColor: '#032b57',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Aceptar'
                });

            }
        },
        error: function () {
            //Swal.fire({
            //    type: 'error',
            //    title: '¡Estimado(a) Cliente!',
            //    text: 'Error al validar Usuario!!'
            //});
        }
    });
}
function EnvioCorreosMasivo() {
    var selectElements = document.getElementById("bootstrap-duallistbox-selected-list_");
    var opciones = selectElements.options;


    if (opciones.length == 0) {
        create('error', 'Por favor, correo electrónico...', UrlAlertError);
        return;
    }
    var Asumnto = $("#txtAsuntoMasivo").val();
    if (Asumnto == null || Asumnto == "") {
        create('error', 'Por favor, registre el asunto...', UrlAlertError);
        return;
    }

    var Mensaje = $("#txtMensajeMasivo").val();
    if (Mensaje == null || Mensaje == "") {
        create('error', 'Por favor, registre el mensaje..', UrlAlertError);
        return;
    }

    var DtoEnvioCorreo = [];

    for (var i = 0; i < opciones.length; i++) {
        DtoEnvioCorreo.push({
            CorreoEnviar: opciones[i].innerHTML,
            Asunto: Asumnto,
            Mensaje: Mensaje
        });
    }
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    data = { __RequestVerificationToken: token, obj: DtoEnvioCorreo }
    $.ajax({
        type: "POST",
        url: UrlEnvioCorreoMasivo,
        async: true,
        data: data,
        dataType: 'json',
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                /*     GetMisVenta();*/
                Swal.fire({
                    type: 'success',
                    title: '¡Estimado(a) Cliente!',
                    text: respuesta.mensaje
                });

            } else {
                Swal.fire({
                    type: 'warning',
                    title: '¡Estimado(a) Cliente!',
                    text: respuesta.mensaje
                });

            }
        },
        error: function () {
        }
    });

}
//Abrir Modal envio Correo Masivo
function OpenEnviarCorreoMasivo() {
    $("#EnvioCorreosMasivo").addClass('hidden');
    $("#EnvioCorreosPersonalM").removeClass('hidden');
    $('#ModalCorreoMasivo').modal("show");
    $("#txtIdComentarioMasivo").val(0);
    $("#txtcorreoMasivo").val("");

    $("#txtAsuntoMasivo").val("Distribuidora de Belleza Majas, agradece por sus comentarios");
    $("#txtMensajeMasivo").val("Para majas es muy importante por contar con su comentarios y sugerencias, vamos a validar sus caso con los directivos con el fin de mejorar nuestros servicios, Gracias");

    /*    LimpiarCorreo();*/
}
function LimpiarCorreoMasivo() {
    $("#txtAsuntoMasivo").val("");
    $("#txtMensajeMasivo").val("");
    $("#EnvioCorreosMasivo").attr("disabled", false);
    $("#EnvioCorreosPersonalM").attr("disabled", false);
}
function CerrarCorreosMasivo() {
    $('#ModalCorreoMasivo').modal("hide");
    LimpiarCorreoMasivo();
}
//Consulta Correos
function GetConsultaCorreoes() {

    $.ajax({
        type: 'POST',
        async: false,
        url: UrlGetCorreos,
        success: function success(response) {
            if (response.success == true) {

                response.data.forEach((item) => {
                    $("#selectcorreos").append(`<option style="height: 25px !important;" value="${item.idDominio}">${item.descripcion}</option>`);
                });


            } else {

            }
        },
        error: function error(ex) {
        }
    });
}
//Activa el check Masivo o Individual
async function ModalInsUsuarios() {

    let Bloqueado = 0;
    let chequeo = CheckEstadoUsuario.checked;

    if (chequeo) {
        $("#EnvioCorreosMasivo").removeClass('hidden');
        $("#EnvioCorreosPersonalM").addClass('hidden');
        $("#txtcorreoMasivo").val('');

        Bloqueado = 1;
    }
    else {
        Bloqueado = 0;
        $("#EnvioCorreosMasivo").addClass('hidden');
        $("#EnvioCorreosPersonalM").removeClass('hidden');
        $("#txtcorreoMasivo").val('');

    }

    if (chequeo) {
        CambioAbotonSi();
    }
    else {
        CambioAbotonNO();
    }



}
function CambioAbotonSi() {

    $("#idMasivo").css("display", "block");
    $("#idIndividual").css("display", "none");
    $('#CheckEstadoUsuario').prop('checked', 1).trigger('change');
}
function CambioAbotonNO() {


    //$("#bootstrap-duallistbox-nonselected-list_").val(0);
    //$("#bootstrap-duallistbox-nonselected-list_").trigger('change.select2');
    //$("#bootstrap-duallistbox-nonselected-list_").trigger("chosen:updated");

    $("#idMasivo").css("display", "none");
    $("#idIndividual").css("display", "block");
    $('#CheckEstadoUsuario').prop('checked', 0).trigger('change');
}
function encodeQueryData(data) {
    let result = [];
    for (let d in data)
        result.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]));
    return result.join('&');
}


//Desactiva Producto
async function UpdateAtendio(_idComentario) {

    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Se Cambiara el estado a visualizado",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                idComentario: _idComentario,
                vigente: 0
            }
            let Usuario = await UpdateAtendi(dto);
            GetMisComentarios();
            window.location.reload();
        }
    });


}
async function UpdateAtendi(dto) {

    let UpdImagen = await $.ajax({
        type: "POST",
        url: UrlpdateComentarios,
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

            } else {
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
    return UpdImagen;

}