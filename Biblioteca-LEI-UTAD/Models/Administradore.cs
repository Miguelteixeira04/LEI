using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class Administradore
{
    public string UsernameAdmin { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Bloquear> Bloquears { get; set; } = new List<Bloquear>();

    public virtual ICollection<ValidarRegisto> ValidarRegistos { get; set; } = new List<ValidarRegisto>();
}
