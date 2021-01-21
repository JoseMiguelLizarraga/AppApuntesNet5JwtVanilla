

function limpiarInputNumber(objeto)
{
    if (objeto instanceof Object) {
        if (objeto.tagName != null && objeto.tagName.toLowerCase() == "input" && objeto.value != "") {
            var numero = objeto.value;
            if (numero.charAt(0) == "0" && numero.length > 1) { numero = numero.substring(1); }   // Elimino el cero antes del numero
            objeto.value = String(numero).replace(/\D/g, "");                                     // Limpiar letras y espacios en blanco  
        }
        if (objeto.target != null && objeto.target.value != null) {
            var numero = objeto.target.value;
            if (numero.charAt(0) == "0" && numero.length > 1) { numero = numero.substring(1); }   // Elimino el cero antes del numero
            objeto.target.value = String(numero).replace(/\D/g, "");                              // Limpiar letras y espacios en blanco
        }
    }
}

function formatearRut(contenido)
{
    if (!contenido.match(/^[a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ_\s]+$/))  // Si tiene al menos un caracter que no sea una letra
    {
        if (contenido.charAt(0) == "-" || !contenido.match(/^[0-9\k\.\-\/]+$/)) // Si el primer caracter es un guion o el contenido no es valido
        {
            contenido = contenido.slice(0, -1);
        }

        var dvRut = contenido.split("-")[1];
        if (dvRut != null && dvRut.length > 1) { contenido = contenido.slice(0, -1); }  // Remueve mas de un digito verificador

        var ultimoCaracter = contenido.substr(-1);

        if (!contenido.includes("-"))   // Si no se agrego el guion, entonces formatea el numero
        {
            if (ultimoCaracter.includes("k")) { contenido = contenido.slice(0, -1); }

            var num = contenido.split("-")[0].replace(/\./g, "");
            num = num.toString().split("").reverse().join("").replace(/(?=\d*\.?)(\d{3})/g, "$1.");
            num = num.split("").reverse().join("").replace(/^[\.]/, "");
            contenido = num;
        }
        else {
            if ((contenido.split("-").length - 1) > 1) { contenido = contenido.slice(0, -1); }  // Si tiene mas de un guion
        }
    }
    return contenido;
}

function formatearNumeroEntero(e)
{
    var numero;

    // Si se llamo directamente desde el input
    if (e instanceof Object && e.tagName != null && e.tagName.toLowerCase() == "input" && e.value != "") {
        numero = e.value;
    }
    else {
        if (e instanceof Object) numero = e.target.value;    // Si se recibio un objeto
        if (typeof e === "string") numero = e;               // Si se recibio un string  
        if (typeof e === "number") numero = "" + e;          // Si se recibio un numero
    }

    numero = numero.replace(/[^0-9]/g, '');  // Limpia el numero

    var num = numero.replace(/[^0-9]/g, '');  // Remueve otros caracteres

    if (!isNaN(num)) {
        num = num.toString().split("").reverse().join("").replace(/(?=\d*\.?)(\d{3})/g, "$1.");
        num = num.split("").reverse().join("").replace(/^[\.]/, "");
        numero = num;
    } else {
        numero = input.value.replace(/[^\d\.]*/g, "");
    }

    numero = numero.replace(/^0+/, '');   // Remover ceros a la izquierda

    // Retorno dependiendo del valor recibido

    // Si se llamo directamente desde el input
    if (e instanceof Object && e.tagName != null && e.tagName.toLowerCase() == "input" && e.value != "") {
        e.value = numero;
    }
    else {
        if (e instanceof Object) e.target.value = numero;    // Si se recibio un objeto
        if (typeof e === "string") return numero;            // Si se recibio un string 
        if (typeof e === "number") return numero;            // Si se recibio un numero
    }

}

