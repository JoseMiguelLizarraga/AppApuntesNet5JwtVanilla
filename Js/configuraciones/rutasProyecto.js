
import { Inicio } from "../componentes/inicio/inicio.js";
import { ApuntesTema } from "../componentes/apuntesTema/apuntesTema.js";
import { ApuntesDetalleTema } from "../componentes/apuntesDetalleTema/apuntesDetalleTema.js";
import { Login } from "../componentes/login/login.js";
import { ApuntesCategoria } from "../componentes/apuntesCategoria/apuntesCategoria.js";

// El html del menu que llama a estas rutas esta en templates/menuNavegacionMain.js

export const rutasProyecto = [
    {nombre: "inicio", componente: Inicio, default: true},
    {nombre: "login", componente: Login},
    {nombre: "apuntesCategoria", componente: ApuntesCategoria},
    {nombre: "apuntesTema", componente: ApuntesTema},
    {nombre: "apuntesDetalleTema", componente: ApuntesDetalleTema}
];





