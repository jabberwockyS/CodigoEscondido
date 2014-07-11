using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesoroQR.Models.Tesoro
{
    public class Avance
    {
        public int AvanceID { get; set; }
        public int UltimaPista { get; set; }

        public Juego Juego { get; set; }
        public Circuito Circuito { get; set; }
        
    }
}