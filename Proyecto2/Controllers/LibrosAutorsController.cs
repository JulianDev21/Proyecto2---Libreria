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
    public class LibrosAutorsController : Controller
    {
        private readonly LibreriaDbContext _context;

        public LibrosAutorsController(LibreriaDbContext context)
        {
            _context = context;
        }

        // GET: LibrosAutors
        public async Task<IActionResult> Index()
        {
            var libreriaDbContext = _context.LibrosAutors.Include(l => l.IdAutorNavigation).Include(l => l.IsbnNavigation);
            return View(await libreriaDbContext.ToListAsync());
        }

        // GET: LibrosAutors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var librosAutor = await _context.LibrosAutors
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IsbnNavigation)
                .FirstOrDefaultAsync(m => m.IdLibroAutor == id);
            if (librosAutor == null)
            {
                return NotFound();
            }

            return View(librosAutor);
        }

        // GET: LibrosAutors/Create
        public IActionResult Create()
        {
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor");
            ViewData["Isbn"] = new SelectList(_context.Libros, "Isbn", "Isbn");
            return View();
        }

        // POST: LibrosAutors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLibroAutor,IdAutor,Isbn")] LibrosAutor librosAutor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(librosAutor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor", librosAutor.IdAutor);
            ViewData["Isbn"] = new SelectList(_context.Libros, "Isbn", "Isbn", librosAutor.Isbn);
            return View(librosAutor);
        }

        // GET: LibrosAutors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var librosAutor = await _context.LibrosAutors.FindAsync(id);
            if (librosAutor == null)
            {
                return NotFound();
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor", librosAutor.IdAutor);
            ViewData["Isbn"] = new SelectList(_context.Libros, "Isbn", "Isbn", librosAutor.Isbn);
            return View(librosAutor);
        }

        // POST: LibrosAutors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLibroAutor,IdAutor,Isbn")] LibrosAutor librosAutor)
        {
            if (id != librosAutor.IdLibroAutor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(librosAutor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibrosAutorExists(librosAutor.IdLibroAutor))
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
            ViewData["IdAutor"] = new SelectList(_context.Autors, "IdAutor", "IdAutor", librosAutor.IdAutor);
            ViewData["Isbn"] = new SelectList(_context.Libros, "Isbn", "Isbn", librosAutor.Isbn);
            return View(librosAutor);
        }


        //// GET: LibrosAutors/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var librosAutor = await _context.LibrosAutors
        //        .Include(l => l.IdAutorNavigation)  // Incluye autor relacionado
        //        .Include(l => l.IsbnNavigation)    // Incluye ISBN relacionado
        //        .FirstOrDefaultAsync(m => m.IdLibroAutor == id);

        //    if (librosAutor == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(librosAutor);
        //}



        //// POST: LibrosAutors/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var librosAutor = await _context.LibrosAutors.FindAsync(id);
        //    if (librosAutor == null)
        //    {
        //        return Json(new { success = false, errorMessage = "El registro no fue encontrado." });
        //    }

        //    // Eliminar el registro directamente
        //    _context.LibrosAutors.Remove(librosAutor);
        //    await _context.SaveChangesAsync();

        //    return Json(new { success = true, message = "Registro eliminado correctamente." });
        //}


        // GET: LibrosAutors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libroAutor = await _context.LibrosAutors
                .FirstOrDefaultAsync(m => m.IdLibroAutor == id);
            if (libroAutor == null)
            {
                return NotFound();
            }

            return View(libroAutor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Buscar el autor por su ID
            var librosAutor = await _context.LibrosAutors.FindAsync(id);
            if (librosAutor == null)
            {
                // Si no se encuentra el autor, devolver un mensaje de error
                return Json(new { success = false, errorMessage = "El Libro - Autor no fue encontrado." });
            }


            // Si no está relacionado, eliminamos el autor
            _context.LibrosAutors.Remove(librosAutor);
            await _context.SaveChangesAsync();

            // Si la eliminación es exitosa, devolver un mensaje de éxito
            return Json(new { success = true, message = "Libro - Autor fue eliminado correctamente." });
        }



        private bool LibrosAutorExists(int id)
        {
            return _context.LibrosAutors.Any(e => e.IdLibroAutor == id);
        }
    }
}
