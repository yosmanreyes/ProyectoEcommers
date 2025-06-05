

/*$(document).ready(function () {*/
    GetConsultaDominios();
/*});*/
$("#txtFechaNacimiento").kendoDatePicker({
    culture: "es-CO",
    interval: 1,
    animation: {
        close: {
            effects: "fadeOut zoom:out",
            duration: 300
        },
        open: {
            effects: "fadeIn zoom:in",
            duration: 300
        }
    }
});
function GetConsultaDominios() {
    $.ajax({
        type: "POST",
        url: UrlGetDominioRoles,
        async: true,        
        dataType: 'json',
        cache: false,
        success: function success(response) {
            if (response.success == true) {

                response.data.forEach((item) => {
                    $("#selectRoles").append(`<option value="${item.idDominio}">${item.descripcion}</option>`);
                });

                response.data1.forEach((item) => {
                    $("#selectCliente").append(`<option value="${item.idDominio}">${item.descripcion}</option>`);
                });

            } else {

            }
        },
        error: function error(ex) {
        }
    });
}
function GetEmpleado(_identifcaion) {

    let identificación = Number($("#txtIdentificacion").val());
    if (identificación < 1) {
        create('error', 'Debe digitar número de Identificación', UrlAlertError);
        return;
    }
    $.ajax({
        type: "POST",
        url: UrlGetTarjetaEmpleado,
        async: true,
        data: { _Identifcacion: $("#txtIdentificacion").val() },
        dataType: 'json',
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                $("#txtFuncionario").val(respuesta.data[0].Identificacion);
                $("#txtNombres").val(respuesta.data[0].Nombres);
                $("#txtApellidos").val(respuesta.data[0].Apellidos);
                $("#txtCorreo").val(respuesta.data[0].Correo);
                $("#txtDireccionResidencia").val(respuesta.data[0].DireccionResidencia);
                $("#txtCelular").val(respuesta.data[0].Celular);
                $("#txtFechaNacimiento").val(respuesta.data[0].FechaNacimiento);
                $("#txtClave").val(respuesta.data[0].Clave);
                $("#IdUsuario").val(respuesta.data[0].IdUsuario);

                if (respuesta.data[0].Vigente == "SI") {
                    CambioAbotonSi();
                } else {
                    CambioAbotonNO();
                }
                GetGrilla(identificación);
            } else {


                Swal.fire({
                    type: 'error',
                    title: '¡Estimado(a) Cliente!',
                    text: "No se Encontro el Funcionario"
                });
                btnLimpiar();
            }
        },
        error: function () {
            Swal.fire({
                type: 'error',
                title: '¡Estimado(a) Cliente!',
                text: 'No es posible grabar Revise con el Administrador del Sistema!!'
            });
        }
    });
}
function GetGrilla(Identificacion) {

    if ($.fn.dataTable.isDataTable("#tbGrilla")) {
        $("#tbGrilla").DataTable().destroy();
    }

    $("#tbGrilla").DataTable({
        "ajax": {
            type: "POST",
            url: UrlGetEmpleadoGrilla,
            async: true,
            data: { _Identifcacion: Identificacion },
            datatype: "json",
            cache: false
        },
        "initComplete": function (settings, json) {
            if (json.success) {
                $("#tbGrilla").removeClass('hidden');
            }
            else {
                $("#tbGrilla").DataTable().destroy();
            }
        },
        language: glOpcionesIdioma,
        responsive: true,
        "columns": [
            {
                data: null, title: "Acción", className: "celdaCenter celda1", render: function (row) {
                    var inicioBoton = '<div class="dropdown dropend"><button class="btn btn-success" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false" ><span class="fas fa-list"></span></button ><ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">';
                    if (row.Vigente == "NO") {
                        var Ver = `<li><a class="dropdown-item" href="javascript:DesactivarActivarUsuario('${row.IdUserRol}','${row.IdUsuario}','${row.IdRol}','SI')"><i class="fa fa-check-square-o" style="color:green"></i> Activar</a></li>`;
                    } else {
                        var Ver = `<li><a class="dropdown-item" href="javascript:DesactivarActivarUsuario('${row.IdUserRol}','${row.IdUsuario}','${row.IdRol}','NO')"><i class="far fa-trash-alt" style="color:red"></i> Desactivar</a></li>`;
                    }
                    var finBoton = '</ul></div>';
                    return resultado = inicioBoton + Ver + finBoton;
                }
            },

            { "title": "Id Rol", "data": "IdRol", "name": "IdRol", className: "celdaCenter celda2" },
            { "title": "Descripción", "data": "Descripcion", "name": "Descripcion", className: "celdaCenter celda2" },
            { "title": "Fecha Creación", "data": "FechaCreacion", "name": "FechaCreacion", className: "celdaCenter celda2" },
            { "title": "Vigente", "data": "Vigente", "name": "Vigente", className: "celdaCenter celda2" },


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
async function DesactivarActivarUsuario(IdUserRol, IdUsuario, IdRol, Vigente) {

    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: "Quiere desactivar el usuario",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                IdUserRol: IdUserRol,
                IdUsuario: IdUsuario,
                IdRol: IdRol,
                Vigente: Vigente,

            }
            let Usuario = await InsDesactivarUsuarios(dto);
            GetGrilla($("#txtIdentificacion").val());
        }
    });


}
async function InsDesactivarUsuarios(dto) {

    let InsertarUsuario = await $.ajax({
        type: "POST",
        url: UrlInsDesactivarUsuario,
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
                    title: 'Señor(a) Usuario(a:)',
                    text: respuesta.message
                });

            }
        },
        error: function () {
            Swal.fire({
                type: 'error',
                title: 'Señor(a) Usuario(a:)',
                text: 'Error al validar Usuario!!'
            });
        }
    });
    return InsertarUsuario;

}
function btnLimpiar() {
    if ($.fn.dataTable.isDataTable("#tbGrilla")) {
        $("#tbGrilla").DataTable().destroy();
    }
    $("#txtFuncionario").val("");
    $("#txtNombres").val("");
    $("#txtApellidos").val("");
    $("#txtCorreo").val("");
    $("#txtDireccionResidencia").val("");
    $("#txtCelular").val("");
    $("#txtFechaNacimiento").val("");
    $("#txtClave").val("");
    $("#IdUsuario").val(""),
        $("#selectRoles").val(0);
    $('#selectRoles').trigger('change.select2');
    $("#selectRoles").trigger("chosen:updated");
    $("#selectCliente").val(0);
    $('#selectCliente').trigger('change.select2');
    $("#selectCliente").trigger("chosen:updated");
}
function CambioAbotonSi() {
    $('#CheckEstadoUsuario').prop('checked', 1).trigger('change');
}
function CambioAbotonNO() {
    $('#CheckEstadoUsuario').prop('checked', 0).trigger('change');
}
//Modificar Usuario
async function ModalInsUsuarios() {

    let Mensaje = "";
    let Bloqueado = 0;
    let Vigente = "";
    let chequeo = CheckEstadoUsuario.checked;
    let Pass = "";
    let Claves = $("#txtClave").val();
    if (Claves == "") {
        Pass = "Majas12345";
    } else {
        Pass = Claves;
    }
    if (chequeo) {
        Mensaje = "Está a punto de desactivar el usuario";
        Bloqueado = 0;
        Vigente = "NO";
    }
    else {
        Mensaje = "Esta seguro de activar el usuario";
        Bloqueado = 1;
        Vigente = "SI";
    }


    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: Mensaje,
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                IdUsuario: $("#IdUsuario").val(),
                Bloqueado: Bloqueado,
                Identificacion: $("#txtIdentificacion").val(),
                Correo: $("#txtCorreo").val(),
                Apellidos: $("#txtApellidos").val(),
                Nombres: $("#txtNombres").val(),
                DireccionResidencia: $("#txtDireccionResidencia").val(),
                Cliente: $("#selectCliente").val(),
                Clave: Pass,
                Celular: $("#txtCelular").val(),
                FechaNacimiento: $("#txtFechaNacimiento").val(),
                Vigente: Vigente,
                IdRol: Number($("#selectRoles").val()),
            }
            let Usuario = await InsModificaUsuarios(dto);


        }
        else {
            if (chequeo) {
                CambioAbotonSi();
            }
            else {
                CambioAbotonNO();
            }
        }
    });





}
async function InsModificaUsuarios(dto) {

    let InsertarUsuario = await $.ajax({
        type: "POST",
        url: UrlUpdateUsuario,
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
                GetEmpleado($("#txtIdentificacion").val());
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
//signar usuario
async function InsAsignarUsuario() {

    let Vigente = 0;
    let Bloqueado = 0;
    let chequeo = CheckEstadoUsuario.checked;
    let Pass = "";
    let Claves = $("#txtClave").val();
    if (Claves == "") {
        Pass = "Majas12345";
    } else {
        Pass = Claves;
    }
    if (chequeo) {
        Bloqueado = 1;
        Vigente = "SI";
    }
    else {
        Bloqueado = 0;
        Vigente = "NO";
    }


    dto = {
        IdUsuario: $("#IdUsuario").val(),
        Bloqueado: Bloqueado,
        Identificacion: $("#txtIdentificacion").val(),
        Correo: $("#txtCorreo").val(),
        Apellidos: $("#txtApellidos").val(),
        Nombres: $("#txtNombres").val(),
        DireccionResidencia: $("#txtDireccionResidencia").val(),
        Cliente: $("#selectCliente").val(),
        Clave: Pass,
        Celular: $("#txtCelular").val(),
        FechaNacimiento: $("#txtFechaNacimiento").val(),
        Vigente: Vigente,
        IdRol: Number($("#selectRoles").val()),
    };

    let InsertarUsuario = await InsUsuario(dto);
    if (InsertarUsuario.success) {
        Swal.fire({
            type: 'success',
            title: '¡Estimado(a) Cliente!',
            text: InsertarUsuario.message
        });
        GetEmpleado($("#txtIdentificacion").val());

    } else {
        Swal.fire({
            type: 'warning',
            title: '¡Estimado(a) Cliente!',
            text: InsertarUsuario.message
        });
    }




}
async function InsUsuario(dto) {
    if (dto.Identificacion === "") {
        validacion = {
            success: false,
            message: "Debe registrar el número de identificación",
        }
        return validacion;
    };
    if (dto.IdRol === "") {
        validacion = {
            success: false,
            message: "Debe seleccionar un rol",
        }
        return validacion;
    }

    let InsertarUsuario = await $.ajax({
        type: "POST",
        url: UrlInsUsuario,
        async: true,
        data: dto,
        dataType: 'json',
        cache: false,
        success: function (respuesta) {

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
//Modificar Usuario
async function InsActualizaUsuarios() {

    let Mensaje = "";
    let Bloqueado = 0;
    let Vigente = "";
    let chequeo = CheckEstadoUsuario.checked;
    let Pass = "";
    let Claves = $("#txtClave").val();
    if (Claves == "") {
        Pass = "Majas12345";
    } else {
        Pass = Claves;
    }
    if (chequeo) {
        Mensaje = "Está a punto de actualizar la información del funcionario o estado";
        Bloqueado = 0;
        Vigente = "NO";
    }
    else {
        Mensaje = "Esta seguro de activar el usuario";
        Bloqueado = 1;
        Vigente = "SI";
    }


    Swal.fire({
        type: 'warning',
        title: '¡Estimado(a) Cliente!',
        text: Mensaje,
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonText: "¡Adelante!",
        closeOnConfirm: false
    }).then(async function (result) {

        if (result.value) {
            dto = {
                IdUsuario: $("#IdUsuario").val(),
                Bloqueado: Bloqueado,
                Identificacion: $("#txtIdentificacion").val(),
                Correo: $("#txtCorreo").val(),
                Apellidos: $("#txtApellidos").val(),
                Nombres: $("#txtNombres").val(),
                DireccionResidencia: $("#txtDireccionResidencia").val(),
                Cliente: $("#selectCliente").val(),
                Clave: Pass,
                Celular: $("#txtCelular").val(),
                FechaNacimiento: $("#txtFechaNacimiento").val(),
                Vigente: Vigente,
                IdRol: Number($("#selectRoles").val()),
            }
            let Usuario = await InsActualizaUsuarios(dto);


        }
        else {
            if (chequeo) {
                CambioAbotonSi();
            }
            else {
                CambioAbotonNO();
            }
        }
    });





}
async function InsActualizaUsuarios(dto) {
    let InsertarUsuario = await $.ajax({
        type: "POST",
        url: UrlUpdateUsuario,
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
                GetEmpleado($("#txtIdentificacion").val());
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