function formatearNumeroDecimal(e)
{
    var numero;

    if (e instanceof Object) numero = e.target.value;    // Si se recibio un objeto
    if (typeof e === "string") numero = e;               // Si se recibio un string  

    if (typeof e === "number")                           // Si se recibio un numero
    {
        numero = ("" + e).split(".").join(",");  // Reemplaza punto por coma

        if (numero != "0") {
            if (numero.includes(",")) {
                numero = formatearNumeroEntero(numero.split(",")[0]) + "," + numero.split(",")[1];
            }
            else {
                numero = formatearNumeroEntero(numero);
            }

            if (("" + e).charAt(0) == "0") numero = "0" + numero;   // Si el numero comienza en cero, coloca ese cero en su posicion
        }
    }
    else {
        var primerCaracter = numero.charAt(0);
        var ultimoCaracter = numero[numero.length - 1];
        var cantidadComasNumero = (numero.match(/,/g) || []).length;
        var primerosDosCaracteres = numero.substring(0, 2);
        var decimales = numero.split(",")[1];

        if (primerosDosCaracteres == "0,")  // En caso de ser un cero, decimales ... Ej: 0,5
        {
            decimales = decimales.replace(/[^0-9]/g, '');                    // Limpia los decimales
            if (decimales.length > 5) decimales = decimales.slice(0, -1);
            numero = "0," + decimales;
        }
        else {
            if (primerCaracter == "0" && numero.length == 1) {
                numero = primerCaracter;
            }
            else if (isNaN(primerCaracter) || cantidadComasNumero > 1) {
                numero = formatearNumeroEntero(numero).replace(",", "");   // Remueve solo una coma
            }
            else {
                if (!numero.includes(","))   // Si no se agrego la coma
                {
                    numero = formatearNumeroEntero(numero)
                }
                else  // Si se agrego la coma
                {
                    decimales = decimales.replace(/[^0-9]/g, '');  // Limpia los decimales

                    if (decimales.length > 5) decimales = decimales.slice(0, -1);

                    var cuerpoNumero = numero.split(",")[0];
                    numero = formatearNumeroEntero(cuerpoNumero) + "," + decimales;
                }
            }
        }
    }

    // Retorno dependiendo del valor recibido

    if (e instanceof Object) e.target.value = numero;    // Si se recibio un objeto
    if (typeof e === "string") return numero;            // Si se recibio un string 
    if (typeof e === "number") return numero;            // Si se recibio un numero
}

function colocarCeroInputVacio(objeto)
{
    if (objeto.target.value == "") objeto.target.value = "0";
}

function colocarUnoInputVacio(objeto)
{
    if (objeto.target.value == "") objeto.target.value = "1";
}

function validarNuloVacio(e)
{
    //console.log(e);

    //if (typeof something === "undefined") return false;

    if (e instanceof Object) {
        if (e.hasOwnProperty("target") && e.target.tagName &&
            (e.target.tagName.toLowerCase() == "input" || e.target.tagName.toLowerCase() == "select")
        ) {
            if (e.target.value == null || e.target.value == "") { return false; }
            return true;
        }
    }
    if (typeof e === "string" || e instanceof String) {
        if (e == null || e == "") { return false; }
        return true;
    }

    return false;
}

function validarFecha(dateString)
{
    // Analizar las partes de fecha en enteros
    var parts = dateString.split("-");
    var day = parseInt(parts[2], 10);
    var month = parseInt(parts[1], 10);
    var year = parseInt(parts[0], 10);

    if (day >= 1900)  // Si se esta usando el formato dd-MM-yyyy
    {
        var diaEncontrado = day;
        day = year;
        year = diaEncontrado;
    }

    // Checkear los rangos de mes y a�o
    if (year < 1000 || year >= 3000 || month == 0 || month > 12)
        return false;

    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // Ajustar para los a�os bisiestos
    if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
        monthLength[1] = 29;

    if ((day > 0 && day <= monthLength[month - 1]) == false) {
        //mostrarMensaje("El d�a ingresado no corresponde al mes seleccionado", "Informacion");
    }

    // Verificar el rango del d�a
    return day > 0 && day <= monthLength[month - 1];
}

function mostrarModal(datos)
{
    var url = datos.url;
    var tipoEnvio = datos.tipoEnvio;
    var data = datos.data;
    var idModal = datos.idModal;
    var idContenidoModal = datos.idContenidoModal;

    $.ajax({
        url: url,
        type: tipoEnvio,
        data: data,            // { idArea: 1 }
        success: function (data) {
            if (data.status != false) {
                $(idContenidoModal).html(data);
                $(idModal).modal('show');
            }
            else {
                jQuery.each(data.errors, function (key, val) {
                    alert_error(val.ErrorMessage);
                });
            }
        }
    });
}

