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
    public class CategoriasController : Controller
    {
        private readonly LibreriaDbContext _context;

        public CategoriasController(LibreriaDbContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorias.ToListAsync());
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.CodigoCategoria == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodigoCategoria,Nombre")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodigoCategoria,Nombre")] Categoria categoria)
        {
            if (id != categoria.CodigoCategoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.CodigoCategoria))
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
            return View(categoria);
        }

        //// GET: Categorias/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var categoria = await _context.Categorias
        //        .FirstOrDefaultAsync(m => m.CodigoCategoria == id);
        //    if (categoria == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(categoria);
        //}

        //// POST: Categorias/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var categoria = await _context.Categorias.FindAsync(id);
        //    if (categoria != null)
        //    {
        //        _context.Categorias.Remove(categoria);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.CodigoCategoria == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Buscar el Categoria por su ID
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                // Si no se encuentra el Categoria, devolver un mensaje de error
                return Json(new { success = false, errorMessage = "El Categoria no fue encontrado." });
            }

            // Verificar si el Categoria está relacionado en la tabla LibrosCategorias
            bool tieneRelacion = await _context.Libros.AnyAsync(la => la.CodigoCategoria == id);

            if (tieneRelacion)
            {
                // Si está relacionado, no se puede eliminar
                return Json(new { success = false, errorMessage = "No se puede eliminar el Categoria porque está relacionado con otros registros." });
            }

            // Si no está relacionado, eliminamos el Categoria
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            // Si la eliminación es exitosa, devolver un mensaje de éxito
            return Json(new { success = true, message = "Categoria eliminado correctamente." });
        }


        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.CodigoCategoria == id);
        }
    }
}
