using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventario_2.Models;

public class FechaValidaAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime fecha;
        if (DateTime.TryParse(value.ToString(), out fecha))
        {
            if (fecha > DateTime.Now)
            {
                return new ValidationResult("La fecha no debe ser posterior a la fecha actual del sistema.");
            }
        }
        else
        {
            return new ValidationResult("El formato de fecha no es válido.");
        }

        return ValidationResult.Success;
    }
}

public partial class Transaccione
{
    public int IdTransaccion { get; set; }

    [Display(Name = "Tipo de Transaccion")]
    public string TipoTransaccion { get; set; } = null!;

    [Display(Name = "Articulos")]
    public int? IdArticulos { get; set; }

    [DataType(DataType.Date)]
    [FechaValida(ErrorMessage = "Registre una fecha correcta.")]
    public DateTime Fecha { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad deben ser mayor  a cero.")]
    public int Cantidad { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El Monto debe ser mayor a cero.")]
    public decimal? Monto { get; set; }

    [Display(Name = "Articulos")]
    public virtual Articulo? IdArticulosNavigation { get; set; }
}
