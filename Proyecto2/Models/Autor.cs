using System;
using System.Collections.Generic;

namespace Proyecto2.Models;

public partial class Autor
{
    public int IdAutor { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public string Nacionalidad { get; set; }

    public virtual ICollection<LibrosAutor> LibrosAutors { get; set; } = new List<LibrosAutor>();
}
