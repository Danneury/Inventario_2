using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventario_2.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Inventario_2.Controllers
{
    public class AsientoContablesController : Controller
    {
        private readonly InventarioContext _context;
        private readonly HttpClient _httpClient = new HttpClient();

        public AsientoContablesController(InventarioContext context)
        {
            _context = context;

            // Configurar el HttpClient con la base address y los encabezados
            _httpClient.BaseAddress = new Uri("http://129.80.203.120:5000/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: AsientoContables
        public async Task<IActionResult> Index(string buscar)
        {

            var vAsientoContables = from asientoContable in _context.AsientoContables
                                    join cuentaContableDB in _context.CuentaContables
                                    on asientoContable.CuentaDb equals cuentaContableDB.IdCuentaContable
                                    join cuentaContableCr in _context.CuentaContables
                                    on asientoContable.CuentaCr equals cuentaContableCr.IdCuentaContable
                                    select new AsientoContable
                                    {
                                        IdMovimiento = asientoContable.IdMovimiento,
                                        Descripcion = asientoContable.Descripcion,
                                        Auxiliar = asientoContable.Auxiliar,
                                        CuentaDb = asientoContable.CuentaDb,
                                        CuentaDbDesc = cuentaContableDB.Descripcion,
                                        CuentaCr = asientoContable.CuentaCr,
                                        CuentaCrDesc = cuentaContableCr.Descripcion,
                                        Monto = asientoContable.Monto
                                    };

            var vAsientoContablesList = await vAsientoContables.ToListAsync();

            if (!string.IsNullOrEmpty(buscar))
            {
                vAsientoContablesList = vAsientoContablesList.Where(s =>
                    s.Descripcion.Contains(buscar, StringComparison.OrdinalIgnoreCase) ||
                    s.CuentaDbDesc.Equals(buscar, StringComparison.OrdinalIgnoreCase) ||
                    s.CuentaCrDesc.Equals(buscar, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return View(vAsientoContablesList);
        }

        // GET: AsientoContables/Contabilizar/5
        public async Task<IActionResult> Contabilizar(int? id)
        {
            if (id == null || _context.AsientoContables == null)
            {
                return NotFound();
            }

            var asientoContable = await _context.AsientoContables
                .FirstOrDefaultAsync(m => m.IdMovimiento == id);

            if (asientoContable == null)
            {
                return NotFound();
            }

            return View(asientoContable);
        }

        // POST: AsientoContables/Contabilizar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contabilizar([Bind("IdMovimiento,Descripcion,Auxiliar,CuentaDb,CuentaCr,Monto")] AsientoContable asientoContable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string endpoint = "post-accounting-entries";

                    var requestData = new
                    {
                        descripcion = asientoContable.Descripcion,
                        auxiliar = asientoContable.Auxiliar,
                        cuentaDB = asientoContable.CuentaDb,
                        cuentaCR = asientoContable.CuentaCr,
                        monto = asientoContable.Monto
                    };

                    var json = JsonSerializer.Serialize(requestData);

                    using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                    using (var response = await _httpClient.PostAsync(endpoint, content))
                    {
                        response.EnsureSuccessStatusCode(); 

                        var responseContent = await response.Content.ReadAsStringAsync();

                        return RedirectToAction("Index");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", $"An error occurred while sending the data to the API. Please try again later. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError("", $"An unexpected error occurred. Please try again later. Error: {ex.Message}");
            }

            return View(asientoContable);
        }

    }
}