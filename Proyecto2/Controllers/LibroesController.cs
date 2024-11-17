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
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria");
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit");
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
                    ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
                    ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
                    return View(libro);
                }

                // Si el ISBN no existe, continuar con la creación del libro
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, devolver la vista con los errores
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
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
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
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
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
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
