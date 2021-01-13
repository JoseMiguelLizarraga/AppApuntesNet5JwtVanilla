

const cerrarSesion = (evento) => 
{
    evento.preventDefault();
    localStorage.removeItem("token_app_apuntes");
    window.location.href = "../";  
}

window.cerrarSesion = cerrarSesion;

