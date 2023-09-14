using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Profesor
{
    public int Cedula { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Domicilio { get; set; } = null!;

    public virtual ICollection<Impartematerium> Impartemateria { get; set; } = new List<Impartematerium>();

    public virtual ICollection<Seccionasignadum> Seccionasignada { get; set; } = new List<Seccionasignadum>();

    public virtual ICollection<Telefonoprofesor> Telefonoprofesors { get; set; } = new List<Telefonoprofesor>();
}
