


$(document).ready(function () {

    if (PaginaPrincipal == 4) {
        Prueba();
        CargarDominio1();
        CargarDominio2();
    }
});
var lista = [];
let currentPageIndex = 0;
let pageSize = 5;

function Prueba() {
    TotalPrecio();
    let lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    if (!lista || lista.length === 0) {
        console.log("La lista está vacía.");
        return;
    }

    $("#IdtablaCarrito tbody").empty(); // Limpiar la tabla antes de agregar nuevos elementos

    let startIndex = currentPageIndex * pageSize;
    let endIndex = Math.min(startIndex + pageSize, lista.length);

    // Iterar sobre los elementos dentro del rango de la página actual
    for (let i = startIndex; i < endIndex; i++) {
        let item = lista[i];
        $("#IdtablaCarrito tbody").append(`<tr>
                            <td class="product-thumbnail">
                              
                                  <a onclick="myModalProductoCarrito(${item.ProductoId}); return false;" data-bs-toggle="modal" data-bs-target="#productModalId1" class="w-img">
                                   <img src="${UrlImgProductos + item.listaImagenes[0].FileName}" alt="">
                                </a>
                            </td>
                            <td class="product-name"><a href="#">${item.Nombre}, - , ${item.marca}</a></td>
                            <td class="product-price"><span class="amount"  id="idprecio">${ConvertirEnString(item.Precio)}</span></td>
                            <td class="product-quantity">
                                <div class="cart-plus-minus" style="cursor:pointer"><input type="text" value="${item.cantidadProducto}" id="contadorProducto_${item.ProductoId}"><div class="dec qtybutton" onclick="RestarCantidadProducto(${item.ProductoId},${item.Precio})">-</div><div class="inc qtybutton" onclick="SumarCantidadProducto(${item.ProductoId},${item.Precio},${item.CantidadTotal})">+</div></div>
                            </td>
                            <td class="product-subtotal"><span class="amount" id="idTotal_${item.ProductoId}">${ConvertirEnString(item.precioTotal)}</span></td>
                            <td><button onclick="EliminarProductoCarrito(${item.ProductoId})" class="deleteButton"><i class="fa fa-times"></i></button></td>
                        </tr>`);
    }

    // Actualizar información de cantidad de productos y página actual
    $("#cantidadProductos").html(`<span style="color: black;">Total de productos: ${lista.length}</span>`);
    $("#paginaActual").html(`<span style="color: black;">Página actual: ${currentPageIndex + 1}</span>`);

    // Agregar botones de navegación para la paginación
    let totalPaginas = Math.ceil(lista.length / pageSize);
    let botonesNavegacion = '';
    if (currentPageIndex > 0) {
        botonesNavegacion += `<button onclick="cambiarPagina(${currentPageIndex - 1})">Anterior</button>`;
    }
    for (let i = 0; i < totalPaginas; i++) {
        botonesNavegacion += `<button onclick="cambiarPagina(${i})">${i + 1}</button>`;
    }
    if (currentPageIndex < totalPaginas - 1) {
        botonesNavegacion += `<button onclick="cambiarPagina(${currentPageIndex + 1})">Siguiente</button>`;
    }
    $("#botonesNavegacion").html(botonesNavegacion);
}

// Función para cambiar a una página específica
function cambiarPagina(index) {
    currentPageIndex = index;
    Prueba();
}

//function Prueba() {
//    TotalPrecio();
//    lista = JSON.parse(localStorage.getItem("ListaCarrito"));
//    $("#IdtablaCarrito tbody tr").remove();
//    lista.forEach((item) => {
//        $("#IdtablaCarrito tbody").append(`<tr>
//                                        <td class="product-thumbnail">
//                                            <a href="#">
//                                               <img src="${UrlImgProductos + item.listaImagenes[0].FileName}" alt="">
//                                            </a>
//                                        </td>
//                                        <td class="product-name"><a href="#">${item.Nombre}, - , ${item.marca}</a></td>
//                                        <td class="product-price"><span class="amount"  id="idprecio">${ConvertirEnString(item.Precio)}</span></td>
//                                        <td class="product-quantity">
//                                            <div class="cart-plus-minus" style="cursor:pointer"><input type="text" value="${item.cantidadProducto}" id="contadorProducto_${item.ProductoId}"><div class="dec qtybutton" onclick="RestarCantidadProducto(${item.ProductoId},${item.Precio})">-</div><div class="inc qtybutton" onclick = "SumarCantidadProducto(${item.ProductoId},${item.Precio},${item.CantidadTotal})">+</div></div>
//                                        </td>
//                                        <td class="product-subtotal"><span class="amount" id="idTotal_${item.ProductoId}">${ConvertirEnString(item.precioTotal)}</span></td>
//                                        <td ><button onclick="EliminarProductoCarrito(${item.ProductoId})" id="deleteButton"><i class="fa fa-times"></i></a></td>
//                                    </tr>`);


