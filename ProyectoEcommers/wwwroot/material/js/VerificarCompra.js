

//document.addEventListener("DOMContentLoaded", function () {
//    var boton = document.getElementById("checkout-btn");
//    if (boton) {
//        boton.click();
//    }
//});


//document.addEventListener("DOMContentLoaded", function () {
//    // Espera a que el documento esté completamente cargado
//    document.getElementById("checkout-btn").addEventListener("click", function (event) {
//        event.preventDefault(); // Prevenir la acción predeterminada del botón
//        Ins_compraProducto(); // Llamar a la función de validación y procesamiento
//    });
//});
//prueba();
//function prueba() {


//    const Toast = Swal.mixin({
//        toast: true,
//        position: "top-end",
//        showConfirmButton: false,
//        timer: 3000,
//        timerProgressBar: true,
//        didOpen: (toast) => {
//            toast.onmouseenter = Swal.stopTimer;
//            toast.onmouseleave = Swal.resumeTimer;
//        }
//    });
//    Toast.fire({
//        icon: "success",
//        title: "Pagína de Compra"
//    });
//}

var NumeroCompra = "";
var DatosClienteCompra = [];
var TablaDatosNumeroCompra = "";
var TablaDatosClienteCompra = [];
var preferenceId_Consulta_Inicial = "";
var TablapreferenceId = "";

if (PaginaPrincipal == 2) {
    CargarLista();
    CargarListaIndicativos();
    ConsultaCompraPendiente();
    ValidaCampos();

}
/*no Funciona*/
function Ins_compraProductoActivarMercadoPAgo() {

 
    // Obtener lista del carrito
    const listaP = JSON.parse(localStorage.getItem("ListaCarrito"));
    if (!listaP || listaP.length === 0) {
        create('error', 'Usted no tiene productos agregados al carrito, por favor agréguelos, gracias…', UrlAlertError);
        return;
    }

    // Obtener valores del formulario
    let _PaisV = $("#IdPais").val();
    let combo = document.getElementById("IdPais");
    let _Pais = combo.options[combo.selectedIndex].text;
    let _Indicativo = $("#IdPaisIndicativo").val();
    let _Nombres = $("#txtNombre").val();
    let _Apellidos = $("#txtApellidos").val();
    let _NombreEmpresa = $("#txtNombreEmpresa").val();
    let _Direccion = $("#txtDireccion").val();
    let _optionalDireccion = $("#txtoptionalDireccion").val();
    let _CodigoPostal = $("#txtCodigoPostal").val();
    let _Celular = $("#txtCelular").val();
    let _CorreoElectronico = $("#txtCorreoElectronico").val();
    let _Comentario = $("#txtComentario").val();
    let _Departamento2 = $("#txtDepartamento2").val();
    let _DepartamentoFinal = "";
    let _CiudadFinal = "";
    let _DepartamentoFinalV = "";
    let _CiudadFinalV = "";

    // Validaciones
    if (!_Pais) {
        create('error', 'Ingrese su país, por favor...', UrlAlertError);
        return;
    }
    if (!_Nombres) {
        create('error', 'Ingrese sus nombres, por favor...', UrlAlertError);
        return;
    }
    if (!_Apellidos) {
        create('error', 'Ingrese sus apellidos, por favor...', UrlAlertError);
        return;
    }
    if (!_Direccion) {
        create('error', 'Ingrese su dirección de residencia, por favor...', UrlAlertError);
        return;
    }
    if (_Pais === 'COLOMBIA') {
        _CodigoPostal = "0";
    } else {
        if (validarCodigoPostal() === 1) {
            create('error', 'Por favor, introduce un código postal válido (5 dígitos numéricos)...', UrlAlertError);
            return;
        }
        if (!_CodigoPostal) {
            create('error', 'Ingrese su código postal, por favor...', UrlAlertError);
            return;
        }
    }
    if (!_Indicativo || _Indicativo === 0) {
        create('error', 'seleccione el indicativo de su país, por favor...', UrlAlertError);
        return;
    }
    if (validarNumeroTelefono() === 1) {
        create('error', 'Por favor, su número de teléfono debe ser igual en los dos campos...', UrlAlertError);
        return;
    }
    if (validarCorreoVenta() === 1) {
        create('error', 'Por favor, introduce un correo electrónico válido...', UrlAlertError);
        return;
    }
    if (validarCorreos() === 1) {
        create('error', 'Los correos electrónicos no son iguales....', UrlAlertError);
        return;
    }
    if (!_Celular) {
        create('error', 'Ingrese su número celular, por favor...', UrlAlertError);
        return;
    }
    if (!_CorreoElectronico) {
        create('error', 'Ingrese su correo electrónico, por favor...', UrlAlertError);
        return;
    }

    // Validar y asignar departamento y ciudad
    if (_Pais === "COLOMBIA") {
        const combo2 = document.getElementById("selectDepartamento");
        const _Departamento = combo2.options[combo2.selectedIndex].text;
        const combo3 = document.getElementById("selectCiudad");
        const _Ciudad = combo3.options[combo3.selectedIndex].text;
        if (!_Departamento) {
            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
            return;
        }
        if (!_Ciudad) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinal = _Departamento;
        _CiudadFinal = _Ciudad;

        const _DepartamentoV = $("#selectDepartamento").val();
        const _CiudadV = $("#selectCiudad").val();
        if (!_DepartamentoV) {
            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
            return;
        }
        if (!_CiudadV) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinalV = _DepartamentoV;
        _CiudadFinalV = _CiudadV;
    } else {
        if (!_Departamento2) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinal = _Departamento2;
        _CiudadFinal = "";
        _DepartamentoFinalV = _Departamento2;
        _CiudadFinalV = "";
    }

    $("#IdMercadoPago").css("display", "block");
    $("#IdOcultaMercadoPago").css("display", "block");
    var boton = document.getElementById("checkout-btn");
    if (boton) {
        boton.click();
    }
    realizarConsultaPaginadoCompra();

    $("#IdOcultaRealizarPedido").css("display", "none");
    $("#IdOcultaMercadoPago").css("display", "block");
  
}


