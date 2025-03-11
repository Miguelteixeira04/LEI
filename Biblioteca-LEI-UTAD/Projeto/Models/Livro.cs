using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class Livro
{
    public long Isbn { get; set; }

    public decimal Preco { get; set; }

    public string Titulo { get; set; } = null!;

    public int NExemplares { get; set; }

    public string Genero { get; set; } = null!;

    public string UsernameBibAdd { get; set; } = null!;

    public int IdAutor { get; set; }

    public int IdBiblioteca { get; set; }

    public virtual Autor IdAutorNavigation { get; set; } = null!;

    public virtual Biblioteca IdBibliotecaNavigation { get; set; } = null!;

    public virtual ICollection<Requisicao> Requisicaos { get; set; } = new List<Requisicao>();

    public virtual Bibliotecario UsernameBibAddNavigation { get; set; } = null!;
}
