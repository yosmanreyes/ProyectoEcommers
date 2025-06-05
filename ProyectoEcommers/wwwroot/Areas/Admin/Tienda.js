$(document).ready(function () {



    if (PaginaPrincipal == 1) {

        const panelEncuesta = document.getElementById('sliderSection');
        /*      panelEncuesta.classList.remove('visible');*/
        panelEncuesta.classList.add('visible');
        tiempoInicio();
        consultarImagenesPeriodicamente();
        ConsultaProductosGeral(0, 0);
        GetConsultaDominios();
        consultarImagenesIzquierdaPeriodicamente()
        ConsultaImagenInferior();
        ConsultaImagenComentarios();
        GetMensajeScroll();
        cargueImagen();
  

    }
    else {
        tiempoFinal();
    }


    document.addEventListener('DOMContentLoaded', function () {
        var modal = document.getElementById('modal');

        // Mostrar la modal cuando haces clic en el enlace
        document.querySelector('.cart__toggle').addEventListener('click', function () {
            modal.style.display = 'block';
        });

        // Ocultar la modal cuando haces clic fuera de ella
        document.addEventListener('click', function (event) {
            if (!modal.contains(event.target) && !document.querySelector('.cart__toggle').contains(event.target)) {
                modal.style.display = 'none';
            }
        });
    });


});
function tiempoInicio() {

    /*    setTimeout(function () {*/
    $("#Process").css("display", "block");
    /*    }, 2000);*/
}
function tiempoFinal() {
    $("#Process").css("display", "none");
}
function cargueImagen() {
    setTimeout(function () {

        let rrrr = document.getElementsByClassName('slick-track');
        let yyy = rrrr[0].childNodes[1].firstChild.nextSibling.src;
        rrrr[0].childNodes[6].firstChild.nextSibling.src = yyy;

    }, 3000)
    tiempoFinal();
}

if ($(window).width() < 780) {
    $("#contenedorGrillaFive").remove();
    $("#contenedorGrillaList").remove();
    $("#Pruebas100").css("display", "none");
    $("#Pruebas101").css("display", "none");
    $("#idselectTodos").css("display", "none");
    function abrirModal() {
        $("#idModal").addClass("opened");
        openingModal = true;
    }

    // Función para cerrar la modal
    function cerrarModal() {
        $("#idModal").removeClass("opened");
    }

    // Abrir la modal cuando se hace clic en un botón o enlace
    $("#abrirModalBtn").click(function () {
        abrirModal();
    });

    // Cerrar la modal cuando se hace clic en el botón de cerrar
    $(".offcanvas__close-btn").click(function () {
        cerrarModal();
    });


    // Cerrar la modal cuando se hace clic fuera de ella, solo si está abierta
    $(document).click(function (event) {
        if ($('#idModal').hasClass('opened')) {
            if (!$(event.target).closest('#idModal').length) {
                if (!openingModal) { // Verificar si se está abriendo la modal
                    cerrarModal();
                } else {
                    openingModal = false; // Reiniciar la variable de control
                }
            }
        }
    });
}

// Función para consultar las imágenes iniciales y establecer un intervalo de actualización
async function consultarImagenesPeriodicamente() {
    // Consulta inicial al cargar la página
    await ConsultaImagenPrincipal();

    // Establecer intervalo para consultas periódicas cada 1 hora
    setInterval(async () => {
        await ConsultaImagenPrincipal();
    }, 60 * 60 * 1000); // 60 minutos * 60 segundos * 1000 milisegundos = 1 hora
}

// Función asincrónica para consultar las imágenes desde el servidor

// JavaScript
//async function ConsultaImagenPrincipal() {
//    try {
//        const panelEncuesta = document.getElementById('PanelEncuesta');

//        if (!panelEncuesta) {
//            console.error('No se encontró el contenedor de imágenes: PanelEncuesta.');
//            return;
//        }

//        // Obtener caché de imágenes desde localStorage
//        const cachedImages = JSON.parse(localStorage.getItem('cachedImages')) || [];

//        // Realizar la consulta AJAX
//        const response = await $.ajax({
//            type: 'POST',
//            url: UrlConsultaImagenSliderPrincipal
//        });

//        if (!response.success) {
//            console.error('La consulta no fue exitosa:', response);
//            return;
//        }

//        const tipo = response.data;
//        const imagenes = 5;
//        const newImageUrls = tipo.slice(0, imagenes).map(item => `${UrlImgCarrusel + item.Ruta}`);

//        // Comparar caché con nuevas URLs
//        if (JSON.stringify(cachedImages) === JSON.stringify(newImageUrls)) {
//            console.log('Las imágenes están en caché. No se necesita hacer una nueva solicitud.');
//            // Mostrar las imágenes desde caché
//            mostrarImagenes(newImageUrls);
//            return;
//        }

//        // Actualizar la caché
//        localStorage.setItem('cachedImages', JSON.stringify(newImageUrls));

//        // Mostrar las nuevas imágenes
//        mostrarImagenes(newImageUrls, true); // Pasar un parámetro para indicar que hay una actualización
//    } catch (error) {
//        console.error('Error al realizar la consulta:', error);
//    }
//}

//function mostrarImagenes(imageUrls, isUpdate = false) {
//    const imagenes = 5;
//    let loadedImagesCount = 0;

//    // Ocultar el contenedor de imágenes hasta que todas las imágenes se carguen
//    const panelEncuesta = document.getElementById('PanelEncuesta');
//    panelEncuesta.classList.add('hidden');

//    imageUrls.forEach((imgUrl, index) => {
//        const imgElement = document.getElementById(`img${index + 1}`);

//        if (!imgElement) {
//            console.error(`No se encontró el elemento con ID img${index + 1}`);
//            return;
//        }

//        imgElement.src = imgUrl;

//        // Esperar a que la imagen se cargue completamente
//        imgElement.onload = () => {
//            loadedImagesCount++;
//            imgElement.style.opacity = 1; // Hacer visible la imagen cuando esté cargada

//            if (loadedImagesCount === imagenes) {
//                console.log('Todas las imágenes se han cargado.');
//                // Mostrar el contenedor de imágenes solo después de que todas las imágenes estén cargadas
//                panelEncuesta.classList.remove('hidden');
//            }
//        };