function mostrarOcultarColumnasDatatable(datos)
{
    var tabla = $(datos.idTabla).DataTable();

    datos.info.forEach(function (c) {
        if (Object.keys($(c.id)).length > 0)  // Si el elemento con ese id si existe
        {
            if ($("select" + c.id + " option:selected").val() != "") {
                tabla.column(c.columna).visible(false);
            } else {
                tabla.column(c.columna).visible(true);
            }
        }
    });

    $(datos.idTabla).width("100%");
}

function ocultarElemento(identificador)
{
    $(identificador).fadeOut(250);
}

function mostrarElemento(identificador)
{
    $(identificador).fadeIn(250);
}

function validarRut(rutCompleto)
{
    rutCompleto = rutCompleto.split(".").join("");  // Se remueven los puntos

    rutCompleto = rutCompleto.replace("?", "-");
    if (!/^[0-9]+[-|?]{1}[0-9kK]{1}$/.test(rutCompleto))
        return false;
    var tmp = rutCompleto.split('-');
    var digv = tmp[1];
    var rut = tmp[0];
    if (digv == 'K') digv = 'k';

    return (Fn.dv(rut) == digv);
}

var pulsado = false;            // Variable usada para saber si hay una tecla pulsada
var permitirEnvioAjax = true;   // Variable usada para saber si se puede hacer un envio por ajax o no

function impedirTeclaPulsada(idElemento)
{
    $(idElemento).keydown(function (e) {
        if (e.keyCode == 8 || e.keyCode == 46)  // Si las teclas presionadas son borrar o suprimir
        {
            permitirEnvioAjax = false;
            return true;
        }
        if (pulsado) // Detiene la capacidad de escritura de teclas pulsadas
        {
            permitirEnvioAjax = false;
            return false;
        }
        pulsado = true;
    })
        .keyup(function () {
            pulsado = false;
            permitirEnvioAjax = true;
        });
}

function llenarSelect2(datos)
{
    var idElemento = datos.idElemento;
    var url = datos.url;
    var registrosPorPagina = datos.registrosPorPagina;
    var placeholder = datos.placeholder;
    var params = datos.params;
    var datosSeleccionInicial = datos.initSelection;
	var valor = datos.valor;

    var multiple = (datos.multiple != null) ? datos.multiple : false;

    $(idElemento).select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        allowClear: true,
        multiple: multiple,
        formatSearching: function () { return "Buscando resultados.."; },
        formatNoMatches: function () { return "<a href='#' class='btn btn-danger'>No hay resultados</a>"; },
        ajax: {
            quietMillis: 150,    // Tiempo de espera en el envio de solicitudes de busqueda
            url: url,
            dataType: 'json',
            transport: function (params, success, failure) {
                var contenido = params.data.busqueda;

                if (datos.consultaRut != null) {
                    if (contenido != "" && !contenido.match(/^[a-zA-Z������������������������_\s]+$/))  // Si tiene al menos un caracter que no sea una letra
                    {
                        var input = $(this).select2("dropdown").find('.select2-input');
                        input.val(formatearRut(input.val()));   // Formatea el rut

                        var dvRut = contenido.split("-")[1];

                        // // Si no tiene al menos un guion, y un valor despues de este
                        if (!contenido.includes("-") || dvRut.length < 1) return false;

                        if (dvRut.length >= 1 && isNaN(dvRut) && dvRut != "k") return false;
                    }
                }

                var $request = $.ajax(params);
                return $request;
            },
            data: function (term, page) 
            {
                if (datos.params != null && typeof datos.params === "function")  // Si se coloca directamente la funcion
                {
                    var datosEnvio = datos.params();

                    datosEnvio.registrosPorPagina = registrosPorPagina;
                    datosEnvio.numeroPagina = page;
                    datosEnvio.busqueda = term;

                    return datosEnvio;
                }
                
                if (params != null && params instanceof Object)  // Si se recibio un json
                {
                    params.registrosPorPagina = registrosPorPagina;
                    params.numeroPagina = page;
                    params.busqueda = term;
                    return params;
                }

                return {
                    registrosPorPagina: registrosPorPagina,
                    numeroPagina: page,
                    busqueda: term,
                };
            },
            results: function (data, page) {
                var more = (page * registrosPorPagina) < data.Total;
                return { results: data.Results, more: more };
            }
        },
        initSelection: function (element, callback)  // Si es un select2 multiple pre seleccionado
        {
            if (multiple && datosSeleccionInicial != null) {
                callback(datosSeleccionInicial);
            }
        }
    })
    .on("change", datos.onchange)       // Funcion opcional para ejecutar en caso de seleccionar o quitar un option
    .on("select2-open", datos.onclick)  // Funcion opcional para ejecutar en caso de hacer click en un select2
    .on("select2-close", datos.onblur); // Funcion opcional para ejecutar en caso de quitar la seleccion de un select2


    $(idElemento).on("change", function () {
        $(idElemento).val($(this).val());   // Se coloca el valor seleccionado en el input correspondiente
    }); 

    if (multiple && datosSeleccionInicial != null)  // Si es un select2 multiple pre seleccionado
    {
        var arregloIdsSeleccionados = Array.from(datosSeleccionInicial, c => c.id);  // Se obtiene un arreglo con solo los ids del json
        $(idElemento).select2("val", arregloIdsSeleccionados);                       // Se colocan los numeros en el value del input
    }
	
	if (valor != null && valor.id != null && valor.texto != null) // Esto es nuevo
	{
		$(idElemento).val(valor.id).select2("data", { id: valor.id, text: valor.texto }); 
	}
}  

