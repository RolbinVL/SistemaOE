using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Materiacursada
{
    public int IdMateriaCursadas { get; set; }

    public int NumMateria { get; set; }

    public int Estudiante { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Estudiante EstudianteNavigation { get; set; } = null!;

    public virtual Materium NumMateriaNavigation { get; set; } = null!;
}
