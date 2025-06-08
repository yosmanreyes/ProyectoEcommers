
let listaVentasPen = [];
let listaDespachadosPen = [];
GetMisVenta();
GetMisVentaEnviadas();
GetConsultaCorreoes();
var listaProductoVenta = [];
var listaProductoVenta2 = [];
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
function GetMisVenta() {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbGrilla")) {
        $("#tbGrilla").DataTable().destroy();
    }

    $("#tbGrilla").DataTable({
        "ajax": {
            type: "POST",
            url: UrlGetConsultaMisventas,
            async: true,
            datatype: "json",
            cache: false
        },
        "initComplete": function (settings, json) {
            if (json.success) {
                listaProductoVenta = json.data;
                var Mens = json.mensaje;
                $("#lblAnotacionesVenta").html(Mens);
            }
            else {     
            }
        },
        language: glOpcionesIdioma,
        responsive: true,

        "columns": [
            {
                data: null, title: "Acción", className: "celdaCenter", render: function (row) {
                    return resultado = `<div class="dropdown dropend">
                        <button class="btn btn-success" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
                            <span class="fas fa-list"></span>
                        </button>
                        <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">                        
                            <li>
                                <a class="dropdown-item" href="javascript:VerDetalleMiPorducto('${row.id_compra_realizada}')">
                                    <i class="fa fa-cart-arrow-down faa-wrench animated pink" style="color:pink"></i> Ver Detalle
                                </a>
                            </li>
                              <li>
                                <a class="dropdown-item" href="javascript:OpenEnviarCorreo('${row.id_compra_realizada}','${row.correo_Electronico}')">
                                    <i class="fa fa-envelope-o faa-wrench animated blue" style="color:blue"></i> Enviar Correo
                                </a>
                            </li>
                              <li>
                                <a class="dropdown-item" href="javascript:OpenEnviarwhatsapp('${row.id_compra_realizada}','${row.celularConIndicativo}')">
                                    <i class="fab fa-whatsapp faa-wrench animated green" style="color:green"></i> Enviar Mensaje Whatsapp
                                </a>
                            </li>
                                <li>
                                <a class="dropdown-item" href="javascript:VisualizarPDF('${row.id_compra_realizada}')">
                                    <i class="fa fa-file-pdf-o faa-pulse animated red" style="color:red"></i> Ver Factura
                                </a>
                            </li>
                                 <li>
                        <a class="dropdown-item" href="javascript:editarFila('${row.id_compra_realizada}')">
                            <i class="fa fa-edit faa-wrench animated orange" style="color:orange"></i> Editar
                        </a>
                    </li>
                        </ul>
                    </div>`
                }
            },
            { "title": "Id Venta", "data": "id_compra_realizada", "name": "id_compra_realizada", className: "celdaCenter celda1" },
         

            {
                data: null,
                title: "Entrega Mercancia en el Local(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.entregaMercanciaLocal == null ? "" : row.entregaMercanciaLocal;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="entregaMercanciaLocal" onchange="capturarSeleccion(this)">
                    <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null, title: "Id Preference Mercado Pago", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="preferenceId">${row.preferenceId == null ? "" : row.preferenceId}</div>`
                }
            },
            {
                data: null, title: "Estado Mercado Pago", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="status">${row.status == null ? "" : row.status || row.status == "approved" ? "Pago Aprobado" : row.status }</div>`
                }
            },
            {
                data: null, title: "ID de pedido Mercado Pago", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="merchant_order_id">${row.merchant_order_id == null ? "" : row.merchant_order_id}</div>`
                }
            },
            {
                data: null, title: "Cantidad", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="cantidad">${row.cantidad}</div>`
                }
            },
            
            {
                data: null, title: "Precio Total Productos", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="precioTotal">${row.precioTotal}</div>`
                }
            },
            {
                data: null, title: "costo Envío", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="costoEnvio">${row.costoEnvio}</div>`
                }
            },      
            {
                data: null, title: "Precio General", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="precioGeneral">${row.precioGeneral}</div>`
                }
            },
            {
                data: null, title: "fecha Creación", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable Calendario" data-id="${row.id_compra_realizada}" data-columna="fecha_CreacionS">${row.fecha_CreacionS}</div>`
                }
            },
            {
                data: null,
                title: "Fecha de Envío del producto",
                className: "celdaCenter White_Space",
                render: function (row) {
                    return `<input type="date" contenteditable="true" class="dropdown dropend editable fechaInput Calendario" id="fechaEnvioPro_${row.id_compra_realizada}" data-id="${row.id_compra_realizada}" data-columna="fecha_Envio_productoS" value="${row.fecha_Envio_productoS}" />`
                }
            },
            {
                data: null, title: "Empresa Entrega", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="empresa_Entrega">${row.empresa_Entrega == null ? "" : row.empresa_Entrega}</div>`
                }
            },
            {
                data: null,
                title: "Enviado Satisfactoriamente",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.enviado_Satisfactoriamente == null ? "N/A" : row.enviado_Satisfactoriamente;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="enviado_Satisfactoriamente" onchange="capturarSeleccion(this)">
                        <option value="0" ${selectedValue === "0" ? "selected" : ""}>N/A</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                            <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null,
                title: "Fecha de llegada del producto",
                className: "celdaCenter White_Space",
                render: function (row) {
                    /*     var fecha_Llegada_productoS = row.fecha_Llegada_productoS || "";*/
                    return `<input type="date" contenteditable="true" class="dropdown dropend editable fechaInput Calendario" id="fechaLlegadaPro_${row.id_compra_realizada}" data-id="${row.id_compra_realizada}" data-columna="fecha_Llegada_productoS" value="${row.fecha_Llegada_productoS}" />`
                }
            },
            {
                data: null,
                title: "Recibio Producto(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.estado_producto == null ? "N/A" : row.estado_producto;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="estado_producto" onchange="capturarSeleccion(this)">
                        <option value="0" ${selectedValue === "0" ? "selected" : ""}>N/A</option>
                      <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                            <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null, title: "Dirección", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="direccion">${row.direccion == null ? "" : row.direccion}</div>`
                }
            },
            {
                data: null, title: "País", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="pais">${row.pais == null ? "" : row.pais}</div>`
                }
            },
            {
                data: null, title: "Departamento", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="departamento">${row.departamento == null ? "" : row.departamento}</div>`
                }
            },
            {
                data: null, title: "Ciudad", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="ciudad">${row.ciudad == null ? "" : row.ciudad}</div>`
                }
            },
            {
                data: null, title: "Codígo Postal", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="codigo_Postal">${row.codigo_Postal}</div>`
                }
            },
            {
                data: null, title: "Número Teléfono", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="celular">${row.celular}</div>`
                }
            },
            {
                data: null, title: "Número Teléfono con Indicativo del país", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="celular">${row.celularConIndicativo}</div>`
                }
            },
            {
                data: null, title: "Correo Electrónico", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="correo_Electronico">${row.correo_Electronico}</div>`
                }
            },
            {
                data: null, title: "Comentario", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="comentario">${row.comentario == null ? "" : row.comentario}</div>`
                }
            },
            {
                data: null, title: "Nombres", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="nombres">${row.nombres}</div>`
                }
            },
            {
                data: null, title: "Apellidos", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="apellidos">${row.apellidos}</div>`
                }
            },
            {
                data: null, title: "Envío Correo", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="envioCorreo">${row.envioCorreo == null ? "NO" : row.envioCorreo}</div>`
                }
            },
            {
                data: null,
                title: "Cancelada(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.cancelada == null ? "NO" : row.cancelada;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="cancelada" onchange="capturarSeleccion(this)">
                    
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                            <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null, title: "Direccion", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="direccionS">${row.direccionS == null ? "" : row.direccionS}</div>`
                }
            },
            {
                data: null, title: "OpcionDireccion", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="opcional_Direccion">${row.opcional_Direccion == null ? "" : row.opcional_Direccion}</div>`
                }
            },

            {
                data: null,
                title: "Entrega Mercancia en el Local(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.entregaMercanciaLocal == null ? "" : row.entregaMercanciaLocal;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="entregaMercanciaLocal" onchange="capturarSeleccion(this)">                      
                         <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null,
                title: "Pago Nequi(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.pagoNequi == null ? "" : row.pagoNequi;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="pagoNequi" onchange="capturarSeleccion(this)">                      
                           <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null,
                title: "Pago Daviplata(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.pagoDaviplata == null ? "" : row.pagoDaviplata;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="pagoDaviplata" onchange="capturarSeleccion(this)">                      
                    <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null,
                title: "Otro Medio Pago(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.otroMedioPago == null ? "" : row.otroMedioPago;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="otroMedioPago" onchange="capturarSeleccion(this)">                      
                     <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null,
                title: "Pago Efectivo(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.pagoEfectivo == null ? "" : row.pagoEfectivo;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="pagoEfectivo" onchange="capturarSeleccion(this)">                     
                               <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                      </select>`;
                    return options;
                }
            }



        ],
        "rowCallback": function (row, data, index) {

            

            if (data.status == "approved") {
                $(row).find('td:eq(4)').addClass('SemaforoVerde');

            } else {
                $(row).find('td:eq(4)').addClass('SemaforoNaranja');
            }

            if (data.precioGeneral == "" || data.precioGeneral == null) {
                $(row).find('td:eq(9)').addClass('SemaforosVerde');
            } else {
                $(row).find('td:eq(9)').addClass('SemaforosVerde');
            }

            if (data.fecha_Envio_productoS == "" || data.fecha_Envio_productoS == null || data.fecha_Envio_productoS == "01/01/0001 12:00 a. m.") {
                $(row).find('td:eq(11)').addClass('SemaforoNaranja');
            } else {
                $(row).find('td:eq(11)').addClass('SemaforoVerde');
            }
            if (data.empresa_Entrega == "" || data.empresa_Entrega == null) {
                $(row).find('td:eq(12)').addClass('SemaforoNaranja');
            } else {
                $(row).find('td:eq(12)').addClass('SemaforoVerde');
            }
            if (data.enviado_Satisfactoriamente == "" || data.enviado_Satisfactoriamente == null) {
                $(row).find('td:eq(13)').addClass('SemaforoNaranja');
            } else if (data.enviado_Satisfactoriamente == "NO") {
                $(row).find('td:eq(13)').addClass('SemaforoAzul');
            } else if (data.enviado_Satisfactoriamente == "SI") {
                $(row).find('td:eq(13)').addClass('SemaforoVerde');
            }

            if (data.entregaMercanciaLocal == "" || data.entregaMercanciaLocal == null || data.entregaMercanciaLocal == "N/A") {
                $(row).find('td:eq(2)').addClass('SemaforoNaranja');
            } else if (data.entregaMercanciaLocal == "NO") {
                $(row).find('td:eq(2)').addClass('SemaforoAzul');
            } else if (data.entregaMercanciaLocal == "SI") {
                $(row).find('td:eq(2)').addClass('SemaforoVerde');
            }

            


            if (data.fecha_Llegada_productoS == "" || data.fecha_Envio_productoS == null || data.fecha_Envio_productoS == "01/01/0001 12:00 a. m.") {
                $(row).find('td:eq(14)').addClass('SemaforoNaranja');
            } else {
                $(row).find('td:eq(14)').addClass('SemaforoVerde');
            }
            if (data.estado_producto == null) {
                $(row).find('td:eq(15)').addClass('SemaforoNaranja');
            } else if (data.estado_producto == "NO") {
                $(row).find('td:eq(15)').addClass('SemaforoAzul');
            }
            else {
                $(row).find('td:eq(15)').addClass('SemaforoVerde');
            }
            if (data.cancelada == "NO") {
                $(row).find('td:eq(28)').addClass('SemaforoVerde');
            } else {
                $(row).find('td:eq(1)').addClass('SemaforoRojo');
                $(row).find('td:eq(2)').addClass('SemaforoRojo');
                $(row).find('td:eq(3)').addClass('SemaforoRojo');
                $(row).find('td:eq(4)').addClass('SemaforoRojo');
                $(row).find('td:eq(5)').addClass('SemaforoRojo');
                $(row).find('td:eq(6)').addClass('SemaforoRojo');
                $(row).find('td:eq(7)').addClass('SemaforoRojo');
                $(row).find('td:eq(8)').addClass('SemaforoRojo');
                $(row).find('td:eq(9)').addClass('SemaforoRojo');
                $(row).find('td:eq(10)').addClass('SemaforoRojo');
                $(row).find('td:eq(11)').addClass('SemaforoRojo');
                $(row).find('td:eq(12)').addClass('SemaforoRojo');
                $(row).find('td:eq(13)').addClass('SemaforoRojo');
                $(row).find('td:eq(14)').addClass('SemaforoRojo');
                $(row).find('td:eq(15)').addClass('SemaforoRojo');
                $(row).find('td:eq(16)').addClass('SemaforoRojo');
                $(row).find('td:eq(17)').addClass('SemaforoRojo');
                $(row).find('td:eq(18)').addClass('SemaforoRojo');
                $(row).find('td:eq(19)').addClass('SemaforoRojo');
                $(row).find('td:eq(20)').addClass('SemaforoRojo');
                $(row).find('td:eq(21)').addClass('SemaforoRojo');
                $(row).find('td:eq(22)').addClass('SemaforoRojo');
                $(row).find('td:eq(23)').addClass('SemaforoRojo');
                $(row).find('td:eq(24)').addClass('SemaforoRojo');
                $(row).find('td:eq(25)').addClass('SemaforoRojo');
                $(row).find('td:eq(26)').addClass('SemaforoRojo');
                $(row).find('td:eq(27)').addClass('SemaforoRojo');
                $(row).find('td:eq(28)').addClass('SemaforoRojo');
                $(row).find('td:eq(29)').addClass('SemaforoRojo');
                $(row).find('td:eq(30)').addClass('SemaforoRojo');
            }
            listaVentasPen.push(data);
        },

        ordering: true,
        pageLength: 10,
        bLengthChange: true,
        searching: true,
        paging: true,
        info: true
    });

    setTimeout(() => {
        fecha();
    }, 200);

    setTimeout(() => {
        listaVentasPen.forEach((item) => {

            if (item.fecha_Llegada_productoS == "01/01/0001 12:00 a. m." || item.fecha_Llegada_productoS == null) {

                let producto = document.getElementById('fechaLlegadaPro_' + item.id_compra_realizada);
                producto.value = null;

            } else {
                let producto = document.getElementById('fechaLlegadaPro_' + item.id_compra_realizada);
                producto.value = item.fecha_Llegada_productoS;
            }

            if (item.fecha_Envio_productoS == "01/01/0001 12:00 a. m." || item.fecha_Envio_productoS == null) {

                let productos = document.getElementById('fechaEnvioPro_' + item.id_compra_realizada);
                productos.value = null;

            } else {
                let productos = document.getElementById('fechaEnvioPro_' + item.id_compra_realizada);
                productos.value = item.fecha_Envio_productoS;
            }


          
        });

    }, 400);

    $("#tbGrilla tbody").on('input change', '.editable', function () {
        var nuevoValor = "";
        var Resultado = "";
        var idVenta = $(this).data("id");
        var columna = $(this).data("columna");

        // Verifica si el elemento actual es un select
        if ($(this).is("select")) {
            Resultado = $(this).val(); // Captura el valor seleccionado
            if (Resultado == "0") {
                nuevoValor == null;
                // Captura la fecha seleccionada
            } else {
                nuevoValor = $(this).val();
            }


        } else if ($(this).hasClass("fechaInput")) { // Verifica si es un Kendo DatePicker
            nuevoValor = $(this).val();
        } else {
            nuevoValor = $(this).text();
        }

        ModificarVentaProducto(idVenta, columna, nuevoValor);
    });

}
function GetMisVentaEnviadas() {
    $("#Process").hide();
    if ($.fn.dataTable.isDataTable("#tbGrilla2")) {
        $("#tbGrilla2").DataTable().destroy();
    }
    $("#tbGrilla2").DataTable({
        "ajax": {
            type: "POST",
            url: UrlGetConsultaMisventasEnviadas,
            async: true,
            datatype: "json",
            cache: false
        },
        "initComplete": function (settings, json) {
            if (json.success) {
                listaProductoVenta2 = json.data;
                var Mens = json.mensaje;
                $("#lblAnotacionesVenta2").html(Mens);
            }
            else {
                var Mens = json.mensaje;
                $("#lblAnotacionesVenta2").html(Mens);
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
                                <a class="dropdown-item" href="javascript:VerDetalleMiPorducto2('${row.id_compra_realizada}')">
                                    <i class="fa fa-cart-arrow-down faa-wrench animated pink" style="color:pink"></i> Ver Detalle
                                </a>
                            </li>
                              <li>
                                <a class="dropdown-item" href="javascript:OpenEnviarCorreo('${row.id_compra_realizada}','${row.correo_Electronico}')">
                                    <i class="fa fa-envelope-o faa-wrench animated blue" style="color:blue"></i> Enviar Correo
                                </a>
                            </li>
                              <li>
                                <a class="dropdown-item" href="javascript:OpenEnviarwhatsapp('${row.id_compra_realizada}','${row.celularConIndicativo}')">
                                    <i class="fab fa-whatsapp faa-wrench animated green" style="color:green"></i> Enviar Mensaje Whatsapp
                                </a>
                            </li>
                                   <li>
                                <a class="dropdown-item" href="javascript:VisualizarPDF('${row.id_compra_realizada}')">
                                    <i class="fa fa-file-pdf-o faa-pulse animated red" style="color:red"></i> Ver Factura
                                </a>
                            </li>
                            <li>
                        <a class="dropdown-item" href="javascript:editarFila2('${row.id_compra_realizada}')">
                            <i class="fa fa-edit faa-wrench animated orange" style="color:orange"></i> Editar
                        </a>
                    </li>
                        </ul>
                    </div>`
                }
            },
            { "title": "Id Venta", "data": "id_compra_realizada", "name": "id_compra_realizada", className: "celdaCenter celda1" },
            
            {
                data: null,
                title: "Recibio Producto(SI/NO)",
                className: "celdaCenter celda4",
                render: function (row) {
                    var selectedValue = row.estado_producto == null ? "N/A" : row.estado_producto;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="estado_producto" onchange="capturarSeleccion(this)">
                        <option value="0" ${selectedValue === "0" ? "selected" : ""}>N/A</option>
                       <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                        <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null, title: "Id Preference Mercado Pago", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="preferenceId">${row.preferenceId == null ? "" : row.preferenceId}</div>`
                }
            },
            {
                data: null, title: "Estado Mercado Pago", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="status">${row.status == null ? "" : row.status || row.status == "approved" ? "Pago Aprobado" : row.status}</div>`
                }
            },
            {
                data: null, title: "ID de pedido Mercado Pago", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="merchant_order_id">${row.merchant_order_id == null ? "" : row.merchant_order_id}</div>`
                }
            },
            {
                data: null, title: "Cantidad", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="cantidad">${row.cantidad}</div>`
                }
            },
            {
                data: null, title: "Precio Total Productos", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="precioTotal">${row.precioTotal}</div>`
                }
            },
            {
                data: null, title: "costo Envío", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="costoEnvio">${row.costoEnvio}</div>`
                }
            },
            {
                data: null, title: "Precio General", className: "celdaCenter celda1 ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="precioGeneral">${row.precioGeneral}</div>`
                }
            },
            {
                data: null, title: "fecha Creación", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable Calendario" data-id="${row.id_compra_realizada}" data-columna="fecha_CreacionS">${row.fecha_CreacionS}</div>`
                }
            },
            {
                data: null,
                title: "Fecha de Envío del producto (01/01/2020)",
                className: "celdaCenter White_Space",
                render: function (row) {
                    return `<input type="date" contenteditable="true" class="dropdown dropend editable fechaInput2 Calendario"  id="fechaEnvioPro2_${row.id_compra_realizada}" data-id="${row.id_compra_realizada}" data-columna="fecha_Envio_productoS" value="${row.fecha_Envio_productoS}" />`
                }
            },
            {
                data: null, title: "Empresa Entrega", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="empresa_Entrega">${row.empresa_Entrega == null ? "" : row.empresa_Entrega}</div>`
                }
            },
            {
                data: null,
                title: "Enviado Satisfactoriamente",
                className: "celdaCenter celda4",
                render: function (row) {
                    var selectedValue = row.enviado_Satisfactoriamente == null ? "N/A" : row.enviado_Satisfactoriamente;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="enviado_Satisfactoriamente" onchange="capturarSeleccion(this)">
                        <option value="0" ${selectedValue === "0" ? "selected" : ""}>N/A</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                            <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null,
                title: "Fecha de llegada del producto (01/01/2020)",
                className: "celdaCenter White_Space",
                render: function (row) {
                    return `<input type="date" contenteditable="true" class="dropdown dropend editable fechaInput2 Calendario" id="fechaLlegadaPro2_${row.id_compra_realizada}" data-id="${row.id_compra_realizada}" data-columna="fecha_Llegada_productoS" value="${row.fecha_Llegada_productoS}" />`
                }
            },
            {
                data: null, title: "Dirección", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="direccion">${row.direccion == null ? "" : row.direccion}</div>`
                }
            },
            {
                data: null, title: "País", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="pais">${row.pais == null ? "" : row.pais}</div>`
                }
            },
            {
                data: null, title: "Departamento", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="departamento">${row.departamento == null ? "" : row.departamento}</div>`
                }
            },
            {
                data: null, title: "Ciudad", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="ciudad">${row.ciudad == null ? "" : row.ciudad}</div>`
                }
            },
            {
                data: null, title: "Codígo Postal", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="codigo_Postal">${row.codigo_Postal}</div>`
                }
            },
            {
                data: null, title: "Número Teléfono", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="celular">${row.celular}</div>`
                }
            },
            {
                data: null, title: "Número Teléfono con Indicativo del país", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="celular">${row.celularConIndicativo}</div>`
                }
            },
            {
                data: null, title: "Correo Electrónico", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="correo_Electronico">${row.correo_Electronico}</div>`
                }
            },
            {
                data: null, title: "Comentario", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="comentario">${row.comentario == null ? "" : row.comentario}</div>`
                }
            },
            {
                data: null, title: "Nombres", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="nombres">${row.nombres}</div>`
                }
            },
            {
                data: null, title: "Apellidos", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="apellidos">${row.apellidos}</div>`
                }
            },
            {
                data: null, title: "Envío Correo", className: "celdaCenter celda1", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="envioCorreo">${row.envioCorreo == null ? "NO" : row.envioCorreo}</div>`
                }
            },
            {
                data: null,
                title: "Cancelada(SI/NO)",
                className: "celdaCenter celda1",
                render: function (row) {
                    var selectedValue = row.cancelada == null ? "NO" : row.cancelada;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="cancelada" onchange="capturarSeleccion(this)">
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                            <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null, title: "Direccion", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="direccionS">${row.direccionS == null ? "" : row.direccionS}</div>`
                }
            },
            {
                data: null, title: "OpcionDireccion", className: "celdaCenter ", render: function (row) {
                    return resultado = `<div contenteditable="true" class="dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="opcional_Direccion">${row.opcional_Direccion == null ? "" : row.opcional_Direccion}</div>`
                }
            },
            {
                data: null,
                title: "Pago Nequi(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.pagoNequi == null ? "" : row.pagoNequi;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="pagoNequi" onchange="capturarSeleccion(this)">                      
                      <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                   
                      </select>`;
                    return options;
                }
            },
            {
                data: null,
                title: "Pago Daviplata(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.pagoDaviplata == null ? "" : row.pagoDaviplata;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="pagoDaviplata" onchange="capturarSeleccion(this)"> 
                               <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                      
                      </select>`;
                    return options;
                }
            },
            {
                data: null,
                title: "Otro Medio Pago(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.otroMedioPago == null ? "" : row.otroMedioPago;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="otroMedioPago" onchange="capturarSeleccion(this)">                     
                        <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                      </select>`;
                    return options;
                }
            },
            {
                data: null,
                title: "Pago Efectivo(SI/NO)",
                className: "celdaCenter ",
                render: function (row) {
                    var selectedValue = row.pagoEfectivo == null ? "" : row.pagoEfectivo;
                    var options = `<select class="form-select dropdown dropend editable" data-id="${row.id_compra_realizada}" data-columna="pagoEfectivo" onchange="capturarSeleccion(this)">
                     <option value="NO" ${selectedValue === "NO" ? "selected" : ""}>NO</option>
                    <option value="SI" ${selectedValue === "SI" ? "selected" : ""}>SI</option>
                      </select>`;
                    return options;
                }
            }

        ],
        "rowCallback": function (row, data, index) {
            if (data.status == "approved") {
                $(row).find('td:eq(4)').addClass('SemaforoVerde');

            } else {
                $(row).find('td:eq(4)').addClass('SemaforoNaranja');
            }
            if (data.precioGeneral == "" || data.precioGeneral == null) {
                $(row).find('td:eq(9)').addClass('SemaforosAzul');
            } else {
                $(row).find('td:eq(9)').addClass('SemaforosAzul');
            }

            if (data.fecha_Envio_productoS == "" || data.fecha_Envio_productoS == null || data.fecha_Envio_productoS == "01/01/0001 12:00 a. m.") {
                $(row).find('td:eq(11)').addClass('SemaforoNaranja');
            } else {
                $(row).find('td:eq(11)').addClass('SemaforoVerde');
            }
            if (data.empresa_Entrega == "" || data.empresa_Entrega == null) {
                $(row).find('td:eq(12)').addClass('SemaforoNaranja');
            } else {
                $(row).find('td:eq(12)').addClass('SemaforoVerde');
            }
            if (data.enviado_Satisfactoriamente == "" || data.enviado_Satisfactoriamente == null) {
                $(row).find('td:eq(13)').addClass('SemaforoNaranja');
            } else if (data.enviado_Satisfactoriamente == "NO") {
                $(row).find('td:eq(13)').addClass('SemaforoAzul');
            } else if (data.enviado_Satisfactoriamente == "SI") {
                $(row).find('td:eq(13)').addClass('SemaforoVerde');
            }

            if (data.entregaMercanciaLocal == "" || data.entregaMercanciaLocal == null || data.entregaMercanciaLocal == "N/A") {
                $(row).find('td:eq(2)').addClass('SemaforoNaranja');
            } else if (data.entregaMercanciaLocal == "NO") {
                $(row).find('td:eq(2)').addClass('SemaforoAzul');
            } else if (data.entregaMercanciaLocal == "SI") {
                $(row).find('td:eq(2)').addClass('SemaforoVerde');
            }

            if (data.fecha_Llegada_productoS == "" || data.fecha_Envio_productoS == null || data.fecha_Envio_productoS == "01/01/0001 12:00 a. m.") {
                $(row).find('td:eq(14)').addClass('SemaforoNaranja');
            } else {
                $(row).find('td:eq(14)').addClass('SemaforoVerde');
            }
            if (data.estado_producto == null) {
                $(row).find('td:eq(15)').addClass('SemaforoNaranja');
            } else if (data.estado_producto == "NO") {
                $(row).find('td:eq(15)').addClass('SemaforoAzul');
            }
            else {
                $(row).find('td:eq(15)').addClass('SemaforoVerde');
            }
            if (data.cancelada == "NO") {
                $(row).find('td:eq(28)').addClass('SemaforoVerde');
            } else {
                $(row).find('td:eq(1)').addClass('SemaforoRojo');
                $(row).find('td:eq(2)').addClass('SemaforoRojo');
                $(row).find('td:eq(3)').addClass('SemaforoRojo');
                $(row).find('td:eq(4)').addClass('SemaforoRojo');
                $(row).find('td:eq(5)').addClass('SemaforoRojo');
                $(row).find('td:eq(6)').addClass('SemaforoRojo');
                $(row).find('td:eq(7)').addClass('SemaforoRojo');
                $(row).find('td:eq(8)').addClass('SemaforoRojo');
                $(row).find('td:eq(9)').addClass('SemaforoRojo');
                $(row).find('td:eq(10)').addClass('SemaforoRojo');
                $(row).find('td:eq(11)').addClass('SemaforoRojo');
                $(row).find('td:eq(12)').addClass('SemaforoRojo');
                $(row).find('td:eq(13)').addClass('SemaforoRojo');
                $(row).find('td:eq(14)').addClass('SemaforoRojo');
                $(row).find('td:eq(15)').addClass('SemaforoRojo');
                $(row).find('td:eq(16)').addClass('SemaforoRojo');
                $(row).find('td:eq(17)').addClass('SemaforoRojo');
                $(row).find('td:eq(18)').addClass('SemaforoRojo');
                $(row).find('td:eq(19)').addClass('SemaforoRojo');
                $(row).find('td:eq(20)').addClass('SemaforoRojo');
                $(row).find('td:eq(21)').addClass('SemaforoRojo');
                $(row).find('td:eq(22)').addClass('SemaforoRojo');
                $(row).find('td:eq(23)').addClass('SemaforoRojo');
                $(row).find('td:eq(24)').addClass('SemaforoRojo');
                $(row).find('td:eq(25)').addClass('SemaforoRojo');
                $(row).find('td:eq(26)').addClass('SemaforoRojo');
                $(row).find('td:eq(27)').addClass('SemaforoRojo');
                $(row).find('td:eq(28)').addClass('SemaforoRojo');
                $(row).find('td:eq(29)').addClass('SemaforoRojo');
                $(row).find('td:eq(30)').addClass('SemaforoRojo');
            }
            listaDespachadosPen.push(data);
        },
        ordering: true,
        pageLength: 10,
        bLengthChange: true,
        searching: true,
        paging: true,
        info: true

    });
    setTimeout(() => {
        fecha2();
    }, 200);

    setTimeout(() => {

        listaDespachadosPen.forEach((item) => {
            if (item.fecha_Llegada_productoS == "01/01/0001 12:00 a. m.") {
                let producto = document.getElementById('fechaLlegadaPro2_' + item.id_compra_realizada);
                producto.value = null;

            } else {
                let producto = document.getElementById('fechaLlegadaPro2_' + item.id_compra_realizada);
                producto.value = item.fecha_Llegada_productoS;
            }

            if (item.fecha_Envio_productoS == "01/01/0001 12:00 a. m.") {

                let productos = document.getElementById('fechaEnvioPro2_' + item.id_compra_realizada);
                productos.value = null;

            } else {
                let productos = document.getElementById('fechaEnvioPro2_' + item.id_compra_realizada);
                productos.value = item.fecha_Envio_productoS;
            }

        });


    }, 400);


    $("#tbGrilla2 tbody").on('input change', '.editable', function () {
        var nuevoValor = "";
        var Resultado = "";
        var idVenta = $(this).data("id");
        var columna = $(this).data("columna");

        // Verifica si el elemento actual es un select
        if ($(this).is("select")) {
            Resultado = $(this).val(); // Captura el valor seleccionado
            if (Resultado == "0") {
                nuevoValor == null;
                // Captura la fecha seleccionada
            } else {
                nuevoValor = $(this).val();
            }


        } else if ($(this).hasClass("fechaInput2")) { // Verifica si es un Kendo DatePicker
            nuevoValor = $(this).val();
        } else {
            nuevoValor = $(this).text();
        }

        ModificarVentaProducto(idVenta, columna, nuevoValor);
    });






}



$('#CloseModalPdf').click(function () {
    $('#ModalVisorPdf').modal("hide");
});
function VerDetalleMiPorducto(id_compra) {
    $('#ModalProducto').modal("show");
    $("#IdMisComprasEnvio").css("display", "block");
    $("#IdtablaMisComprasTotal tbody tr").remove();

    let NuevaLista = listaProductoVenta.filter((item) => item.id_compra_realizada == id_compra);
    var lll = NuevaLista[0].listaMisCompras;

    lll.forEach((item) => {
        $("#IdtablaMisComprasTotal tbody").append(`<tr class="cart_item">
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

    let res = listaProductoVenta.find((item) => item.id_compra_realizada == id_compra);
    $("#IdListaCompraC").html('');
    $("#IdListaCompraC").append(`
                            <h5 style="text-align: center">
                                    Trazabilidad del envío
                                </h5>
                                <div class="lineatemp" style="font-size: 11px">
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


    let ss = listaProductoVenta.find((item) => item.id_compra_realizada == id_compra);
    $("#idFechaCreacion").html('');
    $("#idEstadoTrazabilidad").html('');
    var validar = "";
    $("#idFechaCreacion").append(` <h3 style="text-align: center"> Compra Realizada, Fecha: ${ss.fecha_CreacionS}</h3>`);

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
    RedirecionaConsultaMiCompra();

}
function VerDetalleMiPorducto2(id_compra) {
    $('#ModalProducto').modal("show");
    $("#IdMisComprasEnvio").css("display", "block");
    $("#IdtablaMisComprasTotal tbody tr").remove();

    let NuevaLista = listaProductoVenta2.filter((item) => item.id_compra_realizada == id_compra);
    var lll = NuevaLista[0].listaMisCompras;

    lll.forEach((item) => {
        $("#IdtablaMisComprasTotal tbody").append(`<tr class="cart_item">
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

    let res = listaProductoVenta2.find((item) => item.id_compra_realizada == id_compra);
    $("#IdListaCompraC").html('');
    $("#IdListaCompraC").append(`
                            <h5 style="text-align: center">
                                    Trazabilidad del envío
                                </h5>
                                <div class="lineatemp" style="font-size: 11px">
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


    let ss = listaProductoVenta2.find((item) => item.id_compra_realizada == id_compra);
    $("#idFechaCreacion").html('');
    $("#idEstadoTrazabilidad").html('');
    var validar = "";
    $("#idFechaCreacion").append(` <h3 style="text-align: center"> Compra Realizada, Fecha: ${ss.fecha_CreacionS}</h3>`);

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
    RedirecionaConsultaMiCompra();

}
function RedirecionaConsultaMiCompra() {
    const seccionDestino = document.getElementById('IdMisComprasEnvio');
    const posicionDestino = seccionDestino.offsetTop;
    window.scrollTo({
        top: posicionDestino,
        behavior: 'smooth'
    });
}
function ConvertirEnString(valor) {// recibe numero float y lo devuelve en formato con puntos y comas y el signo $

    if (valor - Math.trunc(valor) > 0)
        valor = valor.toFixed(2);

    valor = "$ " + parseFloat(valor).toLocaleString('es-CO');
    return valor;
}
function ModificarVentaProducto(idCompra, columna, nuevoValor) {
    data = { _IdVentaProducto: idCompra, _columna: columna, _Descripcion: nuevoValor }
    $.ajax({
        type: "POST",
        url: UrlUpdateProductoVenta,
        async: true,
        data: data,
        dataType: 'json',
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                if (respuesta.data > 0) {

             
                    if (columna == "estado_producto" || columna == "fecha_Llegada_productoS" || columna == "cancelada" || columna == "enviado_Satisfactoriamente" || columna == "fecha_Envio_productoS") {
                        window.location.reload();
                    }
                    /* window.CantidadVentaProductos.CantidadVentaProducto = respuesta.data.id;*/ // Asigna el nuevo valor aquí
                } else {
                    /* create('error', 'Por favor, correo electrónico...', UrlAlertError);*/
                    /*    console.error("La variable global no está definida o no es accesible desde JavaScript.");*/
                }
                /*     GetMisVenta();*/
                //Swal.fire({
                //    type: 'success',
                //    title: '¡Estimado(a) Cliente!',
                //    text: respuesta.message
                //});

            } else {
                //Swal.fire({
                //    type: 'warning',
                //    title: '¡Estimado(a) Cliente!',
                //    text: respuesta.message
                //});

            }
        },
        error: function () {
            //Swal.fire({
            //    type: 'error',
            //    title: '¡Estimado(a) Cliente!',
            //    text: 'Error al validar Usuario!!'
            //});
        }
    });
}
//Abrir Modal envio Correo Masivo
function OpenEnviarCorreo(_idComentario, _correo) {
    $("#btnEnviarCorreo").addClass('hidden');
    $("#btnEnviarCorreoP").removeClass('hidden');
    $('#ModalCorreo').modal("show");
    $("#txtIdComentario").val(_idComentario);
    $("#txtcorreo").val(_correo);
    $("#txtAsunto").val("Maria Bonita, agradece por sus comentarios");
    $("#txtMensaje").val("Maria Bonita es muy importante por contar con su comentarios y sugerencias, vamos a validar sus caso con los directivos con el fin de mejorar nuestros servicios, Gracias");

    /*    LimpiarCorreo();*/
}
function LimpiarCorreo() {
    $("#txtAsunto").val("");
    $("#txtMensaje").val("");
    $("#btnEnviarCorreo").attr("disabled", false);
    $("#btnEnviarCorreoP").attr("disabled", false);
}
function CerrarCorreos() {
    $('#ModalCorreo').modal("hide");
    LimpiarCorreo();
}
function EnvioCorreosPersonal() {

    var IdComentario = $("#txtIdComentario").val();
    if (IdComentario == null || IdComentario == "") {
        create('error', 'Por favor, seleccione el comentario para envío del correo...', UrlAlertError);
        return;
    }

    var Correo = $("#txtcorreo").val();
    if (Correo == null || Correo == "") {
        create('error', 'Por favor, correo electrónico...', UrlAlertError);
        return;
    }


    var Asumnto = $("#txtAsunto").val();
    if (Asumnto == null || Asumnto == "") {
        create('error', 'Por favor, registre el asunto...', UrlAlertError);
        return;
    }

    var Mensaje = $("#txtMensaje").val();
    if (Mensaje == null || Mensaje == "") {
        create('error', 'Por favor, registre el mensaje..', UrlAlertError);
        return;
    }
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    data = { __RequestVerificationToken: token, _IdComentario: IdComentario, _Correo: Correo, _Asumnto: Asumnto, _Mensaje: Mensaje }
    $.ajax({
        type: "POST",
        url: UrlEnvioCorreoPersonal,
        async: true,
        data: data,
        dataType: 'json',
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                /*     GetMisVenta();*/
                Swal.fire({
                    type: 'success',
                    title: '¡Estimado(a) Cliente!',
                    text: respuesta.mensaje

                }).then((result) => {
                    window.location.reload();
                });

            } else {
                Swal.fire({
                    type: 'warning',
                    title: '¡Estimado(a) Cliente!',
                    text: respuesta.mensaje
                });

            }
        },
        error: function () {
            //Swal.fire({
            //    type: 'error',
            //    title: '¡Estimado(a) Cliente!',
            //    text: 'Error al validar Usuario!!'
            //});
        }
    });
}

function InsActualizabandeja() {
    window.location.reload();
    //$(document).ready(function () {
    //    $(".fechaInput").kendoDateTimePicker({
    //        culture: "es-CO",
    //        value: "",
    //        interval: 1,
    //        format: "dd/MM/yyyy hh:mm tt",
    //        animation: {
    //            close: {
    //                effects: "fadeOut zoom:out",
    //                duration: 300
    //            },
    //            open: {
    //                effects: "fadeIn zoom:in",
    //                duration: 300
    //            }
    //        }
    //    });

    //    $(".k-datetimepicker").css("width", "100%");
    //});
}
function RptExcel() {
    $("#Process").show();
    UrlReporte = UrlReporteExcel;
    setTimeout(function () {
        document.location = UrlReporte;
        $("#Process").hide();
    }, 2000);
}

function RptExcel2() {
    $("#Process").show();
    UrlReporte = UrlReporteExcel2;
    setTimeout(function () {
        document.location = UrlReporte;
        $("#Process").hide();
    }, 2000);
}

function RptExcelGeneral() {
    $("#Process").show();
    UrlReporte = UrlReporteExcelGeneral;
    setTimeout(function () {
        document.location = UrlReporte;
        $("#Process").hide();
    }, 2000);
}

function OpenEnviarwhatsapp(id_comentario, id_numeroCelular) {
    var mas = quitarSignoMas(id_numeroCelular);
    window.location.replace("https://api.whatsapp.com/send?phone=" + mas);
}
function quitarSignoMas(numero) {
    // Verificar si el número comienza con un signo de más
    if (numero.startsWith('+')) {
        // Eliminar el signo de más y devolver el número restante
        return numero.substring(1);
    } else {
        // Si no comienza con un signo de más, devolver el número sin cambios
        return numero;
    }
}

//Enviar Correo MAsivo
function EnvioCorreosPersonalM() {



    var Correo = $("#txtcorreoMasivo").val();
    if (Correo == null || Correo == "") {
        create('error', 'Por favor, correo electrónico...', UrlAlertError);
        return;
    }


    var Asumnto = $("#txtAsuntoMasivo").val();
    if (Asumnto == null || Asumnto == "") {
        create('error', 'Por favor, registre el asunto...', UrlAlertError);
        return;
    }

    var Mensaje = $("#txtMensajeMasivo").val();
    if (Mensaje == null || Mensaje == "") {
        create('error', 'Por favor, registre el mensaje..', UrlAlertError);
        return;
    }

    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    data = { __RequestVerificationToken: token, _Correo: Correo, _Asumnto: Asumnto, _Mensaje: Mensaje }
    $.ajax({
        type: "POST",
        url: UrlEnvioCorreoPersonal,
        async: true,
        data: data,
        dataType: 'json',
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                /*     GetMisVenta();*/
                Swal.fire({
                    type: 'success',
                    title: '¡Estimado(a) Cliente!',
                    text: respuesta.mensaje
                });

            } else {
                Swal.fire({
                    type: 'warning',
                    title: '¡Estimado(a) Cliente!',
                    text: respuesta.mensaje
                });

            }
        },
        error: function () {
            //Swal.fire({
            //    type: 'error',
            //    title: '¡Estimado(a) Cliente!',
            //    text: 'Error al validar Usuario!!'
            //});
        }
    });
}
function EnvioCorreosMasivo() {
    var selectElements = document.getElementById("bootstrap-duallistbox-selected-list_");
    var opciones = selectElements.options;


    if (opciones.length == 0) {
        create('error', 'Por favor, correo electrónico...', UrlAlertError);
        return;
    }
    var Asumnto = $("#txtAsuntoMasivo").val();
    if (Asumnto == null || Asumnto == "") {
        create('error', 'Por favor, registre el asunto...', UrlAlertError);
        return;
    }

    var Mensaje = $("#txtMensajeMasivo").val();
    if (Mensaje == null || Mensaje == "") {
        create('error', 'Por favor, registre el mensaje..', UrlAlertError);
        return;
    }

    var DtoEnvioCorreo = [];

    for (var i = 0; i < opciones.length; i++) {
        DtoEnvioCorreo.push({
            CorreoEnviar: opciones[i].innerHTML,
            Asunto: Asumnto,
            Mensaje: Mensaje
        });
    }

    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    data = { __RequestVerificationToken: token, obj: DtoEnvioCorreo }
    $.ajax({
        type: "POST",
        url: UrlEnvioCorreoMasivo,
        async: true,
        data: data,
        dataType: 'json',
        cache: false,
        success: function (respuesta) {
            if (respuesta.success) {
                /*     GetMisVenta();*/
                Swal.fire({
                    type: 'success',
                    title: '¡Estimado(a) Cliente!',
                    text: respuesta.mensaje
                });

            } else {
                Swal.fire({
                    type: 'warning',
                    title: '¡Estimado(a) Cliente!',
                    text: respuesta.mensaje
                });

            }
        },
        error: function () {
        }
    });

}
//Abrir Modal envio Correo Masivo
function OpenEnviarCorreoMasivo() {
    $("#EnvioCorreosMasivo").addClass('hidden');
    $("#EnvioCorreosPersonalM").removeClass('hidden');
    $('#ModalCorreoMasivo').modal("show");
    $("#txtIdComentarioMasivo").val(0);
    $("#txtcorreoMasivo").val("");

    $("#txtAsuntoMasivo").val("Maria Bonita, agradece por sus comentarios");
    $("#txtMensajeMasivo").val("Maria Bonita es muy importante por contar con su comentarios y sugerencias, vamos a validar sus caso con los directivos con el fin de mejorar nuestros servicios, Gracias");

    /*    LimpiarCorreo();*/
}
function LimpiarCorreoMasivo() {
    $("#txtAsuntoMasivo").val("");
    $("#txtMensajeMasivo").val("");
    $("#EnvioCorreosMasivo").attr("disabled", false);
    $("#EnvioCorreosPersonalM").attr("disabled", false);
}
function CerrarCorreosMasivo() {
    $('#ModalCorreoMasivo').modal("hide");
    LimpiarCorreoMasivo();
}
function CerrarModalProducto() {
    $('#ModalProducto').modal("hide");

}



//Consulta Correos
function GetConsultaCorreoes() {

    $.ajax({
        type: 'POST',
        async: false,
        url: UrlGetCorreos,
        success: function success(response) {
            if (response.success == true) {

                response.data.forEach((item) => {
                    $("#selectcorreos").append(`<option style="height: 25px !important;" value="${item.idDominio}">${item.descripcion}</option>`);
                });


            } else {

            }
        },
        error: function error(ex) {
        }
    });
}
//Activa el check Masivo o Individual
async function ModalInsUsuarios() {

    let Bloqueado = 0;
    let chequeo = CheckEstadoUsuario.checked;

    if (chequeo) {
        $("#EnvioCorreosMasivo").removeClass('hidden');
        $("#EnvioCorreosPersonalM").addClass('hidden');
        $("#txtcorreoMasivo").val('');

        Bloqueado = 1;
    }
    else {
        Bloqueado = 0;
        $("#EnvioCorreosMasivo").addClass('hidden');
        $("#EnvioCorreosPersonalM").removeClass('hidden');
        $("#txtcorreoMasivo").val('');

    }

    if (chequeo) {
        CambioAbotonSi();
    }
    else {
        CambioAbotonNO();
    }



}
function CambioAbotonSi() {

    $("#idMasivo").css("display", "block");
    $("#idIndividual").css("display", "none");
    $('#CheckEstadoUsuario').prop('checked', 1).trigger('change');
}
function CambioAbotonNO() {


    //$("#bootstrap-duallistbox-nonselected-list_").val(0);
    //$("#bootstrap-duallistbox-nonselected-list_").trigger('change.select2');
    //$("#bootstrap-duallistbox-nonselected-list_").trigger("chosen:updated");

    $("#idMasivo").css("display", "none");
    $("#idIndividual").css("display", "block");
    $('#CheckEstadoUsuario').prop('checked', 0).trigger('change');
}


function fecha() {

    $(".fechaInput").kendoDateTimePicker({
        culture: "es-CO",
        value: "",
        interval: 1,
        format: "dd/MM/yyyy hh:mm tt",
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

    $(".k-datetimepicker").css("width", "100%");
}

function fecha2() {

    $(".fechaInput2").kendoDateTimePicker({
        culture: "es-CO",
        value: "",
        interval: 1,
        format: "dd/MM/yyyy hh:mm tt",
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

    $(".k-datetimepicker").css("width", "100%");
}


function editarFila(idVenta) {
    Limpiar();
    // Buscar la fila correspondiente en la tabla
    var fila = listaProductoVenta.find(item => item.id_compra_realizada == idVenta);
    if (fila) {     
        // Rellenar los campos del modal con los valores de la fila seleccionada
        $("#modaIdVenta").val(fila.id_compra_realizada);
        $("#modalEditIdVenta").val(fila.id_compra_realizada);
        $("#modalEditPreferenceId").val(fila.preferenceId);
        $("#modalEditStatus").val(fila.status);
        $("#modalEditMerchantOrderId").val(fila.merchant_order_id);
        $("#modalEditCantidad").val(fila.cantidad);
        $("#modalEditPrecioTotal").val(fila.precioTotal);
        $("#modalEditCostoEnvio").val(fila.costoEnvio);
        $("#modalEditPrecioGeneral").val(fila.precioGeneral);
        $("#modalEditFechaCreacion").val(fila.fecha_CreacionS);
        $("#modalEditFechaEnvioProducto").val(fila.fecha_Envio_productoS);
        $("#modalEditEmpresaEntrega").val(fila.empresa_Entrega);
        $("#modalEditEnviadoSatisfactoriamente").val(fila.enviado_Satisfactoriamente);
        $("#modalEditFechaLlegadaProducto").val(fila.fecha_Llegada_productoS);
        $("#modalEditEstadoProducto").val(fila.estado_producto);
        $("#modalEditDireccion").val(fila.direccionS);
        $("#modalEditPais").val(fila.pais);
        $("#modalEditDepartamento").val(fila.departamento);
        $("#modalEditCiudad").val(fila.ciudad);
        $("#modalEditCodigoPostal").val(fila.codigo_Postal);
        $("#modalEditTelefono").val(fila.celular);
        $("#modalEditCorreoElectronico").val(fila.correo_Electronico);
        $("#modalEditComentario").val(fila.comentario);
        $("#modalEditNombres").val(fila.nombres);
        $("#modalEditApellidos").val(fila.apellidos);
        $("#modalEditEnvioCorreo").val(fila.envioCorreo);
        $("#modalEditCancelada").val(fila.cancelada);
        $("#modalCelularConIndicativo").val(fila.celularConIndicativo);
        $("#modalEditDireccionOpcional").val(fila.opcional_Direccion);

        // Nuevos campos añadidos
        $("#modalEditEntregaMercanciaLocal").val(fila.entregaMercanciaLocal);
        $("#modalEditPagoNequi").val(fila.pagoNequi);
        $("#modalEditPagoDaviplata").val(fila.pagoDaviplata);
        $("#modalEditOtroMedioPago").val(fila.otroMedioPago);
        $("#modalEditPagoEfectivo").val(fila.pagoEfectivo);
    }

    // Abrir el modal
    $('#modalEdit').modal('show');
}

function editarFila2(idVenta) {
    Limpiar();
    // Buscar la fila correspondiente en la tabla
    var fila = listaProductoVenta2.find(item => item.id_compra_realizada == idVenta);
    if (fila) {
        // Rellenar los campos del modal con los valores de la fila seleccionada
        $("#modaIdVenta").val(fila.id_compra_realizada);
        $("#modalEditIdVenta").val(fila.id_compra_realizada);
        $("#modalEditPreferenceId").val(fila.preferenceId);
        $("#modalEditStatus").val(fila.status);
        $("#modalEditMerchantOrderId").val(fila.merchant_order_id);
        $("#modalEditCantidad").val(fila.cantidad);
        $("#modalEditPrecioTotal").val(fila.precioTotal);
        $("#modalEditCostoEnvio").val(fila.costoEnvio);
        $("#modalEditPrecioGeneral").val(fila.precioGeneral);
        $("#modalEditFechaCreacion").val(fila.fecha_CreacionS);
        $("#modalEditFechaEnvioProducto").val(fila.fecha_Envio_productoS);
        $("#modalEditEmpresaEntrega").val(fila.empresa_Entrega);
        $("#modalEditEnviadoSatisfactoriamente").val(fila.enviado_Satisfactoriamente);
        $("#modalEditFechaLlegadaProducto").val(fila.fecha_Llegada_productoS);
        $("#modalEditEstadoProducto").val(fila.estado_producto);
        $("#modalEditDireccion").val(fila.direccionS);
        $("#modalEditPais").val(fila.pais);
        $("#modalEditDepartamento").val(fila.departamento);
        $("#modalEditCiudad").val(fila.ciudad);
        $("#modalEditCodigoPostal").val(fila.codigo_Postal);
        $("#modalEditTelefono").val(fila.celular);
        $("#modalEditCorreoElectronico").val(fila.correo_Electronico);
        $("#modalEditComentario").val(fila.comentario);
        $("#modalEditNombres").val(fila.nombres);
        $("#modalEditApellidos").val(fila.apellidos);
        $("#modalEditEnvioCorreo").val(fila.envioCorreo);
        $("#modalEditCancelada").val(fila.cancelada);
        $("#modalCelularConIndicativo").val(fila.celularConIndicativo);
        $("#modalEditDireccionOpcional").val(fila.opcional_Direccion);

        // Nuevos campos añadidos
        $("#modalEditEntregaMercanciaLocal").val(fila.entregaMercanciaLocal);
        $("#modalEditPagoNequi").val(fila.pagoNequi);
        $("#modalEditPagoDaviplata").val(fila.pagoDaviplata);
        $("#modalEditOtroMedioPago").val(fila.otroMedioPago);
        $("#modalEditPagoEfectivo").val(fila.pagoEfectivo);
    }

    // Abrir el modal
    $('#modalEdit').modal('show');
}

function Limpiar() {

      $("#modaIdVenta").val(0);
     $("#modalEditIdVenta").val(0);
    $("#modaIdVenta").val("");
    $("#modalEditIdVenta").val("");
    $("#modalEditPreferenceId").val("");
    $("#modalEditStatus").val("");
    $("#modalEditMerchantOrderId").val("");
    $("#modalEditCantidad").val("");
    $("#modalEditPrecioTotal").val("");
    $("#modalEditCostoEnvio").val("");
    $("#modalEditPrecioGeneral").val("");
    $("#modalEditFechaCreacion").val("");
    $("#modalEditFechaEnvioProducto").val("");
    $("#modalEditEmpresaEntrega").val("");
    $("#modalEditEnviadoSatisfactoriamente").val("");
    $("#modalEditFechaLlegadaProducto").val("");
    $("#modalEditEstadoProducto").val("");
    $("#modalEditDireccion").val("");
    $("#modalEditPais").val("");
    $("#modalEditDepartamento").val("");
    $("#modalEditCiudad").val("");
    $("#modalEditCodigoPostal").val("");
    $("#modalEditTelefono").val("");
    $("#modalEditCorreoElectronico").val("");
    $("#modalEditComentario").val("");
    $("#modalEditNombres").val("");
    $("#modalEditApellidos").val("");
    $("#modalEditEnvioCorreo").val("");
    $("#modalEditCancelada").val("");
    $("#modalCelularConIndicativo").val("");
    $("#modalEditDireccionOpcional").val("");
    $("#modalEditEntregaMercanciaLocal").val("");
    $("#modalEditPagoNequi").val("");
    $("#modalEditPagoDaviplata").val("");
    $("#modalEditOtroMedioPago").val("");
    $("#modalEditPagoEfectivo").val("");

}


function guardarEdicion() {

    var dtoCompra = {
        id_compra_realizada: $("#modalEditIdVenta").val(),
        Pais: $("#modalEditPais").val() || null,
        Nombres: $("#modalEditNombres").val() || null,
        Apellidos: $("#modalEditApellidos").val() || null,
        Nombre_Empresa: $("#modalEditNombreEmpresa").val() || null,
        Direccion: $("#modalEditDireccion").val() || null,
        Opcional_Direccion: $("#modalEditDireccionOpcional").val() || null,
        Departamento: $("#modalEditDepartamento").val() || null,
        Ciudad: $("#modalEditCiudad").val() || null,
        Codigo_Postal: $("#modalEditCodigoPostal").val() || null,
        Celular: $("#modalEditCelular").val(),
        Correo_Electronico: $("#modalEditCorreoElectronico").val() || null,
        Comentario: $("#modalEditComentario").val() || null,
        Fecha_Creacion: $("#modalEditFechaCreacion").val(),
        Fecha_Envio_producto: $("#modalEditFechaEnvioProducto").val(),
        Fecha_Llegada_producto: $("#modalEditFechaLlegadaProducto").val(),

        PrecioGeneral: $("#modalEditPrecioGeneral").val() || null,
        PrecioFinal: $("#modalEditPrecioFinal").val() || null,
        CostoEnvio: $("#modalEditCostoEnvio").val() || null,
        preferenceId: $("#modalEditPreferenceId").val() || null,
        CelularConIndicativo: $("#modalCelularConIndicativo").val() || null,
        Cantidad: $("#modalEditCantidad").val() || null,
        PrecioTotal: $("#modalEditPrecioTotal").val() || null,
        Cancelada: $("#modalEditCancelada").val() || null,
        Enviado_Satisfactoriamente: $("#modalEditEnviadoSatisfactoriamente").val() || null,
        Estado_producto: $("#modalEditEstadoProducto").val() || null,
        Empresa_Entrega: $("#modalEditEmpresaEntrega").val() || null,

        // Nuevos campos agregados
        EntregaMercanciaLocal: $("#modalEditEntregaMercanciaLocal").val() || null,
        PagoNequi: $("#modalEditPagoNequi").val() || null,
        PagoDaviplata: $("#modalEditPagoDaviplata").val() || null,
        OtroMedioPago: $("#modalEditOtroMedioPago").val() || null,
        PagoEfectivo: $("#modalEditPagoEfectivo").val() || null,
    };

    $.ajax({
        type: "POST",
        url: UrlUpdateVentaProducto,
        async: true,
        data: dtoCompra,
        dataType: 'json',
        cache: false,
        success: function (response) {
            GetMisVenta();
            GetMisVentaEnviadas();
            GetConsultaCorreoes();
            /*   alert(response.mensaje); */
            $('#modalEdit').modal('hide');
            Limpiar();
        },
        error: function (xhr, status, error) {
            alert("Error al actualizar la compra.");
        }
    });
}




