using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrogSystem.Models
{
    [Table("Producto")]
    public class Product
    {
        [Key]
        public int ProductoId { get; set; }

        [Required()]
        [MaxLength(50)]
        [DisplayName("Nombre Producto")]
        public string NombreProducto { get; set; }

        [DisplayName("Minimo Stock")]
        public int MinStock { get; set; }

        [DisplayName("Maximo Stock")]
        public int MaxStock { get; set; }

        [MaxLength(200)]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [MaxLength(200)]
        [DisplayName("Componentes")]
        public string Componentes { get; set; }

        public virtual ICollection<ProductDetail> ProductDetail { get; set; }

    }
}