//        // Manejar el caso en que la carga de la imagen falle
//        imgElement.onerror = () => {
//            console.error(`No se pudo cargar la imagen: ${imgUrl}`);
//        };
//    });
//}


async function ConsultaImagenPrincipal() {
    try {
        const panelEncuesta = document.getElementById('PanelEncuesta');
        const imagenes = 5;
        let loadedImagesCount = 0;

        // Ocultar el contenedor de imágenes inicialmente
        panelEncuesta.classList.remove('visible');

        // Realizar la consulta AJAX
        const response = await $.ajax({
            type: 'POST',
            url: UrlConsultaImagenSliderPrincipal
        });

        if (response.success) {
            const tipo = response.data;

            tipo.slice(0, imagenes).forEach((item, index) => {
                const imgElement = document.getElementById(`img${index + 1}`);
                const imgUrl = `${UrlImgCarrusel + item.Ruta}`;

                if (!imgElement) {
                    console.error(`No se encontró el elemento con ID img${index + 1}`);
                    return;
                }

                imgElement.src = imgUrl;

                // Esperar a que la imagen se cargue completamente
                imgElement.onload = () => {
                    loadedImagesCount++;
                    imgElement.style.opacity = 1; // Hacer visible la imagen cuando esté cargada

                    if (loadedImagesCount === imagenes) {
                        // Mostrar el contenedor de imágenes
                        panelEncuesta.classList.add('visible');
                    }
                };

                // Manejar el caso en que la carga de la imagen falle
                imgElement.onerror = () => {
                    console.error(`No se pudo cargar la imagen: ${imgUrl}`);
                };
            });
        } else {
            console.error('La consulta no fue exitosa.');
        }
    } catch (error) {
        console.error('Error al realizar la consulta:', error);
    }
}



//async function ConsultaImagenPrincipal() {
//    try {
//        const panelEncuesta = document.getElementById('PanelEncuesta');
//        panelEncuesta.classList.add('hidden');

//        const response = await $.ajax({
//            type: 'POST',
//            url: UrlConsultaImagenSliderPrincipal
//        });

//        if (response.success) {
//            const tipo = response.data;
//            const imagenes = 5;
//            let loadedImagesCount = 0;

//            tipo.slice(0, imagenes).forEach((item, index) => {
//                const imgElement = document.getElementById(`img${index + 1}`);
//                const imgUrl = `${UrlImgCarrusel + item.Ruta}`;

//                imgElement.src = imgUrl;

//                // Esperar a que la imagen se cargue completamente
//                imgElement.onload = () => {
//                    loadedImagesCount++;
//                    if (loadedImagesCount === imagenes) {
//                        panelEncuesta.classList.remove('hidden');
//                        // Aquí puedes mostrar un mensaje o realizar otra acción
//                    }
//                };

//                // Manejar el caso en que la carga de la imagen falle
//                imgElement.onerror = () => {
//                    console.error(`No se pudo cargar la imagen: ${imgUrl}`);
//                };
//            });
//        } else {
//            console.error('La consulta no fue exitosa.');
//        }
//    } catch (error) {
//        console.error('Error al realizar la consulta:', error);
//    }
//}

//async function ConsultaImagenPrincipal() {
//    try {
//        const response = await $.ajax({
//            type: 'POST',
//            url: UrlConsultaImagenSliderPrincipal
//        });

//        if (response.success) {
//            const tipo = response.data;
//            const imagenes = 5;

//            tipo.slice(0, imagenes).forEach((item, index) => {
//                document.getElementById(`img${index + 1}`).setAttribute('src', `${UrlImgCarrusel + item.Ruta}`);
//            });
//        } else {
//            console.error('La consulta no fue exitosa.');
//        }
//    } catch (error) {
//        console.error('Error al realizar la consulta:', error);
//    }
//}


async function consultarImagenesIzquierdaPeriodicamente() {
    // Consulta inicial al cargar la página
    await ConsultaImagenIzquierda();

    // Establecer intervalo para consultas periódicas cada 1 hora
    setInterval(async () => {
        await ConsultaImagenIzquierda();
    }, 60 * 60 * 1000); // 60 minutos * 60 segundos * 1000 milisegundos = 1 hora
}

// Función asincrónica para consultar las imágenes izquierdas desde el servidor
async function ConsultaImagenIzquierda() {
    try {
        const panelEncuestaIzquierda = document.getElementById('PanelEncuestaIzquierda');
        panelEncuestaIzquierda.classList.remove('visible');
        const response = await $.ajax({
            type: 'POST',
            url: UrlConsultaImagenIzquierda
        });

        if (response.success) {
            const tipo = response.data;
            tipo.forEach((item, index) => {
                document.getElementById(`imge${index + 1}`).setAttribute('src', `${UrlImg + item.Ruta}`);
            });
            panelEncuestaIzquierda.classList.add('visible');
        } else {
            console.error('La consulta no fue exitosa.');
        }
    } catch (error) {
        /*   console.error('Error al realizar la consulta:', error);*/
    }
}

// Llamar a la función para iniciar la consulta periódica de imágenes izquierdas
consultarImagenesIzquierdaPeriodicamente();

