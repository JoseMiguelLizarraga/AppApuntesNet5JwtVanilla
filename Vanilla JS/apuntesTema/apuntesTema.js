
import ColocarTemplate from '../modules/colocarTemplate.js';
import {mostrarLoadingSpinner, ocultarLoadingSpinner} from '../modules/funcionesGenericas.js';
import {configuracionesProyecto} from '../modules/configuracionesProyecto.js';
import {selectBuscador} from '../modules/buscadorPaginado.js';
import '../js/jquery.js';
import '../js/bootstrap.min.js';
import '../js/tinymce.min.js';


var apuntesTema = {};
var listasBuscador = []; 

var url = configuracionesProyecto.rutaWebApi; 
// var url = "http://localhost:8777";    

var operacion = "";            // Fue modificado

var paginaActual = 0;    // Atributo opcional en caso de usar Paginación 
var totalPaginas = 0;    // Atributo opcional en caso de usar Paginación 


class ApuntesTema extends ColocarTemplate
{
    constructor() 
    {
        super();  // Llama al constructor de la clase padre

        // Coloca los metodos y variables para que esten disponibles desde el html
        Object.getOwnPropertyNames(ApuntesTema.prototype).forEach(c=> { window[c] = this[c]; });
        window["paginaActual"] = paginaActual;

        // console.log($("#loading-spinner"));  // Funciona

        obtenerListaPrincipal();
    }


    inicializarSelectBuscador(evento)  // Convertir select normal en un select buscador con paginacion  
    { 
        var objetoHTML = evento.target; 

        if (objetoHTML.localName == "select" && objetoHTML.getAttribute("pagina") == null && objetoHTML.size <= 1)   // Si aun no ha sido inicializado el select buscador 
        { 
            selectBuscador({ 
                objetoHTML: objetoHTML, 
                token: localStorage.getItem("token_app_apuntes"),
                url: url + objetoHTML.getAttribute("rutaWebApi"), 
                params: { clase: objetoHTML.getAttribute("entidad"), atributoBuscado: objetoHTML.getAttribute("atributoBuscado") }, 
                registrosPorPagina: {nombre: "registrosPorPagina", valor: 10}, 
                nombrePagina: "numeroPagina", nombreBusqueda: "busqueda", mensajeBuscando: "buscando resultados ..." 
            }); 
            objetoHTML.click();  // Abre el select  
        } 
    } 

    obtenerConsultaBuscador() 
    { 
        var tituloId = document.getElementById("tituloId"); 
        var apuntesCategoriaId = document.getElementById("apuntesCategoriaId"); 

        var consulta = {}; 

        if (tituloId != null && ! ["", null].includes(tituloId["value"])) 
            consulta["titulo"] = tituloId["value"]; 

        if (apuntesCategoriaId != null && ! ["", null].includes(apuntesCategoriaId["value"])) 
            consulta["apuntesCategoria.id"] = apuntesCategoriaId["value"];   // Foreign key  

        return consulta; 
    } 


