using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario_2.Models;

public partial class AsientoContable
{
    public int IdMovimiento { get; set; }

    public string Descripcion { get; set; } = null!;

    [NotMapped]
    public int? Auxiliar { get; set; }

    [Display(Name = "Cuenta Debito")]
    public int CuentaDb { get; set; }

    [NotMapped]
    public string? CuentaDbDesc { get; set; }

    [Display(Name = "Cuenta Credito")]
    public int CuentaCr { get; set; }

    [NotMapped]
    public string? CuentaCrDesc { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser mayor o igual a cero.")]
    public decimal? Monto { get; set; }
}