async function ConsultaImagenInferior() {
    try {
        const PanelEncuestaInferior = document.getElementById('PanelEncuestaInferior');
        PanelEncuestaInferior.classList.remove('visible');


        const response = await $.ajax({
            type: 'POST',
            url: UrlConsultaImagenInferior
        });

        if (response.success) {
            const tipo = response.data;
            tipo.forEach((item, index) => {
                document.getElementById(`imgI${index + 1}`).setAttribute('src', `${UrlImgInferior + item.Ruta}`);
            });
            PanelEncuestaInferior.classList.add('visible');
        } else {
            console.error('La consulta no fue exitosa.');
        }
    } catch (error) {
        console.error('Error al realizar la consulta:', error);
    }
}
function ConsultaImagenComentarios() {
    $.ajax({
        type: 'POST',
        async: false,
        url: UrlConsultaImagenComentarios,
        success: function success(response) {
            if (response.success == true) {
                tipo = response.data;
                let contador = 0;
                let ttt = $(".owl-stage")[0].children;
                let arr = Array.prototype.slice.call(ttt);

                arr.forEach((item) => {
                    /*   item.children[0].children[0].children[0].children[0].setAttribute('src', `${tipo[contador].Ruta}`);*/
                    item.children[0].children[0].children[0].children[0].setAttribute('src', `${UrlImgComentarios + tipo[contador].Ruta}`);
                    item.children[0].children[1].children[0].children[0].innerText = tipo[contador].TituloComentario;
                    item.children[0].children[1].children[1].children[1].innerText = tipo[contador].FechaComentario;
                    item.children[0].children[1].children[2].innerText = tipo[contador].Comentario;
                    contador++;
                    if (contador > 5) {
                        contador = 0;
                    }
                });

            } else {

            }
        },
        error: function error(ex) {
        }
    });
}
var lista = [];
var listaProductos = [];
var listaProductosAgregados = [];
function GuardarCarrito(Id_carrito) {
    let Nuev = listaProductosAgregados.filter((item) => item.ProductoId == Id_carrito);
    let variabke = Nuev;
    if (variabke != "") {
        CerrarModal();
        create('error', 'Usted ya agrego este producto...', UrlAlertError);
        return;

    } else {
        CerrarModal();
        create('success', 'Su producto fue agregado a su carrito exitosamente...', UrlAlertSucess);
    }
    let producto = listaProductos.find((item) => item.ProductoId == Id_carrito);
    listaProductosAgregados.push(producto);
    AgregarLista();
    myPrecios();
    Prueba();

}
function ConsultaProductosGeral(_marca, _categoria) {
    const IdProductosGeneral = document.getElementById('IdProductosGeneral');
    IdProductosGeneral.classList.remove('visible');

    $("#contenedorGrillaFour").html("");
    let data = { marca: _marca, categoria: _categoria }
    $.post(UrlGetProductosGeral, data).done(function (result) {

        if (result.data.length > 0) {

            fetchProducts(result.data);
            if (_marca == 0 && _categoria == 0) {
                _marca = null;
                _categoria = null;
            }
            if (_marca != null && _categoria != null) {
                $("#offcanvas__close-btn").click();
                RedirecionaConsulta();
            }
            IdProductosGeneral.classList.add('visible');
        }
        else {
            fetchProducts(listaProductos);


            create('error', 'Los productos seleccionados no se encuentran en Stock, por favor selecione otra Marca o Categoria, Gracias...', UrlAlertError);
        }
    }).fail();

}