//    });
//}
function SumarCantidadProducto(productoId, Precio, cantidadP) {

    var kk = $("#contadorProducto_" + productoId).val();
    var ss = " productos en Stock";
    let valor = parseInt($("#contadorProducto_" + productoId).val());

    let Cantidad = 0;
    let operacion = 0;
    Cantidad = valor + 1;
    let total = Precio;
    operacion = (total * Cantidad);


    if (kk >= cantidadP) {
        create('error', 'Existen solo  ' + cantidadP + ss, UrlAlertError);
        $("#idTotal_" + productoId).html(ConvertirEnString(operacion - Precio));
        $("#contadorProducto_" + productoId).val(cantidadP);
        return;

    } else {
        lista.forEach((item) => {

            if (item.ProductoId == productoId) {
                item.precioTotal = operacion;
                item.cantidadProducto = Cantidad;
            }
        })
        let su = 0;

        lista.forEach((index) => {
            su = su + index.precioTotal;
        })

        var Result = $("#contadorProducto_" + productoId).val(valor + 1);
        $("#idTotal_" + productoId).html(ConvertirEnString(operacion));
        $("#idtotalCarrito ul li").remove();
        $("#idtotalCarrito ul").append(`<li>Subtotal <span>${ConvertirEnString(su)}</span></li>
                                        <li>Total <span>${ConvertirEnString(su)}</span></li>`);
        localStorage.removeItem("ListaCarrito");
        localStorage.setItem("ListaCarrito", JSON.stringify(lista));
        myPrecios();
    }

}
function RestarCantidadProducto(productoId, Precio) {

    let valor = parseInt($("#contadorProducto_" + productoId).val());
    if (valor < 1) {
        return false;
    }
    $("#contadorProducto_" + productoId).val(valor - 1);

    let Cantidad = valor - 1;
    let total = Precio;
    let operacion = (total * Cantidad);

    lista.forEach((item) => {

        if (item.ProductoId == productoId) {
            item.precioTotal = operacion;
            item.cantidadProducto = Cantidad;
        }
    })

    let su = 0;

    lista.forEach((index) => {
        su = su + index.precioTotal;
    })
    $("#idTotal_" + productoId).html(ConvertirEnString(operacion));
    $("#idtotalCarrito ul li").remove();
    $("#idtotalCarrito ul").append(`<li>Subtotal <span>${ConvertirEnString(su)}</span></li>
                                        <li>Total <span>${ConvertirEnString(su)}</span></li>`);

    localStorage.removeItem("ListaCarrito");
    localStorage.setItem("ListaCarrito", JSON.stringify(lista));
    myPrecios();
}
function EliminarProductoCarrito(Id_carrito) {

    let NuevaLista = lista.filter((item) => item.ProductoId != Id_carrito);
    lista = NuevaLista;

    localStorage.removeItem("ListaCarrito");
    localStorage.setItem("ListaCarrito", JSON.stringify(lista));

    Prueba();

    let su = 0;

    lista.forEach((index) => {
        su = su + index.precioTotal;
    })

    $("#idtotalCarrito ul li").remove();
    $("#idtotalCarrito ul").append(`<li>Subtotal <span>${ConvertirEnString(su)}</span></li>
                                        <li>Total <span>${ConvertirEnString(su)}</span></li>`);
    AgregarLista1();
    myPrecios();
    ListaVentaFinal();
    $("#IdOcultaMercadoPago").css("display", "none");
    $("#IdOcultaRealizarPedido").css("display", "block");
    NoOcultaCarrito();


   
  



 
    


    


}

