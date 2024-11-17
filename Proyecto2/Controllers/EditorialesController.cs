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
    public class EditorialesController : Controller
    {
        private readonly LibreriaDbContext _context;

        public EditorialesController(LibreriaDbContext context)
        {
            _context = context;
        }

        // GET: Editoriales
        public async Task<IActionResult> Index()
        {
            return View(await _context.Editoriales.ToListAsync());
        }

        // GET: Editoriales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editoriale = await _context.Editoriales
                .FirstOrDefaultAsync(m => m.Nit == id);
            if (editoriale == null)
            {
                return NotFound();
            }

            return View(editoriale);
        }

        // GET: Editoriales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Editoriales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nit,Nombres,Telefono,Direccion,Email,Sitioweb")] Editoriale editoriale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editoriale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editoriale);
        }

        // GET: Editoriales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editoriale = await _context.Editoriales.FindAsync(id);
            if (editoriale == null)
            {
                return NotFound();
            }
            return View(editoriale);
        }

        // POST: Editoriales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nit,Nombres,Telefono,Direccion,Email,Sitioweb")] Editoriale editoriale)
        {
            if (id != editoriale.Nit)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editoriale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditorialeExists(editoriale.Nit))
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
            return View(editoriale);
        }

        //// GET: Editoriales/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var editoriale = await _context.Editoriales
        //        .FirstOrDefaultAsync(m => m.Nit == id);
        //    if (editoriale == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(editoriale);
        //}

        //// POST: Editoriales/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var editoriale = await _context.Editoriales.FindAsync(id);
        //    if (editoriale != null)
        //    {
        //        _context.Editoriales.Remove(editoriale);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        // GET: Editoriales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editorial = await _context.Editoriales
                .FirstOrDefaultAsync(m => m.Nit == id);
            if (editorial == null)
            {
                return NotFound();
            }

            return View(editorial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Buscar el editorial por su ID
            var editorial = await _context.Editoriales.FindAsync(id);
            if (editorial == null)
            {
                // Si no se encuentra el editorial, devolver un mensaje de error
                return Json(new { success = false, errorMessage = "La editorial no fue encontrada." });
            }

            // Verificar si el editorial está relacionado en la tabla Libros
            bool tieneRelacion = await _context.Libros.AnyAsync(la => la.NitEditorial == id);

            if (tieneRelacion)
            {
                // Si está relacionado, no se puede eliminar
                return Json(new { success = false, errorMessage = "No se puede eliminar la editorial porque está relacionado con otros registros." });
            }

            // Si no está relacionado, eliminamos la editorial
            _context.Editoriales.Remove(editorial);
            await _context.SaveChangesAsync();

            // Si la eliminación es exitosa, devolver un mensaje de éxito
            return Json(new { success = true, message = "Editorial eliminado correctamente." });
        }
        private bool EditorialeExists(int id)
        {
            return _context.Editoriales.Any(e => e.Nit == id);
        }
    }
}
