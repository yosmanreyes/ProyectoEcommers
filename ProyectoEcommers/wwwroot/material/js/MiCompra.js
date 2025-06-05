

var listaProductoComprados = [];
let currentPageIndex3 = 0; // Índice de la página actual para "Mis Compras"
let pageSize1 = 5;

$(document).ready(function () {

    if (PaginaPrincipal == 5) {
 // Tamaño de la página (número de elementos por página)
        verificarEstadoPagoID();
        if ($(window).width() < 780) {
            $("#btnGrabaMisCompras").css("display", "none");
            $("#btnGrabaMisComprasCelular").css("display", "block");

        }
    }
});



function validarCorreoCompra(correo) {
    var expresionRegularCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (expresionRegularCorreo.test(correo)) {
        mensajeCodigoMiCompra.innerHTML = "";
    } else {
        mensajeCodigoMiCompra.innerHTML = "Por favor, introduce un correo electrónico válido...";
        mensajeCodigoMiCompra.style.color = "red";
        return 1;
    }
}

function GetMisCompras() {
    var _correo = document.getElementById("txtConsultaMiscompra").value;
    var co = validarCorreoCompra(_correo);

    if (co == 1) {
        // Restablecer elementos y salir si el correo es inválido
        $("#IdtablaMisCompras tbody tr").remove();
        $("#IdtablaMisComprasTotal tbody tr").remove();
        $("#IdListaCompraC").html('');
        $("#idFechaCreacion").html('');
        $("#idEstadoTrazabilidad").html('');
        $("#botonesNavegacion").html(''); // Limpiar botones de navegación
        $("#IdMisCompras").css("display", "none");
        $("#IdMisComprasEnvio").css("display", "none");
        return;
    }

    let data = { correo: _correo };
    $.post(UrlGetConsultaMicomptasR, data).done(function (result) {
        if (result.data.length > 0) {
            // Limpiar la tabla antes de agregar nuevos elementos
            $("#IdtablaMisCompras tbody").empty();
            $("#IdMisComprasEnvio").css("display", "none");
            $("#IdMisCompras").css("display", "block");
            listaProductoComprados = result.data;
            // Calcular los índices de inicio y fin para la paginación
            let startIndex = currentPageIndex3 * pageSize1;
            let endIndex = Math.min(startIndex + pageSize1, result.data.length);

            // Iterar sobre los elementos dentro del rango de la página actual
            for (let i = startIndex; i < endIndex; i++) {
                let item = result.data[i];
                $("#IdtablaMisCompras tbody").append(`<tr class="cart_item">
                                                        <td>
                                                           <button tabindex="21" type="button" onclick="VerDetalleMiPorducto(${item.id_compra_realizada}); return false;" class="btn btn-grad my-button"><span class="fa fa-cart-arrow-down faa-wrench animated"></span> Ver Detalle</button>
                                                        </td>
                                                              <td class="product-total">
                                                            <span class="amount">${item.fecha_CreacionS}</span>
                                                        </td>
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
                                                            <span class="amount">${item.direccion}  ${item.opcional_Direccion == null ? "" : item.opcional_Direccion}</span>
                                                        </td>
                                                    </tr>`);
            }

            // Agregar botones de navegación para la paginación
            let totalPaginas = Math.ceil(result.data.length / pageSize1);
            let botonesNavegacion = '';
            if (currentPageIndex3 > 0) {
                botonesNavegacion += `<button onclick="cambiarPaginaMisCompras(${currentPageIndex3 - 1})">Anterior</button>`;
            }
            for (let i = 0; i < totalPaginas; i++) {
                botonesNavegacion += `<button onclick="cambiarPaginaMisCompras(${i})">${i + 1}</button>`;
            }
            if (currentPageIndex3 < totalPaginas - 1) {
                botonesNavegacion += `<button onclick="cambiarPaginaMisCompras(${currentPageIndex3 + 1})">Siguiente</button>`;
            }
            $("#botonesNavegacion").html(botonesNavegacion); // Agregar botones al contenedor correcto

            // Actualizar la información de la cantidad de productos y la página actual
            $("#cantidadProductos").text(`Total de productos: ${result.data.length}`).css("color", "black");
            $("#paginaActual").text(`Página actual: ${currentPageIndex3 + 1}`).css("color", "black");
        } else {
            // Si no hay datos, restablecer elementos
            $("#IdtablaMisCompras tbody tr").remove();
            $("#IdtablaMisComprasTotal tbody tr").remove();
            $("#IdListaCompraC").html('');
            $("#idFechaCreacion").html('');
            $("#idEstadoTrazabilidad").html('');
            $("#botonesNavegacion").html(''); // Limpiar botones de navegación
            $("#IdMisCompras").css("display", "none");
            $("#IdMisComprasEnvio").css("display", "none");
            create('error', 'Usted no cuenta con productos, o por favor valide su correo electrónico correctamente. gracias...', UrlAlertError);
        }
    }).fail();
}

