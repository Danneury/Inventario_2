using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario_2.Models;

public partial class TiposInventario
{
    public int IdInventario { get; set; }

    public string Descripcion { get; set; } = null!;

    [Display(Name = "Cuenta Contable")]
    public int CuentaContable { get; set; }
    [NotMapped]
    public string? CuentaContableDesc { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
}
