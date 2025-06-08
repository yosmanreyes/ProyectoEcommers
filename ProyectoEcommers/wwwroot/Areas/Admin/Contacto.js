//function ModalExitoso(titulo, texto) {
//    $("#TituloMensaje").html("<h3><img src='/img/AlertSucess.png' width='50' height='50' />&nbsp" + titulo + "</h3>");
//    $("#ContenidoCambio").html("<div class='alert alert-success' role='alert' style='text-align: justify; font-size:20px;'><span class='fas fa-info'></span>&nbsp" + texto + "</div>");
//    $('#Modal_Transacciones').modal("show");
//}
//function ModalError(titulo, texto) {
//    $("#TituloMensaje").html("<h3><img src='/img/AlertError.png' width='50' height='50' />&nbsp" + titulo + "</h3>");
//    $("#ContenidoCambio").html("<div class='alert alert-danger' role='alert' style='text-align: justify; font-size:20px;'><span class='fas fa-info'></span>&nbsp" + texto + "</div>");
//    $('#Modal_Transacciones').modal("show");
//}


//function create(Type, Mensaje, Icon) {
//    VanillaToasts.create({
//        title: 'Estimado(a) Cliente...',
//        text: Mensaje,
//        type: Type,
//        icon: Icon,
//        timeout: '6000'
//    });
//}

function ModalExitoso(titulo, texto) {
    $("#TituloMensaje").html("<h3><img src='/img/AlertSucess.png' width='50' height='50' />&nbsp" + titulo + "</h3>");
    $("#ContenidoCambio").html("<div class='alert alert-success' role='alert' style='text-align: justify; font-size:20px;'><span class='fas fa-info'></span>&nbsp" + texto + "</div>");
    $('#Modal_Transacciones').modal("show");
}
function ModalError(titulo, texto) {
    $("#TituloMensaje").html("<h3><img src='/img/AlertError.png' width='50' height='50' />&nbsp" + titulo + "</h3>");
    $("#ContenidoCambio").html("<div class='alert alert-danger' role='alert' style='text-align: justify; font-size:20px;'><span class='fas fa-info'></span>&nbsp" + texto + "</div>");
    $('#Modal_Transacciones').modal("show");
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

function Ins_Contacto() {
    let _Nombres = $("#Nombres").val();
    let _CorreoElectronico = $("#CorreoElectronico").val();
    let _NumeroTelefono = Number($("#NumeroTelefono").val());
    let _Comentarios = $("#Comentarios").val();

    var Indicativo = '+57';
    let _Indicativo = Indicativo;

    if (_Nombres == "") {
        create('error', 'Ingresa su nombre por favor...', UrlAlertError);
        return;
    }
    if (_CorreoElectronico == "") {
        create('error', 'Ingresa su correo electrónico por favor...', UrlAlertError);
        return;
    }

    if (_Indicativo == "" || _Indicativo == 0) {
        create('error', 'seleccione el indicativo de su pais, por favor...', UrlAlertError);
        return;
    }

    if (_NumeroTelefono < 1) {
        create('error', 'Ingresa su número de teléfono por favor...', UrlAlertError);
        return;
    }

    if (_Comentarios == "") {
        create('error', 'Ingresa su comentario por favor...', UrlAlertError);
        return;
    }
 // Usa el nombre del encabezado configurado
    let Archivo = new FormData();

    Archivo.append('Nombres', _Nombres);
    Archivo.append('CorreoElectronico', _CorreoElectronico);
    Archivo.append('NumeroTelefono',  _NumeroTelefono);
    Archivo.append('Comentarios', _Comentarios);
    Archivo.append('NumeroTelefonoConIndicativo', Indicativo + _NumeroTelefono);


    $.ajax({
        type: 'POST',
        url: urlMensaje,
        dataType: 'json',
        data: Archivo,
        cache: false,
        contentType: false,
        processData: false,
        enctype: 'multipart/form-data',
        success: function (response) {
            if (response.success == true) {
                Limpiar();
                create('success', 'Maria Bonita, Agradece por sus comentarios, inquietudes y sugerencias, para nosotros es muy importante...', UrlAlertSucess);
                return;
            }

            else {

                create('error', 'No fue posible guardar su comentario.', UrlAlertError);
                return;
            }

        },
    });
}
function Limpiar() {

    $("#Nombres").val("");
    $("#CorreoElectronico").val("");
    $("#NumeroTelefono").val("");
    $("#Comentarios").val("");

}

//if (PaginaPrincipal == 3) {
//    CargarListaIndicativos();
//}
function CargarListaIndicativos() {
    let container = document.querySelector('#IdPaisIndicativoContacto');
    if (container != null) { 
    container.innerHTML = "";
    DominioMarcas = JSON.parse(localStorage.getItem("listaPaises"));
    container.innerHTML += `<option value="0" >Seleccione</option>`;
    DominioMarcas.forEach((item) => {
        container.innerHTML += `<option value="${item.abreviatura}">  ${item.descripcion} (${item.abreviatura})</option>`;
    });
    }
  
}