function cambiarPaginaMisCompras(index) {
    currentPageIndex3 = index;
    GetMisCompras();
}


//function GetMisCompras() {

//    var _correo = document.getElementById("txtConsultaMiscompra").value;
//    var co = validarCorreoCompra(_correo);

//    if (co == 1) {
//        $("#IdtablaMisCompras tbody tr").remove();
//        $("#IdtablaMisComprasTotal tbody tr").remove();
//        $("#IdListaCompraC").html('');
//        $("#idFechaCreacion").html('');
//        $("#idEstadoTrazabilidad").html('');
//        $("#IdMisCompras").css("display", "none");
//        $("#IdMisComprasEnvio").css("display", "none");
//        return;
//    }

//    /*    $("#IdtablaMisCompras").html("");*/
//    let data = { correo: _correo }
//    $.post(UrlGetConsultaMicomptas, data).done(function (result) {

//        if (result.data.length > 0) {
//            $("#IdtablaMisCompras tbody tr").remove();
//            $("#IdMisComprasEnvio").css("display", "none");
//            $("#IdMisCompras").css("display", "block");
//            listaProductoComprados = result.data;
//            listaProductoComprados.forEach((item) => {
//                $("#IdtablaMisCompras tbody").append(`<tr class="cart_item">
//                                                    <td >

//                                                   <button tabindex="21" type="button"  onclick="VerDetalleMiPorducto(${item.id_compra_realizada}); return false;" class="btn btn-grad my-button"><span class="fa fa-cart-arrow-down faa-wrench animated"></span> Ver Detalle</button>
//                                                      </td>
//                                                   <td class="product-total">
//                                                    <span class="amount">${item.cantidad}</span>
//                                                    </td>
//                                                         <td class="product-total">
//                                                        <span class="amount">${ConvertirEnString(item.precioTotal)}</span>
//                                                    </td>
//                                                    <td class="product-total">
//                                                    <span class="amount">${ConvertirEnString(item.costoEnvio)}</span>
//                                                    </td>
//                                                    <td class="product-total">
//                                                        <span class="amount">${ConvertirEnString(item.precioFinal)}</span>
//                                                    </td>
//                                                 <td class="product-total">
//                                                        <span class="amount">${item.fecha_CreacionS}</span>
//                                                    </td>
//                                                  <td class="product-total">
//                                                        <span class="amount">${item.direccion}  ${item.opcional_Direccion == null ? "" :  item.opcional_Direccion}</span>
//                                                    </td>
//                                               </tr>
//             `);

//            });
//        }
//        else {
//            $("#IdtablaMisCompras tbody tr").remove();
//            $("#IdtablaMisComprasTotal tbody tr").remove();
//            $("#IdListaCompraC").html('');
//            $("#idFechaCreacion").html('');
//            $("#idEstadoTrazabilidad").html('');
//            $("#IdMisCompras").css("display", "none");
//            $("#IdMisComprasEnvio").css("display", "none");
//            create('error', 'Usted no cuenta con productos, o por favor valide su correo electrónico correctamente. gracias...', UrlAlertError);
//        }
//    }).fail()



//    //lista = JSON.parse(localStorage.getItem("ListaCarrito"));
//    //if (lista != null) {
//    //    $("#IdtablaCarrito2 tbody tr").remove();
//    //    lista.forEach((item) => {

