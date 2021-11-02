

const mostrarLoadingSpinner = () =>
{
	document.querySelector("#loading-spinner").style.display = "block";
}

const ocultarLoadingSpinner = () =>
{
	document.querySelector("#loading-spinner").style.display = "none";
}

export { mostrarLoadingSpinner, ocultarLoadingSpinner }