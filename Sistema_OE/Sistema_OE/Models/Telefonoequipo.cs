using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Telefonoequipo
{
    public int IdTelefono { get; set; }

    public int Numero { get; set; }

    public string Tipo { get; set; } = null!;

    public int EquipoInterdiciplinario { get; set; }

    public virtual Equipointerdiciplinario EquipoInterdiciplinarioNavigation { get; set; } = null!;
}
