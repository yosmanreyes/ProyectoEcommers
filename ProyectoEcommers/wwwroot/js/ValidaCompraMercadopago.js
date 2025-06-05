
$(document).ready(function () {

    verificarEstadoPagoID();

});


function verificarEstadoPagoID() {
    TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
    const TablapreferenceId = JSON.parse(localStorage.getItem("preferenceId_Consulta_Inicial"));

    if (!TablapreferenceId || typeof TablapreferenceId !== 'string' || TablapreferenceId.trim() === "") {
        return;  // Detener la ejecución si no es válido
    }

    // Crear el objeto de datos que vamos a enviar
    const data = {
        preference_id: TablapreferenceId,
        idCompra: TablaDatosNumeroCompra
    };

    fetch(urlConsultarEstadoPago, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'  // Definimos que el cuerpo será JSON
        },
        body: JSON.stringify(data)  // Convertimos el objeto a formato JSON
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {

                RedirecionaConsultaMiCompra2();
            } else {

            }
        })
        .catch(error => {
         /*   console.error("Error al verificar el estado de pago:", error);*/
        });
}


function RedirecionaConsultaMiCompra2() {
    const seccionDestino = document.getElementById('IdMisComprasEnvio2');
    const posicionDestino = seccionDestino.offsetTop;
    GetEnvioCorreo();
    limpiarCarrito3();
    window.scrollTo({
        top: posicionDestino,
        behavior: 'smooth'
    });
}

function limpiarCarrito3() {
    localStorage.removeItem("ListaCarrito");
    localStorage.removeItem("NumeroCompra");
    localStorage.removeItem("DatosClienteCompra");
    localStorage.removeItem("TablaDatosNumeroCompra");
    localStorage.removeItem("TablaDatosClienteCompra");
    localStorage.removeItem("preferenceId_Consulta_Inicial");
    $("#IdPais").val("");
    $("#txtNombre").val();
    $("#txtApellidos").val();
    $("#txtNombreEmpresa").val();
    $("#txtDireccion").val("");
    $("#txtoptionalDireccion").val("");
    $("#txtDepartamento2").val("");
    $("#selectCiudad").val("");
    $("#txtCodigoPostal").val("");
    $("#txtCelular").val("");
    $("#txtCorreoElectronico").val("");
    $("#txtComentario").val("");
    $("#IdPaisIndicativo").val("");



}

function GetEnvioCorreo() {


    TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));



    let data = { idCompra: TablaDatosNumeroCompra }
    $.post(UrlGetEnviarCorreoId, data).done(function (result) {

        if (result.data.length > 0) {

        }

        else {


        }

    }).fail()


}
