namespace Demo.Ocelot.Gateway.Modelos;

public class Usuario
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public List<Postagem> Posts { get; set; } = new List<Postagem>();
}