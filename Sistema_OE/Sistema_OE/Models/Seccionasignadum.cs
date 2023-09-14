using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Seccionasignadum
{
    public int IdseccionAsignada { get; set; }

    public int ProfesorCedula { get; set; }

    public int NumSeccion { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Seccion NumSeccionNavigation { get; set; } = null!;

    public virtual Profesor ProfesorCedulaNavigation { get; set; } = null!;
}
