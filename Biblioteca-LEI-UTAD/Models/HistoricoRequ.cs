using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class HistoricoRequ
{
    public int IdHistorico { get; set; }

    public int IdRequisicao { get; set; }

    public virtual Requisicao IdRequisicaoNavigation { get; set; } = null!;
}
