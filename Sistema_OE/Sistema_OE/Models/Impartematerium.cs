using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Impartematerium
{
    public int IdImparteMateria { get; set; }

    public int ProfesorCedula { get; set; }

    public int NumMateria { get; set; }

    public DateTime Horario { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Materium NumMateriaNavigation { get; set; } = null!;

    public virtual Profesor ProfesorCedulaNavigation { get; set; } = null!;
}
