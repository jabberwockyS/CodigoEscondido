using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoroQR.Models.Tesoro;

namespace TesoroQR.CapaNegocio
{

    public class BPartida
    {

        JuegoDBContext db = new JuegoDBContext();

        public void CrearPartida(DateTime Fecha, string Descripcion)
        {

            Partida Partida = new Partida() { Fecha = Fecha, Descripcion = Descripcion };
            db.Partidas.Add(Partida);
            db.SaveChanges();
        }
    }
}