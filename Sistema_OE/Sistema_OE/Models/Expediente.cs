using System;
using System.Collections.Generic;

namespace Sistema_OE.Models;

public partial class Expediente
{
    public int NumExpediente { get; set; }

    public int Estudiante { get; set; }

    public int CedulaEncargado { get; set; }

    public string EncargadoLegal { get; set; } = null!;

    public string EmailEncargado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public virtual Estudiante EstudianteNavigation { get; set; } = null!;

    public virtual ICollection<Telefonoencargado> Telefonoencargados { get; set; } = new List<Telefonoencargado>();
}
