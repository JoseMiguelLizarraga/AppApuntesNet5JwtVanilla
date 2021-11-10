
import { CargaVista } from "../cargaVista.js";
import { mostrarLoadingSpinner, ocultarLoadingSpinner } from "../../funciones/funcionesGenericas.js";
import { configuracionesProyecto } from "../../configuraciones/configuracionesProyecto.js";


export class Login extends CargaVista
{
    constructor()
    {
        super({ validarSesionUsuario: false });

        this.url = configuracionesProyecto.rutaWebApi;

        for (var prop in this) { window[prop] = this[prop]; }                                          // Coloca los atributos de la clase para que esten disponibles desde el html
        Object.getOwnPropertyNames(this.constructor.prototype).forEach(c=> { window[c] = this[c]; });  // Coloca los metodos de la clase para que esten disponibles desde el html

        mostrarLoadingSpinner();
        this.cargarVista();
    }

    destructor() {
        for (var prop in this) { delete window[prop]; }                                             // Remueve los atributos de la clase para que no queden en la ventana
        Object.getOwnPropertyNames(this.constructor.prototype).forEach(c=> { delete window[c]; });  // Remueve los metodos de la clase para que no queden en la ventana
    }

    realizarLogin(evento) 
    {
        evento.preventDefault();
        mostrarLoadingSpinner();
        
        var form = new FormData(evento.target);
        var username = form.get("username");
        var password = form.get("password");
        
        if (username == "") {  return alert("Es necesario colocar el username");  }
        if (password == "") {  return alert("Es necesario colocar el password");  }
        
        fetch(url + "/account/login", {    
            method: "POST",
            headers: {"Accept": "application/json", "Content-Type": "application/json"},
            body: JSON.stringify({"username": username, "password": password})
        })
        .then(response => 
        {
            if(response.ok) 
            {
                response.json().then(data => {
                    //console.log(data);
                    localStorage.setItem("token_app_apuntes", data.token);	
                    //window.location.href = "http://localhost/AppApuntes/apuntesTema";  
					console.log(data.token);

                    window.location.href = "#apuntesDetalleTema";
                    window.location.reload();
                });
            } 
            else {
                response.text().then(textoError => alert(textoError));
            }
        })
        .catch(error => {
            alert(JSON.stringify(error));
        })
        .finally(c=> {
            ocultarLoadingSpinner();
        });
    };


    cargarVista()
    {
        let texto = `
        <div class="container theme-showcase" role="main">

            <form method="POST" enctype="multipart/form-data" onsubmit="realizarLogin(event)">

                <br/><br/>

                <div class="row"> 
                    <div class="col-md-2"> 
                        <div class="form-group"> 

                            <label class="control-label" for="username"> Usuario </label> 
                            <input type="text" name="username" class="form-control" /> 

                        </div> 
                    </div> 
                    <div class="col-md-2"> 
                        <div class="form-group"> 

                            <label class="control-label" for="password"> Password </label> 
                            <input type="password" name="password" class="form-control" /> 

                        </div> 
                    </div> 
                    <div class="col-md-12">
                        <div class="form-group">   
                            <button type="submit" class="btn btn-success "> Entrar </button>
                        </div>
                    </div>
                </div> 

            </form>

        </div>
        `;

        this.cargarHtml({ 
            textoHtml: texto,
            onload: () => {  ocultarLoadingSpinner();  }
        }); 

        //this.cargarHtml({textoHtml: "aaaaaaaaaaaa"});
    }
    
};

