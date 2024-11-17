using System;
using System.Collections.Generic;

namespace Proyecto2.Models;

public partial class Editoriale
{
    public int Nit { get; set; }

    public string Nombres { get; set; }

    public string Telefono { get; set; }

    public string Direccion { get; set; }

    public string Email { get; set; }

    public string Sitioweb { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
