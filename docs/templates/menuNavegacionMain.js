

export const menuNavegacionMain = `
<nav class="navbar navbar-inverse" role="navigation">  
    <div class="container">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="#"> AppApuntesSPA </a>
        </div>
        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
            <ul class="nav navbar-nav navbar-right">	
        
                <li> <a href="#" link="#inicio"> Inicio </a></li>
                <li> <a href="#" link="#apuntesTema"> Ver por Temas </a> </li>  
                <li> <a href="#" link="#apuntesDetalleTema"> Ver por detalles de tema </a> </li>   
                <li> <a href="#" onclick="cerrarSesion(event)"> Cerrar Sesión </a> </li>  					                
            </ul>
        </div>
    </div>
</nav> 
`;