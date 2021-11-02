
import { configuracionesProyecto } from '../configuraciones/configuracionesProyecto.js';
import {mostrarLoadingSpinner, ocultarLoadingSpinner} from '../funciones/funcionesGenericas.js';
import { menuNavegacionMain } from '../templates/menuNavegacionMain.js';


export class CargaVista
{
    constructor(json = { validarSesionUsuario: true })
    {
        this.sesionValidada = false;

        if (json.validarSesionUsuario == true) 
        {
            this.sesionValidada = this.validarSesionUsuario();
            window["cerrarSesion"] = this.cerrarSesion;  // Asi el metodo cerrarSesion esta disponible desde el html y la consola
        }

        //this.url = configuracionesProyecto.rutaWebApi;
    }

    validarSesionUsuario()
    {
        if ([null, "null", ""].includes(localStorage.getItem("token_app_apuntes"))) 
        { 
            window.location.href = "#login";
            window.location.reload();
            throw "Usted no ha iniciado sesión";
        }
        return true;
    }

	cerrarSesion(event)
	{
		event.preventDefault();
		localStorage.removeItem("token_app_apuntes");
        window.location.href = "#login";
        window.location.reload();
	}

    revisarCargaImagenes()
    {
        if (Array.from(document.getElementsByTagName("img")).length > 0)  // Si el documento tiene imagenes
        {
            Promise.all(Array.from(document.images).map(img => 
            {
                if (img.complete) return Promise.resolve(img.naturalHeight !== 0);

                return new Promise(resolve => {
                    img.addEventListener("load", () => resolve(true));
                    img.addEventListener("error", () => resolve(false));
                });
            }))
            .then(results => {
                if (results.every(res => res)) 
                {
                    //console.log("Todas las imagenes han sido cargadas");
                    ocultarLoadingSpinner();
                }
                else {
                    //console.log("Algunas imagenes fallaron al cargar. Termino el proceso de carga");
                }
            });
        }
        else 
        {
            ocultarLoadingSpinner();
        }
    }

    desplegarMenuNavegacion()
    {
        let etiquetaMenuNavegacion = document.getElementsByTagName("menu-navegacion-app")[0];
 
        if (this.sesionValidada && etiquetaMenuNavegacion.innerHTML.length == 0) {
            etiquetaMenuNavegacion.innerHTML = menuNavegacionMain;
        }
    }

    cargarHtml(json = {})
    {
        this.desplegarMenuNavegacion();
        let etiquetaContenido = document.getElementsByTagName("contenido-app")[0];

        mostrarLoadingSpinner();

        if (json.rutaArchivo != null) 
        {
            fetch(`${configuracionesProyecto.rutaComponentes}${json.rutaArchivo}`)
            .then(response => 
            {
                if (! response.ok) {
                    throw new Error(`Error ${response.status} al incluir el archivo ubicado en ${configuracionesProyecto.rutaComponentes}${json.rutaArchivo}`);
                }
                return response.text();
            })
            .then(contenido => 
            {
                etiquetaContenido.innerHTML = contenido;

                this.revisarCargaImagenes();

                if (json.onload != null && typeof json.onload == "function") {  json.onload();  }  // Si desea ejecutar alguna funcion despues de cargar el contenido
            })
            .finally(c=> {
                //ocultarLoadingSpinner();
            });
        }
        if (json.textoHtml != null) 
        {
            mostrarLoadingSpinner();
            etiquetaContenido.innerHTML = json.textoHtml;

            if (json.onload != null && typeof json.onload == "function") {  json.onload();  }  // Si desea ejecutar alguna funcion despues de cargar el contenido
            
            this.revisarCargaImagenes();
            //ocultarLoadingSpinner();
        }

    }
    
}