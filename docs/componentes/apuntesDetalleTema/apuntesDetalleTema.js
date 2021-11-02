
import { mostrarLoadingSpinner, ocultarLoadingSpinner } from "../../funciones/funcionesGenericas.js";
import { selectBuscador } from "../../funciones/selectBuscador.js";
import { configuracionesProyecto } from '../../configuraciones/configuracionesProyecto.js';
import { CargaVista } from "../cargaVista.js";


export class ApuntesDetalleTema extends CargaVista
{
    constructor()
    {
        super();

        this.url = configuracionesProyecto.rutaWebApi;
        
        this.operacion = "";            // La primera operacion es listar 
        this.apuntesDetalleTema = {}; 
        this.paginaActual = 0;          // Atributo opcional en caso de usar Paginación 
        this.totalPaginas = 0;          // Atributo opcional en caso de usar Paginación 
        this.rutaImagenesApuntes = configuracionesProyecto.rutaImagenesApuntes;

        for (var prop in this) { window[prop] = this[prop]; }                                          // Coloca los atributos de la clase para que esten disponibles desde el html
        Object.getOwnPropertyNames(this.constructor.prototype).forEach(c=> { window[c] = this[c]; });  // Coloca los metodos de la clase para que esten disponibles desde el html

        this.onInit();
    }

    onInit()
	{
		this.cargarVista().then(() => {
			this.obtenerListaPrincipal();
		});
	}

    destructor() {
        for (var prop in this) { delete window[prop]; }                                             // Remueve los atributos de la clase para que no queden en la ventana
        Object.getOwnPropertyNames(this.constructor.prototype).forEach(c=> { delete window[c]; });  // Remueve los metodos de la clase para que no queden en la ventana
    }

    cargarBuscadores()
    {
        selectBuscador({
            querySelector: "#apuntesCategoriaId",                 //buscador_apuntesCategoria_select2
            token: localStorage.getItem("token_app_apuntes"),
            placeholder: "Seleccione",
            url: url + "/ApuntesCategoria/llenarSelect2",
            params: { "clase": "ApuntesCategoria", "atributoBuscado": "titulo" },
            registrosPorPagina: {nombre: "registrosPorPagina", valor: 10},
            nombrePagina: "numeroPagina", nombreBusqueda: "busqueda", mensajeBuscando: "buscando resultados ...",
            onchange: () => 
            {  
                // Deja vacio el select para buscar por Tema
                var c = document.getElementById("apuntesTemaId");		c.textContent = "";  
                c.removeAttribute("textoSeleccionado"); 				c.removeAttribute("valorSeleccionado");
                var nuevoOption = document.createElement("option"); 	nuevoOption.text = "- Seleccione -";						
                nuevoOption.value = "";									c.appendChild(nuevoOption);   // Agrega un option al select

                //$("#buscador_apuntesTema_select2").select2("data", null).val("");  // Agregado. Vacia el otro select2 del buscador
                resetearPaginacion();  
                obtenerListaPrincipal();  
            }
        });

        selectBuscador({
            querySelector: "#apuntesTemaId",                         //buscador_apuntesTema_select2
            token: localStorage.getItem("token_app_apuntes"),
            placeholder: "Seleccione",
            url: url + "/ApuntesTema/llenarSelect2", 
            params: { 
                "clase": "ApuntesTema", 
                "atributoBuscado": "titulo",
                "idApuntesCategoria": function () {   // Agregado
                    return (document.getElementById("apuntesCategoriaId").value != "") ? document.getElementById("apuntesCategoriaId").value : 0; 
                }   
            },
            registrosPorPagina: {nombre: "registrosPorPagina", valor: 10},
            nombrePagina: "numeroPagina", nombreBusqueda: "busqueda", mensajeBuscando: "buscando resultados ...",
            onchange: () => {  
                resetearPaginacion();  
                obtenerListaPrincipal();  
            }
        });

        selectBuscador({
            querySelector: "#apuntesTema",                 //apuntesTema_select2
            token: localStorage.getItem("token_app_apuntes"),
            placeholder: "Seleccione",
            url: url + "/ApuntesTema/llenarSelect2", 
            params: { "clase": "ApuntesTema", "atributoBuscado": "titulo", "idApuntesCategoria": 0 }, 
            registrosPorPagina: {nombre: "registrosPorPagina", valor: 10},
            nombrePagina: "numeroPagina", nombreBusqueda: "busqueda", mensajeBuscando: "buscando resultados ...",
            onchange: () => { 
                apuntesDetalleTema.apuntesTema.id = (event.target["value"] != "") ? event.target["value"] : null;  
                apuntesDetalleTema.apuntesTema.toString = event.target.options[event.target.selectedIndex].text
            }
        });
    }

    //=============================================================================================>>>>>>

