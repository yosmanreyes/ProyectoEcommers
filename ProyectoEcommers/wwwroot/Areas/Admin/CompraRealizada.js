


$(document).ready(function () {

    if (PaginaPrincipal == 6) {
        GetMisCompras2();
    }
});

if ($(window).width() < 780) {
    $("#btnGrabaMisCompras").css("display", "none");
    $("#btnGrabaMisComprasCelular").css("display", "block");
}


var listaProductoComprados = [];


function GetMisCompras2() {


    TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));

    if (TablaDatosNumeroCompra != "" || TablaDatosNumeroCompra != null) {

        // Capturar el valor de payment_id
        var paymentIdElement = document.getElementById('payment_id');
        var paymentId = paymentIdElement ? paymentIdElement.getAttribute('data-value') : null;

        // Capturar el valor de status
        var statusElement = document.getElementById('status');
        var status = statusElement ? statusElement.getAttribute('data-value') : null;

        // Capturar el valor de external_reference
        var externalReferenceElement = document.getElementById('external_reference');
        var externalReference = externalReferenceElement ? externalReferenceElement.getAttribute('data-value') : null;

        // Capturar el valor de merchant_order_id
        var merchantOrderIdElement = document.getElementById('merchant_order_id');
        var merchantOrderId = merchantOrderIdElement ? merchantOrderIdElement.getAttribute('data-value') : null;


        if (paymentId) {
         /*   console.log('Preference-ID encontrado:', preferenceId);*/
            // Aquí puedes hacer lo que necesites con el preference-id
    
        /*    $("#IdtablaMisCompras").html("");*/
            let data = { idCompra: TablaDatosNumeroCompra, preferenceId: paymentId, Idstatus: status, Idexternal_reference: externalReference, Idmerchant_order_id: merchantOrderId  }
        $.post(UrlGetConsultaMicomptas, data).done(function (result) {

            if (result.data.length > 0) {
                $("#IdtablaMisCompras2 tbody tr").remove();
                $("#IdMisComprasEnvio2").css("display", "none");
                $("#IdMisCompras").css("display", "block");
                create('success', 'MAJAS, agradece por su compra, en su correo electrónico encontrarás los detalles de la compra, o puedes hacerle seguimiento dando clic en mi carrito MIS COMPRAS REALIZADAS, adiciona su correo y listo...', UrlAlertSucess);
                listaProductoComprados = result.data;
                listaProductoComprados.forEach((item) => {
                    $("#IdtablaMisCompras2 tbody").append(`<tr class="cart_item">
                                                   
                                                   <td class="product-total">
                                                    <span class="amount">${item.cantidad}</span>
                                                    </td>                                           
                                                    <td class="product-total">
                                                        <span class="amount">${ConvertirEnString(item.precioTotal)}</span>
                                                    </td>
                                                             <td class="product-total">
                                                    <span class="amount">${ConvertirEnString(item.costoEnvio)}</span>
                                                    </td>
                                                     <td class="product-total">
                                                    <span class="amount">${ConvertirEnString(item.precioFinal)}</span>
                                                    </td>
                                                 <td class="product-total">
                                                        <span class="amount">${item.fecha_CreacionS}</span>
                                                    </td>
                                                  <td class="product-total">
                                                        <span class="amount">${item.direccion}  ${item.opcional_Direccion == null ? "" : item.opcional_Direccion}</span>
                                                    </td>
                                               </tr>
             `);

                });
            }

            else {
                verificarEstadoPagoID();
                $("#IdtablaMisCompras2 tbody tr").remove();
                $("#IdtablaMisComprasTotal2 tbody tr").remove();
                $("#IdListaCompraC").html('');
                $("#idFechaCreacion").html('');
                $("#idEstadoTrazabilidad").html('');
                $("#IdMisCompras").css("display", "none");
                $("#IdMisComprasEnvio2").css("display", "none");
                create('success', 'MAJAS, agradece por su compra, en su correo electrónico encontrarás los detalles de la compra, o puedes hacerle seguimiento dando clic en mi carrito MIS COMPRAS REALIZADAS, adiciona su correo y listo...', UrlAlertSucess);
              
           
                /* create('error', 'Usted no cuenta con productos, o por favor valide su correo electrónico correctamenteX. gracias...', UrlAlertError);*/
            }
            VerDetalleMiPorductoCompra();
        }).fail()

        } else {
           
        }

    }

    //lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    //if (lista != null) {
    //    $("#IdtablaCarrito2 tbody tr").remove();
    //    lista.forEach((item) => {

    //    });
    //}
}



function VerDetalleMiPorductoCompra() {

    TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));

    if (TablaDatosNumeroCompra != "" || TablaDatosNumeroCompra != null) {
        VerDetalleMiPorducto2(TablaDatosNumeroCompra);
      
    }
}


