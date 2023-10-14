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
    public class ArticulosController : Controller
    {
        private readonly InventarioContext _context;

        public ArticulosController(InventarioContext context)
        {
            _context = context;
        }

        // GET: Articulos
        public async Task<IActionResult> Index(string buscar)
        {
            var vArticulos = from Articulo in _context.Articulos select Articulo;

            if (!string.IsNullOrEmpty(buscar))
            {
                string buscarUpper = buscar.ToUpper(); // Convertimos la búsqueda a mayúsculas

                vArticulos = vArticulos.Where(s => s.Descripcion!.Contains(buscar) ||
                                                   s.IdArticulos.ToString().Contains(buscar) ||
                                                   s.Estado.ToUpper() == buscarUpper); // Hacemos la comparación insensible a mayúsculas y minúsculas
            }

            var inventarioContext = vArticulos.Include(a => a.IdInventarioNavigation);
            return View(await inventarioContext.ToListAsync());

        }

        // GET: Articulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Articulos == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.IdInventarioNavigation)
                .FirstOrDefaultAsync(m => m.IdArticulos == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // GET: Articulos/Create
        public IActionResult Create()
        {
            // Obtener los datos de TiposInventario desde la base de datos
            List<TiposInventario> tiposInventarioList = _context.TiposInventarios.ToList();

            // Convertir la lista en una lista de objetos SelectListItem para el DropDownList
            List<SelectListItem> opcionesTiposInventario = tiposInventarioList.Select(ti => new SelectListItem
            {
                Value = ti.IdInventario.ToString(),
                Text = ti.Descripcion
            }).ToList();

            // Asignar la lista de opciones del DropDownList a ViewBag o al ViewModel
            ViewBag.TiposInventarioList = opcionesTiposInventario;

            return View();
        }

        // POST: Articulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdArticulos,Descripcion,Existencia,IdInventario,CostoUnitario,Estado")] Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdInventario"] = new SelectList(_context.TiposInventarios, "IdInventario", "IdInventario", articulo.IdInventario);
            return View(articulo);
        }

        // GET: Articulos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Articulos == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }

            // Obtener los datos de TiposInventario desde la base de datos
            List<TiposInventario> tiposInventarioList = _context.TiposInventarios.ToList();

            // Convertir la lista en una lista de objetos SelectListItem para el DropDownList
            List<SelectListItem> opcionesTiposInventario = tiposInventarioList.Select(ti => new SelectListItem
            {
                Value = ti.IdInventario.ToString(),
                Text = ti.Descripcion
            }).ToList();

            // Asignar la lista de opciones del DropDownList a ViewBag o al ViewModel
            ViewData["IdInventario"] = opcionesTiposInventario;
            return View(articulo);
        }

        // POST: Articulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArticulos,Descripcion,Existencia,IdInventario,CostoUnitario,Estado")] Articulo articulo)
        {
            if (id != articulo.IdArticulos)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticuloExists(articulo.IdArticulos))
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
            ViewData["IdInventario"] = new SelectList(_context.TiposInventarios, "IdInventario", "IdInventario", articulo.IdInventario);
            return View(articulo);
        }

        // GET: Articulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Articulos == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.IdInventarioNavigation)
                .FirstOrDefaultAsync(m => m.IdArticulos == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // POST: Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Articulos == null)
            {
                return Problem("Entity set 'InventarioContext.Articulos'  is null.");
            }
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo != null)
            {
                _context.Articulos.Remove(articulo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticuloExists(int id)
        {
          return (_context.Articulos?.Any(e => e.IdArticulos == id)).GetValueOrDefault();
        }
    }
}
