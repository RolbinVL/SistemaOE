using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Telefonoadmin
{
    public int IdTelefono { get; set; }

    public int Numero { get; set; }

    public string Tipo { get; set; } = null!;

    public int IdAdministrativo { get; set; }

    public virtual Administrativo IdAdministrativoNavigation { get; set; } = null!;
}
