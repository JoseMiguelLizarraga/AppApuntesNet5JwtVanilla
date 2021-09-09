
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using AppApuntesNet5.Models;
using AppApuntesNet5.Dto;

namespace WebApiPaises.Controllers
{
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly UserManager<UsuarioAutenticado> _userManager;
        private readonly SignInManager<UsuarioAutenticado> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<UsuarioAutenticado> userManager,
            SignInManager<UsuarioAutenticado> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._configuration = configuration;
        }

        [Route("Account/Probando")]  // https://localhost:44322/Account/Probando
        [HttpGet]
        public IActionResult Probando()
        {
            return Ok("Probando");
        }

        // Accion para generar un token en base a un usuario con un email y un password
        [Route("Account/Create")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] LoginDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new UsuarioAutenticado { UserName = model.username, Email = model.username };
                    var result = await _userManager.CreateAsync(user, model.password);

                    if (result.Succeeded)
                    {
                        return BuildToken(model);  // { token: "eyJhbGciOiJIUzI....", expiration: "2021-01-12"​ }
                    }
                    else
                    {
                        return BadRequest("Username or password invalid");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Se encontró el siguiente error: " + ex);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userInfo.username, userInfo.password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildToken(userInfo);  // { token: "eyJhbGciOiJIUzI....", expiration: "2021-01-12"​ }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private IActionResult BuildToken(LoginDto userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.username),
                new Claim("miValor", "Lo que yo quiera"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Pais", "Argentina"),
                new Claim("Admin", "Y")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Llave_super_secreta"]));  // Esta es una variable de ambiente guardada en  launchSettings.json
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);  // Asi el token dura 1 hora

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "yourdomain.com",
               audience: "yourdomain.com",
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration
            });

        }
    }
}

/*

fetch("https://localhost:44322/Account/Create", {
	method: "POST",
	headers: {"Accept": "application/json", "Content-Type": "application/json"},
	body: JSON.stringify({
      "username": "ejemplo@gmail.com",
      "password": "MyN3wP@ssw0rd"
    })
})
.then(response => 
{
	if(response.ok) 
	{
		response.json().then(data => {
			console.log(data);
		});
	} 
	else {
		response.text().then(textoError => alert(textoError));
	}
});


//============================================================================>>>>>

fetch("https://localhost:44322/login", {
	method: "POST",
	headers: {"Accept": "application/json", "Content-Type": "application/json"},
	body: JSON.stringify({
      "username": "ejemplo@gmail.com",
      "password": "MyN3wP@ssw0rd"
    })
})
.then(response => 
{
	if(response.ok) 
	{
		response.json().then(data => {
			console.log(data);
		});
	} 
	else {
		response.text().then(textoError => alert(textoError));
	}
});


*/