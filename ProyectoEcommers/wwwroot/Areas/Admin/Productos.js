
$("#btnGrabar").click(() => {
    Ins_ImagenSlider();
});
$(document).ready(function () {
    GetProducto();
    GetProductoDesactivados();
});
function Ins_ImagenSlider() {
    const fileInput1 = document.getElementById('inputFile1');
    const filePath1 = fileInput1.files[0];
    var cinco = $("#txtNombre").val();
    if (cinco == null || cinco == "") {
        create('error', 'Por favor, registre el nombre del producto...', UrlAlertError);
        return;
    }


    var seis = $("#txtDescripcion").val();
    if (seis == null || seis == "") {
        create('error', 'Por favor, registre la descripción...', UrlAlertError);
        return;
    }

    var uno = Number($("#selectMarca").val());
    if (uno == null || uno == 0) {
        create('error', 'Por favor, seleccione la Marca...', UrlAlertError);
        return;
    }
    var dos = Number($("#selectCategoria").val());
    if (dos == null || dos == 0) {
        create('error', 'Por favor, seleccione la Categoria...', UrlAlertError);
        return;
    }

    var tres = $("#txtCantidad").val();
    if (tres == null || tres == "") {
        create('error', 'Por favor, registre la cantidad...', UrlAlertError);
        return;
    }

   var _producto_id =  $("#IdProducto").val(); 


    var cuatro = $("#txtPrecio").val();
    if (cuatro == null || cuatro == "") {
        create('error', 'Por favor, registre el precio...', UrlAlertError);
        return;
    }

    var allowedExtensions = /(.doc|.docx|.exe|.pdf|.mp4|.png|.xls|.xlsx)$/i;

    if (allowedExtensions.exec(filePath1)) {
        alert('Formato no correcto debe cargar un imagen jpg o jpeg');
        fileInput1.value = '';
        return false;

    } else {
        $("#notificacionDocumento").empty();

        Swal.fire({
            type: 'warning',
            title: '¡Atención!',
            text: "¿Esta seguro de de guardar el producto?",
            showCancelButton: true,
            cancelButtonText: "Cancelar",
            confirmButtonText: "¡Adelante!",
            closeOnConfirm: false
        }).then((result) => {
            if (result.value) {

                var token = document.getElementsByName("__RequestVerificationToken")[0].value;
                let data = new FormData();

                data.append('Imagen1', $("#inputFile1")[0].files[0]);
                data.append('Imagen2', $("#inputFile2")[0].files[0]);
                data.append('Imagen3', $("#inputFile3")[0].files[0]);
                data.append('Imagen4', $("#inputFile4")[0].files[0]);
                data.append('Nombre', $("#txtNombre").val());
                data.append('Descripcion', $("#txtDescripcion").val());
                data.append('MARCA_ID', Number($("#selectMarca").val()));
                data.append('CATEGORIA_ID', Number($("#selectCategoria").val()));
                data.append('Cantidad', parseInt($("#txtCantidad").val()));
                data.append('Precio', $("#txtPrecio").val());
                data.append('PRODUCTO_ID', _producto_id);
                data.append('__RequestVerificationToken', token);

                $.ajax({
                    type: 'POST',
                    url: UrlInsProducto,
                    dataType: 'json',
                    data: data,
                    cache: false,
                    contentType: false,
                    processData: false,
                    enctype: 'multipart/form-data',
                    success: function (response) {
                        if (response.success == true) {
                            GetProducto();
                            Swal.fire({
                                type: 'success',
                                title: '¡Transacción exitosa!',
                                text: "El producto fué guardado",
                                showCancelButton: false,
                                confirmButtonText: "¡Adelante!",
                            }).then((result) => {
                                if (_producto_id > 0) {
                                    ModificaProducto(_producto_id, Number($("#selectMarca").val()), Number($("#selectCategoria").val()), $("#txtNombre").val(), $("#txtDescripcion").val(), parseInt($("#txtCantidad").val()), $("#txtPrecio").val());
                                    const fileInput1 = document.getElementById('inputFile1');
                                    fileInput1.value = '';
                                    const fileInput2 = document.getElementById('inputFile2');
                                    fileInput2.value = '';
                                    const fileInput3 = document.getElementById('inputFile3');
                                    fileInput3.value = '';
                                    const fileInput4 = document.getElementById('inputFile4');
                                    fileInput4.value = '';
                                } else { 

                                    window.location.reload();

                                }
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
}
function ConvertirEnString(valor) {// recibe numero float y lo devuelve en formato con puntos y comas y el signo $

    if (valor - Math.trunc(valor) > 0)
        valor = valor.toFixed(2);

    valor = "$ " + parseFloat(valor).toLocaleString('es-CO');
    return valor;
}
function GetConsultaDominios() {

    $.ajax({
        type: 'POST',
        async: false,
        url: UrlGetDominios,
        success: function success(response) {
            if (response.success == true) {

                response.data.forEach((item) => {
                    $("#selectMarca").append(`<option value="${item.idDominio}">${item.descripcion}</option>`);
                });

                response.data1.forEach((item) => {
                    $("#selectCategoria").append(`<option value="${item.idDominio}">${item.descripcion}</option>`);
                });

            } else {

            }
        },
        error: function error(ex) {
        }
    });
}
//Consulta Grilla Producto
function GetProducto() {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbGrilla")) {
        $("#tbGrilla").DataTable().destroy();
    }
    $("#tbGrilla").DataTable({
        "ajax": {
            type: "POST",
            url: UrlConsultaProducto,
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
                                                        <a class="dropdown-item" href="javascript:DesactivarProducto('${row.ProductoId}')">
                                                        <i class="far fa-trash-alt" style="color:red"></i> Desactivar
                                                        </a>
                                                     </li>
                                                     <li>
                                                        <a class="dropdown-item" href="javascript:ModificarProducto('${row.ProductoId}','${row.MarcaId}','${row.CategoriaId}','${row.Nombre}','${row.Descripcion}','${row.Cantidad}','${row.Precio}')">
                                                        <i class="fas fa-save" style="color:blue"></i> Modificar
                                                        </a>
                                                     </li>
                                                 </ul>
                                         </div>`
                }
            },
            { "title": "Id Producto", "data": "ProductoId", "name": "ProductoId", className: "celdaCenter celda1" },
            { "title": "Marca", "data": "marca", "name": "marca", className: "celdaCenter celda1" },
            { "title": "Categoria", "data": "categoria", "name": "categoria", className: "celdaJust celda1" },
            { "title": "Nombre", "data": "Nombre", "name": "Nombre", className: "celdaCenter celda5" },
            { "title": "Descripción", "data": "Descripcion", "name": "Descripcion", className: "celdaCenter celda10" },
            { "title": "Cantidad", "data": "Cantidad", "name": "Cantidad", className: "celdaCenter celda1" },
            { "title": "Cantidad Total", "data": "CantidadTotal", "name": "CantidadTotal", className: "celdaCenter celda1" },
            { "title": "Precio", "data": "Precio", "name": "Precio", className: "celdaCenter celda3" },
            { "title": "Fecha Creacion", "data": "fechA_CREACIONS", "name": "fechA_CREACIONS", className: "celdaCenter celda3" },

            {
                data: null, title: "Vigente", className: "celdaCenter celda1", render: function (row) {

                    if (row.Vigente === 0) {
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

function GetProductoDesactivados() {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbGrillaDesactivados")) {
        $("#tbGrillaDesactivados").DataTable().destroy();
    }
    $("#tbGrillaDesactivados").DataTable({
        "ajax": {
            type: "POST",
            url: UrlConsultaProductoDesactivados,
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
                    text: "No Cuenta con productos Desactivados",
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
                                                        <a class="dropdown-item" href="javascript:ActivarrProducto('${row.ProductoId}')">
                                                        <i class="far fa-trash-alt" style="color:red"></i> Activar
                                                        </a>
                                                     </li>
                                                     <li>
                                                        <a class="dropdown-item" href="javascript:ModificarProducto('${row.ProductoId}','${row.MarcaId}','${row.CategoriaId}','${row.Nombre}','${row.Descripcion}','${row.Cantidad}','${row.Precio}')">
                                                        <i class="fas fa-save" style="color:blue"></i> Modificar
                                                        </a>
                                                     </li>
                                                 </ul>
                                         </div>`
                }
            },
            { "title": "Id Producto", "data": "ProductoId", "name": "ProductoId", className: "celdaCenter celda1" },
            { "title": "Marca", "data": "marca", "name": "marca", className: "celdaCenter celda1" },
            { "title": "Categoria", "data": "categoria", "name": "categoria", className: "celdaJust celda1" },
            { "title": "Nombre", "data": "Nombre", "name": "Nombre", className: "celdaCenter celda5" },
            { "title": "Descripción", "data": "Descripcion", "name": "Descripcion", className: "celdaCenter celda10" },
            { "title": "Cantidad", "data": "Cantidad", "name": "Cantidad", className: "celdaCenter celda1" },
            { "title": "Cantidad Total", "data": "CantidadTotal", "name": "CantidadTotal", className: "celdaCenter celda1" },
            { "title": "Precio", "data": "Precio", "name": "Precio", className: "celdaCenter celda3" },
            { "title": "Fecha Creacion", "data": "fechA_CREACIONS", "name": "fechA_CREACIONS", className: "celdaCenter celda3" },

            {
                data: null, title: "Vigente", className: "celdaCenter celda1", render: function (row) {

                    if (row.Vigente === 0) {
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

            if (data.Vigente == 0) {
                $(row).find('td:eq(10)').addClass('SemaforoRojo');
            } else {
                $(row).find('td:eq(10)').addClass('SemaforoRojo');
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
//Desactiva Producto
async function DesactivarProducto(identificacion) {

    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere desactivar el producto",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                PRODUCTO_ID: identificacion,
                VIGENTE: 0
            }
            let Usuario = await UpdProducto(dto);
            GetProducto();
            GetProductoDesactivados();
            window.location.reload();
        }
    });


}

async function ActivarrProducto(identificacion) {

    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere desactivar el producto",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                PRODUCTO_ID: identificacion,
                VIGENTE: 1
            }
            let Usuario = await UpdProducto(dto);
            GetProducto();
            GetProductoDesactivados();
            window.location.reload();
        }
    });


}
async function UpdProducto(dto) {

    let UpdProducto = await $.ajax({
        type: "POST",
        url: UrlUpdateProducto,
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
    return UpdProducto;

}
async function ModificarProducto(_ProductoId, _marca, _categoria, _Nombre, _Descripcion, _Cantidad, _Precio) {

    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere modificar el producto",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            let Usuario = ModificaProducto(_ProductoId, _marca, _categoria, _Nombre, _Descripcion, _Cantidad, _Precio);

        }
    });


}
function ModificaProducto(_ProductoId, _marca, _categoria, _Nombre, _Descripcion, _Cantidad, _Precio) {

    $("#txtNombre").val(_Nombre);
    $("#txtDescripcion").val(_Descripcion);
    $("#txtCantidad").val(_Cantidad);
    $("#txtPrecio").val(_Precio);
    $("#selectCategoria").val(_categoria);
    $("#IdProducto").val(_ProductoId);
    $("#selectMarca").val(_marca);
    $("#selectMarca").trigger('change.select2');
    $("#selectMarca").trigger("chosen:updated");
    $("#selectCategoria").val(_categoria);
    $("#selectCategoria").trigger('change.select2');
    $("#selectCategoria").trigger("chosen:updated");
    $("#IdImagenesGrilla").css("display", "block");

    GetImagenes(_ProductoId);

}
function LimpiarIngresoProducto() {

    $("#txtNombre").val("");
    $("#txtDescripcion").val("");
    $("#txtCantidad").val("");
    $("#txtPrecio").val("");
    $("#selectCategoria").val("");
    $("#IdProducto").val("");
    $("#selectMarca").val("");
    $("#selectMarca").trigger('change.select2');
    $("#selectMarca").trigger("chosen:updated");
    $("#selectCategoria").val("");
    $("#selectCategoria").trigger('change.select2');
    $("#selectCategoria").trigger("chosen:updated");
    const fileInput1 = document.getElementById('inputFile1');
    fileInput1.value = '';
    const fileInput2 = document.getElementById('inputFile2');
    fileInput2.value = '';
    const fileInput3 = document.getElementById('inputFile3');
    fileInput3.value = '';
    const fileInput4 = document.getElementById('inputFile4');
    fileInput4.value = '';
    $("#IdImagenesGrilla").css("display", "none");
    GetProducto();
    GetProductoDesactivados();
}
function EliminarProducto(_idProducto) {
    DesactivarProducto(_idProducto);
}
function GetImagenes(_ProductoId) {
    var contador = 0;
    var contador1 = 0;
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbGrillaImagenes")) {
        $("#tbGrillaImagenes").DataTable().destroy();
    }
    $("#tbGrillaImagenes").DataTable({
        "ajax": {
            type: "POST",
            url: UrlConsultaImagen,
            async: true,
            datatype: "json",
            data: { IdProducto: _ProductoId },
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
                                                        <a class="dropdown-item" href="javascript:DesactivarImagen('${row.ImagenId}')">
                                                        <i class="far fa-trash-alt" style="color:red"></i> Desactivar
                                                        </a>
                                                     </li>
                                                 </ul>
                                         </div>`
                }
            },
            {
                "title": "Foto", "data": null, className: "celdaCenter celda1", "render": function (data, type, row) {
                    var inicioBoton = '<div class="form-group">';
                    var Ver = `<img id="imgFoto_${contador}" src="${UrlImgProductos + row.FileName}" class=" img-circle" style="width: 90px;height:  90px; border-radius: 1px; padding: 1px; border:1px solid #021a40; -webkit-box-shadow: 3px 3px 3px rgba(0, 0, 0, 0.5); box-shadow: 3px 3px 3px rgba(0, 0, 0, 0.5);">`;
                    var finBoton = '</div>';
                    return inicioBoton + Ver + finBoton;
                }
            },

            {
                data: null, title: "Nombre", className: "celdaCenter celda1", render: function (row) {
                    return `<label>${row.FileName}</label>`

                }
            },
            {
                data: null, title: "Creado Por", className: "celdaCenter celda1", render: function (row) {
                    return `<label>${row.CreadoPor}</label>`

                }
            },
            {
                data: null, title: "Fecha Creación", className: "celdaCenter celda1", render: function (row) {
                    return `<label>${row.FechaCreacionS}</label>`

                }
            },
            {
                data: null, title: "Vigente", className: "celdaCenter celda1", render: function (row) {
                    if (row.Vigente === 0) {
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
        ordering: true,
        pageLength: 10,
        bLengthChange: true,
        searching: true,
        paging: true,
        info: true
    });


}
//Desactiva Producto
async function DesactivarImagen(identificacion) {

    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere desactivar el producto",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                ImagenId: identificacion,
                Vigente: 0
            }
            let Usuario = await UpdImagen(dto);
            GetProducto();
            GetProductoDesactivados();
            GetImagenes($("#IdProducto").val());
        }
    });


}
async function UpdImagen(dto) {

    let UpdImagen = await $.ajax({
        type: "POST",
        url: UrlUpdateImagen,
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

function RptExcelGeneralP() {
    $("#Process").show();
    UrlReporte = UrlReporteExcelGeneralP;
    setTimeout(function () {
        document.location = UrlReporte;
        $("#Process").hide();
    }, 2000);
}

function RptExcelVigentes() {
    $("#Process").show();
    UrlReporte = UrlReporteExcelVigentes;
    setTimeout(function () {
        document.location = UrlReporte;
        $("#Process").hide();
    }, 2000);
}

function RptExcelDesactivados() {
    $("#Process").show();
    UrlReporte = UrlReporteExcelDesactivados;
    setTimeout(function () {
        document.location = UrlReporte;
        $("#Process").hide();
    }, 2000);
}

