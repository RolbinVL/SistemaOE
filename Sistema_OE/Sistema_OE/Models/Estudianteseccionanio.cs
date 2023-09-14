using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Estudianteseccionanio
{
    public int Id { get; set; }

    public int CedulaEstudiante { get; set; }

    public int NumSeccion { get; set; }

    public int Ano { get; set; }

    public virtual Estudiante CedulaEstudianteNavigation { get; set; } = null!;

    public virtual Seccion NumSeccionNavigation { get; set; } = null!;
}