// Función para mostrar el modal de selección de pago
// Función para mostrar la modal y preguntar el método de pago
// Función para mostrar la modal y preguntar el método de pago
document.getElementById("realizarPedidoBtn").addEventListener("click", function () {
    // Abrir el modal con las opciones de pago

    let IdNumeroCompra = 0;
    const TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
    if (TablaDatosNumeroCompra) {
        IdNumeroCompra = TablaDatosNumeroCompra;

    }

    const listaP = JSON.parse(localStorage.getItem("ListaCarrito"));
    if (!listaP || listaP.length === 0) {
        create('error', 'Usted no tiene productos agregados al carrito, por favor agréguelos, gracias…', UrlAlertError);
        return;
    }
    // Obtener valores del formulario
    let _PaisV = $("#IdPais").val();
    let combo = document.getElementById("IdPais");
    let _Pais = combo.options[combo.selectedIndex].text;
    let _Indicativo = $("#IdPaisIndicativo").val();
    let _Nombres = $("#txtNombre").val();
    let _Apellidos = $("#txtApellidos").val();
    let _NombreEmpresa = $("#txtNombreEmpresa").val();
    let _Direccion = $("#txtDireccion").val();
    let _optionalDireccion = $("#txtoptionalDireccion").val();
    let _CodigoPostal = $("#txtCodigoPostal").val();
    let _Celular = $("#txtCelular").val();
    let _CorreoElectronico = $("#txtCorreoElectronico").val();
    let _Comentario = $("#txtComentario").val();
    let _Departamento2 = $("#txtDepartamento2").val();
    let _DepartamentoFinal = "";
    let _CiudadFinal = "";
    let _DepartamentoFinalV = "";
    let _CiudadFinalV = "";

    // Validaciones
    if (!_Pais) {
        create('error', 'Ingrese su país, por favor...', UrlAlertError);
        return;
    }
    if (!_Nombres) {
        create('error', 'Ingrese sus nombres, por favor...', UrlAlertError);
        return;
    }
    if (!_Apellidos) {
        create('error', 'Ingrese sus apellidos, por favor...', UrlAlertError);
        return;
    }
    if (!_Direccion) {
        create('error', 'Ingrese su dirección de residencia, por favor...', UrlAlertError);
        return;
    }
    if (_Pais === 'COLOMBIA') {
        _CodigoPostal = "0";
    } else {
        if (validarCodigoPostal() === 1) {
            create('error', 'Por favor, introduce un código postal válido (5 dígitos numéricos)...', UrlAlertError);
            return;
        }
        if (!_CodigoPostal) {
            create('error', 'Ingrese su código postal, por favor...', UrlAlertError);
            return;
        }
    }
    if (!_Indicativo || _Indicativo === 0) {
        create('error', 'seleccione el indicativo de su país, por favor...', UrlAlertError);
        return;
    }
    if (validarNumeroTelefono() === 1) {
        create('error', 'Por favor, su número de teléfono debe ser igual en los dos campos...', UrlAlertError);
        return;
    }
    if (validarCorreoVenta() === 1) {
        create('error', 'Por favor, introduce un correo electrónico válido...', UrlAlertError);
        return;
    }
    if (validarCorreos() === 1) {
        create('error', 'Los correos electrónicos no son iguales....', UrlAlertError);
        return;
    }
    if (!_Celular) {
        create('error', 'Ingrese su número celular, por favor...', UrlAlertError);
        return;
    }
    if (!_CorreoElectronico) {
        create('error', 'Ingrese su correo electrónico, por favor...', UrlAlertError);
        return;
    }

    // Validar y asignar departamento y ciudad
    if (_Pais === "COLOMBIA") {
        const combo2 = document.getElementById("selectDepartamento");
        const _Departamento = combo2.options[combo2.selectedIndex].text;
        const combo3 = document.getElementById("selectCiudad");
        const _Ciudad = combo3.options[combo3.selectedIndex].text;
        if (!_Departamento) {
            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
            return;
        }
        if (!_Ciudad) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinal = _Departamento;
        _CiudadFinal = _Ciudad;

        const _DepartamentoV = $("#selectDepartamento").val();
        const _CiudadV = $("#selectCiudad").val();
        if (!_DepartamentoV) {
            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
            return;
        }
        if (!_CiudadV) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinalV = _DepartamentoV;
        _CiudadFinalV = _CiudadV;
    } else {
        if (!_Departamento2) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinal = _Departamento2;
        _CiudadFinal = "";
        _DepartamentoFinalV = _Departamento2;
        _CiudadFinalV = "";
    }



    $('#paymentModal').modal('show');
});

// Función para manejar la selección de Mercado Pago
document.getElementById("mercadoPagoBtn").addEventListener("click", function () {
    // Cerrar la modal
    $('#paymentModal').modal('hide');

    // Mostrar la sección de Mercado Pago
    document.getElementById("IdOcultaMercadoPago").style.display = "block";
    document.getElementById("IdMercadoPago").style.display = "block";

    // Llamar a la función Ins_compraProducto()
    Ins_compraProductoMercadoPago();
});

// Función para manejar la selección de recoger en el negocio
// Función para manejar la acción del botón "Recoger en el negocio"
document.getElementById("recogerBtn").addEventListener("click", function () {
    // Mostrar la modal con el mensaje bonito cuando el usuario hace clic
    $('#recogerModal').modal('show');
    $('#paymentModal').modal('hide');
    Ins_compraProducto();
});

// Acción cuando el usuario hace clic en "¡Entendido!"
document.getElementById("confirmarBtn").addEventListener("click", function () {
    // Cerrar la modal de recoger el producto
    $('#recogerModal').modal('hide');
    $('#paymentModal').modal('hide');
    if (PaginaPrincipal == 2) {
        CargarLista();
        CargarListaIndicativos();
        ConsultaCompraPendiente();
        ValidaCampos();
        limpiarCarritoCompra();

    }

    // Llamar a la función para completar la compra (puedes ajustarlo a tu flujo)

});

// Acción cuando el usuario quiere chatear por WhatsApp
document.getElementById("whatsappBtn").addEventListener("click", function () {
    // Redirigir a WhatsApp para continuar con la compra
    $('#recogerModal').modal('hide');
    $('#paymentModal').modal('hide');
    if (PaginaPrincipal == 2) {
        CargarLista();
        CargarListaIndicativos();
        ConsultaCompraPendiente();
        ValidaCampos();
        limpiarCarritoCompra();

    }
    window.open('https://api.whatsapp.com/send?phone=573124219313', '_blank');  // Reemplaza el número con el de tu negocio
});





