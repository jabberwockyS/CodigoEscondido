using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TesoroQR.Models.Tesoro
{
    public class JuegoDBContext: DbContext
    {
        public JuegoDBContext(): base("DefaultConnection")
        {

        }
      
        public DbSet<Circuito> Circuitos { get; set; }
        public DbSet<Partida> Partidas { get; set; }
        public DbSet<Avance> Avances { get; set; }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pista> Pistas { get; set; }
        public DbSet<Juego> Juegos { get; set; }
    }
}