

import {colocarHead} from './layouts.js';

export default class ColocarTemplate 
{
    constructor() {
        this.validarSesionIniciada();
        this.colocarElementosHtml();
    }

    colocarElementosHtml()
    {
        colocarHead();  // Coloca las importaciones en el head del html
    }

    validarSesionIniciada()
    {
        if (localStorage.getItem("token_app_apuntes") == null )
        {
            window.location.href = "../"; 
        }
    }
}