function Ins_compraProductoMercadoPago() {
    // Inicializar ID de compra
    let IdNumeroCompra = 0;
    const TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
    if (TablaDatosNumeroCompra) {
        IdNumeroCompra = TablaDatosNumeroCompra;
       
    }

   
  /*  var preferenceId = document.getElementById('preferenceId');*/
    //const TablapreferenceId = JSON.parse(localStorage.getItem("preferenceId_Consulta_Inicial"));
    //if (TablapreferenceId) {
    //    preferenceId = TablapreferenceId;

    //}
    
    // Obtener lista del carrito
    const listaP = JSON.parse(localStorage.getItem("ListaCarrito"));
    if (!listaP || listaP.length === 0) {
        create('error', 'Usted no tiene productos agregados al carrito, por favor agréguelos, gracias…', UrlAlertError);
        return;
    }

    // Obtener valores del formulario
    let _PaisV = $("#IdPais").val();
    let combo = document.getElementById("IdPais");
    let _Pais = combo.options[combo.selectedIndex].text;
    let _Indicativo = $("#IdPaisIndicativo").val();
    let _Nombres = $("#txtNombre").val();
    let _Apellidos = $("#txtApellidos").val();
    let _NombreEmpresa = $("#txtNombreEmpresa").val();
    let _Direccion = $("#txtDireccion").val();
    let _optionalDireccion = $("#txtoptionalDireccion").val();
    let _CodigoPostal = $("#txtCodigoPostal").val();
    let _Celular = $("#txtCelular").val();
    let _CorreoElectronico = $("#txtCorreoElectronico").val();
    let _Comentario = $("#txtComentario").val();
    let _Departamento2 = $("#txtDepartamento2").val();
    let _DepartamentoFinal = "";
    let _CiudadFinal = "";
    let _DepartamentoFinalV = "";
    let _CiudadFinalV = "";

    // Validaciones
    if (!_Pais) {
        create('error', 'Ingrese su país, por favor...', UrlAlertError);
        return;
    }
    if (!_Nombres) {
        create('error', 'Ingrese sus nombres, por favor...', UrlAlertError);
        return;
    }
    if (!_Apellidos) {
        create('error', 'Ingrese sus apellidos, por favor...', UrlAlertError);
        return;
    }
    if (!_Direccion) {
        create('error', 'Ingrese su dirección de residencia, por favor...', UrlAlertError);
        return;
    }
    if (_Pais === 'COLOMBIA') {
        _CodigoPostal = "0";
    } else {
        if (validarCodigoPostal() === 1) {
            create('error', 'Por favor, introduce un código postal válido (5 dígitos numéricos)...', UrlAlertError);
            return;
        }
        if (!_CodigoPostal) {
            create('error', 'Ingrese su código postal, por favor...', UrlAlertError);
            return;
        }
    }
    if (!_Indicativo || _Indicativo === 0) {
        create('error', 'seleccione el indicativo de su país, por favor...', UrlAlertError);
        return;
    }
    if (validarNumeroTelefono() === 1) {
        create('error', 'Por favor, su número de teléfono debe ser igual en los dos campos...', UrlAlertError);
        return;
    }
    if (validarCorreoVenta() === 1) {
        create('error', 'Por favor, introduce un correo electrónico válido...', UrlAlertError);
        return;
    }
    if (validarCorreos() === 1) {
        create('error', 'Los correos electrónicos no son iguales....', UrlAlertError);
        return;
    }
    if (!_Celular) {
        create('error', 'Ingrese su número celular, por favor...', UrlAlertError);
        return;
    }
    if (!_CorreoElectronico) {
        create('error', 'Ingrese su correo electrónico, por favor...', UrlAlertError);
        return;
    }

    // Validar y asignar departamento y ciudad
    if (_Pais === "COLOMBIA") {
        const combo2 = document.getElementById("selectDepartamento");
        const _Departamento = combo2.options[combo2.selectedIndex].text;
        const combo3 = document.getElementById("selectCiudad");
        const _Ciudad = combo3.options[combo3.selectedIndex].text;
        if (!_Departamento) {
            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
            return;
        }
        if (!_Ciudad) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinal = _Departamento;
        _CiudadFinal = _Ciudad;

        const _DepartamentoV = $("#selectDepartamento").val();
        const _CiudadV = $("#selectCiudad").val();
        if (!_DepartamentoV) {
            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
            return;
        }
        if (!_CiudadV) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinalV = _DepartamentoV;
        _CiudadFinalV = _CiudadV;
    } else {
        if (!_Departamento2) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinal = _Departamento2;
        _CiudadFinal = "";
        _DepartamentoFinalV = _Departamento2;
        _CiudadFinalV = "";
    }

    // Obtener costo de envío
    const CostoEnvio = ConvertirEnFloat($("#IdCostoEnvio strong").text());
    TotalPrecioEnvio($("#IdCostoEnvio strong").text());
    // Mostrar elemento de MercadoPago
    $("#IdMercadoPago").css("display", "block");
    $("#IdOcultaMercadoPago").css("display", "block");
    var token = document.getElementsByName("__RequestVerificationToken")[0].value; // Usa el nombre del encabezado configurado
    // Crear objeto FormData con los datos del formulario
    const data = new FormData();
    data.append('Pais', _Pais);
    data.append('Nombres', _Nombres);
    data.append('Apellidos', _Apellidos);
    data.append('Nombre_Empresa', _NombreEmpresa);
    data.append('Direccion', _Direccion);
    data.append('Opcional_Direccion', _optionalDireccion);
    data.append('Departamento', _DepartamentoFinal);
    data.append('Ciudad', _CiudadFinal);
    data.append('Codigo_Postal', _CodigoPostal);
    data.append('Celular', _Celular);
    data.append('Correo_Electronico', _CorreoElectronico);
    data.append('Comentario', _Comentario);
    data.append('IdPaisIndicativo', _Indicativo);
    data.append('CostoEnvio', CostoEnvio);
    data.append('id_compra_realizada', IdNumeroCompra);   
    data.append('EntregaMercanciaLocal', "NO");  
    data.append('__RequestVerificationToken', token);

    // Crear objeto de información para almacenar en localStorage
    const informacion = {
        Pais: _PaisV,
        Nombres: _Nombres,
        Apellidos: _Apellidos,
        Nombre_Empresa: _NombreEmpresa,
        Direccion: _Direccion,
        Opcional_Direccion: _optionalDireccion,
        Departamento: _DepartamentoFinalV,
        Ciudad: _CiudadFinalV,
        Codigo_Postal: _CodigoPostal,
        Celular: _Celular,
        Correo_Electronico: _CorreoElectronico,
        Comentario: _Comentario,
        IdPaisIndicativo: _Indicativo,
        CostoEnvio: CostoEnvio,
        EntregaMercanciaLocal: "NO",
    };

    // Enviar datos al servidor
    $.ajax({
        type: 'POST',
        url: urlInsProductoCompra,
        dataType: 'json',
        data: data,
        cache: false,
        contentType: false,
        processData: false,
        enctype: 'multipart/form-data',
        success: function (response) {
            if (response.success) {
                var boton = document.getElementById("checkout-btn");
                if (boton) {
                    boton.click();
                }
                const Respuesta = response.data;
                Ins_carritoProductos(Respuesta);
                localStorage.removeItem("NumeroCompra");
                localStorage.setItem("NumeroCompra", JSON.stringify(Respuesta));
                localStorage.removeItem("DatosClienteCompra");
                localStorage.setItem("DatosClienteCompra", JSON.stringify(informacion));              
              

                realizarConsultaPaginadoCompra();
                $("#IdOcultaRealizarPedido").css("display", "none");
                $("#IdOcultaMercadoPago").css("display", "block");
            
                
                return;
            } else {
                create('error', 'No fue posible guardar su comentario.', UrlAlertError);
                return;
            }
        },
    });
}

function Ins_compraProducto() {
    // Inicializar ID de compra
    let IdNumeroCompra = 0;
    const TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
    if (TablaDatosNumeroCompra) {
        IdNumeroCompra = TablaDatosNumeroCompra;

    }


    /*  var preferenceId = document.getElementById('preferenceId');*/
    //const TablapreferenceId = JSON.parse(localStorage.getItem("preferenceId_Consulta_Inicial"));
    //if (TablapreferenceId) {
    //    preferenceId = TablapreferenceId;

    //}

    // Obtener lista del carrito
    const listaP = JSON.parse(localStorage.getItem("ListaCarrito"));
    if (!listaP || listaP.length === 0) {
        create('error', 'Usted no tiene productos agregados al carrito, por favor agréguelos, gracias…', UrlAlertError);
        return;
    }

    // Obtener valores del formulario
    let _PaisV = $("#IdPais").val();
    let combo = document.getElementById("IdPais");
    let _Pais = combo.options[combo.selectedIndex].text;
    let _Indicativo = $("#IdPaisIndicativo").val();
    let _Nombres = $("#txtNombre").val();
    let _Apellidos = $("#txtApellidos").val();
    let _NombreEmpresa = $("#txtNombreEmpresa").val();
    let _Direccion = $("#txtDireccion").val();
    let _optionalDireccion = $("#txtoptionalDireccion").val();
    let _CodigoPostal = $("#txtCodigoPostal").val();
    let _Celular = $("#txtCelular").val();
    let _CorreoElectronico = $("#txtCorreoElectronico").val();
    let _Comentario = $("#txtComentario").val();
    let _Departamento2 = $("#txtDepartamento2").val();
    let _DepartamentoFinal = "";
    let _CiudadFinal = "";
    let _DepartamentoFinalV = "";
    let _CiudadFinalV = "";

    // Validaciones
    if (!_Pais) {
        create('error', 'Ingrese su país, por favor...', UrlAlertError);
        return;
    }
    if (!_Nombres) {
        create('error', 'Ingrese sus nombres, por favor...', UrlAlertError);
        return;
    }
    if (!_Apellidos) {
        create('error', 'Ingrese sus apellidos, por favor...', UrlAlertError);
        return;
    }
    if (!_Direccion) {
        create('error', 'Ingrese su dirección de residencia, por favor...', UrlAlertError);
        return;
    }
    if (_Pais === 'COLOMBIA') {
        _CodigoPostal = "0";
    } else {
        if (validarCodigoPostal() === 1) {
            create('error', 'Por favor, introduce un código postal válido (5 dígitos numéricos)...', UrlAlertError);
            return;
        }
        if (!_CodigoPostal) {
            create('error', 'Ingrese su código postal, por favor...', UrlAlertError);
            return;
        }
    }
    if (!_Indicativo || _Indicativo === 0) {
        create('error', 'seleccione el indicativo de su país, por favor...', UrlAlertError);
        return;
    }
    if (validarNumeroTelefono() === 1) {
        create('error', 'Por favor, su número de teléfono debe ser igual en los dos campos...', UrlAlertError);
        return;
    }
    if (validarCorreoVenta() === 1) {
        create('error', 'Por favor, introduce un correo electrónico válido...', UrlAlertError);
        return;
    }
    if (validarCorreos() === 1) {
        create('error', 'Los correos electrónicos no son iguales....', UrlAlertError);
        return;
    }
    if (!_Celular) {
        create('error', 'Ingrese su número celular, por favor...', UrlAlertError);
        return;
    }
    if (!_CorreoElectronico) {
        create('error', 'Ingrese su correo electrónico, por favor...', UrlAlertError);
        return;
    }

    // Validar y asignar departamento y ciudad
    if (_Pais === "COLOMBIA") {
        const combo2 = document.getElementById("selectDepartamento");
        const _Departamento = combo2.options[combo2.selectedIndex].text;
        const combo3 = document.getElementById("selectCiudad");
        const _Ciudad = combo3.options[combo3.selectedIndex].text;
        if (!_Departamento) {
            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
            return;
        }
        if (!_Ciudad) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinal = _Departamento;
        _CiudadFinal = _Ciudad;

        const _DepartamentoV = $("#selectDepartamento").val();
        const _CiudadV = $("#selectCiudad").val();
        if (!_DepartamentoV) {
            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
            return;
        }
        if (!_CiudadV) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinalV = _DepartamentoV;
        _CiudadFinalV = _CiudadV;
    } else {
        if (!_Departamento2) {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }
        _DepartamentoFinal = _Departamento2;
        _CiudadFinal = "";
        _DepartamentoFinalV = _Departamento2;
        _CiudadFinalV = "";
    }

    // Obtener costo de envío
/*    const CostoEnvio = ConvertirEnFloat($("#IdCostoEnvio strong").text());*/

    let CostoEnvio = ConvertirEnFloat($("#IdCostoEnvio strong").text());  // Obtienes el costo original
    CostoEnvio = 0;  // Lo actualizas a 0

    // Actualizas el texto en el DOM
    $("#IdCostoEnvio strong").text(CostoEnvio.toFixed(2));
    TotalPrecioEnvio(CostoEnvio);
    // Mostrar elemento de MercadoPago
    $("#IdMercadoPago").css("display", "none");
    $("#IdOcultaMercadoPago").css("display", "none");
    var token = document.getElementsByName("__RequestVerificationToken")[0].value; // Usa el nombre del encabezado configurado
    // Crear objeto FormData con los datos del formulario
    const data = new FormData();
    data.append('Pais', _Pais);
    data.append('Nombres', _Nombres);
    data.append('Apellidos', _Apellidos);
    data.append('Nombre_Empresa', _NombreEmpresa);
    data.append('Direccion', _Direccion);
    data.append('Opcional_Direccion', _optionalDireccion);
    data.append('Departamento', _DepartamentoFinal);
    data.append('Ciudad', _CiudadFinal);
    data.append('Codigo_Postal', _CodigoPostal);
    data.append('Celular', _Celular);
    data.append('Correo_Electronico', _CorreoElectronico);
    data.append('Comentario', _Comentario);
    data.append('IdPaisIndicativo', _Indicativo);
    data.append('CostoEnvio', CostoEnvio);
    data.append('id_compra_realizada', IdNumeroCompra);
    data.append('EntregaMercanciaLocal', "SI");
    data.append('__RequestVerificationToken', token);

    // Crear objeto de información para almacenar en localStorage
    const informacion = {
        Pais: _PaisV,
        Nombres: _Nombres,
        Apellidos: _Apellidos,
        Nombre_Empresa: _NombreEmpresa,
        Direccion: _Direccion,
        Opcional_Direccion: _optionalDireccion,
        Departamento: _DepartamentoFinalV,
        Ciudad: _CiudadFinalV,
        Codigo_Postal: _CodigoPostal,
        Celular: _Celular,
        Correo_Electronico: _CorreoElectronico,
        Comentario: _Comentario,
        IdPaisIndicativo: _Indicativo,
        CostoEnvio: CostoEnvio,
        EntregaMercanciaLocal: "SI"
    };

    // Enviar datos al servidor
    $.ajax({
        type: 'POST',
        url: urlInsProductoCompra,
        dataType: 'json',
        data: data,
        cache: false,
        contentType: false,
        processData: false,
        enctype: 'multipart/form-data',
        success: function (response) {
            if (response.success) {
                var boton = document.getElementById("checkout-btn");
                if (boton) {
                    boton.click();
                }
                const Respuesta = response.data;
                Ins_carritoProductosCompraLocal(Respuesta);
                //localStorage.removeItem("NumeroCompra");
                //localStorage.setItem("NumeroCompra", JSON.stringify(Respuesta));
                //localStorage.removeItem("DatosClienteCompra");
                //localStorage.setItem("DatosClienteCompra", JSON.stringify(informacion));


                realizarConsultaPaginadoCompra();
                $("#IdOcultaRealizarPedido").css("display", "none");
                $("#IdOcultaMercadoPago").css("display", "none");
        

                return;
            } else {
                create('error', 'No fue posible guardar su comentario.', UrlAlertError);
                return;
            }
        },
    });
}

