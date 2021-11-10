
//import { mostrarLoadingSpinner, ocultarLoadingSpinner } from "../../funciones/funcionesGenericas.js";
import { configuracionesProyecto } from '../../configuraciones/configuracionesProyecto.js';
import { CargaVista } from "../cargaVista.js";


export class ApuntesCategoria extends CargaVista
{
    constructor()
    {
        super();

        this.url = configuracionesProyecto.rutaWebApi;
        
        this.operacion = "";            // La primera operacion es listar 
        this.apuntesCategoria = {}; 
        this.paginaActual = 0;          // Atributo opcional en caso de usar Paginación 
        this.totalPaginas = 0;          // Atributo opcional en caso de usar Paginación 

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

    //=============================================================================================>>>>>>

    obtenerConsultaBuscador()
	{ 
		var consulta = {}; 
		var filtroBusqueda = document.getElementById("filtro-busqueda"); 

		if (filtroBusqueda != null) 
		{ 
			Array.from(filtroBusqueda.querySelectorAll("input, select")) 
			.filter(c=> c.getAttribute("nombre") != null && ! ["", null].includes(c.value))  // Filtra por nombre y value no nulos 
			.forEach(c=> 
			{ 
				if (c.hasAttribute("fecha"))  // En caso de ser un datepicker propio, usa la fecha en formato yyyy-MM-dd  
					consulta[c.getAttribute("nombre")] = c.getAttribute("fecha"); 
				else 
					consulta[c.getAttribute("nombre")] = c.value; 
			}); 
		} 

		return consulta; 
	} 

	async obtenerListaPrincipal()  // Obtener lista de ApuntesCategoria de la Web Api 
	{ 
		var registrosPorPagina = 10; 
		var inicio = paginaActual * registrosPorPagina; 

		var consulta = obtenerConsultaBuscador(); 
		consulta["start"] = inicio;                 // Datos para paginacion 
		consulta["length"] = registrosPorPagina;    // Datos para paginacion 

		var parametros = Object.entries(consulta).map(c=> c.join("=")).join("&"); 

		//mostrarLoadingSpinner(); 

		await fetch(url + "/ApuntesCategoria/llenarDataTable?" + parametros, { 
			method: "GET",
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
						
						fila.insertCell(2).innerHTML = (c.logo != null && c.tipoLogo != null) ? 
							`<img src='${c.tipoLogo}, ${c.logo}' />` 
							: "";

						fila.insertCell(3).innerHTML = "<a href='' onclick='editar(event, " + c.id + ")'> Editar </a>"; 
						fila.insertCell(4).innerHTML = "<a href='javascript:;' onclick='eliminar(" + c.id + ")'> Eliminar </a>"; 

					}); 
				}); 
			} 
			else { 
				response.text().then(textoError => 
					alert((response.status == 400 || response.status == 500 && ! [null, ""].includes(textoError)) ? 
					textoError : `Error ${response.status}`) 
				); 
			} 
		}) 
		.catch(error => { 
			alert("Se encontro un error"); 
		}) 
		.finally(c=> { 
			//ocultarLoadingSpinner(); 
		}); 
	} 

	//obtenerListaPrincipal(); 

	resetearPaginacion()
	{ 
		paginaActual = 0; 
		totalPaginas = 0; 
	} 

	construirObjeto() // Colocar los valores del objeto de tipo ApuntesCategoria en el html 
	{ 
		Object.entries(apuntesCategoria).filter(([, valorAtributo]) => ! Array.isArray(valorAtributo))  // No incluye referencias cruzadas en caso de que las tenga 
		.forEach(([nombreAtributo, valorAtributo]) => 
		{ 
			var elementoHTML = document.querySelector(`.apuntesCategoria [name="${nombreAtributo}"]`); 

			if (elementoHTML != null) 
			{ 
				elementoHTML.value = valorAtributo;  // Coloca los valores del objeto en cada elemento del form 
			} 
		}); 

	} 

	crearNuevo() // Iniciar creación de nuevo ApuntesCategoria 
	{ 
		apuntesCategoria = { 
			titulo: "", 
		}; 

		operacion = "crear"; 
		construirObjeto(); 
	} 

	validar(apuntesCategoria) // Validar ApuntesCategoria 
	{ 

		// Se validan los atributos de la entidad 

		if (["", null].includes(apuntesCategoria.titulo)) return "El campo titulo no posee un valor"; 

		return ""; 
	} 

	async guardar(evento)  // Guardar y Actualizar ApuntesCategoria en la Web Api 
	{ 
		evento.preventDefault(); 
		var validacion = validar(apuntesCategoria); 
		if (validacion != "") return alert(validacion); 

		var metodoEnvio = (operacion == "crear") ? "POST" : "PUT"; 

		//mostrarLoadingSpinner(); 

		await fetch(url + "/ApuntesCategoria", { 
			method: metodoEnvio, 
			headers: {"Content-Type": "application/json", "Authorization": "Bearer " + localStorage.getItem("token_app_apuntes") },  // headers: {"Authorization": "Bearer ejemploToken"} 
			body: JSON.stringify(apuntesCategoria) 
		}) 
		.then(response => 
		{ 
			if(response.ok) 
			{ 
				obtenerListaPrincipal();               // Se actualiza la lista de ApuntesCategoria llamando la Web Api 
				$("#modalCrearEditar").modal("hide");  // Cierra el modal 
			} 
			else { 
				response.text().then(textoError => 
					alert((response.status == 400 || response.status == 500 && ! [null, ""].includes(textoError)) ? 
					textoError : `Error ${response.status}`) 
				); 
			} 
		}) 
		.catch(error => { 
			alert("Se encontro un error"); 
		}) 
		.finally(c=> { 
			//ocultarLoadingSpinner(); 
		}); 
	} 

	async editar(evento, id)  // Obtener ApuntesCategoria de la Web Api por su id 
	{ 
		evento.preventDefault(); 

		//mostrarLoadingSpinner(); 

		await fetch(url + "/ApuntesCategoria/" + id, { 
			method: "GET", 
            headers: {"Authorization": "Bearer " + localStorage.getItem("token_app_apuntes") }
		}) 
		.then(response => 
		{ 
			if(response.ok) 
			{ 
				response.json().then(data => 
				{ 
					operacion = "editar"; 
					apuntesCategoria = data; 

					construirObjeto(); 
					$("#modalCrearEditar").modal("show"); 
				}); 
			} 
			else { 
				response.text().then(textoError => 
					alert((response.status == 400 || response.status == 500 && ! [null, ""].includes(textoError)) ? 
					textoError : `Error ${response.status}`) 
				); 
			} 
		}) 
		.catch(error => { 
			alert("Se encontro un error"); 
		}) 
		.finally(c=> { 
			//ocultarLoadingSpinner(); 
		}); 
	} 

	async eliminar(id)  // Eliminar ApuntesCategoria por su id 
	{ 
		var opcionConfirm = confirm("Desea realmente eliminar este registro"); 
		if (opcionConfirm == true) 
		{ 
			//mostrarLoadingSpinner(); 

			await fetch(url + "/ApuntesCategoria/" + id, { 
				method: "DELETE", 
				headers: {"Authorization": "Bearer " + localStorage.getItem("token_app_apuntes") } 
			}) 
			.then(response => 
			{ 
				if(response.ok) 
				{ 
					obtenerListaPrincipal();  // Se actualiza la lista de ApuntesCategoria llamando la Web Api 
				} 
				else { 
					response.text().then(textoError => 
						alert((response.status == 400 || response.status == 500 && ! [null, ""].includes(textoError)) ? 
						textoError : `Error ${response.status}`) 
					); 
				} 
			}) 
			.catch(error => { 
				alert("Se encontro un error"); 
			}) 
			.finally(c=> { 
				//ocultarLoadingSpinner(); 
			}); 
		} 
	} 

    //=============================================================================================>>>>>>
    
    cargarVista()
    {
        //this.cargarHtml({textoHtml: "aaaaaaaaaaaa"});

		return new Promise((resolve) => {

			this.cargarHtml({ 
				rutaArchivo: "apuntesCategoria/index.html", 
				// onload: () => { 
				//     $(".flexslider").flexslider();  // Carga el banner con las fotos
				// } 
			}); 

			document.addEventListener('DOMContentLoaded', resolve);
		});
    }
    
};