function IsNumeric(valor)
{
    var log = valor.length; var sw = "S";

    for (x = 0; x < log; x++) {
        v1 = valor.substr(x, 1);
        v2 = parseInt(v1);
        if (isNaN(v2)) { sw = "N"; }  //Compruebo si es un valor num�rico
    }

    if (sw == "S") { return true; } else { return false; }

}

var primerslap = false;
var segundoslap = false;

function formatearfecha(fecha)
{
    var long = fecha.length;
    var dia;
    var mes;
    var ano;
    var separador = "-";

    if ((long >= 2) && (primerslap == false)) {
        dia = fecha.substr(0, 2);
        if ((IsNumeric(dia) == true) && (dia <= 31) && (dia != "00")) { fecha = fecha.substr(0, 2) + separador + fecha.substr(3, 7); primerslap = true; }
        else { fecha = ""; primerslap = false; }
    }
    else {
        dia = fecha.substr(0, 1);
        if (IsNumeric(dia) == false) { fecha = ""; }
        if ((long <= 2) && (primerslap = true)) { fecha = fecha.substr(0, 1); primerslap = false; }
    }
    if ((long >= 5) && (segundoslap == false)) {
        mes = fecha.substr(3, 2);
        if ((IsNumeric(mes) == true) && (mes <= 12) && (mes != "00")) { fecha = fecha.substr(0, 5) + separador + fecha.substr(6, 4); segundoslap = true; }
        else { fecha = fecha.substr(0, 3);; segundoslap = false; }
    }
    else { if ((long <= 5) && (segundoslap = true)) { fecha = fecha.substr(0, 4); segundoslap = false; } }
    if (long >= 7) {
        ano = fecha.substr(6, 4);
        if (IsNumeric(ano) == false) { fecha = fecha.substr(0, 6); }
        else { if (long == 10) { if ((ano == 0) || (ano < 1900) || (ano > 2100)) { fecha = fecha.substr(0, 6); } } }
    }
    if (long >= 10) {
        fecha = fecha.substr(0, 10);
        dia = fecha.substr(0, 2);
        mes = fecha.substr(3, 2);
        ano = fecha.substr(6, 4);

        // A�o no viciesto y es febrero y el dia es mayor a 28
        if ((ano % 4 != 0) && (mes == 02) && (dia > 28)) { fecha = fecha.substr(0, 2) + separador; }
    }

    return (fecha);

}

