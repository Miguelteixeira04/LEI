using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class Biblioteca
{
    public int IdBiblioteca { get; set; }

    public TimeOnly HorarioI { get; set; }

    public TimeOnly HorarioF { get; set; }

    public string Localizacao { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public long Telefone { get; set; }

    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