// Función para aumentar las estrellas
function aumentarEstrellas(numero, numEstrellas, idProducto) {
    const starContainer = document.getElementById("starContainer");
    const stars = starContainer.querySelectorAll("li");
    // Asegúrate de que el número de estrellas esté en el rango adecuado
    numEstrellas = Math.min(stars.length, Math.max(numEstrellas, numero));
    // Itera sobre las estrellas y actualiza su apariencia
    for (let i = 0; i < stars.length; i++) {
        if (i < numEstrellas) {
            // Agrega una clase para resaltar las estrellas seleccionadas
            stars[i].classList.add("selected");
        } else {
            // Elimina la clase para desmarcar las estrellas no seleccionadas
            stars[i].classList.remove("selected");
        }
    }
}
function myModalProducto(idProducto) {
    // Limpiar contenido previo del modal
    $("#idModalProducto").html("");

    // Encontrar el producto por ID
    const producto = listaProductos.find((item) => item.ProductoId === idProducto);

    // Crear contenido principal del modal
    const modalContent = `
        <div class="row">
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12">
                <div class="product__modal-box">
                    <div class="tab-content pb-10" id="modalTabContent"></div>
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
                        <span>Cantidad:</span>
                        <span>${producto.CantidadTotal}</span>
                    </div>
                    <div class="product__review d-sm-flex">
                        <div class="rating rating__shop mb-15 mr-35" id="starContainer">
                            <ul>
                                ${[1, 2, 3, 4, 5].map((i) => `
                                    <li>
                                        <a onclick="aumentarEstrellas(${1},${i},${producto.ProductoId})">
                                            <i class="fal fa-star"></i>
                                        </a>
                                    </li>`).join('')}
                            </ul>
                        </div>
                        <div class="product__add-review mb-15">
                            <span><a href="#">1 reseña</a></span>
                            <span><a href="#">Agregar una opinión</a></span>
                        </div>
                    </div>
                    <div class="product__price text-center" style="text-align: center !important;">
                        <span id="idTotal2_${producto.ProductoId}">${ConvertirEnString(producto.precioTotal)}</span>
                    </div>
                    <div class="product__modal-form mb-10">
                        <form action="#">
                            <div style="display: flex; align-content: flex-start; justify-content: space-around; flex-wrap: wrap;">
                                <div class="product-quantity mb-25">
                                    <div style="text-align: center !important;">
                                        <div class="cart-plus-minus" style="cursor:pointer">
                                            <div onclick="RestarCantidadProducto2(${producto.ProductoId}, ${producto.Precio})">-</div>
                                            <input type="text" value="${producto.cantidadProducto}" id="contadorProducto_${producto.ProductoId}">
                                            <div onclick="SumarCantidadProducto2(${producto.ProductoId}, ${producto.Precio}, ${producto.CantidadTotal})">+</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="pro-cart-btn mb-25">
                                    <button class="t-y-btn" id="IdProdu_${producto.ProductoId}" onclick="GuardarCarritoXproducto(${producto.ProductoId}, ${producto.cantidadProducto}, ${producto.precioTotal}); return false;">Agregar</button>
                                </div>
                            </div>
                        </form>
                    </div>
  
                </div>
            </div>
        </div>`;

    // Insertar el contenido en el modal
    $("#idModalProducto").append(modalContent);


    // Añadir imágenes del producto al modal
    producto.listaImagenes.forEach((item, index) => {
        const isActive = index === 0 ? "show active" : "";
        const imageHtml = `
            <div class="tab-pane fade ${isActive}" id="nav${index}" role="tabpanel" aria-labelledby="nav${index}-tab">
                <div class="product__modal-img w-img" style="position: relative;">
                    <img src="${UrlImgProductos + item.FileName}" alt="" width="85px" height="auto" />
                </div>
            </div>`;
        $("#modalTabContent").append(imageHtml);

        const tabHtml = `
            <li class="nav-item" role="presentation">
                <button class="nav-link ${isActive}" id="nav${index}-tab" data-bs-toggle="tab" data-bs-target="#nav${index}" type="button" role="tab" aria-controls="nav${index}" aria-selected="false">
                    <img src="${UrlImgProductos + item.FileName}" alt="" width="85px" height="auto" />
                </button>
            </li>`;
        $("#modalTab").append(tabHtml);
    });

}
function ConvertirEnString(valor) {// recibe numero float y lo devuelve en formato con puntos y comas y el signo $

    if (valor - Math.trunc(valor) > 0)
        valor = valor.toFixed(2);

    valor = "$ " + parseFloat(valor).toLocaleString('es-CO');
    return valor;
}
function AgregarLista() {
    $("#IdListaCarrito").html('');
    listaProductosAgregados.forEach((item) => {
        $("#IdListaCarrito").append(`
            <div class="cart__item d-flex justify-content-between align-items-center">
                <div class="cart__inner d-flex">
                    <div class="cart__thumb">
                        <a href="#">
                            <img src="${UrlImgProductos + item.listaImagenes[0].FileName}" alt="">
                        </a>
                    </div>
                    <div class="cart__details">
                        <h6><a href="#"> ${item.Nombre}  </a></h6>
                        <div class="cart__price">
                            <span>${ConvertirEnString(item.Precio)}</span>
                        </div>
                    </div>
                </div>
                <div class="cart__del">
                    <button onclick="EliminarProductoCarrito(${item.ProductoId})" id="deleteButton"><i class="fal fa-trash-alt"></i></button>
                </div>
            </div>`);
    });

    localStorage.removeItem("ListaCarrito");
    localStorage.setItem("ListaCarrito", JSON.stringify(listaProductosAgregados));
    myPrecios();
}
function CerrarModal() {
    $('#productModalId').modal('hide');
}
function AgregarLista1() {
    lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    $("#IdListaCarrito").html('');
    lista.forEach((item) => {
        $("#IdListaCarrito").append(`<div class="cart__item d-flex justify-content-between align-items-center">
                                                        <div class="cart__inner d-flex">
                                                            <div class="cart__thumb">
                                                                <a href="#">
                                                                    <img src="${UrlImgProductos + item.listaImagenes[0].FileName}" alt="">
                                                                </a>
                                                            </div>
                                                            <div class="cart__details">
                                                                <h6><a href="#"> ${item.Nombre}  </a></h6>
                                                                <div class="cart__price">
                                                                    <span>${ConvertirEnString(item.Precio)}</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="cart__del">
                                                            <button onclick="EliminarProductoCarrito(${item.ProductoId})" id="deleteButton"><i class="fal fa-trash-alt"></i></button>
                                                        </div>
                                                    </div>`);
    });
}
function EliminarProductoCarrito(Id_carrito) {
    let NuevaLista = listaProductosAgregados.filter((item) => item.ProductoId != Id_carrito);
    listaProductosAgregados = NuevaLista;
    AgregarLista();
    myPrecios();
}
function ValidarCarrito() {
    TotalPrecio();
    lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    $("#IdtablaCarrito tbody tr").remove();
    lista.forEach((item) => {
        $("#IdtablaCarrito tbody").append(`<tr>
                                        <td class="product-thumbnail">
                                            <a href="#">
                                               <img src="${UrlImgProductos + item.listaImagenes[0].FileName}" alt="">
                                            </a>
                                        </td>
                                        <td class="product-name"><a href="#">${item.Nombre}</a></td>
                                        <td class="product-price"><span class="amount"  id="idprecio">${ConvertirEnString(item.Precio)}</span></td>
                                        <td class="product-quantity">
                                            <div class="cart-plus-minus" style="cursor:pointer"><input type="text" value="${item.cantidadProducto}" id="contadorProducto_${item.ProductoId}"><div class="dec qtybutton" onclick="RestarCantidadProducto(${item.ProductoId},${item.Precio})">-</div><div class="inc qtybutton" onclick = "SumarCantidadProducto(${item.ProductoId},${item.Precio},${item.CantidadTotal})">+</div></div>
                                        </td>
                                        <td class="product-subtotal"><span class="amount" id="idTotal_${item.ProductoId}">${ConvertirEnString(item.precioTotal)}</span></td>
                                        <td ><button onclick="EliminarProductoCarrito(${item.ProductoId})" id="deleteButton"><i class="fa fa-times"></i></a></td>
                                    </tr>`);


    });
}
function RestarCantidadProducto2(productoId, Precio) {

    let valor = parseInt($("#contadorProducto_" + productoId).val());
    if (valor < 1) {
        return false;
    }
    $("#contadorProducto_" + productoId).val(valor - 1);

    let Cantidad = valor - 1;
    let total = Precio;
    let operacion = (total * Cantidad);

    if (lista != null) {

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
    }
    $("#idTotal2_" + productoId).html(ConvertirEnString(operacion));
    $("#IdProdu_" + productoId).attr("onclick", `GuardarCarritoXproducto(${productoId},${Cantidad},${operacion});return false;`);


}
function SumarCantidadProducto2(productoId, Precio, cantidadp) {

    var kk = $("#contadorProducto_" + productoId).val();
    var ss = " productos en Stock";
    let valor = parseInt($("#contadorProducto_" + productoId).val());
    var Result = $("#contadorProducto_" + productoId).val(valor + 1);

    let Cantidad = valor + 1;
    let total = Precio;
    let operacion = (total * Cantidad);
    if (lista != null) {

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
    }
    if (kk >= cantidadp) {
        create('error', 'Existen solo  ' + cantidadp + ss, UrlAlertError);
        $("#idTotal2_" + productoId).html(ConvertirEnString(operacion - Precio));
        $("#contadorProducto_" + productoId).val(cantidadp);
        return;
    }
    $("#idTotal2_" + productoId).html(ConvertirEnString(operacion));
    $("#IdProdu_" + productoId).attr("onclick", `GuardarCarritoXproducto(${productoId},${Cantidad},${operacion});return false;`)

}
function GuardarCarritoXproducto(Id_carrito, Result, operacion) {
    let Nuev = listaProductosAgregados.filter((item) => item.ProductoId == Id_carrito);
    let variabke = Nuev;
    if (variabke != "") {
        create('error', 'Usted ya agrego este producto...', UrlAlertError);
        CerrarModal();
        return;
    } else {

        let producto = listaProductos.find((item) => item.ProductoId == Id_carrito);

        if (producto) {
            producto.cantidadProducto = Result;
            producto.precioTotal = operacion;
            listaProductosAgregados.push(producto);
        } else {
            console.log("Producto no encontrado con el ID especificado.");
        }
        AgregarLista();
        Prueba();
        CerrarModal();
        create('success', 'Su producto fue agregado a su carrito exitosamente...', UrlAlertSucess);
    }
}
function GetMensajeScroll() {

    const IdPanelMaquesina = document.getElementById('IdPanelMaquesina');
    IdPanelMaquesina.classList.remove('visible');


    var urel = urlMensaje;
    var ContMensaje = $("#v_leyenda");
    var ContFuncionario = $("#pMensaje");

    $.ajax({
        type: "POST",
        url: urel,
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                let Validar = respuesta.data;

                if (Validar.length > 0) {
                    let v_leyenda = Validar[0].TituloMensaje;
                    ContMensaje.html(v_leyenda);
                    let Noticias = "";
                    let Resultado = "";
                    for (var i = 0; i < Validar.length; i++) {
                        var fila = Validar[i].Mensaje;
                        Noticias += `<span class='fas fa-star'></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ${fila} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;`;
                        Resultado = Noticias;

                    }
                    ContFuncionario.html(Resultado);
                    IdPanelMaquesina.classList.add('visible');
                    $("#idMensaje").css("display", "block");
                }

                else {
                }
            }
            else {
                ContMensaje.html('');
                ContFuncionario.html('');
                $("#idMensaje").css("display", "none");
            }
        },
        error: function () {

        }

    });

}
function validarCorreoVenta() {
    var correo = document.getElementById("txtCorreoElectronico").value;
    var expresionRegularCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (expresionRegularCorreo.test(correo)) {
    } else {
        return 1;
    }
}
function validarTelefono() {
    var telefono = document.getElementById("txtCelular").value;
    var expresionRegularTelefono = /^\d{10}$/;

    if (expresionRegularTelefono.test(telefono)) {
        mensajeCelular.innerHTML = "Número de télefono celular válido.";
        mensajeCelular.style.color = "green";
    } else {
        mensajeCelular.innerHTML = "Por favor, introduce un número de télefono celular válido.";
        mensajeCelular.style.color = "red";
        return 1;

    }
}
function validarCodigoPostal() {
    var codigoPostal = document.getElementById("txtCodigoPostal").value;
    var expresionRegularCodigoPostal = /^[0-9]{5}$/;
    if (expresionRegularCodigoPostal.test(codigoPostal)) {
        mensajeCodigoPostal.innerHTML = "Código postal válido.";
        mensajeCodigoPostal.style.color = "green";
    } else {


        mensajeCodigoPostal.innerHTML = "Por favor, introduce un código postal válido.";
        mensajeCodigoPostal.style.color = "red";
        return 1;

    }
}
function validarCorreo() {
    var correo = document.getElementById("email").value;
    var expresionRegularCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (expresionRegularCorreo.test(correo)) {
    } else {
        return 1;
    }
}
function validarCorreos() {
    var correo1 = document.getElementById("txtCorreoElectronico").value;
    var correo2 = document.getElementById("txtCorreoElectronicoConfirmacion").value;
    var expresionRegularCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (expresionRegularCorreo.test(correo2)) {
        mensaje.innerHTML = "Correo electrónico válido.";
        mensaje.style.color = "green";

    } else {
        mensaje.innerHTML = "Por favor, introduce un correo electrónico válido.";
        mensaje.style.color = "red";
        return 1;
    }
    mensaje.innerHTML = "";
    if (correo1 === correo2) {

        mensaje.innerHTML = "Los correos electrónicos son iguales.";
        mensaje.style.color = "green"; // Cambia el color a verde si son iguales

    } else {
        mensaje.innerHTML = "Los correos electrónicos no son iguales.";
        mensaje.style.color = "red";
        return 1;
    }
}
function validarCorreo1() {
    var correo1 = document.getElementById("txtCorreoElectronico").value;


    var expresionRegularCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (expresionRegularCorreo.test(correo1)) {
        mensaje1.innerHTML = "Correo electrónico válido.";
        mensaje1.style.color = "green";

    } else {
        mensaje1.innerHTML = "Por favor, introduce un correo electrónico válido.";
        mensaje1.style.color = "red";
        return 1;
    }
}
function validarCorreosContacto() {
    var correo1 = document.getElementById("CorreoElectronico").value;
    var expresionRegularCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (expresionRegularCorreo.test(correo1)) {
        mensajeContacto.innerHTML = "Correo electrónico válido.";
        mensajeContacto.style.color = "green";

    } else {
        mensajeContacto.innerHTML = "Por favor, introduce un correo electrónico válido.";
        mensajeContacto.style.color = "red";
        return 1;
    }

}