function VerDetalleMiPorducto2(id_compra) {

    if (id_compra == null) { } else { 
    $("#IdMisComprasEnvio2").css("display", "block");
    $("#IdtablaMisComprasTotal2 tbody tr").remove();

    let NuevaLista = listaProductoComprados.filter((item) => item.id_compra_realizada == id_compra);
    if (NuevaLista != null) {
        var lll = NuevaLista[0].listaMisCompras;

        lll.forEach((item) => {
            $("#IdtablaMisComprasTotal2 tbody").append(`<tr class="cart_item">
                                                        <td class="product-total">
                                                            <span class="amount"><img style="width: 60px !important; height: 50px !important;" src="${UrlImgProductos + item.listaImagenesC[0].FileName}" alt="product"></span>
                                                        
                                                        </td>
                                                        <td class="">
                                                               <strong class="">${item.producto}</strong>, Cantidad  <strong>${item.cantidad}</strong> Und
                                                        </td>
                                                        <td class="product-total">
                                                            <span class="amount">${ConvertirEnString(item.precio_Unitario)}</span>
                                                        </td>
                                                        <td class="product-total">
                                                            <span class="amount">${ConvertirEnString(item.precio_Total)}</span>
                                                        </td>
                                                    </tr>
                                                `);




        });

        let res = listaProductoComprados.find((item) => item.id_compra_realizada == id_compra);
        $("#IdListaCompraC").html('');
        $("#IdListaCompraC").append(`
                            <h5 style="text-align: center">
                                    Trazabilidad del envío
                                </h5>
                                <div class="lineatemp" style="font-size: 11px;">
                                    <div class="fila">
                                        <div class="disco ${res.fecha_CreacionS == "" ? "" : "validaClick"}"><div></div></div>
                                        <div> ${res.fecha_CreacionS}</div>
                                        <div class="pformulario"><strong>Orden recibida</strong></div>
                                    </div>
                                    <div class="fila">
                                        <div class="disco ${res.fecha_CreacionS == "" ? "" : "validaClick"}"><div></div></div>
                                        <div> ${res.fecha_CreacionS} </div>
                                        <div class="pformulario"><strong>En preparación</strong></div>
                                    </div>
                                    <div class="fila ">
                                        <div class="disco ${res.fecha_Envio_productoS == "" ? "" : "validaClick"}"><div></div></div>
                                        <div> ${res.fecha_Envio_productoS == "" ? "Por Confirmar" : res.fecha_Envio_productoS}</div>
                                        <div class="pformulario"><strong>En camino</strong> </div>

                                    </div>
                                    <div class="fila">
                                        <div class="disco ${res.fecha_Llegada_productoS == "" ? "" : "validaClick"}"><div></div></div>
                                        <div> ${res.fecha_Llegada_productoS == "" ? "Por Confirmar" : res.fecha_Llegada_productoS}</div>
                                        <div class="pformulario"><strong>Entregado</strong> </div>
                                    </div>
                                </div>
                                <br />
                                <div class="" style="text-align: center; font-size: 14px; color: #333; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;">
                                    <strong>Producto enviado por:</strong>
                                </div>
                                <p style="text-align: center; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;">${res.empresa_Entrega == null ? "Por Confirmar" : res.empresa_Entrega}</p>
                                <div class="" style="text-align: center; font-size: 14px; color: #333; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;"><strong>Dirección de envío:</strong></div>
                                <p style="text-align: center; position: relative; width: 276px; margin: 0 auto; border-radius: 10px; ">
                                    ${res.direccion}  ${res.opcional_Direccion == null ? "" : ' - ' + res.opcional_Direccion} 
                                </p>
                                  <p style="text-align: center; position: relative; width: 276px; margin: 0 auto; border-radius: 10px; ">
                                    ${res.pais}  ${res.departamento == null ? "" : '/' + res.departamento}  ${res.ciudad == null ? "" : '/' + res.ciudad} 
                                </p>`);


        let ss = listaProductoComprados.find((item) => item.id_compra_realizada == id_compra);
        $("#idFechaCreacion").html('');
        $("#idEstadoTrazabilidad").html('');
        var validar = "";
        $("#idFechaCreacion").append(` <h3 style="text-align: center"> Mi Compra Realizada, Fecha: ${ss.fecha_CreacionS}</h3>`);

        if (ss.fecha_CreacionS != "") {
            validar = "Orden recibida";
        }
        if (ss.fecha_CreacionS != "") {
            validar = "En preparación";
        }
        if (ss.fecha_Envio_productoS != "") {
            validar = "En camino";
        }
        if (ss.fecha_Llegada_productoS != "") {
            validar = "Entregado";
        }
        $("#idEstadoTrazabilidad").append(` <h3 style="text-align: center"> Estado: ${validar}</h3>`);
        RedirecionaConsultaMiCompra2();
        }

    }
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

function GetEnvioCorreo() {


    TablaDatosNumeroCompra = JSON.parse(localStorage.getItem("NumeroCompra"));



            let data = { idCompra: TablaDatosNumeroCompra }
            $.post(UrlGetEnviarCorreoId, data).done(function (result) {

                if (result.data.length > 0) {
             
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
