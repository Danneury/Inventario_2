using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventario_2.Models;

public partial class Articulo
{
    public int IdArticulos { get; set; }

    public string Descripcion { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Las existencias deben ser mayor a cero.")]
    public int Existencia { get; set; }

    [Display(Name = "Inventario")]
    public int? IdInventario { get; set; }

    [Display(Name = "Costo Unitario")]
    [Range(1, double.MaxValue, ErrorMessage = "El costo debe ser mayor a cero.")]
    public decimal CostoUnitario { get; set; }
    public string Estado { get; set; } = null!;

    [Display(Name = "Inventario")]
    public virtual TiposInventario? IdInventarioNavigation { get; set; }

    public virtual ICollection<Transaccione> Transacciones { get; set; } = new List<Transaccione>();
}
