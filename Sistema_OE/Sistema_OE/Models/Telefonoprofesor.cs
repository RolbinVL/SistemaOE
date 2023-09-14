using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Telefonoprofesor
{
    public int IdTelefono { get; set; }

    public int Numero { get; set; }

    public string Tipo { get; set; } = null!;

    public int ProfesorCedula { get; set; }

    public virtual Profesor ProfesorCedulaNavigation { get; set; } = null!;
}