function validarNumeroTelefono() {
    var correo1 = document.getElementById("txtCelular").value;
    var correo2 = document.getElementById("txtCelulaConfirmacion").value;
    mensajeCelularConfirmacion.innerHTML = "";
    if (correo1 === correo2) {

        mensajeCelularConfirmacion.innerHTML = "Los números de teléfono son iguales.";
        mensajeCelularConfirmacion.style.color = "green"; // Cambia el color a verde si son iguales

    } else {

        mensajeCelularConfirmacion.innerHTML = "Los números de teléfono no son iguales.";
        mensajeCelularConfirmacion.style.color = "red";
        return 1;
    }
}

//----------------paginacion---------------------------
const productsPerPage = 20;
let currentPage = 0;
let totalProducts;
let productsArray;
function fetchProducts(_lista) {

    currentPage = 1;
    productsArray = _lista;
    totalProducts = _lista.length;
    loadProducts(currentPage);

}
function showPage(pageNumber) {
    loadProducts(pageNumber);
    currentPage = pageNumber;
    realizarConsultaPaginado();

}
function nextPage() {
    if (currentPage < getTotalPages()) {
        showPage(currentPage + 1);
        realizarConsultaPaginado();
    }
}
function prevPage() {
    if (currentPage > 1) {
        showPage(currentPage - 1);
        realizarConsultaPaginado();
    }
}
function getTotalPages() {
    return Math.ceil(totalProducts / productsPerPage);
}

