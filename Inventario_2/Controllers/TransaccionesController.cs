using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventario_2.Models;

namespace Inventario_2.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly InventarioContext _context;

        public TransaccionesController(InventarioContext context)
        {
            _context = context;
        }

        // GET: Transacciones
        public async Task<IActionResult> Index(string buscar)
        {
            var vTransacciones = from Transacciones in _context.Transacciones select Transacciones;

            if (!string.IsNullOrEmpty(buscar))
            {
                vTransacciones = vTransacciones.Where(s => s.TipoTransaccion!.Contains(buscar) ||
                                                   s.IdTransaccion.ToString().Contains(buscar));

            }

            var inventarioContext = vTransacciones.Include(a => a.IdArticulosNavigation);
            return View(await inventarioContext.ToListAsync());
        }

        // GET: Transacciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transacciones == null)
            {
                return NotFound();
            }

            var transaccione = await _context.Transacciones
                .Include(t => t.IdArticulosNavigation)
                .FirstOrDefaultAsync(m => m.IdTransaccion == id);
            if (transaccione == null)
            {
                return NotFound();
            }

            return View(transaccione);
        }

        // GET: Transacciones/Create
        public IActionResult Create()
        {
            // Obtener los datos de Articulos desde la base de datos
            List<Articulo> articulosList = _context.Articulos.ToList();

            // Convertir la lista en una lista de objetos SelectListItem para el DropDownList
            List<SelectListItem> opcionesArticulos = articulosList.Select(articulo => new SelectListItem
            {
                Value = articulo.IdArticulos.ToString(),
                Text = articulo.Descripcion
            }).ToList();

            ViewData["IdArticulos"] = opcionesArticulos;
            return View();
        }

        // POST: Transacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTransaccion,TipoTransaccion,IdArticulos,Fecha,Cantidad,Monto")] Transaccione transaccione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaccione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdArticulos"] = new SelectList(_context.Articulos, "IdArticulos", "IdArticulos", transaccione.IdArticulos);
            return View(transaccione);
        }

        // GET: Transacciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transacciones == null)
            {
                return NotFound();
            }

            var transaccione = await _context.Transacciones.FindAsync(id);
            if (transaccione == null)
            {
                return NotFound();
            }

            // Obtener los datos de Articulos desde la base de datos
            List<Articulo> articulosList = _context.Articulos.ToList();

            // Convertir la lista en una lista de objetos SelectListItem para el DropDownList
            List<SelectListItem> opcionesArticulos = articulosList.Select(articulo => new SelectListItem
            {
                Value = articulo.IdArticulos.ToString(),
                Text = articulo.Descripcion
            }).ToList();

            ViewData["IdArticulos"] = opcionesArticulos;
            return View(transaccione);
        }

        // POST: Transacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTransaccion,TipoTransaccion,IdArticulos,Fecha,Cantidad,Monto")] Transaccione transaccione)
        {
            if (id != transaccione.IdTransaccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaccione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransaccioneExists(transaccione.IdTransaccion))
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
            ViewData["IdArticulos"] = new SelectList(_context.Articulos, "IdArticulos", "IdArticulos", transaccione.IdArticulos);
            return View(transaccione);
        }

        // GET: Transacciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transacciones == null)
            {
                return NotFound();
            }

            var transaccione = await _context.Transacciones
                .Include(t => t.IdArticulosNavigation)
                .FirstOrDefaultAsync(m => m.IdTransaccion == id);
            if (transaccione == null)
            {
                return NotFound();
            }

            return View(transaccione);
        }

        // POST: Transacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transacciones == null)
            {
                return Problem("Entity set 'InventarioContext.Transacciones'  is null.");
            }
            var transaccione = await _context.Transacciones.FindAsync(id);
            if (transaccione != null)
            {
                _context.Transacciones.Remove(transaccione);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransaccioneExists(int id)
        {
          return (_context.Transacciones?.Any(e => e.IdTransaccion == id)).GetValueOrDefault();
        }
    }
}
