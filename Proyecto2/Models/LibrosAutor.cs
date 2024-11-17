using System;
using System.Collections.Generic;

namespace Proyecto2.Models;

public partial class LibrosAutor
{
    public int IdLibroAutor { get; set; }

    public int? IdAutor { get; set; }

    public string Isbn { get; set; }

    public virtual Autor IdAutorNavigation { get; set; }

    public virtual Libro IsbnNavigation { get; set; }
}