function loadProducts(pageNumber) {
    const startIndex = (pageNumber - 1) * productsPerPage;
    const endIndex = startIndex + productsPerPage;
    listaProductos = productsArray.slice(startIndex, endIndex);

    // Limpiar los contenedores
    $("#contenedorGrillaFour").html("");
    $("#contenedorGrillaFive").html("");
    $("#contenedorGrillaList").html("");

    if (listaProductos.length > 0) {
        listaProductos.forEach((item) => {
            // Contenido para Grilla Four
            $("#contenedorGrillaFour").append(`
                <div class="col-xxl-3 col-xl-3 col-lg-4 col-md-4">
                    <div class="product__item white-bg mb-30">
                        <div class="product__thumb p-relative" style="height: 100%;max-width: 100%;display: flex;align-items: center; justify-content: center;">
                            <a onclick="myModalProducto(${item.ProductoId}); return false;" data-bs-toggle="modal" data-bs-target="#productModalId" class="w-img">
                                <img src="${UrlImgProductos + item.listaImagenes[0].FileName}" alt="product">
                            </a>
                            <div class="product__action p-absolute">
                                <ul>
                                    <li><a href="#" title="Añadir a la lista de deseos"><i class="fal fa-heart"></i></a></li>
                                    <li><a onclick="myModalProducto(${item.ProductoId}); return false;" title="Vista rápida" data-bs-toggle="modal" data-bs-target="#productModalId"><i class="fal fa-search"></i></a></li>
                                    <li><a href="#" title="Comparar"><i class="far fa-sliders-h"></i></a></li>
                                </ul>
                            </div>
                        </div>
                        <div class="product__content text-center">
                            <h6 class="product-name">
                                <a class="product-item-link" href="#"> ${item.Nombre}, - , ${item.marca}</a>
                            </h6>
                            <div class="rating">
                                <ul>
                                    <li><a href="#"><i class="far fa-star"></i></a></li>
                                    <li><a href="#"><i class="far fa-star"></i></a></li>
                                    <li><a href="#"><i class="far fa-star"></i></a></li>
                                    <li><a href="#"><i class="far fa-star"></i></a></li>
                                    <li><a href="#"><i class="far fa-star"></i></a></li>
                                </ul>
                            </div>
                            <span class="price">${ConvertirEnString(item.Precio)}</span>
                        </div>
                        <div class="product__add-btn">
                            <button type="button" id="${item.ProductoId}" onclick="GuardarCarrito(${item.ProductoId}); return false;">Agregar al carrito</button>
                        </div>
                    </div>
                </div>
            `);

            // Contenido para Grilla Five
            $("#contenedorGrillaFive").append(`
                <div class="col">
                    <div class="product__item white-bg mb-30">
                        <div class="product__thumb p-relative" style="max-height: 258px; max-width: 258px; display: flex; align-items: center; justify-content: center;">
                            <a onclick="myModalProducto(${item.ProductoId}); return false;" data-bs-toggle="modal" data-bs-target="#productModalId" class="w-img">
                                <img src="${UrlImgProductos + item.listaImagenes[0].FileName}" alt="product">
                            </a>
                            <div class="product__action p-absolute">
                                <ul>
                                    <li><a href="#" title="Add to Wishlist"><i class="fal fa-heart"></i></a></li>
                                    <li><a onclick="myModalProducto(${item.ProductoId}); return false;" title="Quick View" data-bs-toggle="modal" data-bs-target="#productModalId"><i class="fal fa-search"></i></a></li>
                                    <li><a href="#" title="Compare"><i class="far fa-sliders-h"></i></a></li>
                                </ul>
                            </div>
                        </div>
                        <div class="product__content text-center">
                            <h6 class="product-name">
                                <a class="product-item-link" href="#"> ${item.Nombre}, - , ${item.marca}</a>
                            </h6>
                            <div class="rating">
                                <ul>
                                    <li><a href="#"><i class="far fa-star"></i></a></li>
                                    <li><a href="#"><i class="far fa-star"></i></a></li>
                                    <li><a href="#"><i class="far fa-star"></i></a></li>
                                    <li><a href="#"><i class="far fa-star"></i></a></li>
                                    <li><a href="#"><i class="far fa-star"></i></a></li>
                                </ul>
                            </div>
                            <span class="price">${ConvertirEnString(item.Precio)}</span>
                        </div>
                        <div class="product__add-btn">
                            <button type="button" id="${item.ProductoId}" onclick="GuardarCarrito(${item.ProductoId}); return false;">Agregar al carrito</button>
                        </div>
                    </div>
                </div>
            `);

            // Contenido para Lista de Productos
            $("#contenedorGrillaList").append(`
                <div class="product__item product__list white-bg mb-30 d-md-flex">
                    <div class="product__thumb p-relative mr-20">
                        <a onclick="myModalProducto(${item.ProductoId}); return false;" data-bs-toggle="modal" data-bs-target="#productModalId" class="w-img">
                            <img src="${UrlImgProductos + item.listaImagenes[0].FileName}" alt="product">
                        </a>
                    </div>
                    <div class="product__content">
                        <h6 class="product-name">
                            <a class="product-item-link" href="#">${item.Nombre}</a>
                        </h6>
                        <div class="rating d-sm-flex d-lg-block d-xl-flex align-items-center">
                            <ul>
                                <li><a href="#"><i class="far fa-star"></i></a></li>
                                <li><a href="#"><i class="far fa-star"></i></a></li>
                                <li><a href="#"><i class="far fa-star"></i></a></li>
                                <li><a href="#"><i class="far fa-star"></i></a></li>
                                <li><a href="#"><i class="far fa-star"></i></a></li>
                            </ul>
                            <div class="product-review-action ml-30">
                                <span><a href="#">2 calificación</a></span>
                                <span><a href="#">Agregar una opinión</a></span>
                            </div>
                        </div>
                        <span class="price">${ConvertirEnString(item.Precio)}</span>
                        <div class="product__list-features">
                            <p class="product-text">${item.Descripcion}</p>
                        </div>                             
                        <div class="product__stock sku mb-05">
                            <span>Marca:</span>
                            <span>${item.marca}</span>
                        </div>
                        <div class="product__stock sku mb-05">
                            <span>Categoria:</span>
                            <span>${item.categoria}</span>
                        </div>
                        <div class="product__stock">
                            <span>Disponibilidad :</span>
                            <span>En Stock</span>
                        </div>
                        <div class="product__stock sku mb-05">
                            <span>Cantidad:</span>
                            <span>${item.CantidadTotal}</span>
                        </div>
                        <div class="pro-cart-btn mb-25 text-center">
                            <button class="t-y-btn" id="IdProd_${item.ProductoId}" onclick="GuardarCarrito(${item.ProductoId}); return false;">Agregar al Carrito</button>
                        </div>                            
                    </div>
                </div>
            `);

            // Configuración adicional del producto
            item.precioTotal = item.Precio;
            item.cantidadProducto = 1;
        });
    } else {
        create('error', 'Los productos seleccionados no se encuentran en Stock, por favor seleccione otra Marca o Categoria, Gracias...', UrlAlertError);
    }

    // Generar paginación y mostrar la información de resultados
    generatePagination();
    showPageContent(pageNumber);
    showResultsInfo(startIndex + 1, endIndex, totalProducts);
}