function NoOcultaCarrito(){

    var modal = document.getElementById('modal');
    var cartToggle = document.querySelector('.cart__toggle');

    // Función para mostrar la modal
    function mostrarModal() {
        modal.style.display = 'block';
    }

    // Función para ocultar la modal
    function ocultarModal() {
        modal.style.display = 'none';
    }

    // Mostrar la modal cuando haces clic en el enlace
    cartToggle.addEventListener('click', function (event) {
        event.stopPropagation(); // Evitar la propagación del evento
        mostrarModal();
    });

    // Ocultar la modal cuando haces clic fuera de ella
    document.addEventListener('click', function (event) {
        if (!modal.contains(event.target) && event.target !== cartToggle) {
            ocultarModal();
        }
    });

    // Detener la propagación del evento clic en elementos dentro de la modal
    modal.addEventListener('click', function (event) {
        event.stopPropagation();
    });

    // Puedes agregar más listeners de clic a los elementos dentro de la modal aquí
    // Por ejemplo, para manejar el clic en el botón de eliminar dentro de la modal
    var deleteButton = document.getElementById('deleteButton');
    if (deleteButton !== null) {
    deleteButton.addEventListener('click', function (event) {
        // Realizar acciones de eliminación aquí
        // Ocultar la modal después de realizar la acción
        ocultarModal();
        event.stopPropagation();
    });
    }

    //// Mostrar la modal cuando haces clic en el enlace

    //var modal = document.getElementById('modal');

    //// Ocultar la modal cuando haces clic fuera de ella
    //document.addEventListener('click', function (evento) {
    //    if (!modal.contains(evento.target) && !document.querySelector('.cart__toggle').contains(evento.target)) {
    //        modal.style.display = 'block';
    //    }

    //});
}
function TotalPrecio() {
    let Resultado = 0;
    lista = JSON.parse(localStorage.getItem("ListaCarrito"));

    if (lista != null) {
        $("#idtotalCarrito ul li").remove();
        lista.forEach((item) => {
            let sumar = item.precioTotal;
            Resultado += sumar;
        });

        $("#idtotalCarrito ul").append(`<li>Subtotal <span>${ConvertirEnString(Resultado)}</span></li>
                                        <li>Total <span>${ConvertirEnString(Resultado)}</span></li>`);
    }
}

function myModalProductoCarrito(idProducto) {
    // Limpiar contenido previo del modal
    $("#idModalProducto1").html("");

    // Encontrar el producto por ID
    const producto = lista.find((item) => item.ProductoId === idProducto);

    // Crear contenido principal del modal
    const modalContent = `
        <div class="row">
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12">
                <div class="product__modal-box">
                    <div class="tab-content pb-10" id="modalTabContent1"></div>
                    <ul class="nav nav-tabs" id="modalTab" role="tablist"></ul>
                </div>
            </div>
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12">
                <div class="product__modal-content">
                    <div style="text-align: center !important;">
                        <h4 style="text-align: center !important; padding-right: 0px!important;">
                            <a href="#">${producto.Nombre}</a>
                        </h4>
                    </div>
                    <div class="product__modal-des mb-20" style="text-align: center !important;">
                        <p>${producto.Descripcion}</p>
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
                        <span>${producto.cantidadProducto}</span>
                    </div>
       
                    <div class="product__price text-center" style="text-align: center !important;">
                        <span id="idTotal2_${producto.ProductoId}">${ConvertirEnString(producto.precioTotal)}</span>
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
    $("#idModalProducto1").append(modalContent);

    // Añadir imágenes del producto al modal
    producto.listaImagenes.forEach((item, index) => {
        const isActive = index === 0 ? "show active" : "";
        const imageHtml = `
            <div class="tab-pane fade ${isActive}" id="nav${index}" role="tabpanel" aria-labelledby="nav${index}-tab">
                <div class="product__modal-img w-img">
                    <img src="${UrlImgProductos + item.FileName}" alt="" width="85px" height="auto" />
                </div>
            </div>`;
        $("#modalTabContent1").append(imageHtml);

        const tabHtml = `
            <li class="nav-item" role="presentation">
                <button class="nav-link ${isActive}" id="nav${index}-tab" data-bs-toggle="tab" data-bs-target="#nav${index}" type="button" role="tab" aria-controls="nav${index}" aria-selected="false">
                    <img src="${UrlImgProductos + item.FileName}" alt="" width="85px" height="auto" />
                </button>
            </li>`;
        $("#modalTab").append(tabHtml);
    });
}






