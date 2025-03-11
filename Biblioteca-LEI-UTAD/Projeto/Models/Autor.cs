using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class Autor
{
    public int IdAutor { get; set; }

    public string NameAutor { get; set; } = null!;

    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
