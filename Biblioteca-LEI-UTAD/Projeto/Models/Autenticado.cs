using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class Autenticado
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Contacto { get; set; }

    public string? Contacto2 { get; set; }

    public string? Contacto3 { get; set; }

    public string Nome { get; set; } = null!;

    public virtual Bibliotecario? Bibliotecario { get; set; }

    public virtual ICollection<Bloquear> Bloquears { get; set; } = new List<Bloquear>();

    public virtual Leitore? Leitore { get; set; }
}
