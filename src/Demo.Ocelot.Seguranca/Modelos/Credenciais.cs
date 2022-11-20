using System.ComponentModel.DataAnnotations;

namespace Demo.Ocelot.Seguranca.Modelos;

/// <summary>
/// Class de credenciais do usuário
/// </summary>
public class Credenciais
{
    [Required]
    public string? Usuario { get; set; }

    [Required]
    public string? Senha { get; set; }
}