function limpiarCarritoCompra() {
    localStorage.removeItem("ListaCarrito");
    localStorage.removeItem("NumeroCompra");
    localStorage.removeItem("DatosClienteCompra");
    localStorage.removeItem("TablaDatosNumeroCompra");
    localStorage.removeItem("TablaDatosClienteCompra");
    localStorage.removeItem("preferenceId_Consulta_Inicial");
    $("#IdPais").val("");
    $("#txtNombre").val("");
    $("#txtApellidos").val("");
    $("#txtNombreEmpresa").val("");
    $("#txtDireccion").val("");
    $("#txtoptionalDireccion").val("");
    $("#txtDepartamento2").val("");
    $("#selectCiudad").val("");
    $("#txtCodigoPostal").val("");
    $("#txtCelular").val("");
    $("#txtCorreoElectronico").val("");
    $("#txtComentario").val("");
    $("#IdPaisIndicativo").val("");
    $("#txtCelulaConfirmacion").val("");
    $("#txtCorreoElectronicoConfirmacion").val("");

    
    
    



}
//function Ins_compraProducto() {

//    let IdNumeroCompra = 0;
//    TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
//    if (TablaDatosNumeroCompra == null) {

//    }else{
//        IdNumeroCompra = TablaDatosNumeroCompra;
//    }




//    $("#IdMercadoPago").css("display", "block");
//    let listaP = JSON.parse(localStorage.getItem("ListaCarrito"));
//    if (listaP == "" || listaP == null) {
//        create('error', 'Usted no tiene productos agregados al carrito, por favor agréguelos, gracias…', UrlAlertError);
//        return;
//    }


//    let _PaisV = $("#IdPais").val();

//    var combo = document.getElementById("IdPais");
//    let _Pais = combo.options[combo.selectedIndex].text;

//    var Indicativo = $("#IdPaisIndicativo").val();
//    let _Indicativo = Indicativo;

//    let _Nombres = $("#txtNombre").val();
//    let _Apellidos = $("#txtApellidos").val();
//    let _NombreEmpresa = $("#txtNombreEmpresa").val();
//    let _Direccion = $("#txtDireccion").val();
//    let _optionalDireccion = $("#txtoptionalDireccion").val();
//    let _CodigoPostal = $("#txtCodigoPostal").val();
//    let _Celular = $("#txtCelular").val();
//    let _CorreoElectronico = $("#txtCorreoElectronico").val();
//    let _Comentario = $("#txtComentario").val();
//    let _Departamento2 = $("#txtDepartamento2").val();
//    let _DepartamentoFinal = "";
//    let _CiudadFinal = "";

//    let _DepartamentoFinalV = "";
//    let _CiudadFinalV = "";

//    if (_Pais == "") {
//        create('error', 'Ingrese su pais, por favor...', UrlAlertError);
//        return;
//    }
//    if (_Nombres == "") {
//        create('error', 'Ingrese sus nombres, por favor...', UrlAlertError);
//        return;
//    }

//    if (_Apellidos == "") {
//        create('error', 'Ingrese sus apellidos, por favor...', UrlAlertError);
//        return;
//    }

//    if (_Direccion == "") {
//        create('error', 'Ingrese su direccion residencia, por favor...', UrlAlertError);
//        return;
//    }



//    if (_Pais == 'COLOMBIA') {
//        _CodigoPostal == 0;
//    } else {
//        var returP = validarCodigoPostal();

//        if (returP == 1) {
//            create('error', 'Por favor, introduce un código postal válido (5 dígitos numéricos)...', UrlAlertError);
//            return;
//        }

//        if (_CodigoPostal == "") {
//            create('error', 'Ingrese su código postal, por favor...', UrlAlertError);
//            return;
//        }
//    }






//    if (_Indicativo == "" || _Indicativo == 0) {
//        create('error', 'seleccione el indicativo de su pais, por favor...', UrlAlertError);
//        return;
//    }

//    var returT = validarNumeroTelefono();

//    if (returT == 1) {
//        create('error', 'Por favor, su número de teléfono debe ser igual en los dos campos...', UrlAlertError);
//        return;
//    }
//    var returV = validarCorreoVenta();

//    if (returV == 1) {
//        create('error', 'Por favor, introduce un correo electrónico válido...', UrlAlertError);
//        return;
//    }

