myPrecios();
function ConvertirEnFloat(valor) {//Recibe numero string con signo pesos y devuelve float para operarlo
    valor = parseFloat(valor.replace("$ ", "").replace(/\./g, '').replace(',', '.'));
    return valor;
}
function ConvertirEnString(valor) {// recibe numero float y lo devuelve en formato con puntos y comas y el signo $

    if (valor - Math.trunc(valor) > 0)
        valor = valor.toFixed(2);

    valor = "$ " + parseFloat(valor).toLocaleString('es-CO');
    return valor;
}
function myPrecios() {

    let Resultado = 0;

    if (localStorage.getItem("ListaCarrito") == '{}' || localStorage.getItem("ListaCarrito") == null) {
        listaProductosAgregados = [];
    } else {
        listaProductosAgregados = JSON.parse(localStorage.getItem("ListaCarrito"));
    }

    $("#idNumero").html(listaProductosAgregados.length);
    $("#idNumero1").html(listaProductosAgregados.length + " Producto(s)");
    listaProductosAgregados.forEach((item) => {
        let sumar = item.precioTotal;
        Resultado += sumar;
    });


    $("#IdTotal").html(ConvertirEnString(Resultado));
    $("#IdTotal1").html(ConvertirEnString(Resultado));

    $("#IdListaCarrito").html('');

    listaProductosAgregados.forEach((item) => {
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
                                                                                                   <span>${ConvertirEnString(item.Precio)} x ${item.cantidadProducto} und</span>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="cart__del">
                                                                                    <button onclick="EliminarProductoCarrito(${item.ProductoId})" id="deleteButton"><i class="fal fa-trash-alt"></i></button>
                                                                                </div>
                                                                            </div>`);
    });
    $("#idtotalCarrito ul li").remove();
    $("#idtotalCarrito ul").append(`<li>Subtotal <span>${ConvertirEnString(Resultado)}</span></li><li>Total <span>${ConvertirEnString(Resultado)}</span></li>`);
}
function ModalExitoso(titulo, texto) {
    $("#TituloMensaje").html("<h3><img src='../../img/alertsucess.png' />&nbsp" + titulo + "</h3>");
    $("#ContenidoCambio").html("<div class='alert alert-success' role='alert' style='text-align: justify; font-size:20px;'><span class='fas fa-info'></span>&nbsp" + texto + "</div>");
    $('#ModalTransacciones').modal("show");
}
function ModalError(titulo, texto) {
    $("#TituloMensaje").html("<h3><img src='../../img/AlertError.png' />&nbsp" + titulo + "</h3>");
    $("#ContenidoCambio").html("<div class='alert alert-danger' role='alert' style='text-align: justify; font-size:20px;'><span class='fas fa-info'></span>&nbsp" + texto + "</div>");
    $('#ModalTransacciones').modal("show");
}
function create(Type, Mensaje, Icon) {
    VanillaToasts.create({
        title: 'Estimado(a) Cliente...',
        text: Mensaje,
        type: Type,
        icon: Icon,
        timeout: '6000'
    });
}
function createCompra(Type, Mensaje, Icon) {
    VanillaToasts.create({
        title: 'Estimado(a) Cliente...',
        text: Mensaje,
        type: Type,
        icon: Icon,
        timeout: '15000'
    });
}
//Consulta el listado de productos final al momento de pagar el producto
ListaVentaFinal();
function ListaVentaFinal() {

    lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    if (lista != null) {
        $("#IdtablaCarrito2 tbody tr").remove();
        lista.forEach((item) => {
            $("#IdtablaCarrito2 tbody").append(`<tr class="cart_item">
                <td class="product-total">
                    <span class="amount">
                     <a onclick="myModalProductoCompra(${item.ProductoId}); return false;" data-bs-toggle="modal" data-bs-target="#productModalId2" class="w-img">
                        <img style="width: 60px !important; height: 50px !important;" src="${UrlImgProductos + item.listaImagenes[0].FileName}" alt="product">
                    </span>
                </td>
                <td class="">
                            <strong class="">${item.Nombre}</strong> x ${item.cantidadProducto} und
                </td>
                    <td class="product-total">
                    <span class="amount">${ConvertirEnString(item.Precio)}</span>
                    </td>
                <td class="product-total">
                    <span class="amount">${ConvertirEnString(item.precioTotal)}</span>
                </td>
            </tr>
        `);
        });
    } else {
        $("#IdtablaCarrito2 tbody tr").remove();
    }
   TotalPrecio();
}
//Consulta el total de precios de producto final al momento de pagar
function TotalPrecio() {
    let Resultado = 0;
    let Total = 0;
    const CostoEnvio = ConvertirEnFloat($("#IdCostoEnvio strong").text());
    /*   let PrecioEnvio = $("#IdCostoEnvio").val();*/
    let PrecioEnvio = CostoEnvio;
    lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    $("#idtotalCarrito2 ul li").remove();
    if (lista != null) {
        lista.forEach((item) => {
            let sumar = item.precioTotal;
            Resultado += sumar;
        });
        Total = Resultado + PrecioEnvio;


        $("#idtotalCarrito2 ul").append(`<li>Subtotal <span>${ConvertirEnString(Resultado)}</span></li>
     <li>Total <span>${ConvertirEnString(Total)}</span></li>`);
    }
  /*  $("#IdCostoEnvio label").append(`Precio de Envío: <strong class="">${ConvertirEnString(PrecioEnvio)}</strong>`);*/


}

function TotalPrecioEnvio(IdCostoEnvio) {
    let Resultado = 0;
    let Total = 0;
    let PrecioEnvio = IdCostoEnvio;
    lista = JSON.parse(localStorage.getItem("ListaCarrito"));
    if (lista != null) { 
    $("#idtotalCarrito2 ul li").remove();
    lista.forEach((item) => {
        let sumar = item.precioTotal;
        Resultado += sumar;
    });
    Total = Resultado + PrecioEnvio;


    $("#idtotalCarrito2 ul").append(`<li>Subtotal <span>${ConvertirEnString(Resultado)}</span></li>
     <li>Total <span>${ConvertirEnString(Total)}</span></li>`);
    /*  $("#IdCostoEnvio label").append(`Precio de Envío: <strong class="">${ConvertirEnString(PrecioEnvio)}</strong>`);*/
    }

}
//Carga Dominio los Paises


//Cargar Dominio Marcas y categoria
var DominioMarcas = [];
var DominioCategoria = [];
function GetConsultaDominios() {

    $.ajax({
        type: 'POST',
        async: false,
        url: UrlGetDominios,
        success: function success(response) {
            if (response.success == true) {
                tipo = response.data;

                DominioMarcas = response.data;
                localStorage.removeItem("DominioMarcas");
                localStorage.setItem("DominioMarcas", JSON.stringify(DominioMarcas));


                tipo1 = response.data1;

                DominioCategoria = response.data1;
                localStorage.removeItem("DominioCategoria");
                localStorage.setItem("DominioCategoria", JSON.stringify(DominioCategoria));
                CargarDominio1();
                CargarDominio2();




            } else {

            }
        },
        error: function error(ex) {
        }
    });
}
function CargarDominio1() {
    let container = document.querySelector('#PanelMarcas1');
    let container1 = document.querySelector('#PanelCategoria1');
    container.innerHTML = "";
    container1.innerHTML = "";


    DominioMarcas = JSON.parse(localStorage.getItem("DominioMarcas"));
    container.innerHTML += `<option value="0" >MARCAS</option>`;
    DominioMarcas.forEach((item) => {
        container.innerHTML += `<option value="${item.idDominio}" >${item.descripcion}</option>`;
        //container.innerHTML += `<li><a href="" id="${item.idDominio}">${item.descripcion}</a ></li>`;

        //`<option value="${item.idDominio}" >${item.descripcion}</option>`;
    });

    DominioCategoria = JSON.parse(localStorage.getItem("DominioCategoria"));
    container1.innerHTML += `<option value="0" >CATEGORIAS</option>`;
    DominioCategoria.forEach((item) => {
        container1.innerHTML += `<option value="${item.idDominio}" >${item.descripcion}</option>`;
        //container1.innerHTML += `<li><a href="" id="${item.idDominio}">${item.descripcion}</a ></li>`;
    });



}
function CargarDominio2() {
    let container0 = document.querySelector('#PanelMarcas2');
    let container2 = document.querySelector('#PanelCategoria2');
    container0.innerHTML = "";
    container2.innerHTML = "";


    DominioMarcas1 = JSON.parse(localStorage.getItem("DominioMarcas"));
    container0.innerHTML += `<option value="0" >MARCAS</option>`;
    DominioMarcas1.forEach((item) => {
        container0.innerHTML += `<option value="${item.idDominio}" >${item.descripcion}</option>`;
        //container.innerHTML += `<li><a href="" id="${item.idDominio}">${item.descripcion}</a ></li>`;
    });

    DominioCategoria1 = JSON.parse(localStorage.getItem("DominioCategoria"));
    container2.innerHTML += `<option value="0" >CATEGORIAS</option>`;
    DominioCategoria1.forEach((item) => {
        container2.innerHTML += `<option value="${item.idDominio}" >${item.descripcion}</option>`;
        //container1.innerHTML += `<li><a href="" id="${item.idDominio}">${item.descripcion}</a ></li>`;
    });


}


function myModalProductoCompra(idProducto) {
    // Limpiar contenido previo del modal
    $("#idModalProducto2").html("");

    // Encontrar el producto por ID
    const producto = lista.find((item) => item.ProductoId === idProducto);

    // Crear contenido principal del modal
    const modalContent = `
        <div class="row">
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12">
                <div class="product__modal-box">
                    <div class="tab-content pb-10" id="modalTabContent2"></div>
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
    $("#idModalProducto2").append(modalContent);

    // Añadir imágenes del producto al modal
    producto.listaImagenes.forEach((item, index) => {
        const isActive = index === 0 ? "show active" : "";
        const imageHtml = `
            <div class="tab-pane fade ${isActive}" id="nav${index}" role="tabpanel" aria-labelledby="nav${index}-tab">
                <div class="product__modal-img w-img">
                    <img src="${UrlImgProductos + item.FileName}" alt="" width="85px" height="auto" />
                </div>
            </div>`;
        $("#modalTabContent2").append(imageHtml);

        const tabHtml = `
            <li class="nav-item" role="presentation">
                <button class="nav-link ${isActive}" id="nav${index}-tab" data-bs-toggle="tab" data-bs-target="#nav${index}" type="button" role="tab" aria-controls="nav${index}" aria-selected="false">
                    <img src="${UrlImgProductos + item.FileName}" alt="" width="85px" height="auto" />
                </button>
            </li>`;
        $("#modalTab").append(tabHtml);
    });
}





