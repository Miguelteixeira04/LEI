using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class Leitore
{
    public string UsernameLei { get; set; } = null!;

    public string Morada { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Requisicao> Requisicaos { get; set; } = new List<Requisicao>();

    public virtual Autenticado UsernameLeiNavigation { get; set; } = null!;
}
