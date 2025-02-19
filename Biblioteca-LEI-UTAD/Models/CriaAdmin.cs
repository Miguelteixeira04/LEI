using System;
using System.Collections.Generic;

namespace Trabalho.Models;

public partial class CriaAdmin
{
    public string UsernameAdmin { get; set; } = null!;

    public string UsernameNovoAdmin { get; set; } = null!;

    public virtual Administradore UsernameAdminNavigation { get; set; } = null!;

    public virtual Administradore UsernameNovoAdminNavigation { get; set; } = null!;
}
