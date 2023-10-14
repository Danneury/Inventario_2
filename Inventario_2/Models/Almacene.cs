using System;
using System.Collections.Generic;

namespace Inventario_2.Models;

public partial class Almacene
{
    public int IdAlmacenes { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Estado { get; set; } = null!;
}
