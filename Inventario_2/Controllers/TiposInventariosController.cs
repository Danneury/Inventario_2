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
    public class TiposInventariosController : Controller
    {
        private readonly InventarioContext _context;

        public TiposInventariosController(InventarioContext context)
        {
            _context = context;
        }

        // GET: TiposInventarios
        public async Task<IActionResult> Index(string buscar)
        {
            var vTiposInventarios = from tiposInventario in _context.TiposInventarios
                                    join cuentaContable in _context.CuentaContables
                                        on tiposInventario.CuentaContable equals cuentaContable.IdCuentaContable
                                    select new TiposInventario
                                    {
                                        IdInventario = tiposInventario.IdInventario,
                                        Descripcion = tiposInventario.Descripcion,
                                        CuentaContableDesc = cuentaContable.Descripcion,
                                        Estado = tiposInventario.Estado
                                    };

            var TiposInventarioList = await vTiposInventarios.ToListAsync();

            if (!string.IsNullOrEmpty(buscar))
            {
                TiposInventarioList = TiposInventarioList.Where(s =>
                    s.Descripcion.Contains(buscar, StringComparison.OrdinalIgnoreCase) ||
                    s.IdInventario.ToString().Equals(buscar) ||
                    s.Estado.Equals(buscar, StringComparison.OrdinalIgnoreCase) ||
                    s.CuentaContableDesc.Contains(buscar, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return View(TiposInventarioList);
        }



        // GET: TiposInventarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var vTiposInventarios = from tiposInventario in _context.TiposInventarios
                                    join cuentaContable in _context.CuentaContables
                                        on tiposInventario.CuentaContable equals cuentaContable.IdCuentaContable
                                    select new TiposInventario
                                    {
                                        IdInventario = tiposInventario.IdInventario,
                                        Descripcion = tiposInventario.Descripcion,
                                        CuentaContableDesc = cuentaContable.Descripcion,
                                        Estado = tiposInventario.Estado
                                    };

            if (id == null || vTiposInventarios == null)
            {
                return NotFound();
            }

            var tiposInventarios = await vTiposInventarios.FirstOrDefaultAsync(m => m.IdInventario == id);
            if (tiposInventarios == null)
            {
                return NotFound();
            }

            return View(tiposInventarios);

        }

        // GET: TiposInventarios/Create
        public IActionResult Create()
        {
            List<CuentaContable> cuentas_contables = (from d in _context.CuentaContables
                                                      select new CuentaContable                                                        {
                                                           IdCuentaContable = d.IdCuentaContable,
                                                           Descripcion = d.Descripcion
                                                        }).ToList();
            //Pasar la lista de cuentas contables a la vista
            ViewBag.cuentas_contables_list = new SelectList(cuentas_contables, "IdCuentaContable", "Descripcion");
            return View();
        }

        // POST: TiposInventarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdInventario,Descripcion,CuentaContable,Estado")] TiposInventario tiposInventario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tiposInventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tiposInventario);
        }

        // GET: TiposInventarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TiposInventarios == null)
            {
                return NotFound();
            }

            var tiposInventario = await _context.TiposInventarios.FindAsync(id);
            if (tiposInventario == null)
            {
                return NotFound();
            }

            // Obtener los datos de CuentaContable desde la base de datos
            List<CuentaContable> cuentas_contables = _context.CuentaContables.ToList();

            // Convertir la lista en una lista de objetos SelectListItem para el DropDownList
            List<SelectListItem> opcionesCuentasContables = cuentas_contables.Select(cc => new SelectListItem
            {
                Value = cc.IdCuentaContable.ToString(),
                Text = cc.Descripcion
            }).ToList();

            // Asignar la lista de opciones del DropDownList a ViewBag o al ViewModel
            ViewData["CuentaContable"] = opcionesCuentasContables;
            return View(tiposInventario);

        }

        // POST: TiposInventarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdInventario,Descripcion,CuentaContable,Estado")] TiposInventario tiposInventario)
        {
            if (id != tiposInventario.IdInventario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tiposInventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TiposInventarioExists(tiposInventario.IdInventario))
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
            return View(tiposInventario);
        }

        // GET: TiposInventarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var vTiposInventarios = from tiposInventario in _context.TiposInventarios
                                    join cuentaContable in _context.CuentaContables
                                    on tiposInventario.CuentaContable equals cuentaContable.IdCuentaContable
                                    select new TiposInventario
                                    {
                                        IdInventario = tiposInventario.IdInventario,
                                        Descripcion = tiposInventario.Descripcion,
                                        CuentaContableDesc = cuentaContable.Descripcion,
                                        Estado = tiposInventario.Estado
                                    };
            if (id == null || _context.TiposInventarios == null)
            {
                return NotFound();
            }

            var tiposInventarios = await vTiposInventarios.FirstOrDefaultAsync(m => m.IdInventario == id);
            if (tiposInventarios == null)
            {
                return NotFound();
            }

            return View(tiposInventarios);
        }

        // POST: TiposInventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TiposInventarios == null)
            {
                return Problem("Entity set 'InventarioContext.TiposInventarios'  is null.");
            }
            var tiposInventario = await _context.TiposInventarios.FindAsync(id);
            if (tiposInventario != null)
            {
                _context.TiposInventarios.Remove(tiposInventario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TiposInventarioExists(int id)
        {
            return (_context.TiposInventarios?.Any(e => e.IdInventario == id)).GetValueOrDefault();
        }
    }
}