//    var returC = validarCorreos()
//    if (returC == 1) {
//        create('error', 'Los correos electrónicos no son iguales....', UrlAlertError);
//        return;
//    }

//    if (_Celular == "") {
//        create('error', 'Ingrese su número celular, por favor...', UrlAlertError);
//        return;
//    }
//    if (_CorreoElectronico == "") {
//        create('error', 'Ingrese su correo electrónico, por favor...', UrlAlertError);
//        return;
//    }
//    if (_Pais == "COLOMBIA") {
//        var combo2 = document.getElementById("selectDepartamento");
//        let _Departamento = combo2.options[combo2.selectedIndex].text;
//        var combo3 = document.getElementById("selectCiudad");
//        let _Ciudad = combo3.options[combo3.selectedIndex].text;
//        if (_Departamento == "") {

//            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
//            return;
//        }
//        if (_Ciudad == "") {
//            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
//            return;
//        }
//        _DepartamentoFinal = _Departamento;
//        _CiudadFinal = _Ciudad;

//    } else {
//        if (_Departamento2 == "") {
//            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
//            return;
//        }

//        _DepartamentoFinal = _Departamento2;
//        _CiudadFinal = "";

//    }

//    if (_Pais == "COLOMBIA") {



//        let _DepartamentoV = $("#selectDepartamento").val();
//        let _CiudadV = $("#selectCiudad").val();
//        if (_DepartamentoV == "") {

//            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
//            return;
//        }
//        if (_CiudadV == "") {
//            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
//            return;
//        }
//        _DepartamentoFinalV = _DepartamentoV;
//        _CiudadFinalV = _CiudadV;

//    } else {
//        if (_Departamento2 == "") {
//            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
//            return;
//        }

//        _DepartamentoFinalV = _Departamento2;
//        _CiudadFinalV = "";

//    }

//    var CostoEnvio = ConvertirEnFloat($("#IdCostoEnvio strong").text());



//    let data = new FormData();
//    data.append('Pais', _Pais);
//    data.append('Nombres', _Nombres);
//    data.append('Apellidos', _Apellidos);
//    data.append('Nombre_Empresa', _NombreEmpresa);
//    data.append('Direccion', _Direccion);
//    data.append('Opcional_Direccion', _optionalDireccion);
//    data.append('Departamento', _DepartamentoFinal);
//    data.append('Ciudad', _CiudadFinal);
//    data.append('Codigo_Postal', _CodigoPostal);
//    data.append('Celular', _Celular);
//    data.append('Correo_Electronico', _CorreoElectronico);
//    data.append('Comentario', _Comentario);
//    data.append('IdPaisIndicativo', Indicativo);
//    data.append('CostoEnvio', CostoEnvio);
//    data.append('id_compra_realizada', IdNumeroCompra);





//    var informacion = {

//        Pais: _PaisV,
//        Nombres: _Nombres,
//        Apellidos: _Apellidos,
//        Nombre_Empresa: _NombreEmpresa,
//        Direccion: _Direccion,
//        Opcional_Direccion: _optionalDireccion,
//        Departamento: _DepartamentoFinalV,
//        Ciudad: _CiudadFinalV,
//        Codigo_Postal: _CodigoPostal,
//        Celular: _Celular,
//        Correo_Electronico: _CorreoElectronico,
//        Comentario: _Comentario,
//        IdPaisIndicativo: Indicativo,
//        CostoEnvio: CostoEnvio,


//    }

//    $.ajax({
//        type: 'POST',
//        url: urlInsProductoCompra,
//        dataType: 'json',
//        data: data,
//        cache: false,
//        contentType: false,
//        processData: false,
//        enctype: 'multipart/form-data',
//        success: function (response) {
//            if (response.success == true) {

//                var Respuesta = response.data;
//                Ins_carritoProductos(Respuesta)
//                localStorage.removeItem("NumeroCompra");
//                localStorage.setItem("NumeroCompra", JSON.stringify(Respuesta));

//                localStorage.removeItem("DatosClienteCompra");
//                localStorage.setItem("DatosClienteCompra", JSON.stringify(informacion));

//                var boton = document.getElementById("checkout-btn");
//                boton.click();
//                return;
//            }

//            else {

//                create('error', 'No fue posible guardar su comentario.', UrlAlertError);
//                return;
//            }
//        },
//    });
//}

function Ins_carritoProductos(_id) {
    // Obtener el costo de envío
    const CostoEnvio = ConvertirEnFloat($("#IdCostoEnvio strong").text());

    // Obtener la lista del carrito desde el localStorage
    const lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    const Validar = [];
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    // Crear los objetos para cada producto en el carrito
    lista.forEach(item => {
        const ListaDto = {
            id_carrito: 0,
            id_compra_realizada: _id,
            id_producto: item.ProductoId,
            Descripcion_Producto: item.Descripcion,
            Cantidad: item.cantidadProducto,
            precio_Unitario: item.Precio,
            Precio_Total: item.precioTotal,
            Precio_Envio: CostoEnvio,
            Descuento: 0,
            Cancelado: "NO"
        };
        Validar.push(ListaDto);
    });

    // Preparar los datos para el envío
    const data = { Validar, __RequestVerificationToken: token };

    // Enviar los datos al servidor
    $.post(urlInsCarritos, data)
        .done(response => {
            if (response.success) {  
                // Acciones en caso de éxito
                /* limpiarCarrito(); */
                /* createCompra('success', 'MAJAS, agradece por su compra, en su WhatsApp encotraras los detalles de la compra, el vendedor tomara contacto para validar la forma de pago...', UrlAlertSucess); */
            } else {            
            }
        })
        .fail(() => {
           
        });
}

function Ins_carritoProductosCompraLocal(_id) {
    // Obtener el costo de envío
    const CostoEnvio = ConvertirEnFloat($("#IdCostoEnvio strong").text());

    // Obtener la lista del carrito desde el localStorage
    const lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    const Validar = [];
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    // Crear los objetos para cada producto en el carrito
    lista.forEach(item => {
        const ListaDto = {
            id_carrito: 0,
            id_compra_realizada: _id,
            id_producto: item.ProductoId,
            Descripcion_Producto: item.Descripcion,
            Cantidad: item.cantidadProducto,
            precio_Unitario: item.Precio,
            Precio_Total: item.precioTotal,
            Precio_Envio: CostoEnvio,
            Descuento: 0,
            Cancelado: "NO"
        };
        Validar.push(ListaDto);
    });

    // Preparar los datos para el envío
    const data = { Validar, __RequestVerificationToken: token };

    // Enviar los datos al servidor
    $.post(urlInsCarritos, data)
        .done(response => {
            if (response.success) {
                GetEnvioCorreo();
                // Acciones en caso de éxito
                /* limpiarCarrito(); */
                /* createCompra('success', 'MAJAS, agradece por su compra, en su WhatsApp encotraras los detalles de la compra, el vendedor tomara contacto para validar la forma de pago...', UrlAlertSucess); */
            } else {
            }
        })
        .fail(() => {

        });
}


function GetEnvioCorreo() {


    TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));



    let data = { idCompra: TablaDatosNumeroCompra }
    $.post(UrlGetEnviarCorreoId, data).done(function (result) {

        if (result.data.length > 0) {
            limpiarCarritoCompra();
        }

        else {


        }

    }).fail()





    //lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    //if (lista != null) {
    //    $("#IdtablaCarrito2 tbody tr").remove();
    //    lista.forEach((item) => {

    //    });
    //}
}

function Ins_ActualiacarritoProductosMerddoLibre(_preferenceId, _NumeroCompra) {
    // Obtener el costo de envío
   
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;


    // Preparar los datos para el envío
    const data = { preferenceId: _preferenceId, NumeroCompra: _NumeroCompra,  __RequestVerificationToken: token };

    // Enviar los datos al servidor
    $.post(urlInsCarritosMercadoLibre, data)
        .done(response => {
            if (response.success) {
                // Acciones en caso de éxito
                /* limpiarCarrito(); */
                /* createCompra('success', 'MAJAS, agradece por su compra, en su WhatsApp encotraras los detalles de la compra, el vendedor tomara contacto para validar la forma de pago...', UrlAlertSucess); */
            } else {
            }
        })
        .fail(() => {

        });
}



