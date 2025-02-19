using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class EntregasAtraso
{
    public int IdEntregas { get; set; }

    public int IdRequisicao { get; set; }

    public virtual Requisicao IdRequisicaoNavigation { get; set; } = null!;
}
