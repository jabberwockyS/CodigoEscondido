using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesoroQR.Models.Tesoro
{
    public class Partida
    {
        public int PartidaID { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }


        public List<Circuito> Circuito { get; set; }
    }
}