//function Ins_carritoProductos(_id) {
//    var CostoEnvio = ConvertirEnFloat($("#IdCostoEnvio strong").text());
//    lista = JSON.parse(localStorage.getItem("ListaCarrito"));
//    var Validar = [];
//    lista.forEach((item) => {
//        let ListaDto = {
//            id_carrito: 0,
//            id_compra_realizada: _id,
//            id_producto: item.ProductoId,
//            Descripcion_Producto: item.Descripcion,
//            Cantidad: item.cantidadProducto,
//            precio_Unitario: item.Precio,
//            Precio_Total: item.precioTotal,
//            Precio_Envio: CostoEnvio,
//            Descuento: 0,
//            Cancelado: "NO"
//        };
//        Validar.push(ListaDto);
//    });

//    let data = { Validar };
//    $.post(urlInsProducto, data).done((response) => {
//        if (response.success == true) {
//            /* limpiarCarrito();*/
//            /* createCompra('success', 'MAJAS, agradece por su compra, en su WhatsApp encotraras los detalles de la compra, el vendedor tomara contacto para validar la forma de pago...', UrlAlertSucess);*/
//        }
//        else {
//        }
//    }).fail();
//}






function CargarLista() {
    let container = document.querySelector('#IdPais');
    container.innerHTML = "";

    // Verificar si DominioMarcas está presente en el localStorage
    let DominioMarcas = JSON.parse(localStorage.getItem("listaPaises"));
    if (DominioMarcas) {
        container.innerHTML += `<option value="0" >Seleccione</option>`;
        DominioMarcas.forEach((item) => {
            container.innerHTML += `<option value="${item.idDominio}"> ${item.descripcion}</option>`;
        });
    } else {
        // Si DominioMarcas no está presente, podrías manejar este caso o simplemente no hacer nada.
  
    }

    container.value = "10";
}
//function CargarLista() {
//    let container = document.querySelector('#IdPais');
//    container.innerHTML = "";
//    DominioMarcas = JSON.parse(localStorage.getItem("listaPaises"));
//    container.innerHTML += `<option value="0" >Seleccione</option>`;
//    DominioMarcas.forEach((item) => {
//        container.innerHTML += `<option value="${item.idDominio}"> ${item.descripcion}</option>`;

//    });

//    container.value = "10";



//}

//function CargarListaIndicativos() {
//    let container = document.querySelector('#IdPaisIndicativo');
//    container.innerHTML = "";
//    DominioMarcas = JSON.parse(localStorage.getItem("listaPaises"));
//    container.innerHTML += `<option value="0" >Seleccione</option>`;
//    DominioMarcas.forEach((item) => {
//        container.innerHTML += `<option value="${item.idDominio}">  ${item.descripcion} (${item.abreviatura})</option>`;
//    });
//    container.value = "10";

//}

function CargarListaIndicativos() {
    let container = document.querySelector('#IdPaisIndicativo');
    container.innerHTML = "";

    // Verificar si DominioMarcas está presente en el localStorage
    let DominioMarcas = JSON.parse(localStorage.getItem("listaPaises"));
    if (DominioMarcas) {
        container.innerHTML += `<option value="0" >Seleccione</option>`;
        DominioMarcas.forEach((item) => {
            container.innerHTML += `<option value="${item.idDominio}">  ${item.descripcion} (${item.abreviatura})</option>`;
        });
    } else {
        // Si DominioMarcas no está presente, podrías manejar este caso o simplemente no hacer nada.
      
    }

    container.value = "10";
}





function CargarListaIndicativo(pais) {

    $("#IdPaisIndicativo").val(pais);
    $("#IdPaisIndicativo").trigger('change.select2');
    $("#IdPaisIndicativo").trigger("chosen:updated");
}


//function ConsultaCompraPendiente() {




//    TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
//    TablaDatosClienteCompra = JSON.parse(localStorage.getItem("DatosClienteCompra"));

//    if (TablaDatosNumeroCompra == null) {
//        return false;
//    }
//    if (TablaDatosClienteCompra == null) {
//        return false;
//    }
//    else {

//        Swal.fire({
//            type: 'warning',
//            title: '¡Estimado(a) Cliente!',
//            text: "¿Desea recuperar la información diligenciada anteriormente?...  ",
//            showCancelButton: true,
//            cancelButtonText: "NO",
//            confirmButtonText: "SI",
//            closeOnConfirm: false
//        }).then((result) => {
//            if (result.value) {
//                TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
//                TablaDatosClienteCompra = JSON.parse(localStorage.getItem("DatosClienteCompra"));

//                RecuperarCompraPendiente();
//                var boton = document.getElementById("checkout-btn");
//                boton.click();
//                return;
//            }
//            else if (result.dismiss == 'cancel') { //Descartar


//                limpiarCarrito2();
//            }
//            else if (result.dismiss == 'backdrop') { //Clic por fuera de la modal

//                limpiarCarrito2();
//            }
//        });
//    }
//}

function ConsultaCompraPendiente() {
    TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
    TablaDatosClienteCompra = JSON.parse(localStorage.getItem("DatosClienteCompra"));

    if (TablaDatosNumeroCompra == null || TablaDatosClienteCompra == null) {
        return false;
    } else {
        Swal.fire({
            type: 'info',
            title: '¡Estimado(a) Cliente!',
            text: "¿Desea recuperar la información diligenciada anteriormente?...",
            showCancelButton: true,
            cancelButtonText: "NO",
            confirmButtonText: "SÍ", // Cambiado "SI" por "SÍ"
            closeOnConfirm: false
        }).then((result) => {
            if (result.value) {
                TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
                TablaDatosClienteCompra = JSON.parse(localStorage.getItem("DatosClienteCompra"));
               
                RecuperarCompraPendiente();
                realizarConsultaPaginadoCompra();
         
                //var boton = document.getElementById("checkout-btn");
                //boton.click();
                return;
            } else if (result.dismiss == 'cancel' || result.dismiss == 'backdrop') { // Descartar o Clic por fuera de la modal
                limpiarCarrito2();
            }
        });
    }
}
//function ConsultaCompraPendiente() {
//     TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
//     TablaDatosClienteCompra = JSON.parse(localStorage.getItem("DatosClienteCompra"));

//    if (TablaDatosNumeroCompra == null || TablaDatosClienteCompra == null) {
//        return false;
//    } else {
//        Swal.fire({
//            type: 'info',
//            title: '¡Estimado(a) Cliente!',
//            text: "¿Desea recuperar la información diligenciada anteriormente?...",
//            showCancelButton: true,
//            cancelButtonText: "NO",
//            confirmButtonText: "SI",
//            closeOnConfirm: false
//        }).then((result) => {
//            if (result.value) {
//                TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));
//                TablaDatosClienteCompra = JSON.parse(localStorage.getItem("DatosClienteCompra"));

//                RecuperarCompraPendiente();
//                var boton = document.getElementById("checkout-btn");
//                boton.click();
//                return;
//            } else if (result.dismiss == 'cancel' || result.dismiss == 'backdrop') { // Descartar o Clic por fuera de la modal
//                limpiarCarrito2();
//            }
//        });
//    }
//}


//function ConsultaCiudades(id_departamento, ciudad) {


//    $("#selectCiudad").empty();
//    $.ajax({
//        type: 'POST',
//        url: UrlGetDepartamentoDominios,
//        dataType: 'json',
//        data: { IdDto: id_departamento },
//        success: function success(response) {
//            $.each(response.data, function (i, dato) {
//                $("#selectCiudad").append('<option value="' + dato.idDominio + '">' + dato.descripcion + '</option>');

//            });

