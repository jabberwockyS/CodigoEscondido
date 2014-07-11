using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesoroQR.Models.Tesoro
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
        public string TipoUsuario { get; set; }


        public List<Juego> Juegos { get; set; }


    }
}