//    //    });
//    //}
//}
var ListaProductoMC = [];
function VerDetalleMiPorducto(id_compra) {
    // Mostrar la sección de envío de compras
    $("#IdMisComprasEnvio").css("display", "block");
    // Limpiar la tabla de compras total
    $("#IdtablaMisComprasTotal tbody tr").remove();

    // Filtrar la lista de productos comprados por el id de compra
    let NuevaLista = listaProductoComprados.filter((item) => item.id_compra_realizada === id_compra);
    if (NuevaLista.length === 0) {
        console.error('No se encontraron productos con el id_compra:', id_compra);
        return;
    }

    // Obtener la lista de compras del primer elemento filtrado
    ListaProductoMC = NuevaLista[0].listaMisCompras;

    // Iterar sobre la lista de compras y agregar filas a la tabla
    ListaProductoMC.forEach((item) => {
        $("#IdtablaMisComprasTotal tbody").append(`
            <tr class="cart_item">
                <td class="product-total">
                    <span class="amount">
                    <a onclick="myModalProductoVenta(${item.id_producto}); return false;" data-bs-toggle="modal" data-bs-target="#productModalId3" class="w-img">
                        <img style="width: 60px !important; height: 50px !important;" src="${UrlImgProductos + item.listaImagenesC[0].FileName}" alt="product">
                    </span>
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

    // Obtener el elemento correspondiente de la lista de productos comprados
    let res = listaProductoComprados.find((item) => item.id_compra_realizada === id_compra);

    // Agregar detalles de trazabilidad del envío
    $("#IdListaCompraC").html(`
        <h5 style="text-align: center">Trazabilidad del envío</h5>
        <div class="lineatemp" style="font-size: 11px;">
            <div class="fila">
                <div class="disco ${res.fecha_CreacionS ? "validaClick" : ""}"><div></div></div>
                <div>${res.fecha_CreacionS}</div>
                <div class="pformulario"><strong>Orden recibida</strong></div>
            </div>
            <div class="fila">
                <div class="disco ${res.fecha_CreacionS ? "validaClick" : ""}"><div></div></div>
                <div>${res.fecha_CreacionS}</div>
                <div class="pformulario"><strong>En preparación</strong></div>
            </div>
            <div class="fila">
                <div class="disco ${res.fecha_Envio_productoS ? "validaClick" : ""}"><div></div></div>
                <div>${res.fecha_Envio_productoS || "Por Confirmar"}</div>
                <div class="pformulario"><strong>En camino</strong></div>
            </div>
            <div class="fila">
                <div class="disco ${res.fecha_Llegada_productoS ? "validaClick" : ""}"><div></div></div>
                <div>${res.fecha_Llegada_productoS || "Por Confirmar"}</div>
                <div class="pformulario"><strong>Entregado</strong></div>
            </div>
        </div>
        <br />
        <div class="" style="text-align: center; font-size: 14px; color: #333; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;">
            <strong>Producto enviado por:</strong>
        </div>
        <p style="text-align: center; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;">
            ${res.empresa_Entrega || "Por Confirmar"}
        </p>
        <div class="" style="text-align: center; font-size: 14px; color: #333; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;">
            <strong>Dirección de envío:</strong>
        </div>
        <p style="text-align: center; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;">
            ${res.direccion} ${res.opcional_Direccion ? ' - ' + res.opcional_Direccion : ''}
        </p>
        <p style="text-align: center; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;">
            ${res.pais} ${res.departamento ? '/' + res.departamento : ''} ${res.ciudad ? '/' + res.ciudad : ''}
        </p>
    `);

    // Actualizar la fecha de creación y el estado de trazabilidad
    $("#idFechaCreacion").html(`<h3 style="text-align: center"> Mi Compra Realizada, Fecha: ${res.fecha_CreacionS}</h3>`);

    let estado = "Orden recibida";
    if (res.fecha_Envio_productoS) {
        estado = "En camino";
    } else if (res.fecha_CreacionS) {
        estado = "En preparación";
    }
    if (res.fecha_Llegada_productoS) {
        estado = "Entregado";
    }
    $("#idEstadoTrazabilidad").html(`<h3 style="text-align: center"> Estado: ${estado}</h3>`);

    // Redirigir a la consulta de mi compra (si es necesario)
    RedirecionaConsultaMiCompra();
}

//function VerDetalleMiPorducto(id_compra) {
//    $("#IdMisComprasEnvio").css("display", "block");
//    $("#IdtablaMisComprasTotal tbody tr").remove();

//    let NuevaLista = listaProductoComprados.filter((item) => item.id_compra_realizada == id_compra);
//    var lll = NuevaLista[0].listaMisCompras;

//    lll.forEach((item) => {
//        $("#IdtablaMisComprasTotal tbody").append(`<tr class="cart_item">
//                                                        <td class="product-total">
//                                                            <span class="amount"><img style="width: 60px !important; height: 50px !important;" src="${UrlImgProductos + item.listaImagenesC[0].FileName}" alt="product"></span>
                                                        
//                                                        </td>
//                                                        <td class="">
//                                                               <strong class="">${item.producto}</strong>, Cantidad  <strong>${item.cantidad}</strong> Und
//                                                        </td>
//                                                        <td class="product-total">
//                                                            <span class="amount">${ConvertirEnString(item.precio_Unitario)}</span>
//                                                        </td>
//                                                        <td class="product-total">
//                                                            <span class="amount">${ConvertirEnString(item.precio_Total)}</span>
//                                                        </td>
//                                                    </tr>
//                                                `);




//    });

//    let res = listaProductoComprados.find((item) => item.id_compra_realizada == id_compra);
//    $("#IdListaCompraC").html('');
//    $("#IdListaCompraC").append(`
//                            <h5 style="text-align: center">
//                                    Trazabilidad del envío
//                                </h5>
//                                <div class="lineatemp" style="font-size: 11px;">
//                                    <div class="fila">
//                                        <div class="disco ${res.fecha_CreacionS == "" ? "" : "validaClick"}"><div></div></div>
//                                        <div> ${res.fecha_CreacionS}</div>
//                                        <div class="pformulario"><strong>Orden recibida</strong></div>
//                                    </div>
//                                    <div class="fila">
//                                        <div class="disco ${res.fecha_CreacionS == "" ? "" : "validaClick"}"><div></div></div>
//                                        <div> ${res.fecha_CreacionS} </div>
//                                        <div class="pformulario"><strong>En preparación</strong></div>
//                                    </div>
//                                    <div class="fila ">
//                                        <div class="disco ${res.fecha_Envio_productoS == "" ? "" : "validaClick"}"><div></div></div>
//                                        <div> ${res.fecha_Envio_productoS == "" ? "Por Confirmar" : res.fecha_Envio_productoS}</div>
//                                        <div class="pformulario"><strong>En camino</strong> </div>

//                                    </div>
//                                    <div class="fila">
//                                        <div class="disco ${res.fecha_Llegada_productoS == "" ? "" : "validaClick"}"><div></div></div>
//                                        <div> ${res.fecha_Llegada_productoS == "" ? "Por Confirmar" : res.fecha_Llegada_productoS}</div>
//                                        <div class="pformulario"><strong>Entregado</strong> </div>
//                                    </div>
//                                </div>
//                                <br />
//                                <div class="" style="text-align: center; font-size: 14px; color: #333; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;">
//                                    <strong>Producto enviado por:</strong>
//                                </div>
//                                <p style="text-align: center; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;">${res.empresa_Entrega == null ? "Por Confirmar" : res.empresa_Entrega }</p>
//                                <div class="" style="text-align: center; font-size: 14px; color: #333; position: relative; width: 276px; margin: 0 auto; border-radius: 10px;"><strong>Dirección de envío:</strong></div>
//                                <p style="text-align: center; position: relative; width: 276px; margin: 0 auto; border-radius: 10px; ">
//                                    ${res.direccion}  ${res.opcional_Direccion == null ? "" : ' - ' + res.opcional_Direccion} 
//                                </p>
//                                   <p style="text-align: center; position: relative; width: 276px; margin: 0 auto; border-radius: 10px; ">
//                                    ${res.pais}  ${res.departamento == null ? "" : '/' + res.departamento}  ${res.ciudad == null ? "" : '/' + res.ciudad} 
//                                </p>`);


//    let ss = listaProductoComprados.find((item) => item.id_compra_realizada == id_compra);
//    $("#idFechaCreacion").html('');
//    $("#idEstadoTrazabilidad").html('');
//    var validar = "";
//    $("#idFechaCreacion").append(` <h3 style="text-align: center"> Mi Compra Realizada, Fecha: ${ss.fecha_CreacionS}</h3>`);

//    if (ss.fecha_CreacionS != "") {
//        validar = "Orden recibida";
//    }
//    if (ss.fecha_CreacionS != "") {
//        validar = "En preparación";
//    }
//    if (ss.fecha_Envio_productoS != "") {
//        validar = "En camino";
//    }
//    if (ss.fecha_Llegada_productoS != "" ) {
//        validar = "Entregado";
//    }
//    $("#idEstadoTrazabilidad").append(` <h3 style="text-align: center"> Estado: ${validar}</h3>`);
//    RedirecionaConsultaMiCompra();

//}


function RedirecionaConsultaMiCompra() {
    const seccionDestino = document.getElementById('IdMisComprasEnvio');
    const posicionDestino = seccionDestino.offsetTop;
    window.scrollTo({
        top: posicionDestino,
        behavior: 'smooth'
    });
}

function myModalProductoVenta(id_compra) {
    // Limpiar contenido previo del modal
    $("#idModalProducto3").html("");
    $("#modalTabContent3").html("");

    // Encontrar el producto por ID
    const producto = ListaProductoMC.find((item) => item.id_producto === id_compra);

    // Crear contenido principal del modal
    const modalContent = `
        <div class="row">
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12">
                <div class="product__modal-box">
                    <div class="tab-content pb-10" id="modalTabContent3"></div>
                    <ul class="nav nav-tabs" id="modalTab" role="tablist"></ul>
                </div>
            </div>
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12">
                <div class="product__modal-content">
                    <div style="text-align: center !important;">
                        <h4 style="text-align: center !important; padding-right: 0px!important;">
                            <a href="#">${producto.producto}</a>
                        </h4>
                    </div>
                    <div class="product__modal-des mb-20" style="text-align: center !important;">
                        <p>${producto.descripcion_Producto}</p>
                    </div>
                     <div class="product__stock sku mb-05">
                        <span>Marca:</span>
                        <span>${producto.marca}</span>
                    </div>
                    <div class="product__stock sku mb-05">
                        <span>Categoria:</span>
                        <span>${producto.categoria}</span>
                    </div>
                    <div class="product__stock">
                        <span>Disponibilidad :</span>
                        <span>En Stock</span>
                    </div>
                    <div class="product__stock sku mb-40">
                        <span>Cantidad Compra:</span>
                        <span>${producto.cantidad}</span>
                    </div>
       
                    <div class="product__price text-center" style="text-align: center !important;">
                        <span id="idTotal2_${producto.id_producto}">${ConvertirEnString(producto.precio_Total)}</span>
                    </div>
               </div>
            </div>
           <div class="modal-footer d-flex justify-content-center">
    <button data-bs-dismiss="modal" class="btn btn-light" style=" border-color: #222; color: #ffffff ; background: var(--color1); align-content: space-around; justify-content: space-around;">
        <i class="fal fa-times-circle"></i> Cerrar
    </button>
     </div>
        </div>`;

    // Insertar el contenido en el modal
    $("#idModalProducto3").append(modalContent);
 
    // Añadir imágenes del producto al modal
    producto.listaImagenesC.forEach((item, index) => {
        const isActive = index === 0 ? "show active" : "";
     
        const imageHtml = `
            <div class="tab-pane fade ${isActive}" id="nav${index}" role="tabpanel" aria-labelledby="nav${index}-tab">
                <div class="product__modal-img w-img">
                    <img src="${UrlImgProductos + item.FileName}" alt="" width="85px" height="auto" />
                </div>
            </div>`;
        $("#modalTabContent3").append(imageHtml);

        const tabHtml = `
            <li class="nav-item" role="presentation">
                <button class="nav-link ${isActive}" id="nav${index}-tab" data-bs-toggle="tab" data-bs-target="#nav${index}" type="button" role="tab" aria-controls="nav${index}" aria-selected="false">
                    <img src="${UrlImgProductos + item.FileName}" alt="" width="85px" height="auto" />
                </button>
            </li>`;
        $("#modalTab").append(tabHtml);
    });
}