//            $("#selectCiudad").val(ciudad);
//            $("#selectCiudad").trigger('change.select2');
//            $("#selectCiudad").trigger("chosen:updated");
//        /*    ValidaPrecioEnvioDepartamento(id_departamento);*/


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
function ConsultaCiudades(id_departamento, ciudad) {
    $("#selectCiudad").empty();
    $.ajax({
        type: 'POST',
        url: UrlGetDepartamentoDominios,
        dataType: 'json',
        data: { IdDto: id_departamento },
        success: function (response) {
            if (response && response.data && response.data.length > 0) {
                $.each(response.data, function (i, dato) {
                    $("#selectCiudad").append('<option value="' + dato.idDominio + '">' + dato.descripcion + '</option>');
                });

                $("#selectCiudad").val(ciudad);
                $("#selectCiudad").trigger('change.select2');
                $("#selectCiudad").trigger("chosen:updated");
            } else {
                Swal.fire({
                    type: 'error',
                    title: '¡Estimado(a) Cliente!',
                    text: 'No se encontraron ciudades para este departamento'
                });
            }
        },
        error: function (ex) {
            Swal.fire({
                type: 'error',
                title: '¡Estimado(a) Cliente!',
                text: 'No es posible cargar los datos, revise'
            });
        }
    });
};


//function ValidaPrecioEnvioDepartamento(Departamento) {
//    $("#IdCostoEnvio label").html('');
//    let idEnvio = 0;
//    $.ajax({
//        type: 'POST',
//        url: UrlGetCostoEnvioDepartamento,
//        dataType: 'json',
//        data: { IdDto: Departamento },
//        success: function success(respuesta) {

//            if (respuesta.success) {
//                $("#IdCostoEnvio label").append(`Precio de Envío: <strong class="">${ConvertirEnString(respuesta.data.costoEnvio)}</strong>`);
//                idEnvio = respuesta.data.costoEnvio;

//                TotalPrecioEnvio(idEnvio);
//            }
//        },
//        error: function error(ex) {
//            Swal.fire({
//                type: 'error',
//                title: '¡Estimado(a) Cliente!',
//                text: 'No es posible visualizar el departamento, revise'
//            });
//        }
//    });
//};
function RecuperarCompraPendiente() {

   
    $("#IdPais").val(TablaDatosClienteCompra.Pais);
    $("#selectDepartamento").val(TablaDatosClienteCompra.Departamento);
    $("#selectCiudad").val(TablaDatosClienteCompra.Ciudad);

    $("#txtNombre").val(TablaDatosClienteCompra.Nombres);
    $("#txtApellidos").val(TablaDatosClienteCompra.Apellidos);
    $("#txtNombreEmpresa").val(TablaDatosClienteCompra.Nombre_Empresa);
    $("#txtDireccion").val(TablaDatosClienteCompra.Direccion);
    $("#txtoptionalDireccion").val(TablaDatosClienteCompra.Opcional_Direccion);
    $("#txtCodigoPostal").val(TablaDatosClienteCompra.Codigo_Postal);
    $("#txtCelular").val(TablaDatosClienteCompra.Celular);
    $("#txtCelulaConfirmacion").val(TablaDatosClienteCompra.Celular);
    $("#txtCorreoElectronico").val(TablaDatosClienteCompra.Correo_Electronico);
    $("#txtCorreoElectronicoConfirmacion").val(TablaDatosClienteCompra.Correo_Electronico);
    $("#txtComentario").val(TablaDatosClienteCompra.Comentario);
    $("#IdPaisIndicativo").val(TablaDatosClienteCompra.IdPaisIndicativo);
    /*     $("#IdCostoEnvio label").append(`Precio de Envío: <strong class="">${ConvertirEnString(TablaDatosClienteCompra.CostoEnvio)}</strong>`);*/
    CargarListaPaisMemoria(TablaDatosClienteCompra.Pais, TablaDatosClienteCompra.Departamento, TablaDatosClienteCompra.Ciudad);
    CargarListaIndicativo(TablaDatosClienteCompra.Pais)
    ValidaCampos();
 
}

function CargarListaPaisMemoria(_Pais, _Deparatamento, _Ciudad) {


    if (_Pais == "10") {
        $("#idDepartamento1").css("display", "block");
        $("#idDepartamento2").css("display", "none");
        $("#IdCodigoPostal").css("display", "none");

    } else {
        $("#idDepartamento1").css("display", "none");
        $("#idDepartamento2").css("display", "block");
        $("#IdCodigoPostal").css("display", "block");
    }


    if (_Pais == "10") {

        let _DepartamentoV = _Deparatamento;
        let _CiudadV = _Ciudad;
        if (_DepartamentoV == "") {

            create('error', 'Ingrese su departamento, por favor', UrlAlertError);
            return;
        }
        if (_CiudadV == "") {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }

        $("#IdPais").val(_Pais);
        $("#IdPais").trigger('change.select2');
        $("#IdPais").trigger("chosen:updated");
        $("#selectDepartamento").val(_DepartamentoV);
        $("#selectDepartamento").trigger('change.select2');
        $("#selectDepartamento").trigger("chosen:updated");

        ConsultaCiudades(TablaDatosClienteCompra.Departamento, _CiudadV);

        $("#txtDepartamento2").val("");

    } else {

        $("#IdPais").val(_Pais);
        $("#IdPais").trigger('change.select2');
        $("#IdPais").trigger("chosen:updated");

        if (_Deparatamento == "") {
            create('error', 'Ingrese su ciudad, por favor', UrlAlertError);
            return;
        }

        $("#txtDepartamento2").val(_Deparatamento);
        $("#selectCiudad").val(0);
        $("#selectCiudad").trigger('change.select2');
        $("#selectCiudad").trigger("chosen:updated");

    }



}



function limpiarCarrito2() {
   /* localStorage.removeItem("ListaCarrito");*/
 /*   localStorage.removeItem("NumeroCompra");*/
    localStorage.removeItem("DatosClienteCompra");
    localStorage.removeItem("TablaDatosNumeroCompra");
    localStorage.removeItem("TablaDatosClienteCompra");
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
    $("#IdPais").val(10);
    $("#IdPais").trigger('change.select2');
    $("#IdPais").trigger("chosen:updated");
    $("#selectDepartamento").val(1227);
    $("#selectDepartamento").trigger('change.select2');
    $("#selectDepartamento").trigger("chosen:updated");
    $("#selectCiudad").val(1228);
    $("#selectCiudad").trigger('change.select2');
    $("#selectCiudad").trigger("chosen:updated");
    $("#IdPaisIndicativo").val(10);
    $("#IdPaisIndicativo").trigger('change.select2');
    $("#IdPaisIndicativo").trigger("chosen:updated");
  
 
    //localStorage.clear();
    //window.location.reload();

}


