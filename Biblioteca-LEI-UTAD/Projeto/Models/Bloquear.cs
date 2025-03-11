using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class Bloquear
{
    public int IdBloqueio { get; set; }

    public int IdMotivoBloq { get; set; }

    public string UsernameAdmin { get; set; } = null!;

    public string Username { get; set; } = null!;

    public DateTime DataBloqueio { get; set; }

    public bool Bloqueado { get; set; }

    public virtual MotivoBloq IdMotivoBloqNavigation { get; set; } = null!;

    public virtual Administradore UsernameAdminNavigation { get; set; } = null!;

    public virtual Autenticado UsernameNavigation { get; set; } = null!;
}