    // Forma opcional en caso de usar Paginación 
    obtenerListaPrincipal(pagina = null)  // Obtener lista de ApuntesTema de la Web Api 
    { 
        if (pagina != null) {                       // NUEVO
            paginaActual = pagina;                  // NUEVO
            window["paginaActual"] = paginaActual;  // NUEVO
        }

        var registrosPorPagina = 10; 
        var inicio = paginaActual * registrosPorPagina; 

        var consulta = obtenerConsultaBuscador(); 
        consulta["start"] = inicio;                 // Datos para paginacion 
        consulta["length"] = registrosPorPagina;    // Datos para paginacion 

        var parametros = Object.entries(consulta).map(c=> c.join("=")).join("&"); 
        
        mostrarLoadingSpinner();

        fetch(url + "/ApuntesTema/llenarDataTable?" + parametros, {
            method: "POST",
            headers: {"Authorization": "Bearer " + localStorage.getItem("token_app_apuntes")}
        })
        .then(response => 
        {
            if(response.ok) 
            {
                response.json().then(data => 
                {
                    totalPaginas = Math.ceil( parseInt(data["recordsTotal"]) / registrosPorPagina ) - 1; 

                    if (totalPaginas > 0) { 
                        // Habilitar o desabilitar botones de navegacion para la paginacion 
                        document.getElementById("nav_paginacion").style.display = "block"; 
                        document.getElementById("btn_pagina_anterior").disabled = (paginaActual == 0) ? true : false; 
                        document.getElementById("btn_pagina_siguiente").disabled = (paginaActual >= totalPaginas) ? true : false; 
                    } 
                    else {  
                        document.getElementById("nav_paginacion").style.display = "none";    
                    }

                    var tabla = document.getElementById("tablaPrincipal").getElementsByTagName("tbody")[0]; 
                    tabla.innerHTML = "";   // Limpia la tabla 

                    data["data"].reverse().forEach((c, index) => 
                    {
                        var fila = tabla.insertRow(0);

                        fila.insertCell(0).innerHTML = c.id;
                        fila.insertCell(1).innerHTML = c.titulo;
                        fila.insertCell(2).innerHTML = c.apuntesCategoria.titulo;
                        fila.insertCell(3).innerHTML = "<a href='javascript:;' onclick='editar(" + c.id + "); $(\"#modalCrearEditar\").modal(\"show\")'> Editar </a>";
                        fila.insertCell(4).innerHTML = "<a href='javascript:;' onclick='eliminar(" + c.id + ")'> Eliminar </a>";
                    });
                });
            } 
            else {
                response.text().then(textoError => alert(textoError));
                //throw new Error("Se encontro un error");
            }
        })
        .catch(error => {
            alert(JSON.stringify(error));
        })
        .finally(c=> {
            ocultarLoadingSpinner();
        });

    } 


    construirObjeto(evento = null, jsonAtributosEntidadPadre = null)
    {
        if (evento != null)  // Si desde el html se desea crear/modificar el objeto de tipo ApuntesTema
        {
            if (jsonAtributosEntidadPadre == null) {
                apuntesTema[evento.target.name] = evento.target.value;  // Atributo comun
            }
            else {
                for (var prop in jsonAtributosEntidadPadre) {  
                    apuntesTema[evento.target.name][prop] = jsonAtributosEntidadPadre[prop];  // Atributo fk
                }
            }
        }

        Array.from(document.getElementsByClassName("apuntesTema")).forEach(c => 
        {
            if (c.name != undefined) 
            {
                var atributo = apuntesTema[c.name];

                if (atributo == null) return console.log("El objeto apuntesTema no tiene un atributo con el nombre " + c.name);

                if (atributo instanceof Object && c.localName == "select")  // Si es una foreign key
                {
                    //debugger;
                    
                    if (c.name == "apuntesCategoria") 
                    {
                        c.textContent = "";    // Borra todos los options del select
                        var nuevoOption = document.createElement("option"); 
                        
                        if (atributo["id"] != null && atributo["id"] != "")
                        {
                            //$("#apuntesCategoria_select2").select2("data", null).val("");  // Vacia el select2
                            //$("#apuntesCategoria_select2").val(atributo["id"]).select2("data", { id: atributo["id"], text: atributo["titulo"] });
                            
                            nuevoOption.selected = true;
                            nuevoOption.text = atributo["toString"];						
                            nuevoOption.value = atributo["id"];	 
                        }
                        else {					
                            nuevoOption.text = "- Seleccione -";						
                            nuevoOption.value = "";	
                        }
                        c.appendChild(nuevoOption);   // Agrega un option al select
                    }
                }
                else {
                    c.value = apuntesTema[c.name];  // Coloca los valores del objeto en cada elemento del form
                }  
            }
        });

        //=====================================================>>>>>

        var tablaApuntesDetalleTema = document.getElementById("tablaApuntesDetalleTema").getElementsByTagName("tbody")[0];
        tablaApuntesDetalleTema.innerHTML = "";   // Limpia la tabla

        apuntesTema["listaApuntesDetalleTema"].forEach((c, index) => 
        {
            var fila = tablaApuntesDetalleTema.insertRow(0);

            fila.insertCell(0).innerHTML = "<textarea name='titulo' onkeyup='actualizarApuntesDetalleTema(event, " + index + ")' class='form-control'> " + c.titulo + " </textarea>";

            //fila.insertCell(1).innerHTML = "<input type='text' name='contenido' value='" + c.contenido + "' onkeyup='actualizarApuntesDetalleTema(event, " + index + ")' " + c.contenido + "' />";
            
            fila.insertCell(1).innerHTML = "<textarea name='" + index + "' class='contenidos_detalles'> " + c.contenido + " </textarea>";

            fila.insertCell(2).innerHTML = "<button type='button' onclick='quitarApuntesDetalleTema(" + index + ")'> Remover </button>";
            //fila.insertCell(2).innerHTML = "<button onclick='alert(\"babu ricarda\");'> Remover </button>";
        });

        tinymce.init({ 
            selector: ".contenidos_detalles",   
            setup: function (editor) {
                editor.on("keyup", function (e) 
                {
                    var elementoJson = apuntesTema["listaApuntesDetalleTema"][e.currentTarget.dataset.id];
                    //elementoJson["contenido"] = e.rangeParent.textContent;  // Texto sin html
                    elementoJson["contenido"] = e.currentTarget.innerHTML;

                    //console.log( elementoJson["contenido"] );
                });
            }
        });

        window["apuntesTema"] = apuntesTema;  // Asi el objeto es visible desde la vista    NUEVO
    }

   
    crearNuevo()  // Iniciar creación de nuevo ApuntesTema 
    { 
        operacion = "crear";   // NUEVO
    
        apuntesTema = { 
            titulo: "", 
            apuntesCategoria: {"id": "", "toString": ""},  // Foreign key       
            listaApuntesDetalleTema: [], 
        }; 
    
        construirObjeto();  // NUEVO
    } 