function ValidaCampos() {

    var txtDireccion = document.getElementById('txtDireccion');
    function highlightField() {
        if (txtDireccion.value.trim() !== '') {
            txtDireccion.style.backgroundColor = '#fff3f8'; 
        } else {
            txtDireccion.style.backgroundColor = '';
        }
    }
    highlightField();
    txtDireccion.addEventListener('input', function () {
        highlightField();
    });

    var txtNombre = document.getElementById('txtNombre');
    function txtNombreField() {
        if (txtNombre.value.trim() !== '') {
            txtNombre.style.backgroundColor = '#fff3f8'; // Color amarillo claro
        } else {
            txtNombre.style.backgroundColor = '';
        }
    }
    txtNombreField();
    txtNombre.addEventListener('input', function () {
        txtNombreField();
    });

    var txtApellidos = document.getElementById('txtApellidos');
    function txtApellidosField() {
        if (txtApellidos.value.trim() !== '') {
            txtApellidos.style.backgroundColor = '#fff3f8'; // Color amarillo claro
        } else {
            txtApellidos.style.backgroundColor = '';
        }
    }
    txtApellidosField();
    txtApellidos.addEventListener('input', function () {
        txtApellidosField();
    });


    var txtNombreEmpresa = document.getElementById('txtNombreEmpresa');
    function txtNombreEmpresaField() {
        if (txtNombreEmpresa.value.trim() !== '') {
            txtNombreEmpresa.style.backgroundColor = '#fff3f8'; // Color amarillo claro
        } else {
            txtNombreEmpresa.style.backgroundColor = '';
        }
    }
    txtNombreEmpresaField();
    txtNombreEmpresa.addEventListener('input', function () {
      txtNombreEmpresaField();
    });





    var txtoptionalDireccion = document.getElementById('txtoptionalDireccion');
    function txtoptionalDireccionField() {
        if (txtoptionalDireccion.value.trim() !== '') {
            txtoptionalDireccion.style.backgroundColor = '#fff3f8'; // Color amarillo claro
        } else {
            txtoptionalDireccion.style.backgroundColor = '';
        }
    }
    txtoptionalDireccionField();
    txtoptionalDireccion.addEventListener('input', function () {
        txtoptionalDireccionField();
    });

    var txtDepartamento2 = document.getElementById('txtDepartamento2');
    function txtDepartamento2Field() {
        if (txtDepartamento2.value.trim() !== '') {
            txtDepartamento2.style.backgroundColor = '#fff3f8'; // Color amarillo claro
        } else {
            txtDepartamento2.style.backgroundColor = '';
        }
    }
    txtDepartamento2Field();
    txtDepartamento2.addEventListener('input', function () {
        txtDepartamento2Field();
    });



    var txtCodigoPostal = document.getElementById('txtCodigoPostal');
    function txtCodigoPostalField() {
        if (txtCodigoPostal.value.trim() === '' || txtCodigoPostal.value.trim() === '0') {
            txtCodigoPostal.style.backgroundColor = '';
        } else {
            txtCodigoPostal.style.backgroundColor = '#fff3f8'; // Color amarillo claro
           
        }
    }
    txtCodigoPostalField();
    txtCodigoPostal.addEventListener('input', function () {
        txtCodigoPostalField();
    });




    var txtCelular = document.getElementById('txtCelular');
    function txtCelularlField() {
        if (txtCelular.value.trim() !== '') {
            txtCelular.style.backgroundColor = '#fff3f8'; // Color amarillo claro
        } else {
            txtCelular.style.backgroundColor = '';
        }
    }
    txtCelularlField();
    txtCelular.addEventListener('input', function () {
        txtCelularlField();
    });




    var txtCelulaConfirmacion = document.getElementById('txtCelulaConfirmacion');
    function txtCelulaConfirmacionField() {
        if (txtCelulaConfirmacion.value.trim() !== '') {
            txtCelulaConfirmacion.style.backgroundColor = '#fff3f8'; // Color amarillo claro
        } else {
            txtCelulaConfirmacion.style.backgroundColor = '';
        }
    }
    txtCelulaConfirmacionField();
    txtCelulaConfirmacion.addEventListener('input', function () {
       txtCelulaConfirmacionField();
    });





    var txtCorreoElectronico = document.getElementById('txtCorreoElectronico');
    function txtCorreoElectronicoField() {
        if (txtCorreoElectronico.value.trim() !== '') {
            txtCorreoElectronico.style.backgroundColor = '#fff3f8'; // Color amarillo claro
        } else {
            txtCorreoElectronico.style.backgroundColor = '';
        }
    }
    txtCorreoElectronicoField();
    txtCorreoElectronico.addEventListener('input', function () {
       txtCorreoElectronicoField();
    });



    var txtCorreoElectronicoConfirmacion = document.getElementById('txtCorreoElectronicoConfirmacion');
    function txtCorreoElectronicoConfirmacionField() {
        if (txtCorreoElectronicoConfirmacion.value.trim() !== '') {
            txtCorreoElectronicoConfirmacion.style.backgroundColor = '#fff3f8'; // Color amarillo claro
        } else {
            txtCorreoElectronicoConfirmacion.style.backgroundColor = '';
        }
    }
    txtCorreoElectronicoConfirmacionField();
    txtCorreoElectronicoConfirmacion.addEventListener('input', function () {
        txtCorreoElectronicoConfirmacionField();
    });



    var txtComentario = document.getElementById('txtComentario');
    function txtComentarioField() {
        if (txtComentario.value.trim() !== '') {
            txtComentario.style.backgroundColor = '#fff3f8'; // Color amarillo claro
        } else {
            txtComentario.style.backgroundColor = '';
        }
    }
    txtComentarioField();
    txtComentario.addEventListener('input', function () {
       txtComentarioField();
    });





    // Obtener el campo select por su ID
    // var selectPais = document.getElementById('IdPais');

    // // Obtener el contenedor del select por su ID
    // var selectContainer = document.getElementById('selectContainer');

    // // Adjuntar un evento de escucha para el evento de cambio
    // selectPais.addEventListener('change', function () {
    //     // Verificar si se ha seleccionado una opción
    //     if (selectPais.value !== '') {
    //         // Si se ha seleccionado una opción, cambiar el color de fondo del contenedor
    //         selectContainer.style.backgroundColor = '#FFD9EB'; // Nuevo color de fondo al seleccionar
    //     } else {
    //         // Si no se ha seleccionado ninguna opción, restaurar el color de fondo original del contenedor
    //         selectContainer.style.backgroundColor = '#733939'; // Color de fondo original
    //     }
    // });
}



//function realizarConsultaPaginadoCompra() {
//    // Obtener la referencia a la sección de destino
//    const seccionDestinos = document.getElementById('IdRedireciona');

//    // Verificar si la sección de destino existe
//    if (seccionDestinos) {
//        // Obtener la posición de la sección de destino
//        const posicionDestino = seccionDestinos.offsetTop;

//        // Realizar el desplazamiento suave hacia la sección de destino
//        window.scrollTo({
//            top: posicionDestino,
//            behavior: 'smooth'
//        });
//    } else {
//        /*console.error("La sección de destino no existe en el documento.");*/
//    }
//}


//function realizarConsultaPaginadoCompra() {

//    //window.onscroll = function () {
//    //    scrollFunction();
//    //};

//    var seccionDestino = document.getElementById("IdRedireciona");

//    if (seccionDestino) {
//        var alturaVentana = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
//        var posicionDestino = seccionDestino.offsetTop + seccionDestino.offsetHeight + alturaVentana * 1; // Desplazamos 20% más allá del final de la sección

//        window.scrollTo({
//            top: posicionDestino,
//            behavior: 'smooth'
//        });
//    } else {
//        console.error("La sección de destino no existe en el documento.");
//    }
//}
//function realizarConsultaPaginadoCompra() {
//    // Maneja el desplazamiento suave al hacer scroll

//    var seccionDestino = document.getElementById("IdRedireciona");

//    if (seccionDestino) {
//        var alturaExtra = 0;
//        var anchoPantalla = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;

//        if (anchoPantalla < 600) {
//            alturaExtra = 1300;
//        } else {
//            alturaExtra = 5; // Corregido el error tipográfico aquí (turaExtra -> alturaExtra)
//        }

//        var alturaVentana = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
//        var posicionDestino = seccionDestino.offsetTop + seccionDestino.offsetHeight + alturaExtra;

//        window.scrollTo({
//            top: posicionDestino,
//            behavior: 'smooth'
//        });
//    } else {

//    }
//}

//// Sobrescribimos window.scrollTo solo cuando se ejecute la función realizarConsultaPaginadoCompra
//realizarConsultaPaginadoCompra.originalScrollTo = window.scrollTo;
//window.scrollTo = function () {
//    // Verificamos si la función que llama a window.scrollTo es realizarConsultaPaginadoCompra
//    if (arguments.callee.caller === realizarConsultaPaginadoCompra) {
//        realizarConsultaPaginadoCompra.originalScrollTo.apply(window, arguments);
//    } else {
//        console.log("Otra llamada a window.scrollTo ha sido bloqueada.");
//    }
//};


function realizarConsultaPaginadoCompra() {
    // Maneja el desplazamiento suave al hacer scroll

    var seccionDestino = document.getElementById("IdRedireciona");

    if (seccionDestino) {
        var alturaExtra = 0;
        var anchoPantalla = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;

        if (anchoPantalla < 600) {
            alturaExtra = 1000;
        } else {
            alturaExtra = 5; // Corregido el error tipográfico aquí (turaExtra -> alturaExtra)
        }

        var alturaVentana = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
        var posicionDestino = seccionDestino.offsetTop + seccionDestino.offsetHeight + alturaExtra;

        // Guardamos una referencia al scrollTo original
        const originalScrollTo = window.scrollTo;

        // Sobrescribimos window.scrollTo solo para esta llamada
        window.scrollTo = function () {
            originalScrollTo({
                top: posicionDestino,
                behavior: 'smooth'
            });
            // Restauramos el scrollTo original después de usarlo
            window.scrollTo = originalScrollTo;
        };
    } else {
        console.log("No se encontró la sección destino.");
    }
}
