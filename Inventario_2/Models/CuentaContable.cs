using System;
using System.Collections.Generic;

namespace Inventario_2.Models;

public partial class CuentaContable
{
    public int IdCuentaContable { get; set; }

    public string Descripcion { get; set; } = null!;
}