    guardar(evento)  // Guardar y Actualizar ApuntesTema en la Web Api 
    { 
        evento.preventDefault(); 
        var validacion = validar(apuntesTema); 
        if (validacion != "") return alert(validacion); 

        var metodoEnvio = (operacion == "crear") ? "POST" : "PUT";
        
        //====================================================================================================================>>>>>>
        mostrarLoadingSpinner();
        
        fetch(url + "/ApuntesTema", {
            method: metodoEnvio,
            headers: { 
                "Authorization": "Bearer " + localStorage.getItem("token_app_apuntes"),
                "Accept": "application/json", "Content-Type": "application/json"
            },
            body: JSON.stringify(apuntesTema)
        })
        .then(response => 
        {
            if(response.ok) 
            {
                obtenerListaPrincipal();               // Se actualiza la lista de ApuntesTema llamando la Web Api 
                $("#modalCrearEditar").modal("hide");  // Cierra el modal
            } 
            else {
                response.text().then(textoError => alert(textoError));
                //throw new Error("Respuesta de red OK pero respuesta HTTP no OK");
            }
        })
        .catch(error => {
            alert(JSON.stringify(error));
        })
        .finally(c=> {
            ocultarLoadingSpinner();
        });
    } 


    validar(apuntesTema)  // Validar ApuntesTema 
    { 
        var listaApuntesDetalleTema = apuntesTema["listaApuntesDetalleTema"]; 

        // Se validan los atributos de la entidad 

        if (["", null].includes(apuntesTema.titulo)) return "El campo titulo no posee un valor"; 
        if (["", null].includes(apuntesTema.apuntesCategoria.id)) return "El campo de tipo ApuntesCategoria no está seleccionado";   

        // Se validan las referencias cruzadas de la entidad ApuntesDetalleTema 

        if (apuntesTema["listaApuntesDetalleTema"].length == 0) 
            return "El apunte no posee ningun detalle de tipo ApuntesDetalleTema";   // NUEVO
        

        if (listaApuntesDetalleTema.some(c=> ["", null].includes(c.titulo))) 
            return "Un detalle de tipo ApuntesDetalleTema tiene vacío el campo titulo"; 

        //if (listaApuntesDetalleTema.some(c=> ["", null].includes(c.contenido))) 
            //return "Un detalle de tipo ApuntesDetalleTema tiene vacío el campo contenido"; 
        
        return ""; 
    } 


