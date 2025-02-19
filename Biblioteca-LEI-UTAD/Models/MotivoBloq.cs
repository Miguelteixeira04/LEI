using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class MotivoBloq
{
    public int IdMotivoBloq { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Bloquear> Bloquears { get; set; } = new List<Bloquear>();
}
