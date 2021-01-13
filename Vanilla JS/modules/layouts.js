

const colocarHead = () =>
{
    var textoHead = `
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">

        <!--<link rel="stylesheet" href="css/bootstrap-submenu.min.css">--> <!-- Esto es para las sub subopciones -->

        <!-- =================================================================== -->
        <!-- Usar calendario -->
        
        <!-- <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/flick/jquery-ui.min.css">  -->
        <link rel="stylesheet" href="../css/jquery-ui.min.css">
        
        <!-- =================================================================== -->
    `;

    document.head.innerHTML += textoHead;

    //=========================================================================>>>>>>

    var textoHeader = `
    <div style="display: none" id="loading-spinner"></div> <!-- Imagen de carga de datos -->


    <!-- Menú de Navegación -->
    
    <nav id="menu-navegacion" class="navbar navbar-inverse" role="navigation">  <!-- class="navbar navbar-inverse navbar-fixed-top" -->
        <div class="container">
        <!--===================================-->  
            <!--  Este Menu queda escondido y aparece solo cuando la pantalla es muy pequeña...son las Barritas -->
        
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="index.php"> AppApuntes </a>
            </div>
        <!--===================================-->		
            
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav navbar-right">				
                    <li> <a href="http://localhost/AppApuntes/apuntesTema/"> Ver por Temas </a> </li>  
                    <li> <a href="http://localhost/AppApuntes/apuntesDetalleTema/"> Ver por detalles de tema </a> </li>   
                    <li> <a href="#" onclick="cerrarSesion(event)"> Cerrar Sesión </a> </li>  					
    
                    <!--
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown"> Opciones <b class="caret"></b></a>
                        <ul class="dropdown-menu">						
                            <li> <a href="cerrar_sesion"> Cerrar Sesion </a> </li> 
                        </ul>
                    </li>      
                    -->
                    
                </ul>
            </div>
        </div>
    </nav>
    
    <!-- Fin Menú de Navegación -->
    <!-- =================================================================== -->
    
    <div class="container theme-showcase" role="main">
    `;
   
    document.getElementsByTagName('cabecera')[0].innerHTML += textoHeader;

    //=========================================================================>>>>>>

    var textoPiePagina = `
    </div>  <!-- Fin div  container theme-showcase -->
    `;

    document.getElementsByTagName('piePagina')[0].innerHTML += textoPiePagina;
}

export { colocarHead };