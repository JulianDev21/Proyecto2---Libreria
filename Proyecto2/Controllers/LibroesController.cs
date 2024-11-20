using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto2.Models;

namespace Proyecto2.Controllers
{
    public class LibroesController : Controller
    {
        private readonly LibreriaDbContext _context;

        public LibroesController(LibreriaDbContext context)
        {
            _context = context;
        }

        // GET: Libroes
        public async Task<IActionResult> Index()
        {
            var libreriaDbContext = _context.Libros.Include(l => l.CodigoCategoriaNavigation).Include(l => l.NitEditorialNavigation);
            return View(await libreriaDbContext.ToListAsync());
        }

        // GET: Libroes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.CodigoCategoriaNavigation)
                .Include(l => l.NitEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libroes/Create
        public IActionResult Create()
        {
            // Concatenar "CodigoCategoria - NombreCategoria" para mostrar en el desplegable de categorías
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias.Select(c => new
            {
                CodigoCategoria = c.CodigoCategoria,
                NombreCategoria = c.CodigoCategoria + " - " + c.Nombre // Muestra el código y nombre juntos
            }), "CodigoCategoria", "NombreCategoria");

            // Concatenar "Nit - NombreEditorial" para mostrar en el desplegable de editoriales
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales.Select(e => new
            {
                Nit = e.Nit,
                NombreEditorial = e.Nit + " - " + e.Nombres // Muestra el nit y nombre juntos
            }), "Nit", "NombreEditorial");

            return View();
        }

        // POST: Libroes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn,Titulo,Descripcion,NombreAutor,Publicacion,FechaRegistro,CodigoCategoria,NitEditorial")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el ISBN ya existe en la base de datos
                var existingLibro = await _context.Libros
                    .FirstOrDefaultAsync(l => l.Isbn == libro.Isbn);

                if (existingLibro != null)
                {
                    // Si el ISBN ya existe, agregar un error al ViewData para mostrar el mensaje en la vista
                    ViewData["IsbnError"] = "El ISBN ingresado ya existe.";

                    // Volver a cargar los datos de categorías y editoriales con concatenación
                    ViewData["CodigoCategoria"] = new SelectList(_context.Categorias.Select(c => new
                    {
                        CodigoCategoria = c.CodigoCategoria,
                        NombreCategoria = c.CodigoCategoria + " - " + c.Nombre
                    }), "CodigoCategoria", "NombreCategoria", libro.CodigoCategoria);

                    ViewData["NitEditorial"] = new SelectList(_context.Editoriales.Select(e => new
                    {
                        Nit = e.Nit,
                        NombreEditorial = e.Nit + " - " + e.Nombres
                    }), "Nit", "NombreEditorial", libro.NitEditorial);

                    return View(libro);
                }

                // Si el ISBN no existe, continuar con la creación del libro
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, volver a cargar los datos de categorías y editoriales
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias.Select(c => new
            {
                CodigoCategoria = c.CodigoCategoria,
                NombreCategoria = c.CodigoCategoria + " - " + c.Nombre
            }), "CodigoCategoria", "NombreCategoria", libro.CodigoCategoria);

            ViewData["NitEditorial"] = new SelectList(_context.Editoriales.Select(e => new
            {
                Nit = e.Nit,
                NombreEditorial = e.Nit + " - " + e.Nombres
            }), "Nit", "NombreEditorial", libro.NitEditorial);

            return View(libro);
        }


        // GET: Libroes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            // Generar el SelectList para categorías concatenando CódigoCategoria y Nombre
            ViewData["CodigoCategoria"] = new SelectList(
                _context.Categorias.Select(c => new
                {
                    CodigoCategoria = c.CodigoCategoria,
                    NombreCategoria = c.CodigoCategoria + " - " + c.Nombre
                }),
                "CodigoCategoria",
                "NombreCategoria",
                libro.CodigoCategoria
            );

            // Generar el SelectList para editoriales concatenando Nit y Nombre
            ViewData["NitEditorial"] = new SelectList(
                _context.Editoriales.Select(e => new
                {
                    Nit = e.Nit,
                    NombreEditorial = e.Nit + " - " + e.Nombres
                }),
                "Nit",
                "NombreEditorial",
                libro.NitEditorial
            );

            return View(libro);
        }

        // POST: Libroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Isbn,Titulo,Descripcion,NombreAutor,Publicacion,FechaRegistro,CodigoCategoria,NitEditorial")] Libro libro)
        {
            if (id != libro.Isbn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Isbn))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // En caso de que el modelo no sea válido, volver a cargar los SelectList con la concatenación
            ViewData["CodigoCategoria"] = new SelectList(
                _context.Categorias.Select(c => new
                {
                    CodigoCategoria = c.CodigoCategoria,
                    NombreCategoria = c.CodigoCategoria + " - " + c.Nombre
                }),
                "CodigoCategoria",
                "NombreCategoria",
                libro.CodigoCategoria
            );

            ViewData["NitEditorial"] = new SelectList(
                _context.Editoriales.Select(e => new
                {
                    Nit = e.Nit,
                    NombreEditorial = e.Nit + " - " + e.Nombres
                }),
                "Nit",
                "NombreEditorial",
                libro.NitEditorial
            );

            return View(libro);
        }


        // GET: Libroes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Buscar el libro por su ID
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                // Si no se encuentra el libro, devolver un mensaje de error
                return Json(new { success = false, errorMessage = "El Libro no fue encontrado." });
            }


            // Si no está relacionado, eliminamos el libro
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            // Si la eliminación es exitosa, devolver un mensaje de éxito
            return Json(new { success = true, message = "Libro eliminado correctamente." });
        }

        private bool LibroExists(string id)
        {
            return _context.Libros.Any(e => e.Isbn == id);
        }
    }
}
