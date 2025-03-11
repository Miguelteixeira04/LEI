using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class Requisicao
{
    public int IdRequisicao { get; set; }

    public string? UsernameBib { get; set; }

    public string UsernameLei { get; set; } = null!;

    public long Isbn { get; set; }

    public DateTime DataRequisicao { get; set; }

    public DateTime? DataDevolucao { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<EntregasAtraso> EntregasAtrasos { get; set; } = new List<EntregasAtraso>();

    public virtual ICollection<HistoricoRequ> HistoricoRequs { get; set; } = new List<HistoricoRequ>();

    public virtual Livro IsbnNavigation { get; set; } = null!;

    public virtual Bibliotecario? UsernameBibNavigation { get; set; }

    public virtual Leitore UsernameLeiNavigation { get; set; } = null!;
}
