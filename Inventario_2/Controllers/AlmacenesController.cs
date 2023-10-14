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
    public class AlmacenesController : Controller
    {
        private readonly InventarioContext _context;

        public AlmacenesController(InventarioContext context)
        {
            _context = context;
        }

        // GET: Almacenes
        public async Task<IActionResult> Index(string buscar)
        {
            var vAlmacenes = from Almacene in _context.Almacenes select Almacene;

            if (!string.IsNullOrEmpty(buscar))
            {
                string buscarUpper = buscar.ToUpper();

                vAlmacenes = vAlmacenes.Where(s => s.Descripcion!.Contains(buscar) ||
                                                   s.IdAlmacenes.ToString().Contains(buscar) ||
                                                   s.Estado.ToUpper() == buscarUpper);

            }

            return View(await vAlmacenes.ToListAsync());
        }

        // GET: Almacenes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Almacenes == null)
            {
                return NotFound();
            }

            var almacene = await _context.Almacenes
                .FirstOrDefaultAsync(m => m.IdAlmacenes == id);
            if (almacene == null)
            {
                return NotFound();
            }

            return View(almacene);
        }

        // GET: Almacenes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Almacenes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAlmacenes,Descripcion,Estado")] Almacene almacene)
        {
            if (ModelState.IsValid)
            {
                _context.Add(almacene);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(almacene);
        }

        // GET: Almacenes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Almacenes == null)
            {
                return NotFound();
            }

            var almacene = await _context.Almacenes.FindAsync(id);
            if (almacene == null)
            {
                return NotFound();
            }
            return View(almacene);
        }

        // POST: Almacenes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAlmacenes,Descripcion,Estado")] Almacene almacene)
        {
            if (id != almacene.IdAlmacenes)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(almacene);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlmaceneExists(almacene.IdAlmacenes))
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
            return View(almacene);
        }

        // GET: Almacenes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Almacenes == null)
            {
                return NotFound();
            }

            var almacene = await _context.Almacenes
                .FirstOrDefaultAsync(m => m.IdAlmacenes == id);
            if (almacene == null)
            {
                return NotFound();
            }

            return View(almacene);
        }

        // POST: Almacenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Almacenes == null)
            {
                return Problem("Entity set 'InventarioContext.Almacenes'  is null.");
            }
            var almacene = await _context.Almacenes.FindAsync(id);
            if (almacene != null)
            {
                _context.Almacenes.Remove(almacene);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlmaceneExists(int id)
        {
          return (_context.Almacenes?.Any(e => e.IdAlmacenes == id)).GetValueOrDefault();
        }
    }
}
