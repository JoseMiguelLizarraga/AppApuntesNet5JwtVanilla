
const nombreProbando = "Juana";

const retornarJson = () =>
{
    return {
      "id": 123
    };
}
  
const retornarJsonArray = () =>
{
    return [
      {"id": 123}, {"id": 234}, {"id": 345}
    ];
}
  
export { nombreProbando, retornarJson, retornarJsonArray };