function generatePagination() {
    const totalPages = getTotalPages();
    const paginationContainer = document.querySelector('.pagination');
    paginationContainer.innerHTML = '';

    for (let i = 1; i <= totalPages; i++) {
        const li = document.createElement('li');
        li.textContent = i;
        li.onclick = function () {
            showPage(i);
        };
        paginationContainer.appendChild(li);
    }

    const prevButton = document.createElement('li');
    prevButton.textContent = 'Anterior';
    prevButton.onclick = prevPage;
    paginationContainer.insertBefore(prevButton, paginationContainer.firstChild);

    const nextButton = document.createElement('li');
    nextButton.textContent = 'Siguiente';
    nextButton.onclick = nextPage;
    paginationContainer.appendChild(nextButton);
}
function showPageContent(pageNumber) {
    const products = document.querySelectorAll('.product');

    products.forEach((product, index) => {
        if (index + 1 === pageNumber) {
            product.style.display = 'block';
        } else {
            product.style.display = 'none';
        }
    });
}
function realizarConsultaPaginado() {
    const seccionDestino = document.getElementById('seccionDestino');
    const posicionDestino = seccionDestino.offsetTop;
    window.scrollTo({
        top: posicionDestino,
        behavior: 'smooth'
    });

}
function showResultsInfo(startIndex, endIndex, totalResults) {
    const resultsInfo = document.querySelector('.results-info2');
    resultsInfo.textContent = `Mostrando  ${startIndex} - ${endIndex}  de  ${totalResults}  resultados  `;
}
function changeItemsPerPage() {
    const selectElement = document.getElementById('itemsPerPageSelect');
    itemsPerPage = selectElement.value;
    showPage(1); // Cargar la primera página al cambiar la cantidad de elementos por página
}
function Getsubscribete() {

    var retur = validarCorreo();

    if (retur == 1) {
        create('error', 'Por favor, introduce un correo electrónico válido...', UrlAlertError);
        return;
    }

    let email = $("#email").val();

    if (email == "" || email == null) {
        create('success', 'Por favor registre su correo electrónico, Gracias...', UrlAlertSucess);
        return;
    }

    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    var datos = { __RequestVerificationToken: token, _email: email };
    $.ajax({
        type: "POST",
        url: RutaInscripcionBoletin,
        data: datos,
        dataType: 'json',
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                create('success', 'Distruibuidora de belleza MAJAS, Agradece por sus subcripcíon pronto te llegara un correo con más información...', UrlAlertSucess);
                $('#email').val("");
            } else {
                alert('No fue posible subscribirte intentelo mas tarde.');

                $('#email').val("");
            }
        },
        error: function () {
        }
    });
}

/*if (PaginaPrincipal == 2) {*/
GetConsultapaisesDominios();

/*}*/
var listaPais = [];
function GetConsultapaisesDominios() {

    let data = {}
    $.post(UrlGetPaisesDominiosX, data).done(function (result) {

        if (result.data.length > 0) {
            listaPais = result.data;
            localStorage.removeItem("listaPaises");
            localStorage.setItem("listaPaises", JSON.stringify(listaPais));
        } else { }
    }).fail();

}

