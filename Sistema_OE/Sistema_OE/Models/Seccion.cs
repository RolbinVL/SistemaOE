using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Seccion
{
    public int NumSeccion { get; set; }

    public string CantidadEstudiante { get; set; } = null!;

    public virtual ICollection<Estudianteseccionanio> Estudianteseccionanios { get; set; } = new List<Estudianteseccionanio>();

    public virtual ICollection<Seccionasignadum> Seccionasignada { get; set; } = new List<Seccionasignadum>();
}