    editar(id)  // Obtener ApuntesTema de la Web Api por su id 
    { 
        mostrarLoadingSpinner();

        fetch(url + "/ApuntesTema/" + id, {
            method: "GET",
            headers: { "Authorization": "Bearer " + localStorage.getItem("token_app_apuntes") }
        })
        .then(response => 
        {
            if(response.ok) 
            {
                response.json().then(data => 
                {
                    operacion = "editar";   // NUEVO
                    apuntesTema = data;
                    
                    // Asignar atributo toString a objeto(s) padre(s) 

                    var apuntesCategoria = apuntesTema["apuntesCategoria"]; 
                    //apuntesTema["apuntesCategoria"]["toString"] = JSON.stringify(apuntesCategoria["id"]);  // Al objeto padre de tipo ApuntesCategoria le agrega el atributo toString 
                    apuntesTema["apuntesCategoria"]["toString"] = apuntesCategoria["titulo"];  				 // Al objeto padre de tipo ApuntesCategoria le agrega el atributo toString 
                    
                    construirObjeto();
                });
            } 
            else {
                response.text().then(textoError => alert(textoError));
                //throw new Error("Respuesta de red OK pero respuesta HTTP no OK");
            }
        })
        .catch(error => alert(JSON.stringify(error)) )
        .finally(c=> ocultarLoadingSpinner() );
        
    } 

    eliminar(id)  // Eliminar ApuntesTema 
    { 
        var opcionConfirm = confirm("Desea realmente eliminar este registro"); 
        if (opcionConfirm == true) 
        { 
            mostrarLoadingSpinner();
            
            fetch(url + "/ApuntesTema/" + id, {
                method: "DELETE",
                headers: { "Authorization": "Bearer " + localStorage.getItem("token_app_apuntes") }
            })
            .then(response => 
            {
                if(response.ok) {
                    obtenerListaPrincipal();  // Se actualiza la lista de ApuntesTema llamando la Web Api 
                } 
                else {
                    response.text().then(textoError => alert(textoError));
                }
            })
            .catch(error => alert(JSON.stringify(error)) )
            .finally(c=> ocultarLoadingSpinner() );
        } 
    } 

    //====================================================================================================>>>>>> 
    // Control de referencias cruzadas de la entidad ApuntesDetalleTema 

    agregarNuevoApuntesDetalleTema()  // Agregar a ApuntesTema un detalle de tipo ApuntesDetalleTema 
    { 
        apuntesTema["listaApuntesDetalleTema"].push({ 
            titulo: "", 
            contenido: "", 
        }); 
        construirObjeto(); 
    } 

    actualizarApuntesDetalleTema(evento, indice)  // Actualizar detalle de tipo ApuntesDetalleTema que pertenece a ApuntesTema 
    { 
        var posicion = apuntesTema["listaApuntesDetalleTema"][indice];  
        posicion[evento.target.name] = evento.target.value;             
    } 

    quitarApuntesDetalleTema(indice)  // Quitar detalle de tipo ApuntesDetalleTema que pertenece a ApuntesTema 
    { 
        apuntesTema["listaApuntesDetalleTema"].splice(indice, 1);   
        construirObjeto();                               
    } 

    //====================================================================================================>>>>>> 
    
}

new ApuntesTema();

//==============================================================>>>>>

/*
let instancia = new MiClase();

Object.getOwnPropertyNames(MiClase.prototype).forEach(c=> { 
    console.log(c);
    window[c] = instancia[c];
});
*/
