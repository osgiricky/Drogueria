using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrogSystem.Models
{
    [Table("PagoTerceros")]
    public class PaymentProvider
    {
        [Key]
        public int Id_Pago { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Valor Pagado")]
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = false)]
        public decimal Valor_Pago { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Fecha Pago")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha_Pago { get; set; }

        [MaxLength(200)]
        [DisplayName("Observaciones")]
        public string Observacion { get; set; }

        public int TerceroId { get; set; }
        public virtual Provider Provider { get; set; }
    }
}