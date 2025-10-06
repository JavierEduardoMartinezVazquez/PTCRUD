using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRUEBATEC.Models
{
    public class ProductoModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        [Display(Name = "Nombre completo del producto")]
        public string NombreP { get; set; }

        [Required(ErrorMessage = "El SKU es obligatorio.")]
        [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "SKU inválido. Solo letras mayúsculas, números y guiones.")]
        [Display(Name = "Código de barras / SKU")] 
        public string SKU { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser un número positivo.")]
        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor que 0.")]
        [Display(Name = "Precio de venta")]
        public decimal PrecioV { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de costo debe ser mayor que 0.")]
        [Display(Name = "Precio de costo")]
        public decimal PrecioC { get; set; }


        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
    }
}
