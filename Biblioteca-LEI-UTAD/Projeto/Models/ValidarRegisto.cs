using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class ValidarRegisto
{
    public int IdValid { get; set; }

    public string? UsernameAdmin { get; set; }   // Campo vazio ao invés de NULL

    public string UsernameBib { get; set; } = null!;

    public DateTime DataValid { get; set; } = new DateTime(1901, 1, 1);  // Data válida mínima para SMALLDATETIME

    public bool Validado { get; set; }

    public virtual Administradore? UsernameAdminNavigation { get; set; } = null!;

    public virtual Bibliotecario UsernameBibNavigation { get; set; } = null!;
}
