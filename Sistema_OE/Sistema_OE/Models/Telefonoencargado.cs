using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Telefonoencargado
{
    public int IdTelefono { get; set; }

    public int Numero { get; set; }

    public string Tipo { get; set; } = null!;

    public int NumExpediente { get; set; }

    public virtual Expediente NumExpedienteNavigation { get; set; } = null!;
}
