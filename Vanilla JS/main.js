
import { nombreProbando, retornarJson, retornarJsonArray } from './modules/probando.js';

//import * as myModule from '/modules/my-module.js';
//import { mostrarLoadingSpinner, ocultarLoadingSpinner } from './modules/funcionesGenericas.js';
import * as funcionesGenericas from './modules/funcionesGenericas.js';

let ejemploJson = retornarJson();
let ejemploJsonArray = retornarJsonArray();

console.log(nombreProbando);
console.log(ejemploJson);
console.log(ejemploJsonArray);

document.querySelector("#probando").innerHTML = JSON.stringify(ejemploJsonArray);


funcionesGenericas.mostrarLoadingSpinner();