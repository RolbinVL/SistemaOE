using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Administrativo
{
    public int IdAdministrativo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Domicilio { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual ICollection<Telefonoadmin> Telefonoadmins { get; set; } = new List<Telefonoadmin>();
}
