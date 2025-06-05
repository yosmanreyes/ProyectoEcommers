
$(document).ready(function () {
    GetGrillaMensaje();
    $("#Process").hide();
});
function Ins_Mensaje() {

    $("#notificacionDocumento").empty();
    let now = new Date();
    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Esta seguro de de guardar este Mensaje",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then((result) => {
        if (result.value) {

            let Archivo = new FormData();
            let _FechaComentario = now;
            let _TituloComentario = $("#txtTituloComentario").val();
            let _Comentario = $("#txtComentario").val();


            if (_TituloComentario == null || _TituloComentario == "") {
                create('success', 'Ingresa el titulo del mesaje',UrlAlertSucess);
                return;
            }

            if (_Comentario == null || _Comentario == "") {
                create('success', 'Ingrese el mensaje',UrlAlertSucess);
                return;
            }
            Archivo.append('TituloMensaje', _TituloComentario);
            Archivo.append('Mensaje', _Comentario);
            Archivo.append('Vigente', "SI");

            $.ajax({
                type: 'POST',
                url: UrlIns_Mensajes,
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
function GetGrillaMensaje() {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbUsuarios")) {
        $("#tbUsuarios").DataTable().destroy();
    }
    $("#tbUsuarios").DataTable({
        "ajax": {
            type: "POST",
            url: UrlGetMenajeGrilla,
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
                                                        <a class="dropdown-item" href="javascript:DesactivarMenaje('${row.Consecutivo}')">
                                                        <i class="far fa-trash-alt" style="color:red"></i> Desactivar
                                                        </a>
                                                     </li>
                                                 </ul>
                                         </div>`
                }
            },
            { "title": "Id Imagen", "data": "Consecutivo", "name": "Consecutivo", className: "celdaCenter celda1" },
            {
                data: null, title: "Titulo Mensaje Global", className: "celdaCenter celda1", render: function (row) {

                    if (row.TituloMensaje === null) {
                        $("#txtTituloComentario").attr("disabled", false);
                        return `<label></label>`
                    }
                    else {
                        $("#txtTituloComentario").attr("disabled", true);
                        $("#txtTituloComentario").val(row.TituloMensaje);
                        return `<label>${row.TituloMensaje}</label>`
                    }
                }
            },
            { "title": "Mensaje", "data": "Mensaje", "name": "Mensaje", className: "celdaJust celda7" },
            {
                data: null, title: "Vigente", className: "celdaCenter celda1", render: function (row) {

                    if (row.Vigente === "NO") {

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
async function DesactivarMenaje(identificacion) {
    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere desactivar el menaje",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                Consecutivo: identificacion,
                Vigente: "NO"
            }
            let Usuario = await InsMenaje(dto);
            GetGrillaMensaje
            window.location.reload();
        }
    });


}
async function InsMenaje(dto) {

    let InsertarUsuario = await $.ajax({
        type: "POST",
        url: UrlInsIdMensaje,
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


