using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TesoroQR.Models.Tesoro
{
    public class Pista
    {
        [Key]
        public int PistaID { get; set; }

        
        [DisplayName("Nombre Pista")]
        [Required(ErrorMessage = "No ingresaste el nombre -.-'")]         
        public string Nombre { get; set; }

        [DisplayName("Descripcion")]
        [Required(ErrorMessage = "No ingresaste la Descripcion -.-'")]  
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        public Circuito Circuito { get; set; }

        public int orden { get; set; }
    }
}