function cargarDatePicker(datos)
{
    var idElemento = datos.idElemento;
    $(idElemento).attr("autocomplete", "off");

    if (datos.placeholder != null) {
        $(idElemento).attr("placeholder", datos.placeholder);
    }

    $(idElemento).datepicker({
        dateFormat: "dd-mm-yy",
        changeYear: true,   // Seleccionar a�o
        changeMonth: true,  // Seleccionar Dia
        yearRange: "1900:+0",
        firstDay: 1,  // Primer dia de la semana El lunes
        dayNames: ["Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado"],
        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
        onSelect: function (dateText) {
            $(idElemento).val(dateText);   // Colocar fecha seleccionada en el input 

            if (datos.llamarFuncionOnSelect != null)  // Si se desea ejecutar una funcion
            {
                if (typeof datos.llamarFuncionOnSelect === "function") { // Si se coloca directamente la funcion
                    datos.llamarFuncionOnSelect();
                }
                if (typeof datos.llamarFuncionOnSelect === "string") {   // Si es por su nombre
                    window[datos.llamarFuncionOnSelect]();
                }
            }

            if (datos.idGrillaOcultar != null)  // Si se desea ocultar una grilla al seleccionar una fecha
            {
                limpiarDatatable({ "idTabla": datos.idGrillaOcultar, limpiarTodo: true });
            }
        }
    })
        .keyup(function () {
            $(this).val(formatearfecha($(this).val()));

            if (datos.idGrillaOcultar != null)  // Si se desea ocultar una grilla al escribir o borrar la fecha
            {
                limpiarDatatable({ "idTabla": datos.idGrillaOcultar, limpiarTodo: true });
            }
        })
        .focusout(function (evento) {
            if (!validarFecha($(this).val())) {
                $(this).val("");
            }
        });

}

function formatearDatePicker(fecha)
{
    if (typeof fecha === "string" || fecha instanceof String) {
        if (fecha != null && fecha != "") {
            if (fecha.includes("-")) {
                return fecha.split('-').reverse().join('-');
            }
            if (fecha.includes("/")) {
                return fecha.split('/').reverse().join('/');
            }
            return "";
        }
    }
    return "";
}

function limpiarDatatable(datos)
{
    var idTabla = datos.idTabla;

    if (!idTabla.includes("#")) idTabla = "#" + idTabla;  // Agrega el # en caso de que no lo tenga

    if (datos.limpiarTodo)  // Al apretar un filtro de busqueda de la grilla
    {
        $(idTabla).closest("table").find("tbody, tfoot").hide();  // Revisar

        // Se oculta la paginacion, su descripcion y la seleccion de numero de registros por pagina
        $(idTabla + "_info, " + idTabla + "_paginate, " + idTabla + "_length").hide();
    }
    else  // Al refrescar la tabla normalmente
    {
        if ($(idTabla).DataTable().data().any())  // Si la tabla tiene resultados
        {
            $(idTabla).closest("table").find("tbody, tfoot").show();
            $(idTabla + "_info, " + idTabla + "_paginate, " + idTabla + "_length").show();
        }
        else {
            $(idTabla).closest("table").find("tbody").show();  // Muestra solo el cuerpo de la tabla
            $(idTabla + "_info, " + idTabla + "_paginate, " + idTabla + "_length").hide();
        }
    }
}

function obtenerDvRut(numero)
{
    if (typeof numero == 'string') numero = numero.split(".").join("");  // Remover puntos
    if (isNaN(numero) || numero.length == 0) return null;

    nuevo_numero = numero.toString().split("").reverse().join("");

    for (i = 0, j = 2, suma = 0; i < nuevo_numero.length; i++ , ((j == 7) ? j = 2 : j++)) {
        suma += (parseInt(nuevo_numero.charAt(i)) * j);
    }
    n_dv = 11 - (suma % 11);
    return ((n_dv == 11) ? 0 : ((n_dv == 10) ? "K" : n_dv));
}

