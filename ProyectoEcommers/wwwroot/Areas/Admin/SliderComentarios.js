$(document).ready(function () {
    GetImagenes();
    ConsultaImagen();
    $("#Process").hide();
});
function Ins_ImagenSlider() {

    const fileInput = document.getElementById('inputFile');
    const filePath = fileInput.files[0];

    if (!filePath) {
        create('success', 'Selecciona una imagen antes de intentar guardar',UrlAlertSucess);
        return;
    }
    var allowedExtensions = /(.doc|.docx|.exe|.pdf|.mp4|.png|.xls|.xlsx)$/i;

    if (allowedExtensions.exec(filePath)) {
        create('success', 'Formato no correcto debe cargar un imagen jpg o jpeg',UrlAlertSucess);
        fileInput.value = '';
        return false;
    } else {
        $("#notificacionDocumento").empty();

        Swal.fire({
            type: 'warning',
            title: '¡Estimado(a) Cliente!',
            text: "Esta seguro de de guardar la imagen",
            showCancelButton: true,
            cancelButtonText: "Cancelar",
            confirmButtonText: "¡Adelante!",
            closeOnConfirm: false
        }).then((result) => {
            if (result.value) {
                var token = document.getElementsByName("__RequestVerificationToken")[0].value;
                let Archivo = new FormData();
                let _FechaComentario = $("#txtFechaComentario").val();
                let _TituloComentario = $("#txtTituloComentario").val();
                let _Comentario = $("#txtComentario").val();

                if (_FechaComentario == null) {
                    create('success', 'Ingresar la fecha de registro',UrlAlertSucess);
                    return;
                }
                if (_TituloComentario == null) {
                    create('success', 'Ingresar el titulo del comentario',UrlAlertSucess);
                    return;
                }

                if (_Comentario == null) {
                    create('success', 'Ingresar el comentario de la imagen a cargar',UrlAlertSucess);
                    return;
                }


                Archivo.append('Archivo', $("#inputFile")[0].files[0]);
                Archivo.append('FechaComentario', _FechaComentario);
                Archivo.append('TituloComentario', _TituloComentario);
                Archivo.append('Comentario', _Comentario);
                Archivo.append('__RequestVerificationToken', token);
                $.ajax({
                    type: 'POST',
                    url: UrlInsArticuloComentarios,
                    dataType: 'json',
                    data: Archivo,
                    cache: false,
                    contentType: false,
                    processData: false,
                    enctype: 'multipart/form-data',
                    success: function (response) {
                        if (response.success == true) {

                            Swal.fire({
                                type: 'success',
                                title: 'Señor(a) Usuario(a:)',
                                text: response.message
                            });
                            window.location.reload();
                        }

                        else {
                            Swal.fire({
                                type: 'warning',
                                title: 'Señor(a) Usuario (a:)',
                                text: response.message
                            });
                            window.location.reload();
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
function ConsultaImagen() {

    $.ajax({
        type: 'POST',
        async: false,
        url: UrlConsultaImagenComentarios,
        success: function success(response) {
            if (response.success == true) {
                tipo = response.data;
                let contador = 0
                tipo.forEach((item) => {
                    contador++;
                    document.getElementById(`imgC${contador}`).setAttribute('src', `${UrlImgComentarios + item.Ruta}`);
                    document.getElementById(`fecha${contador}`).innerText = item.FechaComentario;
                    document.getElementById(`titulo${contador}`).innerText = item.TituloComentario;
                    document.getElementById(`comentario${contador}`).innerText = item.Comentario;

                });
            } else {

            }
        },
        error: function error(ex) {
        }
    });
}
function GetImagenes() {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbUsuarios")) {
        $("#tbUsuarios").DataTable().destroy();
    }
    $("#tbUsuarios").DataTable({
        "ajax": {
            type: "POST",
            url: UrlGetUsuariosComentarios,
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
                                                        <a class="dropdown-item" href="javascript:DesactivarImagen('${row.Consecutivo}')">
                                                        <i class="far fa-trash-alt" style="color:red"></i> Desactivar
                                                        </a>
                                                     </li>
                                                 </ul>
                                         </div>`
                }
            },
            { "title": "Id Imagen", "data": "Consecutivo", "name": "Consecutivo", className: "celdaCenter celda1" },
            { "title": "Archivo", "data": "FileName", "name": "FileName", className: "celdaCenter celda20" },
            { "title": "Vigente", "data": "Vigente", "name": "Vigente", className: "celdaJust celda1" },
            { "title": "Creado Por", "data": "Identificacion", "name": "Identificacion", className: "celdaCenter celda5" },
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
async function DesactivarImagen(identificacion) {

    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere desactivarla imaagen",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                Consecutivo: identificacion,
                Vigente: 0
            }
            let Usuario = await InsUsuarios(dto);
            GetImagenes();
            window.location.reload();
        }
    });


}
async function InsUsuarios(dto) {

    let InsertarUsuario = await $.ajax({
        type: "POST",
        url: UrlInsUsuariosComentarios,
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
    return InsertarUsuario;

}
function Limpiar() {
    window.location.reload();
}
function EditaEncuesta(_id) {
    $("#Alertas").empty();
    $("#IdEncuesta").val(combo);
    $.ajax({
        type: 'POST',
        url: UrlEditarEncuestaComentarios,
        success: function success(response) {
            if (response.success == true) {
                let container = document.querySelector('#PanelEncuesta');
                container.innerHTML = "";
                tipo.forEach((item) => {
                    container.innerHTML += `<a u=image href="#"><img src="@Url.Content(${item.Ruta})" alt="" /></a>`

                });
            } else {

            }
        },
        error: function error(ex) {
        }
    });
}


