using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Citum
{
    public int NumCita { get; set; }

    public int Estudiante { get; set; }

    public int Administrativo { get; set; }

    public int EquipoInterdiciplinario { get; set; }

    public DateTime FechaAsignada { get; set; }

    public string Motivo { get; set; } = null!;

    public int Tiempo { get; set; }

    public string Tipo { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? Observacion { get; set; }

    public virtual Administrativo AdministrativoNavigation { get; set; } = null!;

    public virtual Equipointerdiciplinario EquipoInterdiciplinarioNavigation { get; set; } = null!;

    public virtual Estudiante EstudianteNavigation { get; set; } = null!;
}
