using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Equipointerdiciplinario
{
    public int Cedula { get; set; }

    public string Nombre { get; set; } = null!;

    public string Especialidad { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Domicilio { get; set; } = null!;

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual ICollection<Telefonoequipo> Telefonoequipos { get; set; } = new List<Telefonoequipo>();
}