function generarReporte(datos)
{
    var url = datos.url;
    var metodoEnvio = (datos.metodoEnvio != null) ? datos.metodoEnvio : "GET";
    var params = (datos.params != null) ? datos.params : null;

    $.ajax({
        cache: false,
        type: metodoEnvio,
        //url: this.href + "?" + $.param(datos),
        url: url,
        data: params,
        contentType: false,
        processData: false,
        xhrFields: { responseType: 'blob' },
        beforeSend: function () {
            mostrarRelojCargaAjax();
        },
        success: function (response, status, xhr) {

            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');

            if (disposition) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches !== null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }

            try {
                var blob = new Blob([response], { type: 'application/octet-stream' });

                if (typeof window.navigator.msSaveBlob !== 'undefined') {
                    //   IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                    window.navigator.msSaveBlob(blob, filename);
                }
                else {
                    var URL = window.URL || window.webkitURL;
                    var downloadUrl = URL.createObjectURL(blob);

                    if (filename) {
                        // use HTML5 a[download] attribute to specify filename
                        var a = document.createElement("a");

                        // safari doesn't support this yet
                        if (typeof a.download === 'undefined') {
                            window.location = downloadUrl;
                        } else {
                            //var link = document.createElement('a');
                            //link.href = window.URL.createObjectURL(blob);
                            //link.target = "_blank";
                            //link.download = filename;
                            //document.body.appendChild(link);
                            //link.click();
                            //document.body.removeChild(link);

                            a.href = downloadUrl;
                            a.download = filename;
                            document.body.appendChild(a);
                            a.target = "_blank";
                            a.click();

                        }
                    } else {
                        window.location = downloadUrl;

                    }
                }

            } catch (ex) {
                console.log(ex);
            }

        },  // Fin success
        complete: function () {
            ocultarRelojCargaAjax();
        }
    })
    .fail(function (data) {
        if (datos.validarError != null && typeof datos.validarError === "function") { datos.validarError(data); }
    });
}

function eliminarRegistro(datos)
{
    var anchoVentana = (datos.anchoVentana != null) ? datos.anchoVentana : "20%";

    if (datos.descripcionElemento != null)
    {
        var texto = "<br />" + "<dl class='dl-horizontal'>";

        datos.descripcionElemento.forEach(c => {

            if (typeof c.texto === "boolean") {
                texto += "<dt> " + c.descripcion + " </dt> <dd> " + (c.texto ? "si" : "no") + " </dd>";  // Si es booleano
            }
            else {
                texto += "<dt> " + c.descripcion + " </dt> <dd> " + c.texto + " </dd>";
            }
        });

        texto += "</dl>";
        datos.pregunta = texto;
    }


    $.confirm({
        title: datos.titulo,
        boxWidth: anchoVentana,
        useBootstrap: false,
        content: datos.pregunta,
        buttons: {
            botonAceptar: {
                text: "Confirmar",
                btnClass: "btn-blue",
                action: function () {
                    $.ajax({
                        url: datos.url,
                        type: "POST",
                        data: datos.params,
                        success: datos.success
                    });
                }
            },
            Cancelar: function () { }
        }
    });

}

function cambiarTextoMayusculas(obj)
{
    if (obj.value != "") {
        obj.value = obj.value.toUpperCase();
    }
}

function mostrarRelojCargaAjax(json = {})
{
    //console.log("Se envi� peticion por ajax");
    $("#reloj-carga-ajax").fadeIn(250);
}

function ocultarRelojCargaAjax()
{
    //console.log("Se recibi� la peticion por ajax");
    $("#reloj-carga-ajax").fadeOut(250);
}

function validarCierreSesion(xhr)
{
    if (xhr.responseJSON != null && xhr.responseJSON.Url != null) {
        window.location.href = xhr.responseJSON.Url;
    }
}

function validarArchivo(datos)
{
    // onchange="validarArchivo({ 'archivo': this, 'tipoRequerido': 'imagen', 'maximoMB': 1, 'removerArchivo': true })"

    var file = datos.archivo;
    var FileSize = file.files[0].size / 1024 / 1024; // Tama�o en MB

    var maximoMB = parseInt(datos.maximoMB);

    if (FileSize > maximoMB) {
        alert("El archivo no debe exceder " + maximoMB + "MB");
        if (datos.removerArchivo != null && datos.removerArchivo == true) $(file).val("");
        return;
    }

    var mimeType = file.files[0]['type'];    //mimeType=image/jpeg or application/pdf etc...
    console.log(mimeType);

    if (datos.tipoRequerido != null && datos.tipoRequerido == "imagen" && !mimeType.includes("image")) {
        alert("El archivo debe ser una imagen");
        if (datos.removerArchivo != null && datos.removerArchivo == true) $(file).val("");
    }
}

