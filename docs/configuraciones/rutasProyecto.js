
import { Inicio } from "../componentes/inicio/inicio.js";
import { ApuntesTema } from "../componentes/apuntesTema/apuntesTema.js";
import { ApuntesDetalleTema } from "../componentes/apuntesDetalleTema/apuntesDetalleTema.js";
import { Login } from "../componentes/login/login.js";


export const rutasProyecto = [
    {nombre: "inicio", componente: Inicio, default: true},
    {nombre: "login", componente: Login},
    {nombre: "apuntesTema", componente: ApuntesTema},
    {nombre: "apuntesDetalleTema", componente: ApuntesDetalleTema}
];





