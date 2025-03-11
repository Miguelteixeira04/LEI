using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class Bibliotecario
{
    public string UsernameBib { get; set; } = null!;

    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();

    public virtual ICollection<Requisicao> Requisicaos { get; set; } = new List<Requisicao>();

    public virtual Autenticado UsernameBibNavigation { get; set; } = null!;

    public virtual ICollection<ValidarRegisto> ValidarRegistos { get; set; } = new List<ValidarRegisto>();
}
