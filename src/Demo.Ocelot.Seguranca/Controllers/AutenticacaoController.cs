using Demo.Ocelot.Seguranca.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace Demo.Ocelot.Seguranca.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class AutenticacaoController : ControllerBase
{
    public AutenticacaoController(ILogger<AutenticacaoController> logger)
    {
    }

    /// <summary>
    /// Gerar o Token de Autenticação (JWT) do usuário
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login", Name = nameof(Login))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> Login(Credenciais credentials)
    {
        if (!IsAdmin(credentials) && !IsUser(credentials))
            return Unauthorized();

        var chaveSecreta = "Minha-Chave-Secreta-de-Seguranca-Anonima";
        var chaveSeguranca = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));

        var jwt = new JwtSecurityToken
        (
            claims: ConstruirClaims(credentials),
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: new SigningCredentials(chaveSeguranca, SecurityAlgorithms.HmacSha512)
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        var resultado = new CredenciaisResposta { TokenAcesso = token };
        return Ok(resultado);
    }

    /// <summary>
    /// Define as claims do tipo Usuário ou Administrador
    /// </summary>
    /// <param name="credenciais"></param>
    /// <returns></returns>
    private static IEnumerable<Claim> ConstruirClaims(Credenciais credenciais)
    {
        return new[]
        {
            new Claim("tipoUsuario", IsAdmin(credenciais) ? "administrador" : "usuario")
        };
    }

    /// <summary>
    /// Método verifica se é um usuário comum
    /// </summary>
    /// <param name="credenciais"></param>
    /// <returns></returns>
    private static bool IsUser(Credenciais credenciais)
    {
        return credenciais.Usuario == "user" && credenciais.Senha == "user";
    }

    /// <summary>
    /// Método verifica se é um administrador
    /// </summary>
    /// <param name="credenciais"></param>
    /// <returns></returns>
    private static bool IsAdmin(Credenciais credenciais)
    {
        return credenciais.Usuario == "admin" && credenciais.Senha == "admin";
    }
}