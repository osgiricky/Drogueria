using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrogSystem.Models
{
    [Table("Fabricante")]
    public class Marker
    {
        [Key]
        public int MarkerId { get; set; }
        [Required()]
        [DisplayName("Nombre Fabricante")]
        public string NombreFabricante { get; set; }

        public virtual ICollection<ProductDetail> ProductDetail { get; set; }
    }
}