$(document).ready(function () {
    let lens, largeLens; // Variables para las lupas pequeña y grande

    // Al mostrar el modal por primera vez
    $('#productModalId').on('shown.bs.modal', function () {
        initializeLens();
        activateThumbnailClickHandlers();
        // Simular un clic en la primera miniatura
        const firstThumbnail = document.querySelector('#modalTab .nav-link');
        if (firstThumbnail) {
            firstThumbnail.click();
        }
    });

    // Al ocultar el modal
    $('#productModalId').on('hidden.bs.modal', function () {
        clearLens();
        deactivateThumbnailClickHandlers();
    });

    // Función para inicializar las lupas
    function initializeLens() {
        const activeTabContent = document.querySelector('#modalTab .tab-pane.active.show');
        const img = activeTabContent ? activeTabContent.querySelector('.product__modal-img img') : null;

        if (img) {
            createLens(img);
            createLargeLens(img); // Crear la segunda lupa grande
            showLenses();
            setTimeout(() => {
                moveLens({ type: 'mousemove', clientX: img.getBoundingClientRect().left + img.width / 2, clientY: img.getBoundingClientRect().top + img.height / 2, target: img });
            }, 0);
        } else {
            /*  console.error('Imagen no encontrada dentro del contenido activo de la pestaña.');*/
        }
    }

    // Función para crear y configurar la lupa pequeña
    function createLens(img) {
        lens = document.createElement("div");
        lens.setAttribute("id", "lens");
        lens.style.position = "absolute";
        lens.style.border = "1px solid #aaa";
        lens.style.width = "100px"; // Tamaño inicial de la lupa
        lens.style.height = "100px";
        lens.style.backgroundRepeat = "no-repeat";
        lens.style.visibility = "hidden";
        lens.style.pointerEvents = "none";
        lens.style.zIndex = "888"; // Ajustar z-index para que esté sobre otros elementos
        img.parentNode.appendChild(lens);

        img.addEventListener("mousemove", moveLens);
        img.addEventListener("mouseleave", hideLens);

        // Añadir eventos para móviles
        img.addEventListener("touchmove", moveLens);
        img.addEventListener("touchend", hideLens);
    }

    // Función para crear y configurar la lupa grande
    function createLargeLens(img) {
        largeLens = document.createElement("div");
        largeLens.setAttribute("id", "largeLens");
        largeLens.style.position = "fixed"; // Fijar posición de la lupa grande
        largeLens.style.border = "2px solid #aaa";
        largeLens.style.backgroundColor = "#fff"; // Fondo blanco
        largeLens.style.backgroundRepeat = "no-repeat";
        largeLens.style.visibility = "hidden";
        largeLens.style.pointerEvents = "none";
        largeLens.style.zIndex = "1061"; // Asegurarse de que esté sobre el modal (bootstrap modal z-index es 1050)
        updateLargeLensPosition();

        window.addEventListener('resize', updateLargeLensPosition);
        document.body.appendChild(largeLens);
    }

    // Función para actualizar la posición de la lupa grande según el tamaño de la pantalla
    function updateLargeLensPosition() {
        if (window.matchMedia("(max-width: 576px)").matches) {
            largeLens.style.width = "200px";
            largeLens.style.height = "200px";
            largeLens.style.left = "50%";
            largeLens.style.top = "60%";
            largeLens.style.transform = "translate(-50%, -50%)";
        } else if (window.matchMedia("(max-width: 768px)").matches) {
            largeLens.style.width = "300px";
            largeLens.style.height = "300px";
            largeLens.style.left = "50%";
            largeLens.style.top = "75%";
            largeLens.style.transform = "translate(-50%, -50%)";
        } else if (window.matchMedia("(max-width: 1895px)").matches) {
            largeLens.style.width = "350px";
            largeLens.style.height = "350px";
            largeLens.style.left = "65%";
            largeLens.style.top = "48%";
            largeLens.style.transform = "translate(-50%, -50%)";
        } else {
            largeLens.style.width = "400px";
            largeLens.style.height = "400px";
            largeLens.style.left = "60%";
            largeLens.style.top = "50%";
            largeLens.style.transform = "translate(-50%, -50%)";
        }
    }

    // Función para mostrar las lupas
    function showLenses() {
        if (lens) {
            lens.style.visibility = "visible";
        }
        if (largeLens) {
            largeLens.style.visibility = "visible";
        }
    }

    // Función para mover las lupas según la posición del cursor
    function moveLens(e) {
        const img = e.target;
        const imgRect = img.getBoundingClientRect();
        const lensRect = lens.getBoundingClientRect();

        let posX, posY;

        if (e.type === "mousemove") {
            posX = e.clientX - imgRect.left;
            posY = e.clientY - imgRect.top;
        } else if (e.type === "touchmove") {
            const touch = e.touches[0];
            posX = touch.clientX - imgRect.left;
            posY = touch.clientY - imgRect.top;
        }

        // Limitar las coordenadas x e y para que la lupa no se salga completamente del área de la imagen
        let x = posX - (lensRect.width / 2);
        let y = posY - (lensRect.height / 2);

        // Calcular las coordenadas relativas a la imagen original
        const scaleX = img.naturalWidth / imgRect.width;
        const scaleY = img.naturalHeight / imgRect.height;
        const bgPosX = -x * scaleX;
        const bgPosY = -y * scaleY;

        // Aplicar las coordenadas al estilo de la lupa pequeña
        lens.style.left = x + "px";
        lens.style.top = y + "px";
        lens.style.backgroundImage = `url('${img.src}')`;
        lens.style.backgroundSize = `${img.naturalWidth}px ${img.naturalHeight}px`;
        lens.style.backgroundPosition = `${bgPosX}px ${bgPosY}px`;

        // Aplicar las coordenadas al estilo de la lupa grande
        largeLens.style.backgroundImage = `url('${img.src}')`;
        largeLens.style.backgroundSize = `${img.naturalWidth * 2}px ${img.naturalHeight * 2}px`;
        largeLens.style.backgroundPosition = `${bgPosX * 2}px ${bgPosY * 2}px`;

        showLenses(); // Mostrar las lupas mientras se mueve el cursor
    }

    // Función para ocultar las lupas cuando el cursor sale de la imagen
    function hideLens() {
        if (lens) {
            lens.style.visibility = "hidden";
        }
        if (largeLens) {
            largeLens.style.visibility = "hidden";
        }
    }

    // Función para limpiar las lupas al cerrar el modal
    function clearLens() {
        if (lens && lens.parentNode) {
            lens.parentNode.removeChild(lens);
        }
        if (largeLens && largeLens.parentNode) {
            largeLens.parentNode.removeChild(largeLens);
        }
    }

    // Función para activar los controladores de clic en miniaturas
    function activateThumbnailClickHandlers() {
        const thumbnails = document.querySelectorAll('#modalTab .nav-link');
        thumbnails.forEach(thumbnail => {
            thumbnail.addEventListener('click', handleThumbnailClick);
        });
    }

    // Función para desactivar los controladores de clic en miniaturas
    function deactivateThumbnailClickHandlers() {
        const thumbnails = document.querySelectorAll('#modalTab .nav-link');
        thumbnails.forEach(thumbnail => {
            thumbnail.removeEventListener('click', handleThumbnailClick);
        });
    }

    // Función para manejar el clic en miniaturas
    function handleThumbnailClick(e) {
        e.preventDefault();
        const tabContentId = e.currentTarget.getAttribute('data-bs-target').substring(1); // Obtener el ID del contenido de la pestaña
        const tabContent = document.getElementById(tabContentId);
        const img = tabContent ? tabContent.querySelector('.product__modal-img img') : null;

        if (img) {
            clearLens(); // Limpiar las lupas actuales
            createLens(img); // Crear una nueva lupa pequeña para la nueva imagen seleccionada
            createLargeLens(img); // Crear una nueva lupa grande para la nueva imagen seleccionada
        }
    }
});









