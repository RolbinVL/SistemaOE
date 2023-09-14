using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Estudiante
{
    public int Cedula { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Genero { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Domicilio { get; set; } = null!;

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual ICollection<Estudianteseccionanio> Estudianteseccionanios { get; set; } = new List<Estudianteseccionanio>();

    public virtual Expediente? Expediente { get; set; }

    public virtual ICollection<Materiacursada> Materiacursada { get; set; } = new List<Materiacursada>();
}