    obtenerConsultaBuscador() 
    { 
        var tituloId = document.getElementById("tituloId"); 
        var contenidoId = document.getElementById("contenidoId"); 
        var apuntesTemaId = document.getElementById("apuntesTemaId");   // Toma el valor del select2 en el buscador para buscar por ApuntesTema 
        var apuntesCategoriaId = document.getElementById("apuntesCategoriaId");   // Agregado
        
        var consulta = {}; 
    
        if (tituloId != null && ! ["", null].includes(tituloId["value"])) 
            consulta["titulo"] = tituloId["value"]; 
    
        if (contenidoId != null && ! ["", null].includes(contenidoId["value"])) 
            consulta["contenido"] = contenidoId["value"]; 
    
        if (apuntesTemaId != null && ! ["", null].includes(apuntesTemaId["value"])) 
            consulta["apuntesTema.id"] = apuntesTemaId["value"];   // Foreign key  
        
        if (apuntesCategoriaId != null && ! ["", null].includes(apuntesCategoriaId["value"]))   // Agregado
            consulta["apuntesTema.apuntesCategoria.id"] = apuntesCategoriaId["value"];   // Foreign key  
    
        return consulta; 
    } 
    
    obtenerListaPrincipal()  // Obtener lista de ApuntesDetalleTema de la Web Api 
    { 
        var registrosPorPagina = 10; 
        var inicio = paginaActual * registrosPorPagina; 
    
        var consulta = obtenerConsultaBuscador(); 
        consulta["start"] = inicio;                 // Datos para paginacion 
        consulta["length"] = registrosPorPagina;    // Datos para paginacion 
    
        var parametros = Object.entries(consulta).map(c=> c.join("=")).join("&"); 
     
        mostrarLoadingSpinner();
    
        fetch(url + "/ApuntesDetalleTema/llenarDataTable?" + parametros, {
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
                    else {  // AGREGAR A GENERADOR DE CODIGO OK.
                        document.getElementById("nav_paginacion").style.display = "none";    
                    }
    
                    var tabla = document.getElementById("tablaPrincipal").getElementsByTagName("tbody")[0]; 
                    tabla.innerHTML = "";   // Limpia la tabla 
    
                    data["data"].reverse().forEach((c, index) => 
                    { 
                        var fila = tabla.insertRow(0); 
                        fila.insertCell(0).innerHTML = c.id; 
                        fila.insertCell(1).innerHTML = c.titulo; 
                
                        if (c.contenido != null && c.contenido != "") {
                            fila.insertCell(2).innerHTML = "<div onclick='verContenido_VentanaNueva(" + c.id + ")' class='ver_contenido_detalle_tema'> Ver Contenido </div>"; 
                        }
                        else {
                            fila.insertCell(2).innerHTML = ""; 
                        }
                        
                        fila.insertCell(3).innerHTML = c.apuntesTema.titulo;   // JSON.stringify(c.apuntesTema)
                        
                        if (c.rutaFoto != null && c.rutaFoto != "") 
                            fila.insertCell(4).innerHTML = "<img src='" + rutaImagenesApuntes + "/" + c.rutaFoto + "' onclick='abrirFoto(event)' class='foto_detalle_tema' />"; 
                        else 
                            fila.insertCell(4).innerHTML = ""; 
    
                        fila.insertCell(5).innerHTML = "<a href='javascript:;' onclick='editar(" + c.id + "); $(\"#modalCrearEditar\").modal(\"show\")'> Editar </a>"; 
                        fila.insertCell(6).innerHTML = "<a href='javascript:;' onclick='eliminar(" + c.id + ")'> Eliminar </a>"; 
    
                    }); 
                });
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


    resetearPaginacion() 
    { 
        paginaActual = 0; 
        totalPaginas = 0; 
    } 

    abrirFoto(evento) 
    { 
        window.open(evento.target.src); 
    } 


    verContenido_VentanaNueva(id)  
    { 
        mostrarLoadingSpinner();

        fetch(url + "/ApuntesDetalleTema/" + id, {
            method: "GET",
            headers: { "Authorization": "Bearer " + localStorage.getItem("token_app_apuntes") }
        })
        .then(response => 
        {
            if(response.ok) 
            {
                response.json().then(data => {
                    var newWindow = window.open();
                    newWindow.document.write("<html><body>" + data.contenido + "</body></html>");
                    newWindow.document.close();
                });
            } 
            else {
                throw new Error("Error al ver el contenido en una nueva ventana");
            }
        })
        .catch(error => alert(JSON.stringify(error)) )
        .finally(c=> ocultarLoadingSpinner() );
    } 


    construirObjeto()  // Colocar los valores del objeto de tipo ApuntesDetalleTema en el html 
    { 

        Array.from(document.getElementsByClassName("apuntesDetalleTema")).forEach(c => 
        { 
            if (c.name != undefined) 
            { 
                var atributo = apuntesDetalleTema[c.name]; 
                
                if (atributo instanceof Object && c.localName == "select")  // Si es una foreign key
                { 
                    if (c.name == "apuntesTema") 
                    { 
                        c.textContent = "";    // Borra todos los options del select
                        var nuevoOption = document.createElement("option"); 
                        
                        if (atributo["id"] != null && atributo["id"] != "")
                        { 
                            //$("#apuntesTema_select2").select2("data", null).val("");  // Vacia el select2 
                            //$("#apuntesTema_select2").val(atributo["id"]).select2("data", { id: atributo["id"], text: atributo.titulo }); 
                            
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
                    c.value = apuntesDetalleTema[c.name];  // Coloca los valores del objeto en cada elemento del form 
                } 
            } 
        }); 

    } 


    crearNuevo()  // Iniciar creación de nuevo ApuntesDetalleTema 
    { 
        apuntesDetalleTema = { 
            titulo: "", 
            contenido: "", 
            //apuntesTema: {"id": null},  // Foreign key 
            apuntesTema: {"id": "", "toString": ""},  // Foreign key        CAMBIAR ESTO EN EL GENERADOR OK. 
            rutaFoto: ""
        }; 

        operacion = "crear"; 
        construirObjeto(); 
    } 

    guardar(evento)  // Guardar y Actualizar ApuntesDetalleTema en la Web Api 
    { 
        evento.preventDefault(); 
        var validacion = validar(apuntesDetalleTema); 
        if (validacion != "") return alert(validacion); 

        var metodoEnvio = (operacion == "crear") ? "POST" : "PUT"; 

        mostrarLoadingSpinner();
        
        fetch(url + "/ApuntesDetalleTema", {
            method: metodoEnvio,
            headers: { 
                "Authorization": "Bearer " + localStorage.getItem("token_app_apuntes"),
                "Accept": "application/json", "Content-Type": "application/json"
            },
            body: JSON.stringify(apuntesDetalleTema)
        })
        .then(response => 
        {
            if(response.ok) 
            {
                obtenerListaPrincipal();               // Se actualiza la lista de ApuntesDetalleTema llamando la Web Api 
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

    validar(apuntesDetalleTema)  // Validar ApuntesDetalleTema 
    { 
        console.log(apuntesDetalleTema);
        
        // Se validan los atributos de la entidad 

        if (["", null].includes(apuntesDetalleTema.titulo)) return "El campo titulo no posee un valor"; 
        //if (["", null].includes(apuntesDetalleTema.contenido)) return "El campo contenido no posee un valor"; 
        
        //if (apuntesDetalleTema.apuntesTema == null || Object.getOwnPropertyNames(apuntesDetalleTema.apuntesTema).length === 0) return "El campo de tipo ApuntesTema no está seleccionado"; 
        if (["", null].includes(apuntesDetalleTema.apuntesTema.id)) return "El campo de tipo ApuntesTema no está seleccionado";  // CAMBIAR ESTO EN EL GENERADOR OK.
        
        return ""; 
    } 

    editar(id)  // Obtener ApuntesDetalleTema de la Web Api por su id 
    { 
        operacion = "editar"; 
        
        mostrarLoadingSpinner();

        fetch(url + "/ApuntesDetalleTema/" + id, {
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
                    apuntesDetalleTema = data; 
                    
                    // Asignar atributo toString a objeto(s) padre(s) 

                    var apuntesTema = apuntesDetalleTema["apuntesTema"]; 
                    //apuntesDetalleTema["apuntesTema"]["toString"] = JSON.stringify(apuntesTema["id"]);  // Al objeto padre de tipo ApuntesTema le agrega el atributo toString 
                    apuntesDetalleTema["apuntesTema"]["toString"] = apuntesTema["titulo"];  			  // Al objeto padre de tipo ApuntesTema le agrega el atributo toString 
                    
                    construirObjeto();
                });
            } 
            else {
                response.text().then(textoError => alert(textoError));
            }
        })
        .catch(error => alert(JSON.stringify(error)) )
        .finally(c=> ocultarLoadingSpinner() );
    } 

    eliminar(id)  // Eliminar ApuntesDetalleTema por su id 
    { 
        var opcionConfirm = confirm("Desea realmente eliminar este registro"); 
        if (opcionConfirm == true) 
        { 
            mostrarLoadingSpinner();
            
            fetch(url + "/ApuntesDetalleTema/" + id, {
                method: "DELETE",
                headers: { "Authorization": "Bearer " + localStorage.getItem("token_app_apuntes") }
            })
            .then(response => 
            {
                if(response.ok) {
                    obtenerListaPrincipal();  // Se actualiza la lista de ApuntesDetalleTema llamando la Web Api 
                } 
                else {
                    response.text().then(textoError => alert(textoError));
                }
            })
            .catch(error => alert(JSON.stringify(error)) )
            .finally(c=> ocultarLoadingSpinner() );
        } 
    } 

    //=============================================================================================>>>>>>

    cargarVista()
    {
		return new Promise((resolve) => {

			this.cargarHtml({ 
                rutaArchivo: "apuntesDetalleTema/index.html",
                onload: () => {
                    this.cargarBuscadores();
                }
			}); 

			document.addEventListener('DOMContentLoaded', resolve);
		});
    }
    
};

