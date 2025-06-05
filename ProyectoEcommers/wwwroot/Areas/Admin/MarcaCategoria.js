

$(document).ready(function () {
    GetMarca();
    GetCategoria();
});

function ConvertirEnString(valor) {// recibe numero float y lo devuelve en formato con puntos y comas y el signo $

    if (valor - Math.trunc(valor) > 0)
        valor = valor.toFixed(2);

    valor = "$ " + parseFloat(valor).toLocaleString('es-CO');
    return valor;
}
//function GetConsultaDominios() {

//    $.ajax({
//        type: 'POST',
//        async: false,
//        url: UrlGetDominios,
//        success: function success(response) {
//            if (response.success == true) {

//                response.data.forEach((item) => {
//                    $("#selectMarca").append(`<option value="${item.idDominio}">${item.descripcion}</option>`);
//                });

//                response.data1.forEach((item) => {
//                    $("#selectCategoria").append(`<option value="${item.idDominio}">${item.descripcion}</option>`);
//                });

//            } else {

//            }
//        },
//        error: function error(ex) {
//        }
//    });
//}




//---------------------------------


$("#btnGrabar").click(() => {
    Ins_Marca();
});

function Ins_Marca() {

    var marca = $("#txtMarca").val();
    if (marca == null || marca == "") {
        Swal.fire({
            title: 'Error',
            text: "Por favor registre Marca a Modificar o Crear...",
            type: 'error',
            showCancelButton: false,
            confirmButtonColor: '#032b57',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
     /*   create('error', 'Por favor, registre Marca a Modificar o Crear...', UrlAlertError);*/
        return;
    }
    var Idmarcas = 0;
    var valida = $("#txtIdMarca").val();
    if (valida == null || valida == "") {
        Idmarcas == 0;
    }
    else {
        Idmarcas = valida;
    }


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
            var token = document.getElementsByName("__RequestVerificationToken")[0].value;
            let data = new FormData();
            data.append('IdDominio', Idmarcas);
            data.append('Descripcion', marca);
            data.append('Vigente', 1);
            data.append('__RequestVerificationToken', token);

            $.ajax({
                type: 'POST',
                url: UrlAddMarca,
                dataType: 'json',
                data: data,
                cache: false,
                contentType: false,
                processData: false,
                enctype: 'multipart/form-data',
                success: function (response) {
                    if (response.success == true) {
                        LimpiarIngresoMarca()
                        Swal.fire({
                            type: 'success',
                            title: '¡Transacción exitosa!',
                            text: "La Marca fué guardada exitosamente",
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

$("#btnGrabarCategoria").click(() => {
    Ins_Categoria();
});

function Ins_Categoria() {

    let Categoria = $("#txtCategoria").val();
    if (Categoria == null || Categoria == "") {
        Swal.fire({
            title: 'Error',
            text: "Por favor registre la categoria a Modificar o Crear...",
            type: 'error',
            showCancelButton: false,
            confirmButtonColor: '#032b57',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar'
        });
     /*   create('error', 'Por favor, registre la categoria a Modificar o Crear...', UrlAlertError);*/
        return;
    }
    let IdCategoria = 0;
    let valida = $("#txtICategoria").val();
    if (valida == null || valida == "" || valida == 0) {
        IdCategoria == 0;
    }
    else {
        IdCategoria = valida;
    }





    Swal.fire({
        type: 'warning',
        title: '¡Atención!',
        text: "¿Esta seguro de de guardar la categoria?",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then((result) => {
        if (result.value) {
            var token = document.getElementsByName("__RequestVerificationToken")[0].value;
            let data = new FormData();
            data.append('IdDominio', IdCategoria);
            data.append('Descripcion', Categoria);
            data.append('Vigente', 1);
            data.append('__RequestVerificationToken', token);


            $.ajax({
                type: 'POST',
                url: UrlAddCategoria,
                dataType: 'json',
                data: data,
                cache: false,
                contentType: false,
                processData: false,
                enctype: 'multipart/form-data',
                success: function (response) {
                    if (response.success == true) {
                        LimpiarIngresoCategoria();
                        Swal.fire({
                            type: 'success',
                            title: '¡Transacción exitosa!',
                            text: "La Categoria fué guardada exitosamente",
                            showCancelButton: false,
                            confirmButtonText: "¡Adelante!",
                        }).then((result) => {
                            window.location.reload();
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
function GetMarca() {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbGrilla")) {
        $("#tbGrilla").DataTable().destroy();
    }
    $("#tbGrilla").DataTable({
        "ajax": {
            type: "POST",
            url: UrlConsultaMarca,
            async: true,
            datatype: "json",
            cache: false
        },
        "initComplete": function (settings, json) {
            if (json.success) {

            }
            else {
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
                                                        <a class="dropdown-item" href="javascript:DesactivarMarca('${row.idDominio}','${row.descripcion}')">
                                                        <i class="far fa-trash-alt" style="color:red"></i> Desactivar
                                                        </a>
                                                     </li>
                                                     <li>
                                                        <a class="dropdown-item" href="javascript:OpenModalMarca('${row.idDominio}','${row.descripcion}')">
                                                        <i class="fas fa-save" style="color:blue"></i> Modificar
                                                        </a>
                                                     </li>
                                                 </ul>
                                         </div>`
                }
            },
            { "title": "Id Marca", "data": "idDominio", "name": "idDominio", className: "celdaCenter celda1" },
            { "title": "Marca", "data": "descripcion", "name": "descripcion", className: "celdaCenter celda1" },


            {
                data: null, title: "vigente", className: "celdaCenter celda1", render: function (row) {

                    if (row.vigente === 0) {
                        return `<label>NO</label>`
                    }
                    else {
                        return `<label>SI</label>`
                    }
                }
            },
        ],
        lengthMenu: [
            [10, 25, 50, -1],
            ['10 registros', '25 registros', '50 registros', 'Todos']
        ],
        "rowCallback": function (row, data, index) {

            if (data.Cantidad == data.CantidadTotal) {
                $(row).find('td:eq(7)').addClass('SemaforoAzul');
            } else {
                $(row).find('td:eq(7)').addClass('SemaforoVerde');
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
function GetCategoria() {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbGrillaCategoria")) {
        $("#tbGrillaCategoria").DataTable().destroy();
    }
    $("#tbGrillaCategoria").DataTable({
        "ajax": {
            type: "POST",
            url: UrlConsultaCategoria,
            async: true,
            datatype: "json",
            cache: false
        },
        "initComplete": function (settings, json) {
            if (json.success) {

            }
            else {
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
                                                        <a class="dropdown-item" href="javascript:DesactivarCategoria('${row.idDominio}','${row.descripcion}')">
                                                        <i class="far fa-trash-alt" style="color:red"></i> Desactivar
                                                        </a>
                                                     </li>
                                                     <li>
                                                        <a class="dropdown-item" href="javascript:OpenModalCategoria('${row.idDominio}','${row.descripcion}')">
                                                        <i class="fas fa-save" style="color:blue"></i> Modificar
                                                        </a>
                                                     </li>
                                                 </ul>
                                         </div>`
                }
            },
            { "title": "Id Categoria", "data": "idDominio", "name": "idDominio", className: "celdaCenter celda1" },
            { "title": "Categoria", "data": "descripcion", "name": "descripcion", className: "celdaCenter celda1" },


            {
                data: null, title: "vigente", className: "celdaCenter celda1", render: function (row) {

                    if (row.vigente === 0) {
                        return `<label>NO</label>`
                    }
                    else {
                        return `<label>SI</label>`
                    }
                }
            },
        ],
        lengthMenu: [
            [10, 25, 50, -1],
            ['10 registros', '25 registros', '50 registros', 'Todos']
        ],
        "rowCallback": function (row, data, index) {

            if (data.Cantidad == data.CantidadTotal) {
                $(row).find('td:eq(7)').addClass('SemaforoAzul');
            } else {
                $(row).find('td:eq(7)').addClass('SemaforoVerde');
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
async function OpenModalMarca(_idProducto, _Descripcion) {

    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "¡Quiere modificar la Marca!",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            $("#txtIdMarca").val(_idProducto);
            $("#txtMarca").val(_Descripcion);


        }
    });


}
async function OpenModalCategoria(_idProducto, _Descripcion) {

    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "¡Quiere modificar la Marca!",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            $("#txtICategoria").val(_idProducto);
            $("#txtCategoria").val(_Descripcion);


        }
    });


}
function LimpiarIngresoMarca() {
    $("#txtIdMarca").val("");
    $("#txtMarca").val("");



    GetMarca();
}
function LimpiarIngresoCategoria() {
    $("#txtICategoria").val("");
    $("#txtCategoria").val("");



    GetCategoria();
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
                IdDominio: _idProducto,
                Descripcion: _descripcion,
                Vigente: 0
            }
            let Usuario = await UpdMarca(dto);
            GetMarca();
          /*  window.location.reload();*/
        }
    });
}

async function UpdMarca(dto) {

    let UpdMarca = await $.ajax({
        type: "POST",
        url: UrlUpdateMarca,
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
    return UpdMarca;

}

//Eliminar Categoria
function DesactivarCategoria(_idProducto, _descripcion) {
    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere desactivar la categoria",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                IdDominio: _idProducto,
                Descripcion: _descripcion,
                Vigente: 0
            }
            let Usuario = await UpdCategoria(dto);
            GetCategoria();
     /*       window.location.reload();*/
        }
    });
}

async function UpdCategoria(dto) {

    let UpdCategoria = await $.ajax({
        type: "POST",
        url: UrlUpdateCategoria,
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
    return UpdCategoria;

}

//Desactiva Producto
