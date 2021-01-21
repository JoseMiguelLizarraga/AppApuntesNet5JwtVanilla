
import { CargaVista } from "../cargaVista.js";
//import "../../js/jquery.flexslider.js";

export class Inicio extends CargaVista
{

    constructor()
    {
        super();
        this.cargarVista();
    }

    destructor() {
        Object.getOwnPropertyNames(Inicio.prototype).forEach(c=> { window[c] = null; });  // Remueve los metodos y variables para que no queden en memoria
    }

    cargarVista()
    {
        this.cargarHtml({ rutaArchivo: "inicio/index.html" }); 

        //this.cargarHtml({textoHtml: "aaaaaaaaaaaa"});
    }
    
};

