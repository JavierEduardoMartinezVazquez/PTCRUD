using System.ComponentModel.DataAnnotations;

namespace PRUEBATEC.Models
{
    public class UsuarioModel
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string Usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
    }
}