function llenarTablaPaginada(datos = {})
{
    var idTabla = datos.idTabla;
    var url = datos.url;
    var botonOpciones = datos.botonOpciones;

    var pagina = parseInt($(idTabla).attr("pagina"));
    var existeMas = false;

    var params = datos.params;

    if (params != null) {
        params.registrosPorPagina = 10;
        params.numeroPagina = pagina;
    }

    if (datos.limpiarConsultaAnterior != null && datos.limpiarConsultaAnterior)  // Si se desea limpiar la consulta anterior
    {
        $(idTabla + " > tbody").html("");       // Se limpia el cuerpo de la tabla
        datos.limpiarConsultaAnterior = null;   // Asi este parametro no se pasa al ir de pagina en pagina
    }

    $.ajax({
        url: url,
        type: "POST",
        data: datos.params,
        async: false,
        success: function (data) {

            if (data.pagination.more) {  // Si la consulta muestra que existen mas datos
                existeMas = true;
            }

            data.results.forEach(c => {

                var fila = "<tr>";    // background-color: darkgray;

                datos.datosFila.forEach(x => {
                    fila += "<td claveUnica='" + c[datos.claveUnica] + "' class='" + x.claseCelda + "' style='width: 100%; cursor: pointer;'> " + c[x.key] + " </td>";
                });

                if (botonOpciones != null) {
                    fila += "<td style='text-align: right;'>" + botonOpciones + "</td >";
                }

                fila += "</tr>";

                $(idTabla + " > tbody").append(fila);   // Lleno el tbody
                var $table = $(idTabla), $bodyCells = $table.find('tbody tr:first').children(), colWidth; // Change the selector if needed

                // Ajustar el ancho de las celdas thead cuando la ventana cambia de tama�o  ... (Debe estar dentro del foreach)
                $(window).resize(function () {
                    colWidth = $bodyCells.map(function () { return $(this).width(); }).get();  // Get the tbody columns width array
                    $table.find('thead tr').children().each(function (i, v) { $(v).width(colWidth[i]); });   // Set the width of thead columns
                }).resize();

            });
        }
    });  // Fin ajax

    if (existeMas)  // Si la consulta muestra que existen mas datos
    {
        $(idTabla + " > tbody").on("scroll", function () {

            if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight)  // Si el scroll llega al limite de abajo
            {
                pagina++;
                $(idTabla).attr("pagina", pagina);    // El atributo de la tabla se incrementa  ...  pagina="2"
                //alert("La nueva pagina es: " + pagina);
                llenarTablaPaginada(datos);                // La funcion se llama a si misma
            }
        });
    }
}

function marcarCeldaTabla(datos = {})  
{
    if (datos.limpiarSiempre) {
        $(datos.seleccionTabla).removeClass(datos.claseCss);   // Se remueven los elementos marcados en la tabla
    }

    Array.from($(datos.seleccionTabla)).filter(a => a.attributes[datos.nombreAtributoCelda].nodeValue == datos.valorAtributoCelda).forEach(a => {
        a.classList.add(datos.claseCss);    // Se agrega otra clase
    });
}

function limpiarElementos(identificador)
{
    // Ej: identificador = "#formEditar .detalle";

    Array.from($(identificador)).forEach(a =>
    {
        var elemento = $("#" + a.id);

        if (a.className.includes("select2")) {
            //console.log("Es un select2");
            //elemento.empty();                       // Vacia el select2 ultima version
            elemento.select2("data", null).val("");   // Vacia select2
        }
        if (["text", "number"].includes(a.type)) {
            //console.log("Es un input");
            elemento.val('');
        }
        if (a.localName == "select") {
            //console.log("Es un select");
            elemento.prop('selectedIndex', 0);      // Dejar como selected el primer option del select
        }
    });
}