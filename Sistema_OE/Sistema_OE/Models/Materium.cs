using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Materium
{
    public int NumMateria { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Impartematerium> Impartemateria { get; set; } = new List<Impartematerium>();

    public virtual ICollection<Materiacursada> Materiacursada { get; set; } = new List<Materiacursada>();
}
